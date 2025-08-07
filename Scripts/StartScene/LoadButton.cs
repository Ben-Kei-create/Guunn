using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
