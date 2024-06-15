using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogHolder : MonoBehaviour
    {

        [Header("Barras de vida")]
        [SerializeField] private GameObject barraVidaAqua;
        [SerializeField] private GameObject barraVidPekora;
        [SerializeField] private GameObject barraLimpieza;
        private IEnumerator dialogSeg;
        [SerializeField] private bool activarJefe = false;
        [SerializeField] private GameObject[] suciedadPekora;

        private PlayerController playerController;
        private PlayerCombatController playerCombatController;
        private PlayerCleaningController playerCleaningController;
        [SerializeField]
        private PekoraController pekoraController;
        [SerializeField] private GameObject limitePeleaPekora;
        private GameObject cajitaNpcPekora;

        [Header("Música")]
        [SerializeField] AudioClip musicaBoss;
        private AudioSource audioSource;

        private GameObject[] suciedadesPekora;


        private void Start()
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            playerController = jugador.GetComponent<PlayerController>();
            playerCombatController = jugador.GetComponent<PlayerCombatController>();
            playerCleaningController = jugador.GetComponent<PlayerCleaningController>();
            audioSource = Camera.main.GetComponent<AudioSource>();

        }

        private void OnEnable()
        {
            dialogSeg = dialogueSequence();
            StartCoroutine(dialogSeg);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape) && transform.name != "cuadro dialogo Final")
            {
                Deactivate();
                gameObject.SetActive(false);
                StopCoroutine(dialogSeg);
                VolverJugar();
            }
            else if (Input.GetKey(KeyCode.Escape) && transform.name == "cuadro dialogo Final")
            {
                {
                    FadeOutController fadeController = FindObjectOfType<FadeOutController>();
                    if (fadeController != null)
                    {
                        fadeController.fadeout();
                    }
                }
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
                if (transform.GetChild(i).gameObject.name == "Dialogo5Revivir")
                {
                    pekoraController.animator.SetTrigger("revivir");
                }
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }

            yield return new WaitForSeconds(0.2f);

            if (transform.name != "cuadro dialogo Final")
            {
                VolverJugar();
            }
            else
            {
                FadeOutController fadeController = FindObjectOfType<FadeOutController>();
                if (fadeController != null)
                {
                    fadeController.fadeout();
                }
            }

        }

        private void VolverJugar()
        {
            gameObject.SetActive(false);
            barraVidaAqua.SetActive(true);
            DesactivarMovimiento(true);
            if (activarJefe)
            {
                pekoraController.SetNpc(false);
                pekoraController.gameObject.tag = "jefe";
                ActivarSuciedadPekora();
                GameObject pekoraDialogo = pekoraController.gameObject.transform.Find("PekoraDialogo").gameObject;
                if (pekoraDialogo != null)
                {
                    pekoraDialogo.SetActive(false);
                }
                limitePeleaPekora.SetActive(true);
                encenderBarraLimpieza();
                barraVidPekora.SetActive(true);
                audioSource.clip = musicaBoss;
                audioSource.Play();
            }
        }

        private void encenderBarraLimpieza()
        {
            barraLimpieza.SetActive(true);

            float cantidadVida = 0;
            suciedadesPekora = GameObject.FindGameObjectsWithTag("suciedad");
            foreach (var item in suciedadesPekora)
            {
                cantidadVida += item.GetComponent<SuciedadController>().vidaMaximaSuciedad;
            }
            barraLimpieza.GetComponent<BarraLimpieza>().SetVidaInicial(cantidadVida);
        }

        private void ActivarSuciedadPekora()
        {
            foreach (var suciedad in suciedadPekora)
            {
                suciedad.SetActive(true);
            }
        }

        private void DesactivarMovimiento(bool valor)
        {
            playerController.activarDesactivarMovimiento(valor);
            playerController.animator.SetBool("walk", false);
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
