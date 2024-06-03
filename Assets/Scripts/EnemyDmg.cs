using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDmg : MonoBehaviour, IDamageable
{
    //* -------------------------------- Variables ------------------------------- */
    [Header("Aqua")]
    public Transform jugador;

    [Header("Movimiento")]
    [SerializeField]
    public float velocidadMovimiento = 0;

    [Header("Golpeado")]
    private int vecesGolpeado = 0;
    public bool golpeado = false;

    [Header("Rigidbody")]
    public Rigidbody2D rb;
    public Collider2D physicsCollider;

    [Header("Vida")]
    public bool vivo = true;
    public float vida;
    private float vidaMaxima;

    [Header("Barra de vida")]
    private BarraVidaEnemigo barraVidaEnemigo;
    [Header("Barra de vida Jefe")]
    [SerializeField]
    private BarraVidaBoss barraVidaBoss;

    [Header("Invencibilidad")]
    private float tiempoInvencibleTranscurrido = 0f;
    public bool esInvencible = false;
    private readonly bool puedeVolverseInvencible = false;

    [Header("Targeteable")]
    public bool esTargeteable = false;
    public bool disableSimulation = false;

    [Header("Pérdida de control")]
    public bool perdidaControl = false;
    public float tiempoPerdidaControlTrasncurrido = 0f;

    [SerializeField]
    public float tiempoPerdidaControl = 0f;

    private FlashEffect flashEffect;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        flashEffect = GetComponent<FlashEffect>();
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        vidaMaxima = Vida;
        if (gameObject.tag != "jefe" && gameObject.tag != "npc")
        {
            barraVidaEnemigo.ActualizarVida(Vida, vidaMaxima);
        }
        else if (gameObject.tag == "jefe" || gameObject.tag == "npc")
        {
            barraVidaBoss.SetVidaInicial(vidaMaxima);
        }
    }

    public void Awake()
    {
        if (gameObject.tag != "jefe")
        {
            barraVidaEnemigo = GetComponentInChildren<BarraVidaEnemigo>();

        }
    }

    void FixedUpdate()
    {
        MirarJugador();
        moverHaciaJugador();

        if (Perdidacontrol)
        {
            tiempoPerdidaControlTrasncurrido += Time.deltaTime;
            if (tiempoPerdidaControlTrasncurrido >= tiempoPerdidaControl)
            {
                Perdidacontrol = false;
            }
        }
        if (vecesGolpeado > 1)
        {
            volverInvencible(4);
            vecesGolpeado = 0;
        }
    }

    public float Vida
    {
        set
        {
            vida = value;
            if (vida <= 0)
            {
                Targeteable = false;
                Destruir();
            }
        }
        get { return vida; }
    }

    public bool Targeteable
    {
        get { return esTargeteable; }
        set
        {
            esTargeteable = true;
            if (disableSimulation)
            {
                rb.simulated = true;
            }

            physicsCollider.enabled = value;
        }
    }
    public bool Invencible
    {
        get { return esInvencible; }
        set
        {
            esInvencible = value;
            if (esInvencible)
            {
                tiempoInvencibleTranscurrido = 0f;
            }
        }
    }

    public void volverInvencible(float duracion)
    {

        StartCoroutine(InvencibleTemporizador(duracion));

    }

    private IEnumerator InvencibleTemporizador(float duracion)
    {
        Invencible = true;
        yield return new WaitForSeconds(duracion);
        Invencible = false;

    }

    public bool Perdidacontrol
    {
        get { return perdidaControl; }
        set
        {
            perdidaControl = value;
            if (perdidaControl)
            {
                tiempoPerdidaControlTrasncurrido = 0f;
            }
        }
    }

    public bool Golpeado
    {
        get { return golpeado; }
        set { golpeado = value; }
    }

    private IEnumerator RecuperarControl(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        perdidaControl = false;
    }

    public void Destruir()
    {
        StartCoroutine(EjecutarDestruccion());
    }

    private IEnumerator EjecutarDestruccion()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void RecibirDmg(float dmg, Vector2 knockback)
    {
        if (!Invencible)
        {
            if (!Golpeado)
            {
                Golpeado = true;
            }

            Vida -= dmg;
            barraVidaEnemigo.ActualizarVida(Vida, vidaMaxima);

            RealizarKnockback(knockback);

            flashEffect.FlashDmg();

            if (puedeVolverseInvencible)
            {
                Invencible = true;
            }

            // * Perdemos el control
            Perdidacontrol = true;
            StartCoroutine(RecuperarControl(0.1f));
        }
        else
        {

            flashEffect.FlashInvulnerable();
        }

    }

    public void RecibirDmg(float dmg)
    {
        if (!Invencible)
        {
            Vida -= dmg;

            barraVidaBoss.recibirDmg(dmg);

            flashEffect.FlashDmg();

            if (puedeVolverseInvencible)
            {
                Invencible = true;
            }

            // * Perdemos el control
            Perdidacontrol = true;
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetTrigger("hurt");
            StartCoroutine(RecuperarControl(0.1f));
            vecesGolpeado++;
        }
        else
        {
            flashEffect.FlashInvulnerable();
        }

    }

    public void RealizarKnockback(Vector2 knockback)
    {
        float direccionKnockback = Mathf.Sign(transform.localScale.x);

        Vector2 knockbackDireccionado = new Vector2(
            knockback.x * direccionKnockback,
            knockback.y
        );

        rb.AddForce(knockbackDireccionado, ForceMode2D.Impulse);
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

    private void moverHaciaJugador()
    {
        if (Golpeado && !Perdidacontrol)
        {
            float direccionHaciaJugador = jugador.position.x - rb.position.x;

            Vector2 direccionMovimiento;
            if (direccionHaciaJugador < 0)
            {
                direccionMovimiento = new Vector2(-velocidadMovimiento, rb.velocity.y);
            }
            else
            {
                direccionMovimiento = new Vector2(velocidadMovimiento, rb.velocity.y);
            }
            rb.velocity = direccionMovimiento;
        }
    }
}
