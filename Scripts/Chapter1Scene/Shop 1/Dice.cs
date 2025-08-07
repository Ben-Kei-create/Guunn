using UnityEngine;

[CreateAssetMenu(fileName = "NewDice", menuName = "Dice Shop/Dice")]
public class Dice : ScriptableObject
{
    public string diceName;
    [TextArea] public string description;
    [TextArea] public string effect;
    public int[] faces; // 出目を配列で管理
    public Sprite icon; // ダイスのアイコン（UI用）
    public int price;
}