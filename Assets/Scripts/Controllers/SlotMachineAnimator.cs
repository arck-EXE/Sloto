using System.Collections;
using UnityEngine;

public class SlotMachineAnimator : MonoBehaviour
{
    [Header("Spin Settings")]
    public Reel[] reels;
    public float spinTime = 3f;
    public float spinSpeed = 5f;

    private bool isSpinning = false;

    public void StartSpin()
    {
        if (!isSpinning)
            StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        float elapsed = 0f;

        while (elapsed < spinTime)
        {
            float delta = spinSpeed * Time.deltaTime;

            foreach (var reel in reels)
            {
                // Move reel up
                foreach (var symbol in reel.symbols)
                {
                    symbol.transform.localPosition += Vector3.up * delta;
                    
                    // Check if symbol needs recycling
                    float upperLimit = (reel.symbolHeight + reel.spacing) * 2;
                    if (symbol.transform.localPosition.y > upperLimit)
                    {
                        reel.RecycleSymbol(symbol);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to grid
        foreach (var reel in reels)
        {
            foreach (var symbol in reel.symbols)
            {
                float step = reel.symbolHeight + reel.spacing;
                float offset = symbol.transform.localPosition.y % step;
                symbol.transform.localPosition = new Vector3(
                    symbol.transform.localPosition.x,
                    symbol.transform.localPosition.y - offset,
                    symbol.transform.localPosition.z
                );
            }
        }

        isSpinning = false;
    }
    
    public bool IsSpinning()
    {
        return isSpinning;
    }
}
