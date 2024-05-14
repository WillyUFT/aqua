using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    private PlayerController playerController;
    private PlayerCombatController playerCombatController;
    private PlayerCleaningController playerCleaningController;

    private void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        playerController = jugador.GetComponent<PlayerController>();
        playerCombatController = jugador.GetComponent<PlayerCombatController>();
        playerCleaningController = jugador.GetComponent<PlayerCleaningController>();
    }

    public void ActivateDialogue()
    {
        DesactivarMovimiento(false);
        playerController.frenarSeco();
        dialogue.SetActive(true);
    }

    public void DesactivarMovimiento(bool valor)
    {
        playerController.activarDesactivarMovimiento(valor);
        playerController.SetSaltoBloqueado(!valor);
        playerCombatController.SetPuedeAtacar(valor);
        playerCombatController.SetPuedeBloquear(valor);
        playerCleaningController.SetPuedeLimpiar(valor);
    }

    public bool DialogueActive()
    {
        return dialogue.activeInHierarchy;
    }

}
