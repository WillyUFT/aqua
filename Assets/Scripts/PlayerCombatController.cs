using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerCombatController : MonoBehaviour
{
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

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        pulsarAtaque();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 tamanoCaja = new Vector2(anchoHitBoxNormal, largoHitboxNormal);
        Gizmos.DrawWireCube((Vector2)controladorGolpe.position, tamanoCaja);
    }
}
