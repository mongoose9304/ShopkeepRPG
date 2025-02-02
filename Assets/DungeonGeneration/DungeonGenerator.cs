using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeons {
    public class DungeonGenerator {
        /// <summary>
        /// The current number of rooms in the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementCount = "elementCount";
        /// <summary>
        /// The maximum number of rooms in the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementCountMaximum = "elementCountMaximum";
        /// <summary>
        /// The maximum remaining number of rooms to generate in the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementCountRemaining = "elementCountRemaining";
        
        /// <summary>
        /// The length of the current branch of the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementDepth = "elementDepth";
        /// <summary>
        /// The maximum length of any branch of the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementDepthMaximum = "elementDepthMaximum";
        /// <summary>
        /// The maximum remaining length of the current branch of the dungeon.
        /// This variable should not be modified by dungeon elements.
        /// </summary>
        private const string VarElementDepthRemaining = "elementDepthRemaining";

        /// <summary>
        /// The layout.
        /// </summary>
        public DungeonLayout Layout {
            get => m_Layout;
        }
        /// <summary>
        /// The elements.
        /// </summary>
        public ReadOnlySpan<DungeonGeneratorElement> Elements {
            get => m_Elements.AsSpan(0, m_ElementsCount);
        }
        /// <summary>
        /// The potentials.
        /// </summary>
        public ref DungeonGeneratorElementPool Potentials {
            get => ref m_ElementSpawningPotentials;
        }
        /// <summary>
        /// The collision.
        /// </summary>
        public ref BoundingRectangleHierarchy Collision {
            get => ref m_BoundingRectangleHierarchy;
        }

        /// <summary>
        /// Constructs the dungeon generator.
        /// </summary>
        /// <param name="layout">The dungeon layout.</param>
        public DungeonGenerator(DungeonLayout layout) {
            //Initalize the generator structures.
            m_Layout = layout;
            m_BoundingRectangleHierarchy = new(default, 100, 100);
            m_ElementSpawningPotentials = new(100);
            m_Elements = new DungeonGeneratorElement[100];
            m_ElementsCount = 0;
            m_ElementsCapacity = 100;
            m_ElementsInstantiated = 0;
            //Initialize pending anchors.
            m_Pending = new();
            PushPending(new() {
                name = "root",
                pool = layout.Root,
                mode = DungeonLayout.AnchorMode.Parent,
                offset = Vector2.zero,
                index = 0,
                depth = 0,
            });
            //Initialize generator variables.
            m_Counters = new();
            m_Counters[VarElementDepthMaximum] = m_Layout.MaximumDepth;
            m_Counters[VarElementCountMaximum] = m_Layout.MaximumRooms;
            foreach (var variable in m_Layout.Variables)
                m_Counters[variable.Name] = variable.Value;
        }

        /// <summary>
        /// Instantiate the sections of the dungeon that have not been placed into the scene.
        /// </summary>
        /// <param name="root">The transform that should contain the dungeon.</param>
        public void Instantiate(Transform root) {
            for (int i = m_ElementsInstantiated; i < m_ElementsCount; ++i) {
                var element = m_Elements[i];
                var prefab = element.Element.Prefab;
                var offset = element.Position;
                if (prefab) {
                    UnityEngine.Object.Instantiate(prefab, new(offset.x, 0.0f, offset.y), Quaternion.identity, root);
                }
            }
            m_ElementsInstantiated = m_ElementsCount;
        }
        /// <summary>
        /// Generates the dungeon; Note that dungeon will not be instantiated until you call <see cref="Instantiate(Transform)"/>.
        /// </summary>
        /// <returns>The number of elements that were generated.</returns>
        public int Generate() {
            return Generate(int.MaxValue);
        }
        /// <summary>
        /// Generates the dungeon; Note that dungeon will not be instantiated until you call <see cref="Instantiate(Transform)"/>.
        /// </summary>
        /// <param name="steps">The maximum number of elements to generate.</param>
        /// <returns>The number of elements that were generated.</returns>
        public int Generate(int steps) {
            var start = m_ElementsCount;
            for (int i = 0; i < steps; ++i)
                if (!GenerateStep())
                    break;
            return m_ElementsCount - start;
        }
        /// <summary>
        /// Generates the dungeon asynchronously; Note that dungeon will not be instantiated until you call <see cref="Instantiate(Transform)"/>.
        /// </summary>
        /// <returns>The asynchronously operation; One element will be generated everytime <see cref="IEnumerator.MoveNext"/> returns true.</returns>
        public IEnumerator GenerateAsync() {
            while (GenerateStep())
                yield return null;
        }
        /// <summary>
        /// Generates the dungeon asynchronously; Note that dungeon will not be instantiated until you call <see cref="Instantiate(Transform)"/>.
        /// </summary>
        /// <param name="steps">The maximum number of elements to generate.</param>
        /// <returns>The asynchronously operation; One element will be generated everytime <see cref="IEnumerator.MoveNext"/> returns true.</returns>
        public IEnumerator GenerateAsync(int steps) {
            while (--steps >= 0 && GenerateStep())
                yield return null;
        }
        /// <summary>
        /// Generates one element in the dungeon; Note that dungeon will not be instantiated until you call <see cref="Instantiate(Transform)"/>.
        /// </summary>
        /// <returns>True if one element was generated; False if the dungeon is done generating.</returns>
        public bool GenerateStep() {
            while (PullPending(out var anchor)) {
                //Validate anchor.
                if (!anchor.pool)
                    continue;
                //Initalize variables.
                m_Counters[VarElementDepth] = anchor.depth;
                m_Counters[VarElementDepthRemaining] = m_Layout.MaximumDepth - anchor.depth;
                m_Counters[VarElementCount] = m_Layout.MaximumRooms - m_ElementsCountExcludingFallbacksAndChildren;
                m_Counters[VarElementCountRemaining] = m_Layout.MaximumRooms - m_ElementsCountExcludingFallbacksAndChildren;
                //Collect and sample potentials.
                var potentialIsFallback = false;
                if (anchor.mode == DungeonLayout.AnchorMode.Parent || (anchor.depth < m_Layout.MaximumDepth && m_ElementsCountExcludingFallbacksAndChildren < m_Layout.MaximumRooms))
                    anchor.pool.ProvideLayoutElements(this, anchor, 1.0f);
                if (Potentials.IsEmpty && anchor.fallback) {
                    potentialIsFallback = true;
                    anchor.fallback.ProvideLayoutElements(this, anchor, 1.0f);
                }
                if (Potentials.Sample(out var selectedElement, out var selectedIndex)) {
                    //Clear potentials.
                    m_ElementSpawningPotentials.Reset();
                    //Validate selected.
                    if (!selectedElement || selectedElement.Prefab == null)
                        continue;
                    //Ensure capacity for element.
                    if (m_ElementsCapacity == m_ElementsCount) {
                        var ncapacity = (m_ElementsCount + 1) * 2;
                        var nelements = new DungeonGeneratorElement[ncapacity];
                        m_Elements.CopyTo(nelements.AsSpan());
                        m_Elements = nelements;
                        m_ElementsCapacity = ncapacity;
                    }
                    //Execute operations.
                    var operations = selectedElement.Operations;
                    var operationsLength = operations.Length;
                    for (int i = 0; i < operationsLength; ++i)
                        ModifyCounter(operations[i].Name, operations[i].Value, operations[i].Mode);
                    //Insert element.
                    var position = anchor.offset - selectedElement.Anchors[selectedIndex].offset;
                    m_Elements[m_ElementsCount++] = new() {
                        Element = selectedElement,
                        Position = position,
                    };
                    //Insert collision.
                    m_BoundingRectangleHierarchy.Add(selectedElement.Bounds, position);
                    //Insert pending anchors.
                    var anchors = selectedElement.Anchors;
                    var anchorsLength = anchors.Length;
                    for (int i = 0; i < anchorsLength; ++i) {
                        if (i == selectedIndex)
                            continue;
                        if (anchors[i].mode == DungeonLayout.AnchorMode.Child)
                            continue;
                        ref readonly var temp = ref anchors[i];
                        PushPending(new DungeonLayout.AnchorConcrete {
                            name = temp.name,
                            mode = temp.mode,
                            offset = position + temp.offset,
                            pool = temp.pool,
                            fallback = temp.fallback,
                            index = i,
                            depth = anchor.depth + 1,
                        });
                    }
                    //Increment element counter.
                    if (anchor.mode != DungeonLayout.AnchorMode.Child && !potentialIsFallback)
                        ++m_ElementsCountExcludingFallbacksAndChildren;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the counter with the specified name.
        /// </summary>
        /// <param name="name">The counter name.</param>
        /// <returns>The counter value.</returns>
        public float GetCounter(string name) {
            m_Counters.TryGetValue(name, out var counter);
            return counter;
        }
        /// <summary>
        /// Sets the counter with the specified name.
        /// </summary>
        /// <param name="name">The counter name.</param>
        /// <param name="value">The counter value.</param>
        /// <returns>The counter value prior to being modified.</returns>
        public float SetCounter(string name, float value) {
            m_Counters.TryGetValue(name, out var counter);
            m_Counters[name] = value;
            return counter;
        }
        /// <summary>
        /// Modifies the counter with the specified name.
        /// </summary>
        /// <param name="name">The counter name.</param>
        /// <param name="right">The right operand.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>The counter value prior to being modified.</returns>
        public float ModifyCounter(string name, float right, OperationMode operation) {
            m_Counters.TryGetValue(name, out var counter);
            switch (operation) {
                case OperationMode.Add:
                    m_Counters[name] = counter + right;
                    break;
                case OperationMode.Sub:
                    m_Counters[name] = counter - right;
                    break;
                case OperationMode.Mul:
                    m_Counters[name] = counter * right;
                    break;
                case OperationMode.Div:
                    m_Counters[name] = counter / right;
                    break;
                case OperationMode.Mod:
                    m_Counters[name] = counter % right;
                    break;
            }
            return counter;
        }
        /// <summary>
        /// Compares counter with the specified name.
        /// </summary>
        /// <param name="name">The counter name.</param>
        /// <param name="right">The right operand.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>True if the comparison succeeded.</returns>
        public bool CompareCounter(string name, float right, ComparisonMode operation) {
            m_Counters.TryGetValue(name, out var counter);
            switch (operation) {
                case ComparisonMode.Equal:
                    return counter == right;
                case ComparisonMode.NotEqual:
                    return counter != right;
                case ComparisonMode.Less:
                    return counter < right;
                case ComparisonMode.LessOrEqual:
                    return counter <= right;
                case ComparisonMode.Greater:
                    return counter > right;
                case ComparisonMode.GreaterOrEqual:
                    return counter >= right;
                case ComparisonMode.DividesEvenly:
                    return (counter % right) == 0.0f;
                case ComparisonMode.DividesOddly:
                    return (counter % right) != 0.0f;
            }
            return false;
        }

        /// <summary>
        /// Validates the dungeon.
        /// </summary>
        /// <returns>True if the dungeon is valid; False if the dungeon is invalid and should be regenerated.</returns>
        public bool Validate() {
            var conditions = m_Layout.Conditions;
            var conditionsLength = conditions.Length;
            for (int i = 0; i < conditionsLength; ++i)
                if (!CompareCounter(conditions[i].Name, conditions[i].Value, conditions[i].Mode))
                    return false;
            return true;
        }

        [Serializable]
        public enum OperationMode { Add, Sub, Mul, Div, Mod }
        [Serializable]
        public enum ComparisonMode { Equal, NotEqual, Less, LessOrEqual, Greater, GreaterOrEqual, DividesEvenly, DividesOddly }
        [Serializable]
        public struct Operation {
            public readonly string Name {
                get => m_Name;
            }
            public readonly OperationMode Mode {
                get => m_Mode;
            }
            public readonly float Value {
                get => m_Value;
            }

            [SerializeField]
            private string m_Name;
            [SerializeField]
            private OperationMode m_Mode;
            [SerializeField]
            private float m_Value;
        }
        [Serializable]
        public struct Condition {
            public readonly string Name {
                get => m_Name;
            }
            public readonly ComparisonMode Mode {
                get => m_Mode;
            }
            public readonly float Value {
                get => m_Value;
            }

            [SerializeField]
            private string m_Name;
            [SerializeField]
            private ComparisonMode m_Mode;
            [SerializeField]
            private float m_Value;
        }

        private void PushPending(in DungeonLayout.AnchorConcrete anchor) {
            m_Pending.Enqueue(anchor);
        }
        private bool PullPending(out DungeonLayout.AnchorConcrete anchor) {
            return m_Pending.TryDequeue(out anchor);
        }
        
        private DungeonLayout m_Layout;
        private Queue<DungeonLayout.AnchorConcrete> m_Pending;
        private readonly Dictionary<string, float> m_Counters;
        private BoundingRectangleHierarchy m_BoundingRectangleHierarchy;
        private DungeonGeneratorElementPool m_ElementSpawningPotentials;
        private DungeonGeneratorElement[] m_Elements;
        private int m_ElementsInstantiated;
        private int m_ElementsCapacity;
        private int m_ElementsCount;
        private int m_ElementsCountExcludingFallbacksAndChildren;
    }
}
