using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //* -------------------------------------------------------------------------- */
    //*                                  Variables                                 */
    //* -------------------------------------------------------------------------- */
    public Rigidbody2D rigidBody;

    //^ ------------------------------- Movimiento ------------------------------- */
    [Header("Movimiento")]
    public float velocidadMovimiento = 10f;
    private Vector2 direccion;
    public bool puedeMoverse = true;

    [Header("Salto")]
    public float fuerzaSaltoSuelo = 5f;
    public float fuerzaSaltoAire = 8f;
    public float aceleracionCaida = 8f;
    public float aceleracionSalto = 2.0f;
    public float distanciaUmbral = 1.25f;
    public int saltosExtra;
    private int saltosExtraRestantes;
    public bool puedeSaltar = true;
    private bool saltoBloqueado = false;
    public Vector2 pies;

    [Header("Colisiones")]
    public float anchoCaja;
    public float altoCaja;
    public bool enSuelo = true;
    public LayerMask layerPiso;

    [Header("Dash")]
    public float velocidadDash = 15f;
    public float tiempoDash = 0.3f;
    public int dashesPermitidos;
    public float cooldownDash = 0.5f;
    private float ultimoTiempoDash = 0;
    private float gravedadInicial;
    private bool puedeDashear = true;
    private int contadorDashes;

    [SerializeField]
    TrailRenderer trailRenderer;

    private Invulnerabilidad invulnerabilidad;


    [Header("Sonidos")]
    [SerializeField]
    private AudioClip salto;

    [SerializeField]
    private AudioClip pasos;

    [SerializeField]
    private AudioClip dash;

    //^ ------------------------------- Animaciones ------------------------------ */
    [Header("Animaciones")]
    public Animator animator;

    private NpcController npcController;

    // * START POSITION
    public Vector2 startPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        invulnerabilidad = GetComponent<Invulnerabilidad>();
    }

    private void Start()
    {
        gravedadInicial = rigidBody.gravityScale;
        startPosition = transform.position;
    }

    public void Respawn()
    {
        Vector2 respawnPosition = GameManager.Instance.GetLastCheckpoint();
        if (respawnPosition != Vector2.zero)
        {
            transform.position = respawnPosition;
        }
        else
        {
            transform.position = startPosition;
        }
    }

    public void Reiniciar()
    {
        GameManager.Instance.SavePlayerPosition(transform.position);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        Movimiento();
        AgarreSalto();
        MejorarSalto();
        Saltar();
        NoCaer();
        DefinirSalto();
        PulsarDash();
        //inDialogue();
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
        contadorDashes = 0;
    }

    private void Saltar()
    {
        if (Input.GetButtonDown("Jump") && !saltoBloqueado)
        {

            // * Ejecutar sonido salto
            SoundManager.instance.PlaySound(salto);

            if (enSuelo)
            {
                saltosExtraRestantes = saltosExtra;
                puedeSaltar = true;
                animator.SetBool("jump", true);
                EjecutarSaltoSuelo();
            }
            else
            {
                if (saltosExtraRestantes > 0)
                {
                    puedeSaltar = true;
                    saltosExtraRestantes -= 1;
                    animator.SetBool("jump", true);
                    EjecutarSaltoAire();
                }
                else
                {
                    puedeSaltar = false;
                }
            }
        }
    }

    private void EjecutarSaltoAire()
    {
        if (puedeSaltar)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.velocity += Vector2.up * fuerzaSaltoAire;
            puedeSaltar = false;
        }
    }

    private void EjecutarSaltoSuelo()
    {
        if (puedeSaltar)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.velocity += Vector2.up * fuerzaSaltoSuelo;
            puedeSaltar = false;
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
        Vector2 tamanoCaja = new Vector2(anchoCaja, altoCaja);

        enSuelo = Physics2D.OverlapBox(
            (Vector2)transform.position + pies,
            tamanoCaja,
            0,
            layerPiso
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 tamanoCaja = new Vector2(anchoCaja, altoCaja);
        Gizmos.DrawWireCube((Vector2)transform.position + pies, tamanoCaja);
    }

    private void NoCaer()
    {
        if (enSuelo)
        {
            animator.SetBool("fall", false);
        }
    }

    public bool GetSaltando()
    {
        return !enSuelo;
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

    public void SetPuedeMoverse(bool estado)
    {
        puedeMoverse = estado;
        if (!estado)
        {
            frenarSeco();
        }
    }

    public void frenarSeco()
    {
        rigidBody.velocity = Vector2.zero;
    }

    private bool VerMovimientoVertical()
    {
        if (rigidBody.velocity.x == 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
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

    public void reproducirPasos()
    {
        SoundManager.instance.PlaySound(pasos);
    }

    private void cambiarDireccion()
    {
        if (puedeMoverse)
        {
            // * Verificamos que nos estemos moviendo
            if (direccion.x != 0 && VerMovimientoVertical())
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
    }

    //* -------------------------------------------------------------------------- */
    //*                                   Dashes                                   */
    //* -------------------------------------------------------------------------- */

    private void PulsarDash()
    {
        if (Input.GetButtonDown("Dash") && puedeDashear)
        {
            StartCoroutine(EjecutarDash());
        }
    }

    private IEnumerator EjecutarDash()
    {
        if (Time.time - ultimoTiempoDash > cooldownDash)
        {
            activarDesactivarMovimiento(false);
            rigidBody.gravityScale = 0;
            if (!enSuelo && contadorDashes < dashesPermitidos)
            {
                Dashear();
                contadorDashes += 1;
            }
            else if (enSuelo)
            {
                contadorDashes = 0;
                Dashear();
            }

            ultimoTiempoDash = Time.time;

            yield return new WaitForSeconds(tiempoDash);

            activarDesactivarMovimiento(true);
            rigidBody.gravityScale = gravedadInicial;
            trailRenderer.emitting = false;
        }
    }

    public void activarDesactivarMovimiento(bool valor)
    {
        puedeMoverse = valor;
        puedeDashear = valor;
    }

    public void SetSaltoBloqueado(bool valor)
    {
        saltoBloqueado = valor;
    }

    private void Dashear()
    {
        rigidBody.velocity = new Vector2(velocidadDash * Mathf.Sign(transform.localScale.x), 0);
        SoundManager.instance.PlaySound(dash);
        animator.SetTrigger("dash");
        invulnerabilidad.VolverseInvulnerableHabilidad(tiempoDash);
        trailRenderer.emitting = true;
    }


    // * ---------------------- COMPORTAMIENTO CON NPCS ---------------------- */
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "npc")
        {
            npcController = other.gameObject.GetComponent<NpcController>();

            // * Acá va
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("npcButton");

            foreach (GameObject button in gameObjects)
            {
                if (npcController != null)
                {
                    PekoraController pekora = npcController.GetComponentInParent<PekoraController>();
                    if (pekora.GetNpc())
                    {
                        button.GetComponent<Image>().enabled = true;
                    }
                    else
                    {
                        button.GetComponent<Image>().enabled = false;
                    }
                }
            }

            if (Input.GetKey(KeyCode.E))
            {
                npcController.ActivateDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        npcController = null;

        if (other.gameObject.tag == "npc")
        {
            // Desactivar los botones cuando el jugador sale del área
            GameObject[] npcButtons = GameObject.FindGameObjectsWithTag("npcButton");

            foreach (GameObject button in npcButtons)
            {
                button.GetComponent<Image>().enabled = false;
            }
        }

    }

    private bool inDialogue()
    {
        if (npcController != null)
        {
            return npcController.DialogueActive();
        }
        else
        {
            puedeMoverse = true;
            return false;
        }
    }

}
