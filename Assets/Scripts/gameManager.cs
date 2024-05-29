using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private NpcController npcController;
    void Start()
    {
        npcController = GetComponent<NpcController>();
        dialogue.SetActive(true);
        npcController.ActivateDialogue();
    }

    void Update()
    {

    }
}
