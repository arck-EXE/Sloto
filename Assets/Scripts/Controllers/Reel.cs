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
        SymbolHolder closestSymbol = null;
        float closestDistance = float.MaxValue;

        foreach (Transform child in transform)
        {
            float distanceToWinLine = Mathf.Abs(child.localPosition.y);
            if (distanceToWinLine < closestDistance)
            {
                closestDistance = distanceToWinLine;
                closestSymbol = child.GetComponent<SymbolHolder>();
            }
        }

        return (closestDistance <= 0.1f) ? closestSymbol : null;
    }

    public void UpdateSymbolSprite(Transform symbolTransform, SlotSymbolSO newSymbol)
    {
        var spriteRenderer = symbolTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && newSymbol != null)
        {
            spriteRenderer.sprite = newSymbol.sprite;
        }
    }
}