using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //* -------------------------------------------------------------------------- */
    //*                                  Variables                                 */
    //* -------------------------------------------------------------------------- */
    private Rigidbody2D rigidBody;

    //^ ------------------------------- Movimiento ------------------------------- */
    [Header("Movimiento")]
    public float velocidadMovimiento = 10f;
    private Vector2 direccion;
    private bool puedeMoverse = true;

    [Header("Salto")]
    public float fuerzaSalto = 5f;
    public float aceleracionCaida = 2.5f;
    public float aceleracionSalto = 2.0f;
    public Vector2 pies;
    public float distanciaUmbral = 1.25f;

    [Header("Colisiones")]
    public float radioColision;
    public bool enSuelo = true;
    public LayerMask layerPiso;

    [Header("Dash")]
    public float velocidadDash = 15f;
    public float tiempoDash = 0.3f;
    public bool puedeDashear = true;
    public float gravedadInicial;

    [SerializeField]
    TrailRenderer trailRenderer;

    //^ ------------------------------- Animaciones ------------------------------ */
    [Header("Animaciones")]
    public Animator animator;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gravedadInicial = rigidBody.gravityScale;
    }

    void Update()
    {
        AgarreSalto();
        Movimiento();
        MejorarSalto();
        Saltar();
        NoCaer();
        DefinirSalto();
        if (Input.GetButtonDown("Dash") && puedeDashear)
        {
            StartCoroutine(EjecutarDash());
        }
    }

    //* -------------------------------------------------------------------------- */
    //*                                    Salto                                   */
    //* -------------------------------------------------------------------------- */

    private void DefinirSalto()
    {
        float velocidad;
        if (rigidBody.velocity.y > 0)
        {
            velocidad = 1;
        }
        else
        {
            velocidad = -1;
        }

        if (!enSuelo)
        {
            animator.SetFloat("velocidadVertical", velocidad);
        }
        else
        {
            if (velocidad == -1)
            {
                FinalizarSalto();
            }
        }
    }

    public void FinalizarSalto()
    {
        if (EstoyCercaDelSuelo())
        {
            animator.SetBool("jump", false);
        }
    }

    private void Saltar()
    {
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            animator.SetBool("jump", true);
            //* Establece la velocidad vertical del Rigidbody a 0.
            //* Esto es útil para normalizar la velocidad antes de aplicar la fuerza de salto.
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            // * Acá se aplica el salto al rigidbody
            rigidBody.velocity += Vector2.up * fuerzaSalto;
        }
    }

    private void MejorarSalto()
    {
        // * En caso de que estemos cayendo (velocidad negativa)
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity +=
                Vector2.up * Physics2D.gravity.y * (aceleracionCaida - 1) * Time.deltaTime;
        }
        // * En caso de que estemos saltando (velocidad positiva)
        else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidBody.velocity +=
                Vector2.up * Physics2D.gravity.y * (aceleracionSalto - 1) * Time.deltaTime;
        }
    }

    private void AgarreSalto()
    {
        enSuelo = Physics2D.OverlapCircle(
            (Vector2)transform.position + pies,
            radioColision,
            layerPiso
        );
    }

    void OnDrawGizmos()
    {
        // Configurar el color del Gizmo
        Gizmos.color = Color.blue;

        // Dibujar un círculo en la posición donde se verifica el suelo
        Gizmos.DrawWireSphere((Vector2)transform.position + pies, radioColision);
    }

    private void NoCaer()
    {
        if (enSuelo)
        {
            animator.SetBool("fall", false);
        }
    }

    private bool EstoyCercaDelSuelo()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            distanciaUmbral,
            layerPiso
        );
        return hit.collider != null;
    }

    //* -------------------------------------------------------------------------- */
    //*                            Movimiento horizontal                           */
    //* -------------------------------------------------------------------------- */
    private void Movimiento()
    {
        // * Obtenemos la posición del eje X y Y
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // * Creamos la dirección con los inputs anteriores
        direccion = new Vector2(x, y);
        caminar();
    }

    private void caminar()
    {
        if (puedeMoverse)
        {
            rigidBody.velocity = new Vector2(
                direccion.x * velocidadMovimiento,
                rigidBody.velocity.y
            );
            cambiarDireccion();
        }
    }

    private void cambiarDireccion()
    {
        // * Verificamos que nos estemos moviendo
        if (direccion != Vector2.zero)
        {
            if (!enSuelo)
            {
                animator.SetBool("jump", true);
            }
            else
            {
                animator.SetBool("walk", true);
            }

            // * Si nos movemos hacia la izquierda, pero el personaje mira hacia la derecha
            if (direccion.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
            else if (direccion.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    //* -------------------------------------------------------------------------- */
    //*                                   Dashes                                   */
    //* -------------------------------------------------------------------------- */

    private IEnumerator EjecutarDash()
    {
        puedeMoverse = false;
        puedeDashear = false;
        rigidBody.gravityScale = 0;
        rigidBody.velocity = new Vector2(velocidadDash * Mathf.Sign(transform.localScale.x), 0);
        animator.SetTrigger("dash");
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(tiempoDash);

        puedeMoverse = true;
        puedeDashear = true;
        rigidBody.gravityScale = gravedadInicial;
        trailRenderer.emitting = false;
    }
}
