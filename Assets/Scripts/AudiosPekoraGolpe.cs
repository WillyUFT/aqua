using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudiosPekoraGolpe : MonoBehaviour
{

    [SerializeField] AudioClip[] audiosPekora;
    private AudioSource audioSource;
    private int ultimoIndice = -1;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PekoraController pekoraController = other.collider.GetComponent<PekoraController>();


        if (other.collider.tag == "proyectilPekora" &&
        (pekoraController != null && pekoraController.atacandoEspada))
        {
            ReproducirAudioAleatorio();
        }

    }

    private void ReproducirAudioAleatorio()
    {
        float probabilidad = UnityEngine.Random.Range(0f, 1f);

        if (probabilidad <= 0.3f)
        {
            int indiceAleatorio;
            do
            {
                indiceAleatorio = UnityEngine.Random.Range(0, audiosPekora.Length);
            } while (indiceAleatorio == ultimoIndice);

            AudioClip audioAleatorio = audiosPekora[indiceAleatorio];
            audioSource.PlayOneShot(audioAleatorio);

            ultimoIndice = indiceAleatorio;
        }
    }
}
