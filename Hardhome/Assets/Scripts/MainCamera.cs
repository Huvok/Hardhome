using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public float smoothTime = .3f;

    Transform target;
    float tLX, tLY, bRX, bRY;

    Vector2 velocity;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update ()
    {
        float numPosX = Mathf.Round(
            Mathf.SmoothDamp(transform.position.x, target.position.x,
            ref velocity.x, smoothTime) * 100) / 100;

        float numPosY = Mathf.Round(
            Mathf.SmoothDamp(transform.position.y, target.position.y,
            ref velocity.y, smoothTime) * 100) / 100;

        transform.position = new Vector3(
            Mathf.Clamp(numPosX, tLX, bRX),
            Mathf.Clamp(numPosY, bRY, tLY),
            transform.position.z
        );
	}

    public void subSetBounds(GameObject goMap)
    {
        Tiled2Unity.TiledMap tmConfig = goMap.GetComponent<Tiled2Unity.TiledMap>();
        float numCameraSize = Camera.main.orthographicSize;

        tLX = goMap.transform.position.x + numCameraSize * 1.78f;
        tLY = goMap.transform.position.y - numCameraSize;
        bRX = goMap.transform.position.x + tmConfig.NumTilesWide - numCameraSize * 1.78f;
        bRY = goMap.transform.position.y - tmConfig.NumTilesHigh + numCameraSize;

        subFastCameraMove();
    }

    public void subFastCameraMove()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z
        );
    }
}
