using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogHolder : MonoBehaviour
    {

        [SerializeField] private GameObject barraVidaAqua;
        [SerializeField] private GameObject barraVidPekora;
        private IEnumerator dialogSeg;
        [SerializeField] private bool activarJefe = false;

        private PlayerController playerController;
        private PlayerCombatController playerCombatController;
        private PlayerCleaningController playerCleaningController;
        [SerializeField]
        private PekoraController pekoraController;
        [SerializeField] private GameObject limitePeleaPekora;
        private GameObject cajitaNpcPekora;

        private void Start()
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            playerController = jugador.GetComponent<PlayerController>();
            playerCombatController = jugador.GetComponent<PlayerCombatController>();
            playerCleaningController = jugador.GetComponent<PlayerCleaningController>();
        }

        private void OnEnable()
        {
            dialogSeg = dialogueSequence();
            StartCoroutine(dialogSeg);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Deactivate();
                gameObject.SetActive(false);
                StopCoroutine(dialogSeg);
            }
        }

        private IEnumerator dialogueSequence()
        {
            gameObject.SetActive(true);
            barraVidaAqua.SetActive(false);
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }

            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
            barraVidaAqua.SetActive(true);
            DesactivarMovimiento(true);
            if (activarJefe)
            {
                pekoraController.SetNpc(false);
                pekoraController.gameObject.tag = "jefe";
                GameObject pekoraDialogo = pekoraController.gameObject.transform.Find("PekoraDialogo").gameObject;
                if (pekoraDialogo != null)
                {
                    pekoraDialogo.SetActive(false);
                }
                limitePeleaPekora.SetActive(true);
                barraVidPekora.SetActive(true);
            }
        }

        private void DesactivarMovimiento(bool valor)
        {
            playerController.activarDesactivarMovimiento(valor);
            playerController.SetSaltoBloqueado(!valor);
            playerCombatController.SetPuedeAtacar(valor);
            playerCombatController.SetPuedeBloquear(valor);
            playerCleaningController.SetPuedeLimpiar(valor);
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

}
