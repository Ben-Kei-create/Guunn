using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
