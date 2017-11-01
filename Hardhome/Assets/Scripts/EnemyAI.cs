using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	/*
	private Vector3 Player;
	private Vector2 PlayerDirection;
	private float DifX;
	private float DifY;
	private float speed;

	void Start(){
		speed = 10;
	}
	// Update is called once per frame
	void Update () {
		Player = GameObject.Find ("Player").transform.position;
		DifX = Player.x - transform.position.x;
		DifY = Player.y - transform.position.y;

		PlayerDirection = new Vector2 (DifX, DifY);
		transform.Translate (PlayerDirection * speed);
	*/
	public Transform target;
	public float speed = 2f;
	private float minDistance = 1f;
	private float range;
	void Update ()
	{
		range = Vector2.Distance(transform.position, target.position);

		if (range > minDistance)
		{
			Debug.Log(range);

			transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}
}

