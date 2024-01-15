using UnityEngine;
using UnityEngine.UI;

public class BarraVidaEnemigo : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private Camera camara;
 
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offSet;

    public void ActualizarVida(float vidaActual, float vidaMaxima)
    {
        slider.value = vidaActual / vidaMaxima;
    }

    void Update()
    {
        transform.rotation = camara.transform.rotation;
        transform.position = target.position + offSet;

        if (slider.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
