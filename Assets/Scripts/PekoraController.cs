using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PekoraController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private Animator animator;

    [Header("Ataque")]
    [SerializeField]
    private float fuerzaDash;

    [SerializeField]
    private float duracionDash;

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
    }
}
