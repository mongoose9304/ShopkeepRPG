using UnityEngine;
using UnityEngine.InputSystem;

public sealed class DungeonGenerator : MonoBehaviour {
    public void ContinueDungeon(InputAction.CallbackContext context) {
        if (context.phase != InputActionPhase.Started)
            return;
        if (m_Context == null) {
            m_Context = new();
            m_Generator = StartCoroutine(m_Context.Generate(m_Layout, transform));
        }
    }
    public void DestroyDungeon() {
        if (m_Generator != null)
            StopCoroutine(m_Generator);
        for (int i = 0; i < transform.childCount; ++i)
            Destroy(transform.GetChild(i).gameObject);
        m_Context = null;
        m_Generator = null;
    }

    private void OnDrawGizmosSelected() {
        if (m_Context != null) {
            Gizmos.color = Color.red;
            foreach (var bound in m_Context.Bounds) {
                Gizmos.DrawWireCube(new(bound.center.x, 1.5f, bound.center.y), new(bound.size.x, 3, bound.size.y));
            }
        }
    }
    
    [SerializeField]
    private DungeonRoomLayoutSelector m_Layout;
    private DungeonGeneratorContext m_Context;
    private Coroutine m_Generator;
}
