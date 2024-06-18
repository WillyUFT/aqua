using UnityEngine;
using UnityEngine.UI;

public class botonNpc : MonoBehaviour
{

    [SerializeField]
    private Image botonE;

    [SerializeField]
    private Vector3 offSet;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Camera camara;

    public Transform jefeTransform;
    public RectTransform uiElement;

    void Update()
    {

        transform.position = target.position + offSet;
        if (jefeTransform.localScale.x < 0)
        {
            // Jefe está volteado a la izquierda
            uiElement.localScale = new Vector3(-1, 1, 1); // Corrige la escala del UI
        }
        else
        {
            // Jefe está volteado a la derecha
            uiElement.localScale = new Vector3(1, 1, 1); // Asegura que la escala del UI sea correcta
        }

    }

}
