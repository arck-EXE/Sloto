using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlotMachineAnimator slotAnimator;
    public Reel[] reels;
    public int winningLineIndex = 1;
    public int totalCoins = 0;
    private bool spinning = false;

    private SlotSymbolSO[] stoppedSymbols;

    public void Spin()
    {
        if (!spinning)
            StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        spinning = true;
        stoppedSymbols = new SlotSymbolSO[reels.Length];

        slotAnimator.StartSpin();
        yield return new WaitUntil(() => !slotAnimator.IsSpinning());

        Debug.Log("━━━━━━ SPIN RESULT ━━━━━━");
        string result = "Line: ";
        bool allSymbolsFound = true;

        for (int i = 0; i < reels.Length; i++)
        {
            SymbolHolder[] holders = reels[i].GetComponentsInChildren<SymbolHolder>();
            bool foundSymbol = false;

            foreach (var holder in holders)
            {
                if (holder != null && holder.isInWinningLine && holder.symbolData != null)
                {
                    stoppedSymbols[i] = holder.symbolData;
                    result += $"[{holder.symbolData.symbolName}] ";
                    foundSymbol = true;
                    break;
                }
            }

            if (!foundSymbol)
            {
                result += "[ERROR] ";
                allSymbolsFound = false;
                Debug.LogError($"No valid symbol found in winning line for reel {i + 1}");
            }
        }

        Debug.Log(result);
        
        if (allSymbolsFound)
        {
            CalculateResults();
        }
        else
        {
            Debug.LogError("Skipping win calculation due to missing symbols");
        }

        spinning = false;
    }

    private void CalculateResults()
    {
        // Calculate consecutive matches from left to right
        SlotSymbolSO firstSymbol = stoppedSymbols[0];
        int consecutiveCount = 1;

        for (int i = 1; i < stoppedSymbols.Length; i++)
        {
            if (stoppedSymbols[i] == firstSymbol)
                consecutiveCount++;
            else
                break;
        }

        // Calculate and display win/loss
        int payout = 0;
        if (consecutiveCount == 2) payout = firstSymbol.payout2;
        else if (consecutiveCount >= 3) payout = firstSymbol.payout3;

        Debug.Log($"━━━━━━ FINAL RESULT ━━━━━━");
        
        if (payout > 0)
        {
            totalCoins += payout;
            Debug.Log($"WIN!\n" +
                     $"Matching Symbol: {firstSymbol.symbolName}\n" +
                     $"Match Count: {consecutiveCount}\n" +
                     $"Payout: +{payout} coins\n" +
                     $"Total Balance: {totalCoins} coins");
        }
        else
        {
            Debug.Log($"No Win\nTotal Balance: {totalCoins} coins");
        }
    }
}