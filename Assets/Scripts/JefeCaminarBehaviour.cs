using System.Collections;
using System.Data.Common;
using UnityEngine;

public class JefeCaminarBehaviour : StateMachineBehaviour
{
    private BossController bossController;
    private Rigidbody2D rigidBody;

    bool triggerCambioDireccion = false;

    [SerializeField] private float velocidadMovimiento;

    [SerializeField] private float distanciaUmbral;

    private float tiempoEnMovimiento;


    [SerializeField] private float[] probabilidadesEstado;

    private PekoraController pekoraController;
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        bossController = animator.GetComponent<BossController>();
        pekoraController = animator.GetComponent<PekoraController>();
        rigidBody = bossController.rigidBody;
        bossController.MirarJugador();

    }

    public void SetVelocidadMovimiento(float valor)
    {
        velocidadMovimiento = valor;
    }

    public float GetVelocidadMovimiento()
    {
        return velocidadMovimiento;
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex
    )
    {
        pekoraController.moverHaciaJugador(velocidadMovimiento);
        // activarAtaques(animator);

    }

    public void activarAtaques(Animator animator)
    {

        float distanciaJugador = bossController.getDistanciaJugador();

        if (distanciaJugador >= 6)
        {
            animator.SetBool("rocket", true);
        }
        else if (distanciaJugador <= 4.5)
        {
            animator.SetBool("attack", true);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }
}
