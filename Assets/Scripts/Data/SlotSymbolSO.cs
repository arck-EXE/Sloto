using UnityEngine;

[CreateAssetMenu(fileName = "SymbolData", menuName = "SlotMachine/Symbol")]
public class SlotSymbolSO : ScriptableObject
{
    public string symbolName;
    public Sprite sprite;
    [Min(1)] public int weight = 1;  // Add default and minimum value
    public int payout2;
    public int payout3;

    private void OnValidate()
    {
        if (weight <= 0) weight = 1;
        if (string.IsNullOrEmpty(symbolName) && sprite != null)
        {
            symbolName = sprite.name;
        }
    }
}