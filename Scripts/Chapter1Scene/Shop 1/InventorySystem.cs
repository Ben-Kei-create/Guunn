using System.Collections.Generic;
using UnityEngine;

// 所持品管理クラス（アイテムとダイスの両方を管理）
public class InventorySystem
{
    // アイテム
    private List<Item> items = new List<Item>();

    // ダイス
    private List<Dice> dices = new List<Dice>();

    // --- アイテム関連 ---
    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool UseItem(Item item)
    {
        if (items.Contains(item))
        {
            bool used = item.Use(Player.Instance);

            if (used && item.type == Item.ItemType.Consumable)
            {
                RemoveItem(item);
            }

            return used;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public List<Item> GetItemsByType(Item.ItemType type)
    {
        return items.FindAll(item => item.type == type);
    }

    public List<Item> GetItems()
    {
        return new List<Item>(items);
    }

    public void SetItems(List<Item> newItems)
    {
        items = new List<Item>(newItems);
    }

    // --- ダイス関連 ---
    public void AddDice(Dice dice)
    {
        if (!dices.Contains(dice))
        {
            dices.Add(dice);
            Debug.Log("ダイス " + dice.diceName + " を入手");
        }
        else
        {
            Debug.Log("既に " + dice.diceName + " を所持しています");
        }
    }

    public bool HasDice(Dice dice)
    {
        return dices.Contains(dice);
    }

    public List<Dice> GetOwnedDices()
    {
        return new List<Dice>(dices);
    }

    public void SetOwnedDices(List<Dice> newDices)
    {
        dices = new List<Dice>(newDices);
    }
}
