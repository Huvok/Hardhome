using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    Animator anim;

	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	public IEnumerator enumShowArea(string strAreaName)
    {
        anim.Play("Area_Show");
        transform.GetChild(0).GetComponent<Text>().text = strAreaName;
        transform.GetChild(1).GetComponent<Text>().text = strAreaName;
        yield return new WaitForSeconds(.8f);
        anim.Play("Area_FadeOut");
    }
}
