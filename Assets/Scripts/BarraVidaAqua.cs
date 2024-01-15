using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaAqua : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float vidaMaxima;
    public float vidaActual;

    [SerializeField] private Slider slider;

    [SerializeField] private PlayerCombatController playerCombatController;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private MenuGameOver menuGameOver;

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
        if (vidaActual - dmg > 0)
        {
            vidaActual -= dmg;
            ActualizarVida();
        }
        else
        {
            vidaActual = 0;
            ActualizarVida();
            Morir();
        }
    }

    public void Morir()
    {
        playerCombatController.animator.SetTrigger("die");
        playerCombatController.PerderControl();
        playerController.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
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
}
