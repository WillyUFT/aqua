using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBoss : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void ActualizarVida(float vidaActual, float vidaMaxima)
    {
        slider.value = vidaActual / vidaMaxima;
    }

    void Update()
    {
    }
}
