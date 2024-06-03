using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PekoraController : MonoBehaviour
{

    [SerializeField] private bool esNpc = true;

    [SerializeField] public Rigidbody2D rigidBody;

    [SerializeField] public Animator animator;

    [Header("Ataque espada")]
    [SerializeField] private float fuerzaDash;

    [SerializeField] private float duracionDash;

    [Header("Ataque misil")]
    [SerializeField] public GameObject misilPrefab;

    private BossController bossController;
    private EnemyDmg enemyDmg;

    private int vecesAtaqueEspada = 0;
    private int vecesAtaqueCohete = 0;

    void Awake()
    {
        bossController = GetComponent<BossController>();
        enemyDmg = GetComponent<EnemyDmg>();


        if (bossController == null)
        {
            Debug.LogError("BossController no se encontró en el GameObject.");
        }
    }

    public bool GetNpc()
    {
        return esNpc;
    }

    public void SetNpc(bool valor)
    {
        esNpc = valor;
        if (bossController != null && !valor)
        {
            bossController.IniciarExitIdleState();
        }
    }

    public int getVecesAtaqueEspada()
    {
        return vecesAtaqueEspada;
    }

    public int getVecesAtaqueCohete()
    {
        return vecesAtaqueCohete;
    }

    //* -------------------------------------------------------------------------- */
    //*                            Ataque con la espadaX                           */
    //* -------------------------------------------------------------------------- */
    public void ataqueNormal()
    {
        enemyDmg.Invencible = true;
        vecesAtaqueCohete = 0;
        bossController.setPuedeMoverse(false);
        rigidBody.velocity = new Vector2(fuerzaDash * -transform.localScale.x, 0);
        animator.SetTrigger("attackDash");

        StartCoroutine(DetenerDash());
    }

    private IEnumerator DetenerDash()
    {
        yield return new WaitForSeconds(duracionDash);
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        bossController.setPuedeMoverse(true);
        vecesAtaqueEspada++;
        animator.SetBool("attack", false);
        enemyDmg.Invencible = false;
    }

    //* -------------------------------------------------------------------------- */
    //*                             Ataque con el misil                            */
    //* -------------------------------------------------------------------------- */
    public void LanzarMisil()
    {
        enemyDmg.Invencible = true;
        vecesAtaqueEspada = 0;
        bossController.setPuedeMoverse(false);
        int anguloAleatorioEnZ = Random.Range(30, 60);
        if (transform.localScale.x < 0)
        {
            anguloAleatorioEnZ = -anguloAleatorioEnZ;
        }
        Instantiate(misilPrefab, transform.position, Quaternion.Euler(0, 0, anguloAleatorioEnZ));
        bossController.setPuedeMoverse(true);
        vecesAtaqueCohete++;
        animator.SetBool("rocket", false);
        enemyDmg.Invencible = false;
    }
}
