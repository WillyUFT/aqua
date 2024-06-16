using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudiosPekoraGolpe : MonoBehaviour
{

    [SerializeField] AudioClip[] audiosPekora;
    private AudioSource audioSource;
    private int ultimoIndice = -1;
    PekoraController pekoraController;
    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        pekoraController = GetComponent<PekoraController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.tag == "Player" &&
        (pekoraController != null && pekoraController.atacandoEspada))
        {
            ReproducirAudioAleatorio();
        }

    }

    private void ReproducirAudioAleatorio()
    {
        float probabilidad = UnityEngine.Random.Range(0f, 1f);

        if (probabilidad <= 0.5f)
        {
            int indiceAleatorio;
            do
            {
                indiceAleatorio = UnityEngine.Random.Range(0, audiosPekora.Length);
            } while (indiceAleatorio == ultimoIndice);

            AudioClip audioAleatorio = audiosPekora[indiceAleatorio];
            SoundManager.instance.PlaySound(audioAleatorio);

            ultimoIndice = indiceAleatorio;
        }
    }
}
