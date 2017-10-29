using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour
{
    public GameObject goTarget;
    public GameObject goTargetMap;

    // Para controlar si empieza o no la transición
    bool start = false;
    // Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;
    // Opacidad inicial del cuadrado de transición
    float alpha = 0;
    // Transición de .8 segundo
    float fadeTime = .8f;

    GameObject area;

    private void Awake()
    {
        Assert.IsNotNull(goTarget);

        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        Assert.IsNotNull(goTargetMap);

        area = GameObject.FindGameObjectWithTag("Zone Name");
    }

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Animator>().enabled = false;
        collider.GetComponent<Player>().enabled = false;
        FadeIn();

        yield return new WaitForSeconds(fadeTime);

        if (collider.tag == "Player")
        {
            collider.transform.position = goTarget.transform.GetChild(0).transform.position;
            Camera.main.GetComponent<MainCamera>().subSetBounds(goTargetMap);
        }

        FadeOut();
        collider.GetComponent<Animator>().enabled = true;
        collider.GetComponent<Player>().enabled = true;

        StartCoroutine(area.GetComponent<Area>().enumShowArea(goTargetMap.name));
    }

    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI()
    {
        // Si no empieza la transición salimos del evento directamente
        if (!start)
            return;

        // Si ha empezamos creamos un color con una opacidad inicial a 0
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Controlamos la transparencia
        if (isFadeIn)
        {
            // Si es la de aparecer le sumamos opacidad
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            // Si es la de desaparecer le restamos opacidad
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            // Si la opacidad llega a 0 desactivamos la transición
            if (alpha < 0) start = false;
        }

    }

    // Método para activar la transición de entrada
    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    // Método para activar la transición de salida
    void FadeOut()
    {
        isFadeIn = false;
    }
}
