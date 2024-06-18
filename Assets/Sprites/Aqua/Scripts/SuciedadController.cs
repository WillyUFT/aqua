using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuciedadController : MonoBehaviour
{
    [SerializeField] public float vidaMaximaSuciedad;
    public float vidaActualSuciedad;

    [SerializeField]
    public BarraLimpieza barraLimpieza;

    [Header("Sonido")]
    [SerializeField]
    public AudioClip descubrirCamino;

    void Start()
    {
        vidaActualSuciedad = vidaMaximaSuciedad;
    }

    public void RecibirDmg(float dmg)
    {
        vidaActualSuciedad -= dmg;
        if (vidaActualSuciedad <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        SoundManager.instance.PlaySound(descubrirCamino);
        if (barraLimpieza != null)
        {
            barraLimpieza.recibirDmg(vidaMaximaSuciedad);
        }
        Destroy(gameObject);
    }

    public float GetVida()
    {
        return vidaActualSuciedad;
    }
}
