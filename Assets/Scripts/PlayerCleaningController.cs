using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlayerCleaningController : MonoBehaviour
{
    //* -------------------------------------------------------------------------- */
    //*                                  Variables                                 */
    //* -------------------------------------------------------------------------- */

    [Header("Limpieza")]
    [SerializeField]
    private Collider2D[] overlapEscoba;

    [SerializeField]
    private Collider2D[] overlapPlumero;

    private bool puedeEliminarCaminoEscoba = false;
    private bool puedeEliminarCaminoPlumero = false;

    [Header("Animación")]
    [SerializeField]
    Animator animator;

    [SerializeField]
    private bool animacionActiva = false;

    [Header("Movimiento")]
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private bool saltando = false;

    [Header("Activación de la animación")]
    private float tiempoDesdeUltimaPresion = 0f;
    private readonly float tiempoMaximoEntrePresiones = 0.5f;

    [Header("Hitbox de la escoba")]
    [SerializeField]
    private float anchoHitBoxEscoba;

    [SerializeField]
    private float largoHitboxEscoba;

    [SerializeField]
    private float limpiezaPorSegundoEscoba;

    [SerializeField]
    public Vector2 escoba;

    [Header("Hitbox del plumero")]
    [SerializeField]
    private float anchoHitBoxPlumero;

    [SerializeField]
    private float largoHitboxPlumero;

    [SerializeField]
    private float limpiezaPorSegundoPlumero;

    [SerializeField]
    public Vector2 plumero;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("EL SCRIPT PAAA");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            saltando = playerController.GetSaltando();
        }
        else
        {
            Debug.LogError("Falta el playerController");
        }
        HandleLimpieza();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 tamanoEscoba = new Vector2(anchoHitBoxEscoba, largoHitboxEscoba);
        Gizmos.DrawWireCube((Vector2)transform.position + escoba, tamanoEscoba);

        Gizmos.color = Color.green;
        Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);
        Gizmos.DrawWireCube((Vector2)transform.position + plumero, tamanoPlumero);
    }

    private void HandleLimpieza()
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (inputVertical < 0 && !saltando)
        {
            Barrer();
        }
        else if (inputHorizontal != 0 && !saltando)
        {
            Sacudir();
        }
        else if (inputVertical > 0 && !saltando)
        {
            Ordenar();
        }
        else if (animacionActiva)
        {
            DetenerAnimacion();
        }
    }

    private void Barrer()
    {
        if (Input.GetButtonDown("Limpiar"))
        {
            tiempoDesdeUltimaPresion = 0f;

            if (!animacionActiva)
            {
                animator.SetBool("exitClean", false);
                IniciarAnimacion(1);
            }
            else
            {
                tiempoDesdeUltimaPresion = 0f;
            }
        }

        if (animacionActiva)
        {
            tiempoDesdeUltimaPresion += Time.deltaTime;
            if (tiempoDesdeUltimaPresion > tiempoMaximoEntrePresiones)
            {
                DetenerAnimacion();
            }
        }
    }

    public void PuedeBarrerPisoFake()
    {
        Vector2 tamanoEscoba = new Vector2(anchoHitBoxEscoba, largoHitboxEscoba);
        overlapEscoba = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + escoba,
            tamanoEscoba,
            0
        );

        foreach (Collider2D collider in overlapEscoba)
        {
            Debug.Log(collider.name);
            if (collider.CompareTag("suciedadCamino"))
            {
                puedeEliminarCaminoEscoba = true;
                EliminarCaminoFake(collider.gameObject);
            }
        }
    }

    private void EliminarCaminoFake(GameObject collider)
    {
        Destroy(collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("suciedad") && animacionActiva)
        {
            SuciedadController suciedad = other.GetComponent<SuciedadController>();
            if (suciedad != null)
            {
                suciedad.RecibirDmg(limpiezaPorSegundoEscoba * Time.deltaTime);
            }
        }
    }

    private void Sacudir()
    {
        if (Input.GetButtonDown("Limpiar"))
        {
            tiempoDesdeUltimaPresion = 0f;

            if (!animacionActiva)
            {
                animator.SetBool("exitClean", false);
                IniciarAnimacion(0);
            }
            else
            {
                tiempoDesdeUltimaPresion = 0f;
            }
        }

        if (animacionActiva)
        {
            tiempoDesdeUltimaPresion += Time.deltaTime;
            if (tiempoDesdeUltimaPresion > tiempoMaximoEntrePresiones)
            {
                DetenerAnimacion();
            }
        }
    }

    public void PuedeSacudirParedFake()
    {
        Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);
        overlapPlumero = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + plumero,
            tamanoPlumero,
            0
        );
        foreach (Collider2D collider in overlapPlumero)
        {
            if (collider.CompareTag("suciedadCamino"))
            {
                puedeEliminarCaminoPlumero = true;
                EliminarCaminoFake(collider.gameObject);
            }
        }
    }

    private void Ordenar()
    {
        if (Input.GetButtonDown("Limpiar"))
        {
            tiempoDesdeUltimaPresion = 0f;

            if (!animacionActiva)
            {
                animator.SetBool("exitClean", false);
                IniciarAnimacion(0.5f);
            }
            else
            {
                tiempoDesdeUltimaPresion = 0f;
            }
        }

        if (animacionActiva)
        {
            tiempoDesdeUltimaPresion += Time.deltaTime;
            if (tiempoDesdeUltimaPresion > tiempoMaximoEntrePresiones)
            {
                DetenerAnimacion();
            }
        }
    }

    private void IniciarAnimacion(float tipoAnimacion)
    {
        playerController.SetPuedeMoverse(false);
        animacionActiva = true;
        animator.SetTrigger("clean");
        animator.SetFloat("tipoLimpieza", tipoAnimacion);
    }

    private void DetenerAnimacion()
    {
        animacionActiva = false;
        animator.SetBool("exitClean", true);
        playerController.SetPuedeMoverse(true);
    }
}
