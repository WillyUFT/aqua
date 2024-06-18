using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{

    public class DialogueBaseClass : MonoBehaviour
    {

        public bool finished { get; protected set; }

        // * Esto hace que el texto salga de a poquito, letra por letra
        protected IEnumerator EscribirTexto(
            string input, Text textHolder,
            Color TextColor, Font textFont, float velocidadTexto, AudioClip sonido,
            string nombrePersonaje, Text textHolderNombre
        )
        {

            textHolderNombre.text = nombrePersonaje;

            textHolder.color = TextColor;
            textHolder.font = textFont;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                SoundManager.instance.PlaySound(sonido);
                yield return new WaitForSeconds(velocidadTexto);
            }
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }

    }


}
