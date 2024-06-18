using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarAtaque : MonoBehaviour
{

    [SerializeField]
    GameObject ataqueController;

    AquaProyectil aquaProyectil;

    public void DesactivarHitbox()
    {
        ataqueController.SetActive(false);
    }


    private void Start()
    {
        aquaProyectil = GameObject.FindAnyObjectByType<AquaProyectil>();
    }

    public void DesactivarAtaques()
    {
        aquaProyectil.SetAtacando(false);
    }

}
