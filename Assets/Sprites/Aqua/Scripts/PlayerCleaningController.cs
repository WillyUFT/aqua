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

    [SerializeField]
    public ZonaLimpieza zonaLimpieza;

    [SerializeField]
    public BarraLimpieza barraLimpieza;

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

    private bool puedeLimpiar = true;

    [Header("Sonido")]
    [SerializeField]
    public AudioClip barrido;

    [SerializeField]
    public AudioClip descubrirCamino;

    [Header("Pekora")]
    [SerializeField]
    public EnemyDmg enemyDmg;


    public void LimparEscoba()
    {
        SoundManager.instance.PlaySound(barrido);

        Vector2 tamanoEscoba = new Vector2(anchoHitBoxEscoba, largoHitboxEscoba);
        overlapEscoba = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + escoba,
            tamanoEscoba,
            0
        );

        foreach (Collider2D collider in overlapEscoba)
        {
            if (collider.CompareTag("suciedadCamino") || collider.CompareTag("suciedad"))
            {
                puedeEliminarCaminoEscoba = true;
                SuciedadController suciedad = collider.GetComponent<SuciedadController>();
                if (suciedad != null)
                {
                    float dmg = limpiezaPorSegundoEscoba;
                    Debug.Log("Haciendo daño a la suciedad (ESCOBA)");
                    suciedad.RecibirDmg(dmg);
                    Debug.Log("Vida máxima suciedad" + suciedad.vidaMaximaSuciedad);
                    Debug.Log("Vida actual suciedad" + suciedad.vidaActualSuciedad);
                }
            }
        }
    }

    public void LimpiarPlumero()
    {
        SoundManager.instance.PlaySound(barrido);

        Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);
        Vector2 posicionPlumero = (Vector2)transform.position + plumero;

        if (transform.localScale.x < 0)
        {
            posicionPlumero = (Vector2)transform.position - plumero;
        }

        overlapPlumero = Physics2D.OverlapBoxAll(
            posicionPlumero,
            tamanoPlumero,
            0
        );
        foreach (Collider2D collider in overlapPlumero)
        {
            if (collider.CompareTag("suciedadCamino") || collider.CompareTag("suciedad"))
            {
                puedeEliminarCaminoPlumero = true;
                SuciedadController suciedad = collider.GetComponent<SuciedadController>();
                if (suciedad != null)
                {
                    float dmg = limpiezaPorSegundoPlumero;
                    Debug.Log("Haciendo daño a la suciedad (PLUMERO)");
                    suciedad.RecibirDmg(dmg);
                    Debug.Log("Vida máxima suciedad" + suciedad.vidaMaximaSuciedad);
                    Debug.Log("Vida actual suciedad" + suciedad.vidaActualSuciedad);
                }
            }
        }

    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Falta el playerController");
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

        // Dibujar la hitbox del plumero
        Gizmos.color = Color.green;
        Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);

        // Ajustar la posición del plumero en función de la dirección del personaje
        Vector2 posicionPlumero = (Vector2)transform.position + plumero;
        if (transform.localScale.x < 0)
        {
            // El personaje está mirando a la izquierda
            posicionPlumero = (Vector2)transform.position - plumero;
        }

        Gizmos.DrawWireCube(posicionPlumero, tamanoPlumero);
    }

    public void SetPuedeLimpiar(bool valor)
    {
        puedeLimpiar = valor;
    }

    private void HandleLimpieza()
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (inputVertical < 0 && !saltando && puedeLimpiar)
        {
            Barrer();
        }
        else if (inputHorizontal != 0 && !saltando && puedeLimpiar)
        {
            Sacudir();
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
        // Vector2 tamanoEscoba = new Vector2(anchoHitBoxEscoba, largoHitboxEscoba);
        // overlapEscoba = Physics2D.OverlapBoxAll(
        //     (Vector2)transform.position + escoba,
        //     tamanoEscoba,
        //     0
        // );

        // foreach (Collider2D collider in overlapEscoba)
        // {
        //     if (collider.CompareTag("suciedadCamino") || collider.CompareTag("suciedad"))
        //     {
        //         puedeEliminarCaminoEscoba = true;
        //     }
        // }

        Debug.Log("PuedeBarrerPisoFake()");

    }

    private void EliminarCaminoFake(GameObject collider)
    {
        // Destroy(collider);
        SoundManager.instance.PlaySound(descubrirCamino);
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if ((other.CompareTag("suciedad") || other.CompareTag("suciedadCamino")) && animacionActiva)
    //     {
    //         SuciedadController suciedad = other.GetComponent<SuciedadController>();
    //         if (suciedad != null)
    //         {
    //             Debug.Log("HACIENDO DAÑO A LA SUCIEDAD");
    //             suciedad.RecibirDmg(limpiezaPorSegundoEscoba * Time.deltaTime);
    //         }
    //     }
    // }

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

    public void puedeSacurdirSuciedad()
    {

        Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);
        overlapPlumero = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + plumero,
            tamanoPlumero,
            0
        );

        foreach (Collider2D collider in overlapPlumero)
        {
            if (collider.CompareTag("suciedad"))
            {
                SuciedadController suciedad = collider.GetComponent<SuciedadController>();
                if (suciedad != null)
                {
                    suciedad.RecibirDmg(limpiezaPorSegundoEscoba * Time.deltaTime);
                }
            }
        }
    }

    public void PuedeSacudirParedFake()
    {

        // if (!animacionActiva) return;

        // Vector2 tamanoPlumero = new Vector2(anchoHitBoxPlumero, largoHitboxPlumero);
        // Vector2 posicionPlumero = (Vector2)transform.position + plumero;

        // if (transform.localScale.x < 0)
        // {
        //     posicionPlumero = (Vector2)transform.position - plumero;
        // }

        // overlapPlumero = Physics2D.OverlapBoxAll(
        //     posicionPlumero,
        //     tamanoPlumero,
        //     0
        // );
        // foreach (Collider2D collider in overlapPlumero)
        // {
        //     if (collider.CompareTag("suciedadCamino") || collider.CompareTag("suciedad"))
        //     {
        //         puedeEliminarCaminoPlumero = true;
        //         SuciedadController suciedad = collider.GetComponent<SuciedadController>();
        //         if (suciedad != null)
        //         {
        //             float dmg = limpiezaPorSegundoPlumero;
        //             suciedad.RecibirDmg(dmg);
        //         }
        //     }
        // }

        Debug.Log("PuedeSacuridParedFake()");
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
