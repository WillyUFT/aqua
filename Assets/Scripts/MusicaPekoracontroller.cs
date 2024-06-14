using System.Collections;
using UnityEngine;

public class MusicaPekoracontroller : MonoBehaviour
{
    [Header("Música")]
    [SerializeField] AudioClip musica;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    public void CambiarMusicaConFade(float fadeDuration)
    {
        StartCoroutine(FadeOutAndChangeMusic(fadeDuration));
    }

    private IEnumerator FadeOutAndChangeMusic(float fadeDuration)
    {
        // Guardamos el volumen original
        float startVolume = audioSource.volume;

        // Reducimos el volumen gradualmente
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Aseguramos que el volumen sea cero
        audioSource.volume = 0;

        // Cambiamos la música y la reproducimos
        audioSource.clip = musica;
        audioSource.Play();

        // Restauramos el volumen original gradualmente
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        // Aseguramos que el volumen sea el original
        audioSource.volume = startVolume;
    }
}
