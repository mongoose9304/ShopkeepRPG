using System;
using System.Drawing;
using UnityEngine;

namespace Dungeons {
    /// <summary>
    /// Bounding rectangle hierarchies are used to efficiently find intersections between geometry and a set of rectangles.
    /// </summary>
    public struct BoundingRectangleHierarchy {
        /// <summary>
        /// The primitives.
        /// </summary>
        public readonly ReadOnlySpan<Prim> Prims {
            get => new(m_Prims, 0, m_PrimsNum);
        }
        /// <summary>
        /// The nodes.
        /// </summary>
        public readonly ReadOnlySpan<Node> Nodes {
            get => m_Nodes;
        }

        /// <summary>
        /// Constructs the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <param name="cap">The number of rectangles that can be added before triggering a resize.</param>
        /// <param name="jmp">The number of rectangles that can be added before triggering a rebuild.</param>
        public BoundingRectangleHierarchy(ReadOnlySpan<Rect> rects, int cap = 0, int jmp = 1) {
            if (cap < rects.Length)
                cap = rects.Length;

            m_Nodes = new Node[0];
            m_Prims = new Prim[cap];
            m_PrimsBld = 0;
            m_PrimsJmp = jmp;
            m_PrimsNum = rects.Length;
            for (int i = 0; i < rects.Length; ++i)
                m_Prims[i] = new(rects[i]);

            Rebuild();
        }

        /// <summary>
        /// Rebuilds the brh.
        /// </summary>
        public void Rebuild() {
            if (m_PrimsNum == 0)
                m_Nodes = Array.Empty<Node>();
            else {
                int nodeIndexNext = 0;
                m_PrimsBld = m_PrimsNum;
                m_Nodes = new Node[m_PrimsNum * 2];
                InternalRebuild(ref nodeIndexNext, 0, m_PrimsNum);
            }
        }

        /// <summary>
        /// Adds the rectangle to the brh.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public void Add(in Rect rect) {
            if (InternalAdd(rect))
                Rebuild();
        }
        /// <summary>
        /// Adds the rectangles to the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        public void Add(in ReadOnlySpan<Rect> rects) {
            if (rects.Length > 0) {
                bool rebuild = false;
                for (int i = 0; i < rects.Length; ++i)
                    rebuild |= InternalAdd(rects[i]);
                if (rebuild)
                    Rebuild();
            }
        }
        /// <summary>
        /// Adds the rectangles to the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <param name="offset">The rectangle offset.</param>
        public void Add(in ReadOnlySpan<Rect> rects, Vector2 offset) {
            if (rects.Length > 0) {
                bool rebuild = false;
                for (int i = 0; i < rects.Length; ++i)
                    rebuild |= InternalAdd(new(rects[i].position + offset, rects[i].size));
                if (rebuild)
                    Rebuild();
            }
        }
        /// <summary>
        /// Removes the rectangle from the brh.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public void Remove(in Rect rect) {
            if (InternalRemove(rect))
                Rebuild();
        }
        /// <summary>
        /// Removes the rectangles from the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        public void Remove(in ReadOnlySpan<Rect> rects) {
            if (rects.Length > 0) {
                bool rebuild = false;
                for (int i = 0; i < rects.Length; ++i)
                    rebuild |= InternalRemove(rects[i]);
                if (rebuild)
                    Rebuild();
            }
        }
        /// <summary>
        /// Removes the rectangles from the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <param name="offset">The rectangle offset.</param>
        public void Remove(in ReadOnlySpan<Rect> rects, Vector2 offset) {
            if (rects.Length > 0) {
                bool rebuild = false;
                for (int i = 0; i < rects.Length; ++i)
                    rebuild |= InternalRemove(new(rects[i].position + offset, rects[i].size));
                if (rebuild)
                    Rebuild();
            }
        }

        /// <summary>
        /// Returns true if the point intersects the brh.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point intersects the brh.</returns>
        public readonly bool Intersects(in Vector2 point) {
            return InternalIntersects(point) || (m_PrimsBld != 0 && InternalIntersects(point, 0));
        }
        /// <summary>
        /// Returns true if the points intersect the brh.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>True if the points intersect the brh.</returns>
        public readonly bool Intersects(in ReadOnlySpan<Vector2> points) {
            for (int i = 0; i < points.Length; ++i)
                if (Intersects(points[i]))
                    return true;
            return false;
        }
        /// <summary>
        /// Returns true if the rectangle intersects the brh.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>True if the rectangle intersects the brh.</returns>
        public readonly bool Intersects(in Rect rect) {
            return InternalIntersects(rect) || (m_PrimsBld != 0 && InternalIntersects(rect, 0));
        }
        /// <summary>
        /// Returns true if the rectangles intersect the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <returns>True if the rectangles intersect the brh.</returns>
        public readonly bool Intersects(in ReadOnlySpan<Rect> rects) {
            for (int i = 0; i < rects.Length; ++i)
                if (Intersects(rects[i]))
                    return true;
            return false;
        }
        /// <summary>
        /// Returns true if the rectangles intersect the brh.
        /// </summary>
        /// <param name="rects">The rectangles.</param>
        /// <param name="offset">The rectangle offset.</param>
        /// <returns>True if the rectangles intersect the brh.</returns>
        public readonly bool Intersects(in ReadOnlySpan<Rect> rects, Vector2 offset) {
            for (int i = 0; i < rects.Length; ++i)
                if (Intersects(new Rect(rects[i].position + offset, rects[i].size)))
                    return true;
            return false;
        }

        /// <summary>
        /// Defines nodes within a brh.
        /// </summary>
        public struct Node {
            /// <summary>
            /// The center.
            /// </summary>
            public Vector2 center;
            /// <summary>
            /// The rectangle.
            /// </summary>
            public Rect rect;
            /// <summary>
            /// The right child.
            /// </summary>
            public int right;
            /// <summary>
            /// The left child.
            /// </summary>
            public int left;
        }
        /// <summary>
        /// Defines primitives within a brh.
        /// </summary>
        public struct Prim {
            public Prim(in Rect rect) {
                center = rect.center;
                this.rect = rect;
            }
            
            /// <summary>
            /// The center.
            /// </summary>
            public Vector2 center;
            /// <summary>
            /// The rectangle.
            /// </summary>
            public Rect rect;
        }

        private readonly int InternalRebuild(ref int nodeIndexNext, int beg, int end) {
            var nodeIndex = nodeIndexNext++;
            ref var node = ref m_Nodes[nodeIndex];
            //Calculate node bounds.
            node.rect = m_Prims[beg].rect;
            for (int i = beg + 1; i < end; ++i)
                node.rect = GrowToFit(node.rect, m_Prims[i].rect);
            node.center = node.rect.center;
            if (beg == end - 1) {
                //Create prim node.
                node.left = 0;
                node.right = 0;
            }
            else {
                //Calculate prim splits.
                var splitAxis = GetAxisSplit(node.rect);
                var splitVal = GetAxisSplit(node.center, splitAxis);
                var splitBeg = beg;
                var splitEnd = end - 1;
                while (splitBeg <= splitEnd) {
                    var primBeg = m_Prims[splitBeg];
                    if (GetAxisSplit(primBeg.center, splitAxis) <= splitVal) {
                        ++splitBeg;
                        continue;
                    }
                    var primEnd = m_Prims[splitEnd];
                    if (GetAxisSplit(primEnd.center, splitAxis) > splitVal) {
                        --splitEnd;
                        continue;
                    }
                    m_Prims[splitEnd] = primBeg;
                    m_Prims[splitBeg] = primEnd;
                    ++splitBeg;
                    --splitEnd;
                }

                //Create left node.
                var mid = beg + (end - beg) / 2;
                node.left = InternalRebuild(ref nodeIndexNext, beg, mid);
                //Create right node.
                node.right = InternalRebuild(ref nodeIndexNext, mid, end);
            }
            return nodeIndex;
        }
        
        private bool InternalAdd(in Rect rect) {
            if (m_PrimsNum >= m_Prims.Length) {
                //Expand primitives array.
                var nPrimsCap = m_PrimsNum + m_PrimsJmp;
                var nPrims = new Prim[nPrimsCap];
                m_Prims.CopyTo(nPrims.AsSpan());
                m_Prims = nPrims;
                //Insert custom primitive.
                m_Prims[m_PrimsNum++] = new(rect);
                //Rebuild nodes array.
                return true;
            }
            else {
                //Insert custom primitive.
                m_Prims[m_PrimsNum++] = new(rect);
                return false;
            }
        }
        private bool InternalRemove(in Rect rect) {
            for (int i = 0; i < m_PrimsNum; ++i) {
                if (m_Prims[i].rect.Equals(rect)) {
                    m_Prims[i] = m_Prims[--m_PrimsNum];
                    return true;
                }
            }
            return false;
        }

        private readonly bool InternalIntersects(in Rect area) {
            for (int i = m_PrimsBld; i < m_PrimsNum; ++i)
                if (m_Prims[i].rect.Overlaps(area))
                    return true;
            return false;
        }
        private readonly bool InternalIntersects(in Rect area, int node) {
            ref var temp = ref m_Nodes[node];
            if (temp.rect.Overlaps(area)) {
                if (temp.right != 0 && InternalIntersects(area, temp.right))
                    return true;
                if (temp.left != 0 && InternalIntersects(area, temp.left))
                    return true;
                if (temp.left == 0 && temp.right == 0)
                    return true;
            }
            return false;
        }
        private readonly bool InternalIntersects(in Vector2 area) {
            for (int i = m_PrimsBld; i < m_PrimsNum; ++i)
                if (m_Prims[i].rect.Contains(area))
                    return true;
            return false;
        }
        private readonly bool InternalIntersects(in Vector2 area, int node) {
            ref var temp = ref m_Nodes[node];
            if (temp.rect.Contains(area)) {
                if (temp.right != 0 && InternalIntersects(area, temp.right))
                    return true;
                if (temp.left != 0 && InternalIntersects(area, temp.left))
                    return true;
                if (temp.left == 0 && temp.right == 0)
                    return true;
            }
            return false;
        }

        public static int GetAxisSplit(in Rect self) {
            if (self.width >= self.height)
                return 0;
            return 1;
        }
        public static float GetAxisSplit(in Vector2 self, int axis) {
            if (axis == 0)
                return self.x;
            return self.y;
        }
        private static Rect GrowToFit(in Rect self, in Rect targ) {
            Vector2 min = Vector2.Min(self.min, targ.min);
            Vector2 max = Vector2.Max(self.max, targ.max);
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private Node[] m_Nodes;
        private Prim[] m_Prims;
        private int m_PrimsBld;
        private int m_PrimsNum;
        private int m_PrimsJmp;
    }
}
