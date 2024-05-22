using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PekoraController : MonoBehaviour
{

    [SerializeField] private bool esNpc = true;

    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private Animator animator;

    [Header("Ataque espada")]
    [SerializeField] private float fuerzaDash;

    [SerializeField] private float duracionDash;

    [Header("Ataque misil")]
    [SerializeField] public GameObject misilPrefab;


    public bool GetNpc()
    {
        return esNpc;
    }

    public void SetNpc(bool valor)
    {
        esNpc = valor;
        BossController bossController = gameObject.GetComponent<BossController>();
        if (bossController != null && !valor)
        {
            bossController.IniciarExitIdleState();
        }
    }

    //* -------------------------------------------------------------------------- */
    //*                            Ataque con la espadaX                           */
    //* -------------------------------------------------------------------------- */
    public void ataqueNormal()
    {
        rigidBody.velocity = new Vector2(fuerzaDash * -transform.localScale.x, 0);
        animator.SetTrigger("attackDash");

        StartCoroutine(DetenerDash());
    }

    private IEnumerator DetenerDash()
    {
        yield return new WaitForSeconds(duracionDash);
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        // animator.SetTrigger("idle");
    }

    //* -------------------------------------------------------------------------- */
    //*                             Ataque con el misil                            */
    //* -------------------------------------------------------------------------- */
    public void LanzarMisil()
    {
        int anguloAleatorioEnZ = Random.Range(30, 60);
        if (transform.localScale.x < 0)
        {
            anguloAleatorioEnZ = -anguloAleatorioEnZ;
        }
        Instantiate(misilPrefab, transform.position, Quaternion.Euler(0, 0, anguloAleatorioEnZ));
        // animator.SetTrigger("idle");
    }
}
