using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator animator;

    public Rigidbody2D rigidBody;

    [Header("Movimiento")]
    public Transform jugador;

    [SerializeField] public float distanciaUmbral;

    [Header("Vida")]
    [SerializeField] private float vida;

    private bool puedeMoverse = true;
    private float distanciaJugador;

    public void setPuedeMoverse(bool valor)
    {
        puedeMoverse = valor;
    }

    public bool getPuedeMoverse()
    {
        return puedeMoverse;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        distanciaJugador = Vector2.Distance(transform.position, jugador.position);
        animator.SetFloat("distanciaJugador", distanciaJugador);
    }

    public float getDistanciaJugador()
    {
        return distanciaJugador;
    }

    public void MirarJugador()
    {
        float direccionHaciaJugador = jugador.position.x - transform.position.x;

        if (
            (direccionHaciaJugador > 0 && transform.localScale.x > 0)
            || (direccionHaciaJugador < 0 && transform.localScale.x < 0)
        )
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    public void IniciarExitIdleState()
    {
        StartCoroutine(ExitIdleState());
    }

    private IEnumerator ExitIdleState()
    {
        float waitTime = UnityEngine.Random.Range(1.5f, 2.0f);
        yield return new WaitForSeconds(waitTime);

        // Cambia a otro estado aquí
        // animator.SetBool("idle", true);
        animator.SetTrigger("idle");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distanciaUmbral);
    }
}
