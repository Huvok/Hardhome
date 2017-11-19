using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlightenedSprout : MonoBehaviour
{
    GameObject goPlayer;
    Rigidbody2D rb2d;

    private void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        rb2d = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            UIManager.fHealth -= 10f;
        }
        else if (col.tag == "Attack Zone")
        {
            if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 90)
            {
                rb2d.AddForce(new Vector2(100, 0), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 270)
            {
                rb2d.AddForce(new Vector2(-100, 0), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 180)
            {
                rb2d.AddForce(new Vector2(0, 100), ForceMode2D.Impulse);
            }
            else if (goPlayer.transform.GetChild(0).transform.localEulerAngles.z == 0)
            {
                rb2d.AddForce(new Vector2(0, -100), ForceMode2D.Impulse);
            }
        }
    }
}
