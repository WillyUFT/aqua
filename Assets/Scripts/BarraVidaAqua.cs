using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaAqua : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField]
    private float vidaMaxima;
    public float vidaActual;
    [SerializeField] private Slider slider;

    void Start()
    {
        vidaActual = vidaMaxima;        
    }

    public void ActualizarVida()
    {
        slider.value = vidaActual / vidaMaxima;
    }

    public void recibirDmg(float dmg)
    {
        vidaActual -= dmg;
        ActualizarVida();
    }
}
