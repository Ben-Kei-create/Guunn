using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Player Player { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Playerオブジェクトを生成してPlayerプロパティに格納
            GameObject playerObject = new GameObject("Player");
            Player = playerObject.AddComponent<Player>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // シーンアンロード時にシングルトンインスタンスをクリア（エディタでの問題を避けるため）
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // 必要に応じて、ゲーム全体でPlayerの状態にアクセスするためのメソッドを追加できます
    // 例：
    // public int GetCurrentHP() => Player.Stats.HP;
}