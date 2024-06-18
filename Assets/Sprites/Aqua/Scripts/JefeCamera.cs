using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class JefeCamera : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    private GameObject jugador;
    private GameObject[] jefes;
    public bool camaraJefe = false;

    private void Start()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        jugador = GameObject.FindGameObjectWithTag("Player");
        jefes = GameObject.FindGameObjectsWithTag("jefe");

        cinemachineTargetGroup.AddMember(jugador.transform, 1, 3);
        foreach (GameObject jefe in jefes)
        {
            cinemachineTargetGroup.AddMember(jefe.transform, 1, 3);
        }
    }

    private void Update()
    {
        if (camaraJefe)
        {
            cinemachineVirtualCamera.Follow = cinemachineTargetGroup.transform;
        }
        else
        {
            camaraJefe = false;
            cinemachineVirtualCamera.Follow = jugador.transform;
        }
    }

    public void setCamaraJefe(bool cam)
    {
        camaraJefe = cam;
    }
}
