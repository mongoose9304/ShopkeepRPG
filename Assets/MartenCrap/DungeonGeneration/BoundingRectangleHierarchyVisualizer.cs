using UnityEngine;

namespace Dungeons {
    [ExecuteInEditMode]
    public class BoundingRectangleHierarchyVisualizer : MonoBehaviour {
        [SerializeField]
        private Rect[] rects;
        [SerializeField]
        private Vector2 point;
        [SerializeField]
        private Color color;
        [SerializeField]
        private Color colorLeaf;
        [SerializeField]
        private Color colorIntercept;

        private void Update() {
            if (rects.Length == 0) {
                rects = new Rect[1000];
                for (int i = 0; i < rects.Length; ++i) {
                    rects[i] = new(Random.Range(0.0f, 1000.0f), Random.Range(0.0f, 1000.0f), Random.Range(3.0f, 25.0f), Random.Range(3.0f, 25.0f));
                }
            }
        }
        private void OnDrawGizmos() {
            Color colorIntercept = this.colorIntercept;
            Color colorInterceptOpaque = new(colorIntercept.r, colorIntercept.g, colorIntercept.b);
            Color colorInterceptTransparent = new(colorIntercept.r, colorIntercept.g, colorIntercept.b, colorIntercept.a * 3.0f);
            BoundingRectangleHierarchy collision = new(rects);

            void DrawN(int node, int depth) {
                var n = collision.Nodes[node];
                if (n.left != 0 || n.right != 0) {
                    if (n.left != 0)
                        DrawN(n.left, depth + 1);
                    if (n.right != 0)
                        DrawN(n.right, depth + 1);
                    Gizmos.color = color;
                }
                else {
                    Gizmos.color = colorLeaf;
                }
                Gizmos.DrawCube(n.rect.center, n.rect.size);
                var axis = BoundingRectangleHierarchy.GetAxisSplit(n.rect);
                if (axis == 0) {
                    var a = n.rect.center;
                    var e = new Vector2(n.rect.width, 0.0f) * 0.5f;
                    Gizmos.DrawLine(a + e, a - e);
                }
                else {
                    var a = n.rect.center;
                    var e = new Vector2(0.0f, n.rect.height) * 0.5f;
                    Gizmos.DrawLine(a + e, a - e);
                }
            }
            void DrawI(int node) {
                var n = collision.Nodes[node];
                if (n.rect.Contains(point)) {
                    Gizmos.color = colorIntercept;
                    Gizmos.DrawCube(n.rect.center, n.rect.size);
                    Gizmos.color = colorInterceptTransparent;
                    Gizmos.DrawWireCube(n.rect.center, n.rect.size);
                    if (n.right != 0)
                        DrawI(n.right);
                    if (n.left != 0)
                        DrawI(n.left);
                }
            }
            DrawN(0, 0);
            Gizmos.color = colorInterceptOpaque;
            Gizmos.DrawSphere(point, 1.0f);
            DrawI(0);
        }
    }
}
