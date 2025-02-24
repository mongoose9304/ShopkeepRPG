using System.Collections;
using System.Collections.Generic;
using UnityEditor.Recorder;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MoveableObjectGrid : MonoBehaviour
{
    [Tooltip("Singleton instance of the class")]
    public static MoveableObjectGrid instance;
    private Dictionary<Vector2Int, Transform> grid = new();
    private const float cellSize = 3f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ProcessGrid();
    }

    void ProcessGrid() 
    {
        foreach(Transform row in transform) 
        {
            foreach(Transform slot in row) 
            {
                Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(slot.position.x / cellSize), Mathf.RoundToInt(row.position.z / cellSize));
                grid[gridPos] = slot;
                if(slot.childCount > 1) 
                {
                    if (slot.GetChild(1).gameObject.name == "BarginBin")
                    {
                        continue;
                    }
                    Pedestal p = slot.GetChild(1).GetComponentInChildren<Pedestal>(); //make it work with pedestal first
                    p.gridPos = gridPos;
                }
            }
            
        }
    }

    public List<Transform> GetNeighbors(Transform node)
    {
        List<Transform> neighbors = new();
        Vector2Int nodePos = new Vector2Int(Mathf.RoundToInt(node.position.x / cellSize), Mathf.RoundToInt(node.position.z / cellSize));

        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(-1, 1),  
            new Vector2Int(1, -1), new Vector2Int(-1, -1) 
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = nodePos + dir;
            if (grid.TryGetValue(neighborPos, out Transform neighbor))
            {
                if (neighbor.childCount > 1)
                {
                    Pedestal p = neighbor.GetChild(1).GetComponentInChildren<Pedestal>(); //make it work with pedestal first
                    Debug.Log(neighborPos);
                }
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
