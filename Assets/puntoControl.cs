using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puntoControl : MonoBehaviour
{

    [SerializeField]
    private AudioClip sonido;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(sonido);
            GameManager.Instance.SetLastCheckpoint(transform.position);
        }
    }

}
