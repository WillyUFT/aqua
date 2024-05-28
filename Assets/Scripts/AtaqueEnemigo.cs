using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [SerializeField] public float dmgAttack;

    [SerializeField] public float dmgTouch;

    private PlayerCombatController playerCombatController;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerCombatController = playerObject.GetComponent<PlayerCombatController>();
        }
    }

    public float getDmgAttack()
    {
        return dmgAttack;
    }

    public float getDmgTouch()
    {
        return dmgTouch;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("block") || other.gameObject.CompareTag("Player"))
        {
            if (gameObject.tag != "npc")
            {
                playerCombatController.RecibirDmg(dmgAttack);
            }

        }
    }
}
