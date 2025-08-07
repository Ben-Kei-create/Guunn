using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    private List<Dice> ownedDices = new List<Dice>();

    public void AddDice(Dice dice)
    {
        if (!ownedDices.Contains(dice))
        {
            ownedDices.Add(dice);
            Debug.Log("ダイス " + dice.diceName + " を入手");
        }
        else
        {
            Debug.Log("既に " + dice.diceName + " を所持しています");
        }
    }

    public bool HasDice(Dice dice)
    {
        return ownedDices.Contains(dice);
    }

    // その他の所持品管理ロジック（お金など）
}