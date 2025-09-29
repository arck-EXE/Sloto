using UnityEngine;
using TMPro;
using System.Collections;

public class SlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI resultText;

    private void Start()
    {
        UpdateBalance(0);
        resultText.gameObject.SetActive(false);
    }

    public void UpdateBalance(int balance)
    {
        balanceText.text = $"Balance: $ {balance:N0} ";
    }

    public void ShowWin(int amount, string symbolName, int matchCount)
    {
        StopAllCoroutines();
        resultText.gameObject.SetActive(true);
        resultText.color = Color.yellow;
        resultText.text = $"WIN!\n{matchCount}x $ {symbolName}\n+{amount} ";
        StartCoroutine(HideResultAfterDelay());
    }

    public void ShowLoss(int betAmount)
    {
        StopAllCoroutines();
        resultText.gameObject.SetActive(true);
        resultText.color = Color.red;
        resultText.text = $"No Win\n-{betAmount} coins";
        StartCoroutine(HideResultAfterDelay());
    }

    private IEnumerator HideResultAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        resultText.gameObject.SetActive(false);
    }
}
