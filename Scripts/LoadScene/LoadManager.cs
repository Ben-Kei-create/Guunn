using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    private const string SAVED_SCENE_KEY = "SavedSceneName";
    private const string SAVED_NODE_KEY = "SavedNodeID"; // テキストアドベンチャー進行用ノードID

    void Start()
    {
        string savedScene = PlayerPrefs.GetString(SAVED_SCENE_KEY, "");
        int savedNodeID = PlayerPrefs.GetInt(SAVED_NODE_KEY, -1);

        if (!string.IsNullOrEmpty(savedScene))
        {
            Debug.Log("保存されたシーンに遷移: " + savedScene);
            // シーン遷移（フェード演出など入れても良い）
            StartCoroutine(LoadSavedScene(savedScene, savedNodeID));
        }
        else
        {
            Debug.LogWarning("保存データが存在しません。タイトルに戻る");
            SceneManager.LoadScene("StartScene");
        }
    }

    private System.Collections.IEnumerator LoadSavedScene(string sceneName, int nodeID)
    {
        yield return new WaitForSeconds(1f); // 演出のための待機
        SceneManager.LoadScene(sceneName);

        // 進行ノードIDはGameManagerなどで受け取って反映
        PlayerPrefs.SetInt("PendingLoadNodeID", nodeID); // 遷移後にGameManagerがこれを見る
    }
}
