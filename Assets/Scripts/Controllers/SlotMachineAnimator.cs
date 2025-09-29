using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineAnimator : MonoBehaviour
{
    [Header("Spin Settings")]
    public Reel[] reels;
    public float spinTime = 3f;
    public float spinSpeed = 5f;

    private bool isSpinning = false;
    private List<List<Transform>> reelSymbols = new List<List<Transform>>();

    void Start()
    {
        CacheReelSymbols();
    }

    void CacheReelSymbols()
    {
        reelSymbols.Clear();

        foreach (var reel in reels)
        {
            List<Transform> symbols = new List<Transform>();
            foreach (Transform child in reel.transform)
            {
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    symbols.Add(child);
                }
            }
            reelSymbols.Add(symbols);
            //reel.InitializeReel();
        }
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            CacheReelSymbols();
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float elapsed = 0f;

        while (elapsed < spinTime)
        {
            float delta = spinSpeed * Time.deltaTime;

            for (int reelIndex = 0; reelIndex < reels.Length; reelIndex++)
            {
                Reel reel = reels[reelIndex];
                List<Transform> symbols = reelSymbols[reelIndex];

                float step = reel.symbolHeight + reel.spacing;
                float upperLimit = step * (reel.numberOfSymbols / 2f);
                float lowerLimit = -upperLimit;

                foreach (Transform symbol in symbols)
                {
                    symbol.localPosition += Vector3.up * delta;

                    if (symbol.localPosition.y > upperLimit)
                    {
                        symbol.localPosition = new Vector3(0, lowerLimit, 0);

                        SlotSymbolSO newSymbol = reel.GetRandomWeightedSymbol();
                        reel.UpdateSymbolSprite(symbol, newSymbol);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap
        for (int reelIndex = 0; reelIndex < reels.Length; reelIndex++)
        {
            Reel reel = reels[reelIndex];
            float step = reel.symbolHeight + reel.spacing;

            foreach (Transform symbol in reelSymbols[reelIndex])
            {
                float currentY = symbol.localPosition.y;

                // Find nearest grid position
                float snappedY = Mathf.Round(currentY / step) * step;

                // Force exact zero if very close to center
                if (Mathf.Abs(currentY) < (step * 0.3f))
                {
                    snappedY = 0f;
                    Debug.Log($"Snapped symbol to exact zero on {reel.name}");
                }

                symbol.localPosition = new Vector3(0, snappedY, 0);
            }
        }

        isSpinning = false;
        Debug.Log($"<color=green>Spin complete - symbols snapped to grid</color>");
    }

    public bool IsSpinning() => isSpinning;
}
