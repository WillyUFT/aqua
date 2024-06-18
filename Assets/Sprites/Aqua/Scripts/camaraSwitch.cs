using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class camaraSwitch : MonoBehaviour
{

    [Header("Cámaras")]
    [SerializeField]
    private CinemachineVirtualCamera camaraAqua;
    [SerializeField]
    private CinemachineVirtualCamera camaraPekora;
    [SerializeField]
    private Transform activadorCamara;

    private bool isFixedCamera = false;

    void Update()
    {
        if (transform.position.x > activadorCamara.position.x && !isFixedCamera)
        {
            CambiarCamaraPekora();
        }
        else if (transform.position.x <= activadorCamara.position.x && isFixedCamera)
        {
            CambiarCamaraAqua();
        }
    }

    private void CambiarCamaraAqua()
    {
        camaraAqua.Priority = 1;
        camaraPekora.Priority = 0;
        isFixedCamera = false;
    }

    private void CambiarCamaraPekora()
    {
        camaraAqua.Priority = 0;
        camaraPekora.Priority = 1;
        isFixedCamera = true;
    }

}
