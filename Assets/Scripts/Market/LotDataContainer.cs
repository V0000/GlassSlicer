using UnityEngine;

[CreateAssetMenu(fileName = "New Lot Data", menuName = "Custom/Lot Data")]
public class LotDataContainer : ScriptableObject
{
    public Sprite activeBackground;
    public int activeID;
    public int money;
    public Lot[] lots;
}