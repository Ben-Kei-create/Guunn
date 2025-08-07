using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 重要：UIイベント用

public class InnEnterButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("DiseShopScene");
    }
}
