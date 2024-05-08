using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogHolder : MonoBehaviour
    {

        [SerializeField] private GameObject barraVidaAqua;

        private void Awake()
        {
            StartCoroutine(dialogueSequence());
        }

        private IEnumerator dialogueSequence()
        {
            // barraVidaAqua.SetActive(false);
            gameObject.SetActive(true);
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            gameObject.SetActive(false);
            // yield return new WaitForSeconds(0.2f);
            // barraVidaAqua.SetActive(true);
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
