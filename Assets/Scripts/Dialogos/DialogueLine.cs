using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {

        private Text textHolder;

        [SerializeField] private string input;

        [Header("Texto")]
        [SerializeField] private Color color;
        [SerializeField] private Font font;
        [SerializeField] private float velocidadTexto;

        [Header("Audio")]
        [SerializeField] private AudioClip sonido;

        private void Awake()
        {
            textHolder = GetComponent<Text>();
            StartCoroutine(EscribirTexto(input, textHolder, color, font, velocidadTexto, sonido));
        }




    }
}