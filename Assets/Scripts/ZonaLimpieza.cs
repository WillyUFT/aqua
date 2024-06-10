using UnityEngine;
using System.Collections.Generic;

public class ZonaLimpieza : MonoBehaviour
{
    private HashSet<GameObject> objetosEnZona = new HashSet<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("suciedadCamino"))
        {
            objetosEnZona.Add(other.gameObject);
            Debug.Log("Objeto añadido a la zona: " + other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("suciedadCamino"))
        {
            objetosEnZona.Remove(other.gameObject);
            Debug.Log("Objeto removido de la zona: " + other.gameObject.name);
        }
    }

    public bool EstaEnZona(GameObject objeto)
    {
        foreach (var item in objetosEnZona)
        {
            Debug.Log(item.name);
        }
        return objetosEnZona.Contains(objeto);
    }
}
