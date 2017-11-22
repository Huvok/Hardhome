using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour
{
    public GameObject goTarget;
    public GameObject goTargetMap;
    public List<GameObject> lstgoDeactivations;
    public List<GameObject> lstgoActivations;
    public AudioClip acToStart;
    AudioCrossfader audioCrossfader;

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

    private void Start()
    {
        audioCrossfader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioCrossfader>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        collider.GetComponent<Animator>().enabled = false;
        collider.GetComponent<Player>().enabled = false;
        FadeIn();

        yield return new WaitForSeconds(fadeTime);

        if (collider.tag == "Player")
        {
            audioCrossfader.CrossFade(acToStart, 1, 3);
            collider.transform.position = goTarget.transform.GetChild(0).transform.position;
            Camera.main.GetComponent<MainCamera>().subSetBounds(goTargetMap);

            foreach (GameObject gameObject in lstgoActivations)
            {
                gameObject.SetActive(true);
            }

            foreach (GameObject gameObject in lstgoDeactivations)
            {
                gameObject.SetActive(false);
            }

            GameObject[] lstgoDestroyables = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject gameObject in lstgoDestroyables)
            {
                Destroy(gameObject);
            }
        }

        FadeOut();
        collider.GetComponent<Animator>().enabled = true;
        collider.GetComponent<Player>().enabled = true;

        StartCoroutine(area.GetComponent<Area>().enumShowArea(goTargetMap.name));
    }

    void OnGUI()
    {
        if (!start)
            return;

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        if (isFadeIn)
        {
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            if (alpha < 0) start = false;
        }

    }

    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    void FadeOut()
    {
        isFadeIn = false;
    }
}
