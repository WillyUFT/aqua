using System.Collections;
using UnityEngine;

public class ZanahoriaPekoraController : MonoBehaviour
{
    public Transform target;

    [SerializeField] public float initialSpeed;
    [SerializeField] public float speed;

    [SerializeField] public float rotateSpeed;
    private Rigidbody2D rigidBody;
    private bool seguirJugador = false;

    [SerializeField] private float tiempoParaSeguir;

    public void SetVelocidad(float valor)
    {
        speed = valor;
    }

    public float GetVelocidad()
    {
        return speed;
    }

    public float GetVelocidadInicial()
    {
        return initialSpeed;
    }

    public void SetVelocidadRotacion(float valor)
    {
        rotateSpeed = valor;
    }

    public float GetVelocidadRotacion()
    {
        return rotateSpeed;
    }

    public void SetTiempo(float valor)
    {
        tiempoParaSeguir = valor;
    }

    public float GetTiempo()
    {
        return tiempoParaSeguir;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // * Generamos un ángulo aleatorio
        float anguloinicial = Random.Range(20f, 70f);
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
        Debug.Log("Velocidad cohete: " + GetVelocidad());
        Debug.Log("Velocidad rotación cohete: " + GetVelocidadRotacion());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChocarConJugador();
        }
        else if (other.CompareTag("mapa") || other.CompareTag("suciedadCamino"))
        {
            Destroy(gameObject);
        }
    }

    void ChocarConJugador()
    {
        Debug.Log("El misil ha chocado con el jugador");
        Destroy(gameObject);
    }
}
