using UnityEngine;

public class SymbolHolder : MonoBehaviour
{
    public SlotSymbolSO symbolData;
    public bool isInWinningLine = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetSymbol(SlotSymbolSO symbol)
    {
        if (symbol == null || spriteRenderer == null) return;
        
        symbolData = symbol;
        spriteRenderer.sprite = symbol.sprite;
    }

    private void Update()
    {
        // Check if this symbol is in the winning line using world position
        isInWinningLine = Mathf.Abs(transform.position.y) < 0.1f;
    }
}