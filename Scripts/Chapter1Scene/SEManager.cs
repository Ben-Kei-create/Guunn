using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
