using System;
using System.Collections;
using UnityEngine;

public class Invulnerabilidad : MonoBehaviour
{
    Renderer rend;
    Color color;
    PlayerCombatController playerCombatController;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        playerCombatController = GetComponent<PlayerCombatController>();
    }

    public void VolverseInvulnerable()
    {
        StartCoroutine("InvulnerableDaño");
    }

    IEnumerator InvulnerableDaño()
    {
        playerCombatController.SetInvulnerable(true);
        Physics2D.IgnoreLayerCollision(10, 8, true);
        color.a = 0.5f;
        rend.material.color = color;
        yield return new WaitForSeconds(1f);
        playerCombatController.SetInvulnerable(false);
        Physics2D.IgnoreLayerCollision(10, 8, false);
        color.a = 1f;
        rend.material.color = color;
    }

    public void VolverseInvulnerableHabilidad(float tiempo)
    {
        StartCoroutine("InvulnerableHabilidad", tiempo);
    }

    IEnumerator InvulnerableHabilidad(float tiempo)
    {
        playerCombatController.SetInvulnerable(true);
        Physics2D.IgnoreLayerCollision(10, 8, true);
        color.a = 0.5f;
        rend.material.color = color;
        yield return new WaitForSeconds(tiempo);
        playerCombatController.SetInvulnerable(false);
        color.a = 1f;
        rend.material.color = color;
        Physics2D.IgnoreLayerCollision(10, 8, false);
    }




}
