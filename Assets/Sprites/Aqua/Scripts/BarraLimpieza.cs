using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BarraLimpieza : MonoBehaviour
{

    private Slider slider;
    [Header("Vida")]
    public float vidaMaxima;
    public float vidaActual;
    private PekoraController pekoraController;

    void Start()
    {
        vidaActual = vidaMaxima;
        slider = GetComponent<Slider>();
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
        }
        else
        {
            vidaActual = 0;
            ActualizarVida();
        }
    }
}
