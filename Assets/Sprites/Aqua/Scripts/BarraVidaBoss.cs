using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBoss : MonoBehaviour
{
    private Slider slider;
    [Header("Vida")]
    private float vidaMaxima;
    public float vidaActual;
    private Portrait portraitController;
    private PekoraController pekoraController;

    void Start()
    {
        vidaActual = vidaMaxima;
        slider = GetComponent<Slider>();
        portraitController = GetComponentInChildren<Portrait>();
        pekoraController = GetComponent<PekoraController>();
    }

    public void SetVidaInicial(float vida)
    {
        vidaMaxima = vida;
        vidaActual = vidaMaxima;
    }

    public void ActualizarVida()
    {
        slider.value = vidaActual / vidaMaxima;
    }

    public void recibirDmg(float dmg)
    {
        if (vidaActual - dmg > 0)
        {
            vidaActual -= dmg;
            ActualizarVida();
            portraitController.ImagenDmg();
        }
        else
        {
            vidaActual = 0;
            ActualizarVida();
            // Morir();
            portraitController.ImagenMuerto();
        }
    }

    public void Morir()
    {
        pekoraController.animator.SetTrigger("morir");
        pekoraController.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        pekoraController.rigidBody.isKinematic = true;
    }
}
