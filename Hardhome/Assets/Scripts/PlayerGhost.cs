using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float fTimer;

	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject goPlayer = GameObject.FindGameObjectWithTag("Player");
        transform.position = goPlayer.transform.position;
        transform.localScale = goPlayer.transform.localScale;
        spriteRenderer.sprite = goPlayer.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.color = new Vector4(50, 50, 50, 0.2f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        fTimer -= Time.deltaTime;

        if (fTimer <= 0)
        {
            Destroy(gameObject);
        }
	}
}
