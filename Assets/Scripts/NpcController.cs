using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private PlayerCombatController playerCombatController;
    [SerializeField]
    private PlayerCleaningController playerCleaningController;
    [SerializeField]
    private PekoraController pekoraController;
    [SerializeField] private bool activarJefe = false;
    [SerializeField]
    GameObject jugador;

    public void ActivateDialogue()
    {
        if (pekoraController.GetNpc())
        {
            DesactivarMovimiento(false);
            playerController.frenarSeco();
            dialogue.SetActive(true);
        }
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
