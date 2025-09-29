using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    [Header("Reel Settings")]
    public List<SlotSymbolSO> availableSymbols;
    public GameObject symbolPrefab;
    public int numberOfSymbols = 10;
    public int visibleCount = 3;
    public float symbolHeight = 1f;
    public float spacing = 0.1f;
    public float maxSpinSpeed = 3f;

    [HideInInspector] public List<SymbolHolder> symbols = new List<SymbolHolder>();
    [HideInInspector] public SlotSymbolSO[] visibleSymbols;

    void Start()
    {
        SpawnInitialSymbols();
    }

    void SpawnInitialSymbols()
    {
        if (symbolPrefab == null)
        {
            Debug.LogError($"Symbol prefab is not assigned on {gameObject.name}!");
            return;
        }

        if (availableSymbols == null || availableSymbols.Count == 0)
        {
            Debug.LogError($"No available symbols assigned on {gameObject.name}!");
            return;
        }

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        symbols.Clear();
        visibleSymbols = new SlotSymbolSO[visibleCount];

        float startY = (numberOfSymbols / 2f) * (symbolHeight + spacing);

        for (int i = 0; i < numberOfSymbols; i++)
        {
            SlotSymbolSO symbolData = GetRandomWeightedSymbol();
            GameObject symbolGO = Instantiate(symbolPrefab, transform);
            
            // Ensure SpriteRenderer exists
            SpriteRenderer renderer = symbolGO.GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = symbolGO.AddComponent<SpriteRenderer>();
            }
            
            // Position symbol
            symbolGO.transform.localPosition = new Vector3(0, startY - (i * (symbolHeight + spacing)), 0);
            
            // Setup SymbolHolder
            SymbolHolder holder = symbolGO.GetComponent<SymbolHolder>();
            if (holder == null)
            {
                holder = symbolGO.AddComponent<SymbolHolder>();
            }
            
            holder.SetSymbol(symbolData);
            symbols.Add(holder);
            
            Debug.Log($"Spawned symbol {symbolData.symbolName} at position {symbolGO.transform.localPosition}");
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

        return availableSymbols[0]; // fallback
    }

    public void SetVisibleSymbols(SlotSymbolSO[] resultSymbols)
    {
        visibleSymbols = resultSymbols;
        for (int i = 0; i < visibleCount; i++)
        {
            symbols[i].SetSymbol(visibleSymbols[i]);
        }
    }

    public SlotSymbolSO GetVisibleSymbol(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= visibleCount) return null;
        return visibleSymbols[rowIndex];
    }

    public void RecycleSymbol(SymbolHolder symbol)
    {
        float bottomY = -((symbolHeight + spacing) * (numberOfSymbols - 1));
        symbol.transform.localPosition = new Vector3(0, bottomY, 0);
        
        // Assign new random symbol
        SlotSymbolSO newSymbol = GetRandomWeightedSymbol();
        symbol.SetSymbol(newSymbol);
    }
}
