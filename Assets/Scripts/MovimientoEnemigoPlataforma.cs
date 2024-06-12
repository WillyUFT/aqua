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
    private float distanciaPared;
    [SerializeField]
    private float anchoPared;
    private Vector2 direccionRaycastPared;

    [SerializeField]
    private bool movimientoDerecha;

    private Rigidbody2D rb;
    private Animator animator;

    private EnemyDmg enemyDmg;
    public LayerMask piso;

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
            // * Detección piso
            RaycastHit2D informacionSuelo = Physics2D.Raycast(
                controladorSuelo.position,
                Vector2.down,
                distancia,
                piso
            );

            // Verificar si hay una pared en la dirección del movimiento
            direccionRaycastPared = movimientoDerecha ? Vector2.right : Vector2.left;
            RaycastHit2D informacionPared = Physics2D.BoxCast(
                controladorSuelo.position,
                new Vector2(anchoPared, distancia),
                0,
                direccionRaycastPared,
                distanciaPared,
                piso
            );
            rb.velocity = new Vector2(velocidadMovimientoActual, rb.velocity.y);

            if (!informacionSuelo.collider || informacionPared.collider)
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
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Vector3 startPositionSuelo = controladorSuelo.position;
            Vector3 endPositionSuelo = controladorSuelo.position + Vector3.down * distancia;
            Gizmos.DrawLine(startPositionSuelo, endPositionSuelo);

            Gizmos.color = Color.blue;
            Vector3 startPositionPared = controladorSuelo.position;
            Vector3 endPositionPared = movimientoDerecha
                ? controladorSuelo.position + Vector3.right * distanciaPared
                : controladorSuelo.position + Vector3.left * distanciaPared;
            Gizmos.DrawWireCube(startPositionPared + (Vector3)(direccionRaycastPared * distanciaPared / 2), new Vector3(distanciaPared, distancia, 1));
        }
    }

    private IEnumerator HacerIdle()
    {
        animator.SetBool("walk", false);
        yield return new WaitForSeconds(1f);
        animator.SetBool("walk", true);
        velocidadMovimientoActual = velocidadMovimiento * (movimientoDerecha ? -1 : 1);
    }
}
