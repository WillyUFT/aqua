using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    [Header("Imagen Aqutan")]
    private Image imagen;

    [SerializeField]
    public Sprite imageIdle;

    [SerializeField]
    public Sprite imageDmg;

    // Start is called before the first frame update
    void Start()
    {
        imagen = GetComponent<Image>();
        if (imagen == null)
            Debug.LogError("Image component not found!");
        imagen.sprite = imageIdle;
    }

    public void ImagenDmg()
    {
        StartCoroutine(cambiarImagenDmgMomentaneo());
    }

    public void ImagenMuerto()
    {
        imagen.sprite = imageDmg;
    }

    private IEnumerator cambiarImagenDmgMomentaneo()
    {
        imagen.sprite = imageDmg;
        yield return new WaitForSeconds(1.5f);
        imagen.sprite = imageIdle;
    }
}
