using System;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    [Header("Stats")]
    public float vidaEnemy;

    [Header("Daño")]
    [SerializeField] private FlashEffect flashEffect;

    public void RecibirDmg(float dmg)
    {
        vidaEnemy -= dmg;
        flashEffect.Flash();
        Debug.Log("vidaEnemy: " + vidaEnemy);
        if (vidaEnemy <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        Debug.Log("Enemigo muerto jajaj");
    }
}
