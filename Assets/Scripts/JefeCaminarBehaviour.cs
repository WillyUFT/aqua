using System.Collections;
using UnityEngine;

public class JefeCaminarBehaviour : StateMachineBehaviour
{
    private BossController bossController;
    private Rigidbody2D rigidBody;

    [SerializeField] private float velocidadMovimiento;

    [SerializeField] private float distanciaUmbral;

    private float tiempoEnMovimiento;

    [SerializeField] private float[] probabilidadesEstado;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        bossController = animator.GetComponent<BossController>();
        rigidBody = bossController.rigidBody;
        bossController.MirarJugador();
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {



        // tiempoEnMovimiento -= Time.deltaTime;

        // if (tiempoEnMovimiento > 0)
        // {
        //     float distanciaAlJugador = Vector2.Distance(
        //         bossController.jugador.position,
        //         rigidBody.position
        //     );

        //     if (distanciaAlJugador <= bossController.distanciaUmbral)
        //     {
        //         animator.SetBool("walk", true);
        //         moverHaciaJugador();
        //     }
        //     else
        //     {
        //         rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        //     }
        // }
        // else
        // {
        //     animator.SetBool("walk", false);
        //     float estadoEscogido = Choose(probabilidadesEstado);
        //     if (estadoEscogido == 0)
        //     {
        //         animator.SetTrigger("attack");
        //     }
        //     else if (estadoEscogido == 1)
        //     {
        //         animator.SetTrigger("rocket");
        //     }
        // }
    }

    private float Choose(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    private void moverHaciaJugador()
    {
        bossController.MirarJugador();

        float direccionHaciaJugador = bossController.jugador.position.x - rigidBody.position.x;

        Vector2 direccionMovimiento;
        if (direccionHaciaJugador < 0)
        {
            direccionMovimiento = new Vector2(-velocidadMovimiento, rigidBody.velocity.y);
        }
        else
        {
            direccionMovimiento = new Vector2(velocidadMovimiento, rigidBody.velocity.y);
        }
        rigidBody.velocity = direccionMovimiento;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }
}
