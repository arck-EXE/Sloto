using System.Collections;
using UnityEngine;

public class SlotMachineAnimator : MonoBehaviour
{
    [Header("Spin Settings")]
    public Reel[] reels;
    public float spinTime = 3f; 
    public float spinSpeed = 500f; 

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
                reel.transform.localPosition += Vector3.up * delta;

                float upperLimit = (reel.symbolHeight + reel.spacing) * (reel.symbols.Count / 2f);

                foreach (var img in reel.symbols)
                {
                    RectTransform rt = img.rectTransform;
                    if (rt.anchoredPosition.y > upperLimit)
                        reel.RecycleTopSymbol();
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        //Snap reels to nearest symbol
        foreach (var reel in reels)
        {
            float step = reel.symbolHeight + reel.spacing;
            float offset = reel.transform.localPosition.y % step;
            reel.transform.localPosition -= new Vector3(0, offset, 0);
        }

        isSpinning = false;
    }
}
