using UnityEngine;

[CreateAssetMenu(fileName = "SymbolData", menuName = "SlotMachine/Symbol")]
public class SlotSymbolSO : ScriptableObject
{
    public string symbolName;
    public Sprite sprite;
    public int payoutValue; 
}