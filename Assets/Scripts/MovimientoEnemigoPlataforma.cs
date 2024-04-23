using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoEnemigoPlataforma : MonoBehaviour
{
    [SerializeField]
    private float velocidadMovimiento;

    [SerializeField]
    private float velocidadMovimientoActual;

    [SerializeField]
    private Transform controladorSuelo;

    [SerializeField]
    private float distancia;

    [SerializeField]
    private bool movimientoDerecha;

    private Rigidbody2D rb;
    private Animator animator;
    private EnemyDmg enemyDmg;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        velocidadMovimientoActual = velocidadMovimiento;
        enemyDmg = GetComponent<EnemyDmg>();
    }

    void FixedUpdate()
    {
        if (!enemyDmg.Perdidacontrol && !enemyDmg.Golpeado)
        {
            RaycastHit2D informacionSuelo = Physics2D.Raycast(
                controladorSuelo.position,
                Vector2.down,
                distancia
            );
            rb.velocity = new Vector2(velocidadMovimientoActual, rb.velocity.y);

            if (!informacionSuelo)
            {
                Girar();
            }
        }
    }

    private void Girar()
    {
        movimientoDerecha = !movimientoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidadMovimientoActual = 0;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine("HacerIdle");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position);
    }

    private IEnumerator HacerIdle()
    {
        animator.SetBool("walk", false);
        yield return new WaitForSeconds(1f);
        animator.SetBool("walk", true);
        velocidadMovimientoActual = velocidadMovimiento * (movimientoDerecha ? -1 : 1);
    }
}
