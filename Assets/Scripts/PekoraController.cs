using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class PekoraController : MonoBehaviour
{
    [SerializeField] private bool esNpc = true;
    private bool desactivada = true;

    [SerializeField] public Rigidbody2D rigidBody;
    [SerializeField] private GameObject barraVida;
    [SerializeField] private GameObject barraLimpieza;
    private BarraLimpieza barraLimpiezaScript;
    [SerializeField]

    private PlayerCombatController aqua;

    [SerializeField] public Animator animator;

    [Header("Ataque espada")]
    [SerializeField] private float fuerzaDashNormal;

    [SerializeField] private float duracionDashNormal;

    public bool atacandoEspada = false;
    [SerializeField] private float dmgEspada;
    [SerializeField] private float dmgTocar;


    [Header("Ataque misil")]
    [SerializeField] public GameObject misilPrefab;
    private ZanahoriaPekoraController zanahoriaPekoraController;

    private BossController bossController;
    private EnemyDmg enemyDmg;

    [Header("Pekora enojada")]
    public bool pekoraEnojada = false;

    [Header("Pekora muerta")]
    public bool pekoraMuerta = false;
    private int vecesAtaqueEspada = 0;
    private int vecesAtaqueCohete = 0;

    [Header("Sonido")]
    [SerializeField]
    private AudioClip coheteSonido;
    [SerializeField]
    private AudioClip espadaSonido;
    [SerializeField]
    private AudioClip pekoraMuerte;
    MusicaPekoracontroller musicaPekoracontroller;


    void Awake()
    {
        bossController = GetComponent<BossController>();
        enemyDmg = GetComponent<EnemyDmg>();
        musicaPekoracontroller = GetComponent<MusicaPekoracontroller>();
        zanahoriaPekoraController = misilPrefab.GetComponent<ZanahoriaPekoraController>();
        rigidBody.isKinematic = true;
        barraLimpiezaScript = barraLimpieza.GetComponent<BarraLimpieza>();

        if (bossController == null)
        {
            Debug.LogError("BossController no se encontró en el GameObject.");
        }
    }

    public void Morir()
    {
        if ((enemyDmg.vida <= 0 || barraLimpiezaScript.vidaActual <= 0) && !pekoraMuerta)
        {
            pekoraMuerta = true;
            SetNpc(true);
            rigidBody.isKinematic = true;
            animator.SetTrigger("morir");
            barraVida.SetActive(false);
            gameObject.tag = "npc";
            gameObject.layer = LayerMask.NameToLayer("NPC");
            enemyDmg.SetMuerto(true);
            barraLimpieza.SetActive(false);
            DesactivarSuciedadPekora();
            activarPekoraDialogoCuadroTrigger();
        }
    }


    private void activarPekoraDialogoCuadroTrigger()
    {
        StartCoroutine(PlaySoundAndActivateDialog());
    }

    private IEnumerator PlaySoundAndActivateDialog()
    {
        SoundManager.instance.PlaySound(pekoraMuerte);

        // Hacer fade out y cambiar la música
        musicaPekoracontroller.CambiarMusicaConFade(pekoraMuerte.length / 1.5f);

        yield return new WaitForSeconds(pekoraMuerte.length);

        GameObject pekoraDialogo = gameObject.transform.Find("PekoraDialogo").gameObject;
        if (pekoraDialogo != null)
        {
            pekoraDialogo.SetActive(true);
        }
    }


    private void DesactivarSuciedadPekora()
    {
        GameObject[] suciedadPekora = GameObject.FindGameObjectsWithTag("suciedad");
        foreach (var item in suciedadPekora)
        {
            item.SetActive(false);
        }
    }

    public bool GetNpc()
    {
        return esNpc;
    }

    public void SetNpc(bool valor)
    {
        esNpc = valor;

        if (valor && !desactivada && !pekoraMuerta)
        {
            desactivada = true;
            animator.SetBool("nada", true);
            barraVida.SetActive(false);
            barraLimpieza.SetActive(false);
            gameObject.tag = "npc";
        }

        if (bossController != null && !valor)
        {
            desactivada = false;
            rigidBody.isKinematic = false;
            gameObject.layer = LayerMask.NameToLayer("Enemigo");
            bossController.IniciarExitIdleState();
        }
    }

    public int getVecesAtaqueEspada()
    {
        return vecesAtaqueEspada;
    }

    public int getVecesAtaqueCohete()
    {
        return vecesAtaqueCohete;
    }

    private void Update()
    {
        EnojarPekora();
        Morir();
    }

    private void EnojarPekora()
    {
        if (!pekoraEnojada && (enemyDmg.vida <= enemyDmg.vidaMaxima * 0.5 ||
         barraLimpiezaScript.vidaActual <= barraLimpiezaScript.vidaMaxima * 0.5))
        {
            pekoraEnojada = true;
            animator.SetTrigger("enojada");
            fuerzaDashNormal *= 2;
            zanahoriaPekoraController.SetVelocidadRotacion(zanahoriaPekoraController.GetVelocidadRotacion() * 1.5f);
            JefeCaminarBehaviour jefeCaminarBehaviour = animator.GetBehaviour<JefeCaminarBehaviour>();
            jefeCaminarBehaviour.SetVelocidadMovimiento(jefeCaminarBehaviour.GetVelocidadMovimiento() * 1.5f);
        }
    }

    public void iniciarAnimacionEnojada()
    {
        enemyDmg.Invencible = true;
        vecesAtaqueCohete = 0;
        vecesAtaqueEspada = 0;
        bossController.setPuedeMoverse(false);
    }

    public void terminarAnimacionEnojada()
    {
        enemyDmg.Invencible = false;
        bossController.setPuedeMoverse(true);
    }

    //* -------------------------------------------------------------------------- */
    //*                            Ataque con la espadaX                           */
    //* -------------------------------------------------------------------------- */

    public void IniciarAnimacionEspada()
    {
        enemyDmg.Invencible = true;
        vecesAtaqueCohete = 0;
        bossController.setPuedeMoverse(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!GetNpc() && atacandoEspada && other.collider.tag == "Player" && !pekoraMuerta)
        {
            aqua.RecibirDmg(dmgEspada);
        }
        else if (!GetNpc() && !atacandoEspada && other.collider.tag == "Player" && !pekoraMuerta)
        {
            aqua.RecibirDmg(dmgTocar);
        }
    }

    public void AtaqueNormal()
    {
        atacandoEspada = true;
        SoundManager.instance.PlaySound(espadaSonido);
        rigidBody.velocity = new Vector2(fuerzaDashNormal * -transform.localScale.x, 0);
        animator.SetTrigger("attackDash");
        StartCoroutine(DetenerDash());
    }

    private IEnumerator DetenerDash()
    {
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(duracionDashNormal);
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        bossController.setPuedeMoverse(true);
        enemyDmg.Invencible = false;
        atacandoEspada = false;
    }

    //* -------------------------------------------------------------------------- */
    //*                             Ataque con el misil                            */
    //* -------------------------------------------------------------------------- */
    public void LanzarMisil()
    {
        StartCoroutine(LanzarMisilesCoroutine());
    }

    private IEnumerator LanzarMisilesCoroutine()
    {
        enemyDmg.Invencible = true;
        bossController.setPuedeMoverse(false);
        int cohetesDisparados;

        if (pekoraEnojada)
        {
            cohetesDisparados = Random.Range(2, 3);
        }
        else
        {
            cohetesDisparados = Random.Range(1, 2);
        }

        for (int cohete = 0; cohete < cohetesDisparados; cohete++)
        {

            int anguloAleatorioEnZ = Random.Range(20, 80);
            if (transform.localScale.x < 0)
            {
                anguloAleatorioEnZ = -anguloAleatorioEnZ;
            }

            if (pekoraEnojada)
            {
                float variacionVelocidad = Random.Range(0.5f, 1.5f);
                zanahoriaPekoraController.SetVelocidad(zanahoriaPekoraController.GetVelocidad() * variacionVelocidad);
            }

            Instantiate(misilPrefab, transform.position, Quaternion.Euler(0, 0, anguloAleatorioEnZ));
            zanahoriaPekoraController.SetVelocidad(zanahoriaPekoraController.GetVelocidadInicial());
            SoundManager.instance.PlaySound(coheteSonido);
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        bossController.setPuedeMoverse(true);
        animator.SetBool("rocket", false);
        enemyDmg.Invencible = false;
    }

    //* --------------------------------------------------------------------- */
    //*                                MOVERSE                                */
    //* --------------------------------------------------------------------- */
    public void moverHaciaJugador(float velocidadMovimiento)
    {
        if (bossController.getPuedeMoverse())
        {
            bossController.MirarJugador();

            float direccionHaciaJugador = bossController.jugadorTransform.position.x - rigidBody.position.x;
            float distanciaMovimiento = Random.Range(1.6f, Mathf.Max(1.6f, direccionHaciaJugador - 1.5f));

            Vector2 direccionMovimiento;
            if (direccionHaciaJugador < 0)
            {
                direccionMovimiento = new Vector2(-velocidadMovimiento, rigidBody.velocity.y);
            }
            else
            {
                direccionMovimiento = new Vector2(velocidadMovimiento, rigidBody.velocity.y);
            }

            StartCoroutine(MoverYDetener(distanciaMovimiento, direccionMovimiento, velocidadMovimiento));
        }
    }

    private IEnumerator MoverYDetener(float distanciaMovimiento, Vector2 direccionMovimiento, float velocidadMovimiento)
    {
        float distanciaRecorrida = 0;
        while (distanciaRecorrida < distanciaMovimiento)
        {
            rigidBody.velocity = direccionMovimiento;
            distanciaRecorrida += velocidadMovimiento * Time.deltaTime;
            yield return null;
        }
        DetenerMovimiento();
        yield return new WaitForSeconds(0.1f);
        rigidBody.drag = 0f;
        JefeCaminarBehaviour jefeCaminarBehaviour = animator.GetBehaviour<JefeCaminarBehaviour>();
        jefeCaminarBehaviour.activarAtaques(animator);
    }

    private void DetenerMovimiento()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        // Aplicar una fuerza de frenado si es necesario
        rigidBody.drag = 10f; // Esto aumentará la fricción lineal para detener el movimiento.
    }
}
