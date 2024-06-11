using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaAqua : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField]
    private float vidaMaxima;
    public float vidaActual;
    private Slider slider;

    [Header("Jugador")]
    [SerializeField]
    public PlayerCombatController playerCombatController;
    public PlayerCleaningController playerCleaningController;
    public PekoraController pekoraController;

    [SerializeField]
    private BarraLimpieza barraLimpieza;

    [SerializeField]
    public PlayerController playerController;

    private Portrait portraitController;

    [Header("Game Over")]
    [SerializeField]
    public MenuGameOver menuGameOver;


    void Start()
    {
        vidaActual = vidaMaxima;
        slider = GetComponent<Slider>();
        portraitController = GetComponentInChildren<Portrait>();
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
            Morir();
            portraitController.ImagenMuerto();
        }
    }

    public void Morir()
    {
        playerCombatController.animator.SetTrigger("die");
        playerCombatController.PerderControl();
        playerController.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        DesactivarMovimiento(false);
        pekoraController.SetNpc(true);
        StartCoroutine(ActivarMenuGameOver());
    }

    private IEnumerator ActivarMenuGameOver()
    {
        yield return new WaitForSeconds(1f);
        menuGameOver.ActivarMenu();
        SacarBarra();
    }

    public void SacarBarra()
    {
        gameObject.SetActive(false);
    }

    private void DesactivarMovimiento(bool valor)
    {
        playerController.activarDesactivarMovimiento(valor);
        playerController.SetSaltoBloqueado(!valor);
        playerCombatController.SetPuedeAtacar(valor);
        playerCombatController.SetPuedeBloquear(valor);
        playerCleaningController.SetPuedeLimpiar(valor);
    }
}
