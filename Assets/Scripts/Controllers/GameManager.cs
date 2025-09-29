//-----------------Ankur Gupta-----------------
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SlotMachineAnimator slotAnimator;
    public Reel[] reels;
    public int totalCoins = 1000;
    public int betAmount = 10;
    private bool spinning = false;
    public SlotUI uiManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !spinning)
            Spin();
    }

    private void Start()
    {
        uiManager.UpdateBalance(totalCoins);
    }

    public void Spin()
    {
        if (totalCoins >= betAmount)
        {
            totalCoins -= betAmount;
            uiManager.UpdateBalance(totalCoins);
            StartCoroutine(SpinRoutine());
        }
    }

    private IEnumerator SpinRoutine()
    {
        spinning = true;
        slotAnimator.StartSpin();
        yield return new WaitUntil(() => !slotAnimator.IsSpinning());
        yield return new WaitForSeconds(0.2f);
        CheckWin();
        spinning = false;
    }

    private void CheckWin()
    {
        Debug.Log($"<color=cyan>━━━━━━ Checking Win ━━━━━━</color>");
        SlotSymbolSO[] winningSymbols = new SlotSymbolSO[reels.Length];
        bool hasValidSymbols = true;

        // Get winning symbols from each reel
        for (int i = 0; i < reels.Length; i++)
        {
            SymbolHolder winner = reels[i].GetWinningSymbol();
            if (winner != null && winner.symbolData != null)
            {
                winningSymbols[i] = winner.symbolData;
                Debug.Log($"Reel {i + 1}: {winner.symbolData.symbolName}");
            }
            else
            {
                hasValidSymbols = false;
                Debug.LogError($"No valid symbol found on reel {i + 1}");
                break;
            }
        }

        if (!hasValidSymbols) return;

        // Calculate matches
        SlotSymbolSO firstSymbol = winningSymbols[0];
        int matchCount = 1;

        for (int i = 1; i < winningSymbols.Length; i++)
        {
            if (winningSymbols[i] == firstSymbol)
                matchCount++;
            else
                break;
        }

        // Calculate payout
        int payout = 0;
        if (matchCount == 2) payout = firstSymbol.payout2;
        else if (matchCount == 3) payout = firstSymbol.payout3;

        if (payout > 0)
        {
            totalCoins += payout;
            uiManager.ShowWin(payout, firstSymbol.symbolName, matchCount);
            uiManager.UpdateBalance(totalCoins);
        }
        else
        {
            uiManager.ShowLoss(betAmount);
        }
    }
}
