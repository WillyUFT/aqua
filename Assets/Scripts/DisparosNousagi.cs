using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisparosNousagi : MonoBehaviour
{

    private EnemyDmg enemyDmg;

    [Header("Ataque misil")]
    [SerializeField] public GameObject misilPrefab;

    [Header("Sonido")]
    [SerializeField]
    private AudioClip coheteSonido;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();


    private void Start()
    {
        enemyDmg = GetComponent<EnemyDmg>();
        InvokeRepeating("LanzarMisilConFrecuenciaAleatoria", Random.Range(2f, 4f), Random.Range(2f, 4f));

    }

    public void LanzarMisilConFrecuenciaAleatoria()
    {
        LanzarMisil();
        CancelInvoke("LanzarMisilConFrecuenciaAleatoria");
        InvokeRepeating("LanzarMisilConFrecuenciaAleatoria", Random.Range(2f, 4f), Random.Range(2f, 4f));
    }

    public void LanzarMisil()
    {
        Coroutine coroutine = StartCoroutine(LanzarMisilesCoroutine());
        activeCoroutines.Add(coroutine);
    }

    public void StopAllActiveCoroutines()
    {
        foreach (Coroutine coroutine in activeCoroutines)
        {
            StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }

    private void OnDisable()
    {
        StopAllActiveCoroutines();
    }

    private IEnumerator LanzarMisilesCoroutine()
    {

        int anguloAleatorioEnZ = Random.Range(20, 80);
        if (transform.localScale.x < 0)
        {
            anguloAleatorioEnZ = -anguloAleatorioEnZ;
        }

        Instantiate(misilPrefab, transform.position, Quaternion.Euler(0, 0, anguloAleatorioEnZ));
        SoundManager.instance.PlaySound(coheteSonido);

        yield return new WaitForSeconds(0.5f);
    }

}
