using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public List<SlotSymbolSO> availableSymbols;
    public GameObject symbolPrefab;
    public int numberOfSymbols = 10;
    public float symbolHeight = 1f;
    public float spacing = 0.1f;

    [HideInInspector] public List<SymbolHolder> symbols = new List<SymbolHolder>();

    void Start()
    {
        SpawnSymbols();
    }

    void SpawnSymbols()
    {
        float startY = (numberOfSymbols / 2f) * (symbolHeight + spacing);

        for (int i = 0; i < numberOfSymbols; i++)
        {
            GameObject symbolGO = Instantiate(symbolPrefab, transform);
            symbolGO.transform.localPosition = new Vector3(0, startY - (i * (symbolHeight + spacing)), 0);

            var holder = symbolGO.GetComponent<SymbolHolder>();
            holder.SetSymbol(GetRandomWeightedSymbol());
            symbols.Add(holder);
        }
    }

    public SlotSymbolSO GetRandomWeightedSymbol()
    {
        int totalWeight = 0;
        foreach (var s in availableSymbols)
            totalWeight += s.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;
        foreach (var s in availableSymbols)
        {
            cumulative += s.weight;
            if (roll < cumulative) return s;
        }
        return availableSymbols[0];
    }

    public SymbolHolder GetWinningSymbol()
    {
        SymbolHolder winner = null;
        float closestToZero = float.MaxValue;

        foreach (var symbol in symbols)
        {
            float distanceToWinLine = Mathf.Abs(symbol.transform.localPosition.y);
            if (distanceToWinLine < closestToZero)
            {
                closestToZero = distanceToWinLine;
                winner = symbol;
            }
        }

        if (closestToZero <= 0.1f && winner != null)
        {
            Debug.Log($"{name}: Found {winner.symbolData.symbolName} at y={winner.transform.localPosition.y:F3}");
            return winner;
        }

        Debug.LogWarning($"{name}: No symbol at y=0. Closest was {closestToZero:F3} units away");
        return null;
    }

    public void UpdateSymbolSprite(Transform symbolTransform, SlotSymbolSO newSymbol)
    {
        var spriteRenderer = symbolTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && newSymbol != null)
        {
            spriteRenderer.sprite = newSymbol.sprite;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw win line position
        Gizmos.color = Color.yellow;
        Vector3 lineStart = transform.position + Vector3.left * 0.5f;
        Vector3 lineEnd = transform.position + Vector3.right * 0.5f;
        Gizmos.DrawLine(lineStart, lineEnd);
    }
}