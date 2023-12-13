using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanahoriaPekoraController : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    public float speed = 12f;

    [SerializeField]
    public float rotateSpeed = 170f;
    private Rigidbody2D rigidBody;
    private bool seguirJugador = false;

    [SerializeField]
    private float tiempoParaSeguir = 1f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // * Generamos un ángulo aleatorio
        float anguloinicial = Random.Range(30f, 60f);
        float anguloEnRadianes = anguloinicial * Mathf.Deg2Rad;

        Vector2 direccionDiagonal = new Vector2(
            Mathf.Cos(anguloEnRadianes),
            Mathf.Sin(anguloEnRadianes)
        );
        rigidBody.velocity = direccionDiagonal.normalized * speed;
        StartCoroutine(ComenzarSeguimiento());
    }

    IEnumerator ComenzarSeguimiento()
    {
        yield return new WaitForSeconds(tiempoParaSeguir);
        seguirJugador = true;
    }

    void FixedUpdate()
    {
        if (seguirJugador)
        {
            Vector2 direccion = (Vector2)target.position - rigidBody.position;
            direccion.Normalize();
            float rotateAmount = Vector3.Cross(direccion, transform.up).z;
            rigidBody.angularVelocity = -rotateAmount * rotateSpeed;
            rigidBody.velocity = transform.up * speed;
        }
        else
        {
            rigidBody.velocity = transform.up * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChocarConJugador();
        }
        else if (!other.CompareTag("jefe") && !other.CompareTag("limitesCamara"))
        {
            Debug.Log(other.name);
            ChocarConOtro();
        }
    }

    void ChocarConJugador()
    {
        Debug.Log("El misil ha chocado con el jugador");
        Destroy(gameObject);
    }

    void ChocarConOtro()
    {
        Debug.Log("El misil ha chocado con algo que no es el jugador ni el jefe");
        Destroy(gameObject);
    }
}
