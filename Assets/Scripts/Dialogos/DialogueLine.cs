using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {


        [Header("Texto")]
        private Text textHolder;

        [SerializeField] private string input;
        [SerializeField] private Color color;
        [SerializeField] private Font font;
        [SerializeField] private float velocidadTexto;
        [SerializeField] private float delayEntreLineas;

        [Header("Nombre del personaje")]
        [SerializeField] private string nombrePersonaje;
        [SerializeField] private Text nombrePersonajeHolder;

        [Header("Audio")]
        [SerializeField] private AudioClip sonido;

        [Header("Imagenes Personaje")]
        [SerializeField] private Sprite sprite;
        [SerializeField] private Image image;

        private IEnumerator lineAppear;

        private void Awake()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";
            image.sprite = sprite;
            image.preserveAspect = true;
        }

        private void Start()
        {
            lineAppear = EscribirTexto(input, textHolder, color, font, velocidadTexto, sonido, nombrePersonaje, nombrePersonajeHolder);
            StartCoroutine(lineAppear);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textHolder.text != input)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = input;
                }
                else
                {
                    finished = true;
                }
            }
        }

    }
}