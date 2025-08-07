using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceShop : MonoBehaviour
{
    public string shopName;
    public List<Dice> availableDices; // このショップで販売するダイスのリスト
    public GameObject diceItemPrefab; // ダイス情報を表示するプレハブ
    public Transform diceListContent; // ダイスリストの親オブジェクト
    public Text shopTitleText;
    public Text selectedDiceNameText;
    public Text selectedDiceDescriptionText;
    public Text selectedDiceEffectText;
    public Button buyButton;
    public Text messageText;

    private Dice selectedDice;
    private InventorySystem playerInventory; // Playerの所持品を管理するスクリプトへの参照

    void Start()
    {
        if (shopTitleText != null)
        {
            shopTitleText.text = shopName;
        }

        playerInventory = PlayerManager.Instance.Player.Inventory; // PlayerInventoryへの参照を取得
        UpdateDiceList();
        ClearSelection();

        buyButton.onClick.AddListener(BuySelectedDice);
    }

    void UpdateDiceList()
    {
        // 既存のアイテムをクリア
        foreach (Transform child in diceListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Dice dice in availableDices)
        {
            // まだ購入していないダイスのみ表示
            if (!playerInventory.HasDice(dice))
            {
                GameObject itemObject = Instantiate(diceItemPrefab, diceListContent);
                DiceShopItem item = itemObject.GetComponent<DiceShopItem>();
                if (item != null)
                {
                    item.SetDice(dice, this);
                }
            }
        }
    }

    public void SelectDice(Dice dice)
    {
        selectedDice = dice;
        selectedDiceNameText.text = dice.diceName;
        selectedDiceDescriptionText.text = dice.description;
        selectedDiceEffectText.text = dice.effect;
        buyButton.interactable = true;
    }

    void ClearSelection()
    {
        selectedDice = null;
        selectedDiceNameText.text = "";
        selectedDiceDescriptionText.text = "";
        selectedDiceEffectText.text = "";
        buyButton.interactable = false;
    }

    void BuySelectedDice()
    {
        if (selectedDice != null)
        {
            // Playerの所持金を確認し、購入処理を行う（仮実装）
            bool canBuy = true; // 仮
            if (canBuy)
            {
                playerInventory.AddDice(selectedDice);
                messageText.text = selectedDice.diceName + " を購入しました。";
                UpdateDiceList(); // リストを更新して購入済みのダイスを非表示にする
                ClearSelection();
            }
            else
            {
                messageText.text = "お金が足りません。";
            }
        }
        else
        {
            messageText.text = "購入するダイスを選択してください。";
        }
    }

    public void ExitShop()
    {
        // 前のシーンに戻るなどの処理
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreviousSceneName"); // シーン名を適宜変更
    }
}