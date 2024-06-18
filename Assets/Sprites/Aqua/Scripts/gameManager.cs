using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject dialogue;
    [SerializeField] private NpcController npcController;

    private Vector2 lastCheckpointPosition;
    private Vector2 playerPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        npcController = GetComponent<NpcController>();
        if (dialogue != null)
        {
            dialogue.SetActive(true);
            npcController.ActivateDialogue();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // ! ========================== PUNTO DE CONTROL ========================= */
    public void SetLastCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    public Vector2 GetLastCheckpoint()
    {
        return lastCheckpointPosition;
    }

    public void SavePlayerPosition(Vector2 position)
    {
        playerPosition = position;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && playerPosition != Vector2.zero)
        {
            player.transform.position = playerPosition;
        }
    }
}
