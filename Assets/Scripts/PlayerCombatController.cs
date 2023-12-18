using System.Collections;
using UnityEngine;
using System.Linq;
using System;

public class PlayerCombatController : MonoBehaviour
{
    [Header("Aqua")]
    [SerializeField]
    private BarraVidaAqua barraVidaAqua;

    private bool saltando = false;

    [SerializeField]
    TimeStop timeStop;
    public Rigidbody2D rigidBody;
    public PlayerController playerController;

    [SerializeField]
    private MenuGameOver menuGameOver;

    [Header("Daño")]
    [SerializeField]
    private Vector2 velocidadKnockBack;

    [Header("Hitbox golpe normal")]
    public Transform controladorGolpe;
    public float anchoHitBoxNormal;
    public float largoHitboxNormal;
    public float dmgGolpeNormal;

    [Header("Cooldown golpe normal")]
    public float cooldownAtaqueNormal = 0.5f;
    public float ultimoAtaqueNormal = 0;
    public bool puedeAtacar = true;

    [Header("Animaciones")]
    public Animator animator;

    [Header("Bloqueo")]
    [SerializeField]
    private GameObject controladorBloqueo;

    [SerializeField]
    private float largoBloqueoHitbox;

    [SerializeField]
    private float anchoBloqueoHitbox;
    public bool bloqueando;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        saltando = playerController.GetSaltando();
        pulsarAtaque();
        Bloquear();
    }

    private void pulsarAtaque()
    {
        if (Input.GetButtonDown("Ataque") && puedeAtacar)
        {
            Debug.Log("antes del coroutine: " + puedeAtacar);
            StartCoroutine(EjecutarAnimacionGolpe());
            Debug.Log("después del coroutine: " + puedeAtacar);
        }
    }

    private IEnumerator EjecutarAnimacionGolpe()
    {
        puedeAtacar = false;
        animacionGolpearNormal();
        yield return new WaitForSeconds(0.3f);
        puedeAtacar = true;
    }

    private void animacionGolpearNormal()
    {
        animator.SetTrigger("attack");
    }

    public void GolpearNormal()
    {
        Vector2 tamanoCaja = new Vector2(anchoHitBoxNormal, largoHitboxNormal);

        Collider2D[] objetos = Physics2D.OverlapBoxAll((Vector2)transform.position, tamanoCaja, 0);
        foreach (
            var collider in from Collider2D collider in objetos
            where collider.CompareTag("enemigo")
            select collider
        )
        {
            collider.transform.GetComponent<EnemyCombatController>().RecibirDmg(dmgGolpeNormal);
        }

        cooldownAtaqueNormal = ultimoAtaqueNormal;
    }

    public void Bloquear()
    {
        if (!saltando)
        {
            if (Input.GetButtonDown("Bloqueo"))
            {
                animator.SetTrigger("blockTrigger");
            }

            if (Input.GetButton("Bloqueo"))
            {
                bloqueando = true;
                playerController.SetPuedeMoverse(false);
                animator.SetBool("block", true);
                controladorBloqueo.SetActive(true);
            }
            else if (Input.GetButtonUp("Bloqueo"))
            {
                playerController.SetPuedeMoverse(true);
                animator.SetBool("block", false);
                controladorBloqueo.SetActive(false);
                bloqueando = false;
            }
        }
    }

    public void RecibirDmg(float dmg)
    {
        if (bloqueando)
        {
            dmg = dmg / 2;
        }
        else
        {
            KnockbackDmg();
            timeStop.StopTime(0.05f, 10, 0.1f);
        }
        barraVidaAqua.recibirDmg(dmg);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 tamanoCaja = new Vector2(anchoHitBoxNormal, largoHitboxNormal);
        Gizmos.DrawWireCube((Vector2)controladorGolpe.position, tamanoCaja);
    }

    public void KnockbackDmg()
    {
        PerderControl();
        animator.SetTrigger("hurt");

        float direccionKnockback = Mathf.Sign(transform.localScale.x);
        rigidBody.velocity = new Vector2(
            -velocidadKnockBack.x * direccionKnockback,
            rigidBody.position.y * velocidadKnockBack.y
        );
    }

    public void RecuperarControl()
    {
        playerController.SetPuedeMoverse(true);
    }

    public void PerderControl()
    {
        playerController.SetPuedeMoverse(false);
    }
}
