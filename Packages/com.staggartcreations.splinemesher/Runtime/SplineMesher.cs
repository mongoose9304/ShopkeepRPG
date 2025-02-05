// Staggart Creations (http://staggart.xyz)
// Copyright protected under Unity Asset Store EULA
// Copying or referencing source code for the production of new asset store, or public content, is strictly prohibited!

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

#if MATHEMATICS
using Unity.Mathematics;
#endif

#if SPLINES
using UnityEngine.Splines;
#endif

namespace sc.modeling.splines.runtime
{
    [ExecuteInEditMode]
    [AddComponentMenu("Splines/Spline Mesher")]
    [HelpURL("https://staggart.xyz/sm-docs/")]
    [Icon(SplineMesher.kPackageRoot + "/Editor/Resources/spline-mesher-icon-64px.psd")]
    [SelectionBase] //Select this object when selecting caps (child objects)
    public partial class SplineMesher : MonoBehaviour
    {
        public const string VERSION = "1.2.2";
        public const string kPackageRoot = "Packages/com.staggartcreations.splinemesher";

        /// <summary>
        /// The input mesh to be used for mesh generation
        /// </summary>
        public Mesh sourceMesh;
        [Tooltip("The axis of the mesh that's considered to its forward direction." +
                 "\n\nConventionally, the Z-axis is forward. If you have to change this it's strongly recommend to fix the mesh's orientation instead!")]
        public Vector3 rotation;

        /// <summary>
        /// The output GameObject to which a <see cref="MeshFilter"/> component may be added. The output mesh will be assigned here.
        /// </summary>
        [Tooltip("The GameObject to which a Mesh Filter component may be added. The output mesh will be assigned here.")]
        public GameObject outputObject;
        [Obsolete("Set the Rebuild Trigger flag \"On Start\" instead", false)]
        public bool rebuildOnStart;
        
        [Flags]
        public enum RebuildTriggers
        {
            [InspectorName("Via scripting")]
            None = 0,
            [InspectorName("On Spline Change")]
            OnSplineChanged = 1,
            OnSplineAdded = 2,
            OnSplineRemoved = 4,
            [InspectorName("On Start()")]
            OnStart = 8,
            OnUIChange = 16
        }

        [Tooltip("Control which sort of events cause the mesh to be regenerated." +
                 "\n\n" +
                 "For instance when the spline changes (default), or on the component's Start() function." +
                 "\n\n" +
                 "If none are selected you need to call the Rebuild() function through script.")]
        public RebuildTriggers rebuildTriggers = RebuildTriggers.OnSplineAdded | RebuildTriggers.OnSplineRemoved | RebuildTriggers.OnSplineChanged | RebuildTriggers.OnUIChange;

        [SerializeField]
        private MeshCollider meshCollider;

        /// <summary>
        /// Settings used for mesh generation
        /// </summary>
        public Settings settings = new Settings();
        
        #pragma warning disable CS0067
        public delegate void Action(SplineMesher instance);
        /// <summary>
        /// Pre- and post-build callbacks. The instance being passed is the Spline Mesher being rebuild.
        /// </summary>
        public static event Action onPreRebuildMesh, onPostRebuildMesh;

        /// <summary>
        /// UnityEvent for a GameObject's function to be executed when river is rebuild. This is exposed in the inspector.
        /// </summary>
        [Serializable]
        public class RebuildEvent : UnityEvent { }
        /// <summary>
        /// UnityEvent, fires whenever the spline is rebuild (eg. editing nodes) or parameters are changed
        /// </summary>
        [HideInInspector]
        public RebuildEvent onPreRebuild, onPostRebuild;
        #pragma warning restore CS0067
        
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("meshFilter")]
        private MeshFilter m_meshFilter;
        /// <summary>
        /// The MeshFilter component added to the output GameObject
        /// </summary>
        public MeshFilter meshFilter
        {
            private set => m_meshFilter = value;
            get => m_meshFilter;
        }

        private void Reset()
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter)
            {
                outputObject = meshFilter.gameObject;
            
                #if UNITY_EDITOR
                //If the mesh is saved to disk, consider it a viable source mesh
                if (UnityEditor.EditorUtility.IsPersistent(meshFilter.sharedMesh))
                {
                    sourceMesh = meshFilter.sharedMesh;
                }
                #endif
            }
            #if SPLINES
            splineContainer = GetComponentInParent<SplineContainer>();
            #endif
        }

        private void Start()
        {
            //In this case the component was likely copied somewhere, or prefabbed. Mesh data will have been lost, so regenerating it is an alternative
            if (rebuildTriggers.HasFlag(RebuildTriggers.OnStart)) Rebuild();
        }

        private void OnEnable()
        {
            #if SPLINES
            SubscribeSplineCallbacks();
            #endif
        }

        private void OnDisable()
        {
            #if SPLINES
            UnsubscribeSplineCallbacks();
            #endif
        }
        
        #if SPLINES
        private partial void SubscribeSplineCallbacks();
        private partial void UnsubscribeSplineCallbacks();
        public partial void UpdateCaps();
        #endif

        #if UNITY_EDITOR
        private readonly System.Diagnostics.Stopwatch rebuildTimer = new System.Diagnostics.Stopwatch();
        #endif

        /// <summary>
        /// Checks for the presence of a <see cref="MeshFilter"/> and <see cref="MeshRenderer"/> component on the assigned output object. Added if missing
        /// </summary>
        public void ValidateOutput()
        {
            if (!outputObject) return;

            //Note: Targeting a specific GameObject, rather than a MeshFilter directly.
            //This makes it easier to add support for multiple output meshes, or LOD Groups, which involves adding components or child objects
            if (!meshFilter) meshFilter = outputObject.GetComponent<MeshFilter>();
            if (!meshFilter)
            {
                meshFilter = outputObject.AddComponent<MeshFilter>();
                
                MeshRenderer meshRenderer = outputObject.GetComponent<MeshRenderer>();
                if (meshRenderer == false)
                {
                    meshRenderer = outputObject.AddComponent<MeshRenderer>();
                }
            }
        }
        
        private Mesh inputMesh;

        /// <summary>
        /// Regenerates the output mesh for all the splines within the assigned <see cref="SplineContainer"/>. Also recreates the collision mesh.
        /// </summary>
        public void Rebuild()
        {
            #if SPLINES && MATHEMATICS
            if (!splineContainer) return;
            
            if (!outputObject) return;

            var createMesh = !(settings.collision.enable && settings.collision.colliderOnly);
            
            meshFilter = outputObject.GetComponent<MeshFilter>();
            
            if (createMesh)
            {
                if (!meshFilter) return;
            }

            onPreRebuildMesh?.Invoke(this);
            onPreRebuild?.Invoke();
            
            Profiler.BeginSample("Spline Mesher: Rebuild", this);

            ValidateData();
 
            #if UNITY_EDITOR
            rebuildTimer.Reset();
            rebuildTimer.Start();
            #endif

            if (sourceMesh)
            {
                if (Application.isPlaying && sourceMesh.isReadable == false)
                {
                    throw new Exception($"[Spline Mesher] To use this at runtime, the mesh \"{sourceMesh.name}\" requires the Read/Write option enabled in its import settings. For procedurally created geometry, use \"Mesh.UploadMeshData(false)\"");
                }

                inputMesh = SplineMeshGenerator.TransformMesh(sourceMesh, rotation, settings.deforming.scale.x < 0, settings.deforming.scale.y < 0);
            }
            else
            {
                #if UNITY_EDITOR
                rebuildTimer.Stop();
                #endif
                
                return;
            }
            
            if (createMesh)
            {
                //Avoid self-collision
                var collision = settings.collision.enable && meshCollider;
                if (collision) meshCollider.enabled = false;

                Profiler.BeginSample("Spline Mesher: Create Mesh", this);

                meshFilter.sharedMesh = SplineMeshGenerator.CreateMesh(splineContainer, inputMesh, outputObject.transform.worldToLocalMatrix, settings, scaleData, rollData,
                    vertexColorRedData, vertexColorGreenData, vertexColorBlueData, vertexColorAlphaData);

                Profiler.EndSample();

                if (collision) meshCollider.enabled = true;
            }
            else
            {
                //Clear
                if(meshFilter && meshFilter.sharedMesh) meshFilter.sharedMesh = null;
            }
            
            CreateCollider();
            
            #if UNITY_EDITOR
            rebuildTimer.Stop();
            #endif
            
            Profiler.EndSample();

            onPostRebuildMesh?.Invoke(this);
            onPostRebuild?.Invoke();
            #endif
        }

        /// <summary>
        /// Returns the build time, in milliseconds, of the last rebuild operation
        /// </summary>
        /// <returns></returns>
        public float GetLastRebuildTime()
        {
            #if UNITY_EDITOR
            return rebuildTimer.ElapsedMilliseconds;
            #else
            return 0;
            #endif
        }

        private void CreateCollider()
        {
            #if SPLINES && MATHEMATICS
            if (!splineContainer) return;

            if (settings.collision.enable)
            {
                if (!meshCollider) meshCollider = outputObject.GetComponent<MeshCollider>();
                if (!meshCollider) meshCollider = outputObject.AddComponent<MeshCollider>();

                var m_collisionMesh = settings.collision.collisionMesh;

                if (settings.collision.type == Settings.ColliderType.Box)
                {
                    m_collisionMesh = SplineMeshGenerator.CreateBoundsMesh(inputMesh, settings.collision.boxSubdivisions);
                }
                else
                {
                    if (settings.collision.collisionMesh)
                    {
                        m_collisionMesh = SplineMeshGenerator.TransformMesh(settings.collision.collisionMesh, rotation, settings.deforming.scale.x < 0, settings.deforming.scale.y < 0);
                    }
                }

                if (m_collisionMesh)
                {
                    //Skip cleaning of degenerate triangles
                    //meshCollider.cookingOptions = MeshColliderCookingOptions.None;
                    
                    //If the visual mesh and collision mesh are identical, simply use that
                    if (m_collisionMesh.GetHashCode() == sourceMesh.GetHashCode())
                    {
                        meshCollider.sharedMesh = meshFilter.sharedMesh;
                    }
                    else
                    {
                        meshCollider.sharedMesh = null; //Avoid self-collision with raycasts

                        //float spacing = settings.distribution.spacing;
                        //settings.distribution.spacing = 0f;
                        
                        meshCollider.sharedMesh = SplineMeshGenerator.CreateMesh(splineContainer, m_collisionMesh, meshCollider.transform.worldToLocalMatrix, settings, scaleData, rollData);
                        
                        //settings.distribution.spacing = spacing;
                        
                        meshCollider.sharedMesh.name += " Collider";
                    }
                }
                else
                {
                    meshCollider.sharedMesh = null;
                }
            }
            else if(meshCollider)
            {
                DestroyImmediate(meshCollider);
            }
            #endif
        }

        [SerializeField] [HideInInspector]
        private Vector3 prevSplinePosition;
        [SerializeField] [HideInInspector]
        private Quaternion prevSplineRotation;
        [SerializeField] [HideInInspector]
        private Vector3 prevSplineScale;
        
        private void OnDrawGizmosSelected()
        {
            #if SPLINES && MATHEMATICS
            if (splineContainer && Time.frameCount % 2 == 0)
            {
                if (rebuildTriggers.HasFlag(RebuildTriggers.OnSplineChanged))
                {
                    Transform splineTransform = splineContainer.transform;
                    
                    var hasChanged = false;

                    hasChanged |= (prevSplinePosition != splineTransform.position);
                    hasChanged |= (prevSplineRotation != splineTransform.rotation);
                    hasChanged |= (prevSplineScale != splineTransform.lossyScale);
                    
                    if (hasChanged)
                    {
                        prevSplinePosition = splineTransform.position;
                        prevSplineRotation = splineTransform.rotation;
                        prevSplineScale = splineTransform.lossyScale;
                        
                        Rebuild();
                    }
                }
            }
            #endif
        }
    }
}