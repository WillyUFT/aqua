using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject dialogue1;
    [SerializeField] private GameObject dialogue2;
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
            dialogue1.SetActive(true);
        }
    }

    public void ActivarDialogueFinal()
    {
        if (pekoraController.GetNpc())
        {
            DesactivarMovimiento(false);
            playerController.frenarSeco();
            dialogue2.SetActive(true);
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
        return dialogue1.activeInHierarchy;
    }

}
