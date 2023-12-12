using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator animator;

    public Rigidbody2D rigidBody;

    [Header("Movimiento")]
    public Transform jugador;

    [SerializeField]
    public float distanciaUmbral;

    [Header("Vida")]
    [SerializeField]
    private float vida;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, distanciaUmbral);
    }
}
