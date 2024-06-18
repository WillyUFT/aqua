using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaProyectil : MonoBehaviour
{

    [Header("Daño")]
    [SerializeField]
    private Vector2 velocidadKnockBack;

    [Header("Hitbox golpe normal")]
    public float anchoHitBoxNormal;
    public float largoHitboxNormal;
    public float dmgGolpeNormal;

    [Header("Cooldown golpe normal")]
    public float cooldownAtaqueNormal = 0.5f;
    public float ultimoAtaqueNormal = 0;

    [Header("Sonidos")]
    [SerializeField]
    private AudioClip golpear;

    private bool atacando = false;
    private HashSet<Collider2D> enemigosGolpeados = new HashSet<Collider2D>();

    private void Update()
    {
        GolpearNormal();
    }

    public void LimpiarEnemigosGolpeados()
    {
        enemigosGolpeados.Clear();
    }

    public bool GetAtacando()
    {
        return atacando;
    }

    public void SetAtacando(bool value)
    {
        atacando = value;
    }

    public void GolpearNormal()
    {

        if (!atacando) return;

        float radioGolpe = anchoHitBoxNormal / 2;

        Collider2D[] objetos = Physics2D.OverlapCircleAll(
            (Vector2)gameObject.transform.position,
            radioGolpe
        );

        foreach (
            var enemyTransform in objetos
        )
        {

            // Verifica si el enemigo ya fue golpeado
            if (enemigosGolpeados.Contains(enemyTransform))
            {
                continue;
            }

            var enemyDmg = enemyTransform.GetComponent<EnemyDmg>();

            if (enemyTransform.CompareTag("enemigo"))
            {
                enemyDmg.RecibirDmg(dmgGolpeNormal, velocidadKnockBack);
                SoundManager.instance.PlaySound(golpear);

                enemigosGolpeados.Add(enemyTransform);

            }
            else if (enemyTransform.CompareTag("jefe"))
            {
                PekoraController pekoraController = enemyTransform.GetComponent<PekoraController>();
                if (pekoraController != null && !pekoraController.GetNpc())
                {
                    enemyDmg.RecibirDmg(dmgGolpeNormal);
                    enemigosGolpeados.Add(enemyTransform);
                }
            }

        }

        cooldownAtaqueNormal = ultimoAtaqueNormal;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float radioGolpe = anchoHitBoxNormal / 2;
        Gizmos.DrawWireSphere((Vector2)gameObject.transform.position, radioGolpe);
    }

}
