using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    [SerializeField] private GameObject menuGameOver;

    [SerializeField] private BarraVidaAqua barraVidaAqua;
    [SerializeField] private PlayerController playerController;

    public void ActivarMenu()
    {
        menuGameOver.SetActive(true);
    }

    public void Start()
    {
    }

    public void Reiniciar()
    {
        playerController.Respawn();
        playerController.Reiniciar();
    }

}
