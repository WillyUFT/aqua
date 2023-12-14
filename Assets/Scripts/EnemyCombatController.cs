using System;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{

    [Header("Cuerpo")]
    [SerializeField] private Rigidbody2D rigidBody;
    private AtaqueEnemigo ataqueEnemigo;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima;
    private float vidaActual;

    [Header("Daño")]
    [SerializeField] private FlashEffect flashEffect;
    [SerializeField] private BarraVidaEnemigo barraVidaEnemigo;
    [SerializeField] private Vector2 velocidadKnockBack;
    [SerializeField] private bool sePuedeMove = true;

    [Header("Aqua")]
    [SerializeField] private PlayerCombatController playerCombatController;

    private void Awake()
    {
        barraVidaEnemigo = GetComponentInChildren<BarraVidaEnemigo>();
        ataqueEnemigo = GetComponent<AtaqueEnemigo>();
    }

    private void Start() {
        vidaActual = vidaMaxima;
        barraVidaEnemigo.ActualizarVida(vidaActual, vidaMaxima);
    }

    public void RecibirDmg(float dmg)
    {
        vidaActual -= dmg;
        KnockbackDmg();
        barraVidaEnemigo.ActualizarVida(vidaActual, vidaMaxima);
        flashEffect.Flash();
        if (vidaActual <= 0)
        {
            Muerte();
        }
    }

    public void KnockbackDmg() {
        float direccionKnockback = Mathf.Sign(transform.localScale.x);
        rigidBody.velocity = new Vector2(-velocidadKnockBack.x * direccionKnockback, rigidBody.position.y * velocidadKnockBack.y);
    }

    private void Muerte()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerCombatController.RecibirDmg(ataqueEnemigo.getDmgTouch());
        }
    }
}
