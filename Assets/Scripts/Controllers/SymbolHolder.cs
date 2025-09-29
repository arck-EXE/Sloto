using UnityEngine;

public class SymbolHolder : MonoBehaviour
{
    public SlotSymbolSO symbolData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSymbol(SlotSymbolSO symbol)
    {
        symbolData = symbol;
        if (spriteRenderer != null && symbol != null)
        {
            spriteRenderer.sprite = symbol.sprite;
        }
    }
}
