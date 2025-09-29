using UnityEngine;

[ExecuteInEditMode]
public class SlotMachineSetup : MonoBehaviour
{
    public float winLineWidth = 5f;
    private GameObject winLine;

    void OnEnable()
    {
        SetupWinLine();
    }

    void SetupWinLine()
    {
        // Create or find win line
        winLine = GameObject.FindWithTag("WinLine");
        if (!winLine)
        {
            winLine = new GameObject("WinLine");
            winLine.tag = "WinLine";
            winLine.transform.position = Vector3.zero;
            
            // Add visible sprite
            var sr = winLine.AddComponent<SpriteRenderer>();
            sr.color = new Color(0, 1, 0, 0.3f);
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = new Vector2(winLineWidth, 0.1f);
            
            // Add collider
            var col = winLine.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = new Vector2(winLineWidth, 0.2f);
        }
    }

    void OnDrawGizmos()
    {
        // Draw win line visualization
        Gizmos.color = Color.green;
        Vector3 pos = Vector3.zero;
        Gizmos.DrawLine(pos + Vector3.left * winLineWidth/2, pos + Vector3.right * winLineWidth/2);
    }
}
