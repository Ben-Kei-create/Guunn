using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 重要：UIイベント用

public class DiseShopEnterButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("DiseShopScene");
    }
}
