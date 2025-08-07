using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// シリアライズ用のデータクラス
[Serializable]
public class PlayerData
{
    public PlayerStatsData stats;
    public List<Item> inventory;
    public List<Card> cards;
    public EquipmentData equipment;
    public string location;
    public List<StatusEffectData> statusEffects;
}

[Serializable]
public class PlayerStatsData
{
    public int hp;
    public int maxHp;
    public int mp;
    public int maxMp;
    public int exp;
    public int lvl;
    public int luck;
}

[Serializable]
public class EquipmentData
{
    public Item equippedDice;
    public Item equippedShield;
    public Item equippedOddTool;
}

[Serializable]
public class StatusEffectData
{
    public string effectId;
    public int duration;
    public float intensity;
}

// メインのプレイヤークラス - すべてのコンポーネントへの参照を持つ
public class Player : MonoBehaviour
{
    // シングルトンパターンの実装（オプション）
    public static Player Instance { get; private set; }

    // 基本ステータス - 頻繁に参照される値
    public PlayerStats Stats { get; private set; }

    // 所持品管理
    public InventorySystem Inventory { get; private set; }
    public CardCollection Cards { get; private set; }

    // 装備管理
    public EquipmentSystem Equipment { get; private set; }

    // 状態管理
    public LocationTracker Location { get; private set; }
    public StatusEffectManager StatusEffects { get; private set; }

    // 初期化とコンポーネント設定
    private void Awake()
    {
        // シングルトンの設定（オプション）
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        Stats = new PlayerStats();
        Inventory = new InventorySystem();
        Cards = new CardCollection();
        Equipment = new EquipmentSystem();
        Location = new LocationTracker();
        StatusEffects = new StatusEffectManager(this); // プレイヤー参照を渡す
        // 初期データのロード（必要に応じて）
        // LoadInitialData();
    }

    private void Start()
    {
        // 初期設定（必要に応じて）
    }

    private void Update()
    {
        // 毎フレーム更新が必要なら
        StatusEffects.UpdateEffects();
    }

    // ゲームデータのセーブ/ロード
    public PlayerData SaveData()
    {
        // 各コンポーネントからデータを集約
        return new PlayerData
        {
            stats = Stats.GetSerializableData(),
            inventory = Inventory.GetItems(),
            cards = Cards.GetCards(),
            equipment = Equipment.GetEquippedItems(),
            location = Location.CurrentLocation,
            statusEffects = StatusEffects.GetActiveEffects()
        };
    }

    public void LoadData(PlayerData data)
    {
        // 各コンポーネントにデータを分配
        Stats.LoadFromData(data.stats);
        Inventory.SetItems(data.inventory);
        Cards.SetCards(data.cards);
        Equipment.SetEquippedItems(data.equipment);
        Location.SetLocation(data.location);
        StatusEffects.SetEffects(data.statusEffects);
    }
}

// 基本ステータスを管理するクラス
public class PlayerStats
{
    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public int MP { get; private set; }
    public int MaxMP { get; private set; }
    public int EXP { get; private set; }
    public int LVL { get; private set; }
    public int Luck { get; private set; }

    // コンストラクタで初期値を設定
    public PlayerStats(int hp = 100, int mp = 50, int lvl = 1, int luck = 5)
    {
        MaxHP = hp;
        HP = MaxHP;
        MaxMP = mp;
        MP = MaxMP;
        LVL = lvl;
        EXP = 0;
        Luck = luck;
    }

    // HP操作メソッド
    public void TakeDamage(int damage)
    {
        HP = Mathf.Max(0, HP - damage);
    }

    public void Heal(int amount)
    {
        HP = Mathf.Min(MaxHP, HP + amount);
    }

    // MP操作メソッド
    public bool UseMp(int amount)
    {
        if (MP >= amount)
        {
            MP -= amount;
            return true;
        }
        return false;
    }

    public void RestoreMp(int amount)
    {
        MP = Mathf.Min(MaxMP, MP + amount);
    }

    // 経験値とレベルアップ
    public void AddExperience(int amount)
    {
        EXP += amount;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        int expNeeded = GetExpForNextLevel();

        if (EXP >= expNeeded)
        {
            EXP -= expNeeded;
            LevelUp();
            CheckLevelUp(); // 複数レベルアップの可能性をチェック
        }
    }

    private void LevelUp()
    {
        LVL++;

        // レベルアップ時のステータス上昇
        MaxHP += 10;
        HP = MaxHP;
        MaxMP += 5;
        MP = MaxMP;

        // 運のボーナスは時々増加
        if (LVL % 3 == 0)
        {
            Luck += 1;
        }
    }

    private int GetExpForNextLevel()
    {
        // 簡単な経験値曲線の例
        return LVL * 100;
    }

    // シリアライズ用のデータを取得
    public PlayerStatsData GetSerializableData()
    {
        return new PlayerStatsData
        {
            hp = HP,
            maxHp = MaxHP,
            mp = MP,
            maxMp = MaxMP,
            exp = EXP,
            lvl = LVL,
            luck = Luck
        };
    }

    // シリアライズされたデータからロード
    public void LoadFromData(PlayerStatsData data)
    {
        MaxHP = data.maxHp;
        HP = data.hp;
        MaxMP = data.maxMp;
        MP = data.mp;
        EXP = data.exp;
        LVL = data.lvl;
        Luck = data.luck;
    }
}

// アイテムの基本クラス
[Serializable]
public class Item
{
    public string id;
    public string name;
    public string description;
    public ItemType type;

    public enum ItemType
    {
        Consumable,
        Equipment,
        Key,
        Quest
    }

    // アイテム使用の基本実装
    public virtual bool Use(Player player)
    {
        // 派生クラスでオーバーライド
        return false;
    }
}

// 回復アイテムの例
[Serializable]
public class HealingItem : Item
{
    public int healAmount;

    public override bool Use(Player player)
    {
        player.Stats.Heal(healAmount);
        return true;
    }
}

// カードクラス（収集要素）
[Serializable]
public class Card
{
    public string id;
    public string name;
    public string description;
    public int rarity;
}


// カードコレクション管理クラス
public class CardCollection
{
    private List<Card> cards = new List<Card>();

    // カード追加
    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    // カードを探す
    public Card FindCard(string id)
    {
        return cards.Find(card => card.id == id);
    }

    // カードリストを取得（セーブ用）
    public List<Card> GetCards()
    {
        return new List<Card>(cards);
    }

    // カードリストを設定（ロード用）
    public void SetCards(List<Card> newCards)
    {
        cards = new List<Card>(newCards);
    }
}

// 装備スロットクラス
public class EquipmentSlot
{
    public string SlotName { get; private set; }
    public Item EquippedItem { get; private set; }

    public EquipmentSlot(string slotName)
    {
        SlotName = slotName;
    }

    // アイテムを装備
    public Item Equip(Item newItem)
    {
        Item previousItem = EquippedItem;
        EquippedItem = newItem;
        return previousItem; // 以前装備していたアイテムを返す
    }

    // 装備を外す
    public Item Unequip()
    {
        Item previousItem = EquippedItem;
        EquippedItem = null;
        return previousItem;
    }
}

// 装備管理クラス
public class EquipmentSystem
{
    public EquipmentSlot DiceSlot { get; private set; }
    public EquipmentSlot ShieldSlot { get; private set; }
    public EquipmentSlot OddToolSlot { get; private set; }

    public EquipmentSystem()
    {
        DiceSlot = new EquipmentSlot("Dice");
        ShieldSlot = new EquipmentSlot("Shield");
        OddToolSlot = new EquipmentSlot("OddTool");
    }

    // 装備の効果計算
    public Dictionary<string, int> CalculateEquipmentBonuses()
    {
        Dictionary<string, int> bonuses = new Dictionary<string, int>
        {
            { "attack", 0 },
            { "defense", 0 },
            { "luck", 0 }
        };

        // それぞれの装備品のボーナスを加算（実際の実装ではアイテム特性に応じて）
        AddItemBonuses(DiceSlot.EquippedItem, bonuses);
        AddItemBonuses(ShieldSlot.EquippedItem, bonuses);
        AddItemBonuses(OddToolSlot.EquippedItem, bonuses);

        return bonuses;
    }

    private void AddItemBonuses(Item item, Dictionary<string, int> bonuses)
    {
        // 装備品特性をボーナスに加算する実装
        // ここでは簡略化のため省略
    }

    // 装備データを取得（セーブ用）
    public EquipmentData GetEquippedItems()
    {
        return new EquipmentData
        {
            equippedDice = DiceSlot.EquippedItem,
            equippedShield = ShieldSlot.EquippedItem,
            equippedOddTool = OddToolSlot.EquippedItem
        };
    }

    // 装備データを設定（ロード用）
    public void SetEquippedItems(EquipmentData data)
    {
        DiceSlot.Equip(data.equippedDice);
        ShieldSlot.Equip(data.equippedShield);
        OddToolSlot.Equip(data.equippedOddTool);
    }
}

// 場所追跡クラス
public class LocationTracker
{
    public string CurrentLocation { get; private set; } = "Unknown";

    // 場所を設定
    public void SetLocation(string locationName)
    {
        CurrentLocation = locationName;
    }
}

// 状態異常クラス
[Serializable]
public class StatusEffect
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public int Duration { get; private set; }
    public float Intensity { get; private set; }

    public StatusEffect(string id, string name, int duration, float intensity)
    {
        Id = id;
        Name = name;
        Duration = duration;
        Intensity = intensity;
    }

    // 効果の適用
    public virtual void Apply(Player player)
    {
        // 派生クラスでオーバーライド
    }

    // 効果の更新（ターン経過など）
    public virtual bool Update()
    {
        Duration--;
        return Duration > 0; // 継続するかどうか
    }

    // シリアライズ用データを取得
    public StatusEffectData GetData()
    {
        return new StatusEffectData
        {
            effectId = Id,
            duration = Duration,
            intensity = Intensity
        };
    }
}

// 状態異常管理クラス
public class StatusEffectManager
{
    private Player player;
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public StatusEffectManager(Player player)
    {
        this.player = player;
    }

    // 状態異常を追加
    public void AddEffect(StatusEffect effect)
    {
        // 同じIDの効果がすでにある場合は上書き
        RemoveEffect(effect.Id);
        activeEffects.Add(effect);
        effect.Apply(player);
    }

    // 状態異常を削除
    public void RemoveEffect(string effectId)
    {
        activeEffects.RemoveAll(effect => effect.Id == effectId);
    }

    // 全ての状態異常を更新
    public void UpdateEffects()
    {
        List<StatusEffect> expiredEffects = new List<StatusEffect>();

        foreach (var effect in activeEffects)
        {
            bool isActive = effect.Update();
            if (!isActive)
            {
                expiredEffects.Add(effect);
            }
        }

        // 期限切れの効果を削除
        foreach (var effect in expiredEffects)
        {
            activeEffects.Remove(effect);
        }
    }

    // 活動中の状態異常データを取得（セーブ用）
    public List<StatusEffectData> GetActiveEffects()
    {
        List<StatusEffectData> effectsData = new List<StatusEffectData>();

        foreach (var effect in activeEffects)
        {
            effectsData.Add(effect.GetData());
        }

        return effectsData;
    }

    // 状態異常データを設定（ロード用）
    public void SetEffects(List<StatusEffectData> effectsData)
    {
        activeEffects.Clear();

        foreach (var data in effectsData)
        {
            // 実際の実装ではEffectFactoryなどから適切な効果を生成
            // ここでは簡略化
            // StatusEffect effect = EffectFactory.CreateEffect(data);
            // AddEffect(effect);
        }
    }
}