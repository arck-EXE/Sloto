using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    [Header("Reel Settings")]
    public List<SlotSymbolSO> availableSymbols;
    public GameObject symbolPrefab;
    public int numberOfSymbols = 10;
    public float symbolHeight = 100f;
    public float spacing = 10f; // spacing between symbols
    public float maxSpinSpeed = 300f;

    [HideInInspector] public List<Image> symbols = new List<Image>();

    void Start()
    {
        SpawnInitialSymbols();
    }

    void SpawnInitialSymbols()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        symbols.Clear();

        for (int i = 0; i < numberOfSymbols; i++)
        {
            SlotSymbolSO symbolData = GetRandomSymbol();
            GameObject symbolGO = Instantiate(symbolPrefab, transform);
            Image img = symbolGO.GetComponent<Image>();
            img.sprite = symbolData.sprite;

            // place in vertical order
            RectTransform rt = symbolGO.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i * (symbolHeight + spacing));

            symbols.Add(img);
        }
    }

    SlotSymbolSO GetRandomSymbol()
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

    public void RecycleTopSymbol()
    {
        if (symbols.Count == 0) return;

        // Move the top symbol to the bottom
        Image top = symbols[0];
        symbols.RemoveAt(0);
        symbols.Add(top);

        RectTransform rt = top.rectTransform;
        float bottomY = symbols[symbols.Count - 2].rectTransform.anchoredPosition.y - (symbolHeight + spacing);
        rt.anchoredPosition = new Vector2(0, bottomY);

        // Assign a new random sprite
        top.sprite = GetRandomSymbol().sprite;
    }

    public IEnumerator SpinRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float speed = Mathf.Lerp(maxSpinSpeed, 40f, t);

            foreach (var img in symbols)
            {
                RectTransform rt = img.rectTransform;
                rt.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

                // recycle symbol if it goes below the bottom
                if (rt.anchoredPosition.y < - (symbolHeight + spacing) * (numberOfSymbols - 1))
                {
                    rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y + (symbolHeight + spacing) * numberOfSymbols);
                    img.sprite = GetRandomSymbol().sprite;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // snap-align to nearest row
        float offset = symbols[0].rectTransform.anchoredPosition.y % (symbolHeight + spacing);
        foreach (var img in symbols)
        {
            RectTransform rt = img.rectTransform;
            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y - offset);
        }
    }
}
