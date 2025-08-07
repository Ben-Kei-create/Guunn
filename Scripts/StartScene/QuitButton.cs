using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Application.Quit();
        Debug.Log("ゲーム終了（エディタ上では無効）");
    }
}
