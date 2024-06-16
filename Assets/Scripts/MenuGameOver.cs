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
    private GameObject fade;

    public void ActivarMenu()
    {
        fade.SetActive(false);
        menuGameOver.SetActive(true);
    }

    public void Start()
    {
        fade = gameObject.transform.Find("Fade").gameObject;
    }

    public void Reiniciar()
    {
        playerController.Respawn();
        playerController.Reiniciar();
    }

}
