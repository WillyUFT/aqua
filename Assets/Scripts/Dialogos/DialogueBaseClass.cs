using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{

    public class DialogueBaseClass : MonoBehaviour
    {

        // * Esto hace que el texto salga de a poquito, letra por letra
        protected IEnumerator EscribirTexto(string input, Text textHolder, Color TextColor, Font textFont, float velocidadTexto, AudioClip sonido)
        {

            textHolder.color = TextColor;
            textHolder.font = textFont;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                // * Audio www
                yield return new WaitForSeconds(velocidadTexto);
            }
        }

    }


}
