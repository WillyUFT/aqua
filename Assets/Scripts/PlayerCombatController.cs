using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    [Header("Animaciones")]
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animacionGolpearNormal();
    }

    private void animacionGolpearNormal()
    {
        if (cooldownAtaqueNormal > 0)
        {
            ultimoAtaqueNormal -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && ultimoAtaqueNormal <= 0)
        {
            animator.SetTrigger("attack");
        }
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
