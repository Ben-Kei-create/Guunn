using UnityEngine;
using UnityEngine.UI;

public class DiceShopItem : MonoBehaviour
{
    public Text diceNameText;
    public Image diceIcon;
    private Dice dice;
    private DiceShop shop;

    public void SetDice(Dice d, DiceShop s)
    {
        dice = d;
        shop = s;
        diceNameText.text = dice.diceName;
        if (diceIcon != null && dice.icon != null)
        {
            diceIcon.sprite = dice.icon;
        }
    }

    public void OnClickItem()
    {
        shop.SelectDice(dice);
    }
}