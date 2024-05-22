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

    void Update()
    {

        transform.position = target.position + offSet;

    }

}
