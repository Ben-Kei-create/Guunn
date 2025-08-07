using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    public void ChangeBackground(Sprite newSprite)
    {
        StartCoroutine(FadeTransition(newSprite));
    }

    private IEnumerator FadeTransition(Sprite newSprite)
    {
        float fadeTime = 1f;
        Color color = backgroundImage.color;

        // フェードアウト
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = 1 - t / fadeTime;
            backgroundImage.color = color;
            yield return null;
        }

        backgroundImage.sprite = newSprite;

        // フェードイン
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            color.a = t / fadeTime;
            backgroundImage.color = color;
            yield return null;
        }
    }
}
