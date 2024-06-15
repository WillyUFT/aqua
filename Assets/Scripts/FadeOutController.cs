using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si estás usando TextMeshPro
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

public class FadeOutController : MonoBehaviour
{
    [SerializeField] private GameObject endGameUI;

    Animator animator;
    private Camera mainCamera;
    [SerializeField]
    private GameObject[] objetosDesactivar;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    public void fadeout()
    {
        animator.Play("fadeOut");
    }

    public void DesactivarElementos()
    {

        DesactivarNousagis();

        foreach (var item in objetosDesactivar)
        {
            item.SetActive(false);
        }

        endGameUI.SetActive(true);

        animator.Play("fadeIn");

        StartCoroutine(recargarEscena());

    }

    private void DesactivarNousagis()
    {

        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("enemigo");


        foreach (GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }
    }

    public IEnumerator recargarEscena()
    {
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
