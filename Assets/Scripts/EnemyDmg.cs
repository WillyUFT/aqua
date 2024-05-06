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
        barraVidaEnemigo.ActualizarVida(Vida, vidaMaxima);
    }

    public void Awake()
    {
        barraVidaEnemigo = GetComponentInChildren<BarraVidaEnemigo>();
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

            flashEffect.Flash();

            if (puedeVolverseInvencible)
            {
                Invencible = true;
            }

            // * Perdemos el control
            Perdidacontrol = true;
            StartCoroutine(RecuperarControl(tiempoPerdidaControl));
        }
    }

    public void RecibirDmg(float dmg)
    {
        if (!Invencible)
        {
            Vida -= dmg;

            if (puedeVolverseInvencible)
            {
                Invencible = true;
            }
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
