using UnityEngine;

public class JefeCaminarBehaviour : StateMachineBehaviour
{
    private BossController bossController;
    private Rigidbody2D rigidBody;

    [SerializeField]
    private float velocidadMovimiento;

    [SerializeField]
    private float distanciaUmbral;

    private float tiempoEnMovimiento;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        tiempoEnMovimiento = Random.Range(1.5f, 2.0f);
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
        tiempoEnMovimiento -= Time.deltaTime;

        if (tiempoEnMovimiento > 0)
        {
            float distanciaAlJugador = Vector2.Distance(
                bossController.jugador.position,
                rigidBody.position
            );

            if (distanciaAlJugador <= bossController.distanciaUmbral)
            {
                moverHaciaJugador();
            }
            else
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
        else
        {
            animator.SetTrigger("attack");
        }
    }

    private void moverHaciaJugador()
    {
        bossController.MirarJugador();

        // Calcula la dirección hacia el jugador
        float direccionHaciaJugador = bossController.jugador.position.x - rigidBody.position.x;

        // Determina la velocidad en la dirección correcta
        Vector2 direccionMovimiento;
        if (direccionHaciaJugador < 0)
        {
            // Jugador a la izquierda
            direccionMovimiento = new Vector2(-velocidadMovimiento, rigidBody.velocity.y);
        }
        else
        {
            // Jugador a la derecha
            direccionMovimiento = new Vector2(velocidadMovimiento, rigidBody.velocity.y);
        }

        // Aplica la velocidad al Rigidbody
        rigidBody.velocity = direccionMovimiento;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }
}
