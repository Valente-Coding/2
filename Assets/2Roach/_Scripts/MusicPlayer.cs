using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip song1;
    public AudioClip song2;
    public float fadeDuration = 1.0f;

    private AudioSource audioSource;

    public static MusicPlayer instance;

    void Awake()
    {

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = song1;
        audioSource.Play();
    }

    public void PlayNextSong()
    {
        var songToPlay = song1;
        if(audioSource.clip == song1) songToPlay = song2;
        if(audioSource.clip == song2) songToPlay = song1;
        StartCoroutine(COR_FadeOutAndPlay(songToPlay));
    }

    private IEnumerator COR_FadeOutAndPlay(AudioClip nextClip)
    {
        float startVolume = audioSource.volume;
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0.0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;

        audioSource.clip = nextClip;
        audioSource.Play();
    }
}