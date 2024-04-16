using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuciedadController : MonoBehaviour
{
    [SerializeField] public float vidaMaximaSuciedad;
    public float vidaActualSuciedad;

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
        Destroy(gameObject);
    }

    public float GetVida() {
        return vidaActualSuciedad;
    }
}
