using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dungeons {
    /// <summary>
    /// The DungeonDeveloper is used when developing dungeons; It basically provides utilities for geenerating, resetting, and stepping through the dungeon generation process while modifying assets.
    /// The hotkeys, setup by the DungeonDeveloper prefab, are as follows.
    /// 
    /// R - start / resume.
    /// P - pause.
    /// G - generate.
    /// S - generate step.
    /// T - generate steps as defined by <see cref="steps"/>.
    /// 
    /// </summary>
    public class DungeonDeveloper : MonoBehaviour {
        [SerializeField]
        private DungeonLayout layout;
        [SerializeField]
        private float speed;
        [SerializeField]
        private int steps;
        [SerializeField]
        private Vector2 point;

        private Coroutine coroutine;
        private DungeonGenerator generator;

        private void OnDrawGizmos() {
            //Draws some debug utilities that was used while developing the dungeon generation algorithm.
            if (false && generator != null) {
                void DrawNode(int node, int depth) {
                    if (node >= generator.Collision.Nodes.Length)
                        return;

                    var n = generator.Collision.Nodes[node];
                    var centerXZ = new Vector3(n.center.x, 9.0f + depth * 0.1f, n.center.y);
                    var sizeXZ = new Vector3(n.rect.size.x, 0.0f, n.rect.size.y);
                    Gizmos.color = new(1, 1, 1, 0.1f);
                    Gizmos.DrawWireCube(centerXZ, sizeXZ);
                    Gizmos.color = new(1, 1, 1, 0.01f);
                    Gizmos.DrawCube(centerXZ, sizeXZ);
                    if (n.left != 0)
                        DrawNode(n.left, depth + 1);
                    if (n.right != 0)
                        DrawNode(n.right, depth + 1);
                }
                void DrawI(int node) {
                    if (node >= generator.Collision.Nodes.Length)
                        return;

                    var n = generator.Collision.Nodes[node];
                    if (n.rect.Contains(point)) {
                        Gizmos.color = new(1, 0, 0, 0.1f);
                        Gizmos.DrawWireCube(new(n.rect.center.x, 15, n.rect.center.y), new(n.rect.size.x, 15, n.rect.size.y));
                        Gizmos.color = new(1, 0, 0, 0.01f);
                        Gizmos.DrawCube(new(n.rect.center.x, 15, n.rect.center.y), new(n.rect.size.x, 15, n.rect.size.y));
                        if (n.right != 0)
                            DrawI(n.right);
                        if (n.left != 0)
                            DrawI(n.left);
                    }
                }
                if (generator.Collision.Intersects(point))
                    DrawI(0);
                DrawNode(0, 0);
                Gizmos.color = new(1, 0, 0, 1);
                Gizmos.DrawSphere(new(point.x, 10, point.y), 3.0f);
            }
        }
        
        public void Step(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator == null)
                generator = new(layout);
            if (coroutine != null)
                StopCoroutine(coroutine);
            generator.GenerateStep();
            generator.Instantiate(transform);
        }
        public void Clear(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator != null)
                generator = null;
            if (coroutine != null)
                StopCoroutine(coroutine);
            for (int i = 0; i < transform.childCount; ++i)
                Destroy(transform.GetChild(i).gameObject);
        }
        public void Pause(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator == null)
                generator = new(layout);
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = null;
        }
        public void Resume(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator == null)
                generator = new(layout);
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(Generate());
        }
        public void Generate(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator == null)
                generator = new(layout);
            System.Diagnostics.Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
            var roomCountPre = generator.Elements.Length;
            generator.Generate();
            var roomCountPost = generator.Elements.Length;
            w.Stop();
            Debug.Log(string.Format("Generating {0} took {1}ms", roomCountPost - roomCountPre, w.Elapsed.TotalMilliseconds));
            generator.Instantiate(transform);
        }
        public void GenerateSteps(InputAction.CallbackContext context) {
            if (context.phase != InputActionPhase.Started)
                return;
            if (generator == null)
                generator = new(layout);
            System.Diagnostics.Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
            var roomCountGenerating = steps;
            var roomCountPre = generator.Elements.Length;
            generator.Generate(roomCountGenerating);
            var roomCountPost = generator.Elements.Length;
            w.Stop();
            Debug.Log(string.Format("Generating {0} took {1}ms", roomCountPost - roomCountPre, w.Elapsed.TotalMilliseconds));
            generator.Instantiate(transform);
        }

        private IEnumerator Generate() {
            if (generator == null)
                generator = new(layout);
            while (generator != null && generator.GenerateStep()) {
                generator.Instantiate(transform);
                if (speed == 0.0f)
                    yield return new WaitForEndOfFrame();
                else
                    yield return new WaitForSeconds(speed);
            }
        }
    }
}
