using System;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima;
    private float vidaActual;

    [Header("Daño")]
    [SerializeField] private FlashEffect flashEffect;
    [SerializeField] private BarraVidaEnemigo barraVidaEnemigo;
    [SerializeField] private Vector2 velocidadKnockBack;
    [SerializeField] private bool sePuedeMove = true;

    private void Awake()
    {
        barraVidaEnemigo = GetComponentInChildren<BarraVidaEnemigo>();
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
        Debug.Log("vidaActual: " + vidaActual);
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
        Debug.Log("Enemigo muerto jajaj");
    }
}
