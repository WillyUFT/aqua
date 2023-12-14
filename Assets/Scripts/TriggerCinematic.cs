using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCinematic : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Rigidbody2D jefeRigidBody;
    public GameObject jefe;
    public Animator jefeAnimator;
    [SerializeField] private CinemachineVirtualCamera jugadorCamara;
    [SerializeField] private JefeCamera jefeCamera;

    void Start()
    {
        playableDirector.stopped += OnCinematicStopped;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jefeRigidBody.gravityScale = 1;
            playableDirector.Play();
        }
    }

    private void OnCinematicStopped(PlayableDirector playableDirector)
    {
        jefeAnimator.SetBool("nada", false);
        jefeCamera.setCamaraJefe(true);
        Destroy(gameObject);
    }
}
