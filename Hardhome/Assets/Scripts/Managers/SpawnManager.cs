﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//                                                          //AUTHOR: Hugo Garcia 
//                                                          //Date: 11/17/2017 
//                                                          //PURPOSE: Manager for enemy spawns. 

//====================================================================================================================== 
public class SpawnManager : MonoBehaviour
{
    public GameObject goEnemyToSpawn;
    public List<Transform> lsttransformEnemyPositions;
    private bool boolHasSpawned = false;

    //------------------------------------------------------------------------------------------------------------------ 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && 
            !boolHasSpawned)
        {
            subSpawnEnemies();
            boolHasSpawned = true;
            subExecuteBasedOnCurrentMap();
        }
    }

    //------------------------------------------------------------------------------------------------------------------ 
    void subSpawnEnemies()
    {
        for (int i = 0; i < lsttransformEnemyPositions.Count; i++)
        {
            Instantiate(goEnemyToSpawn, lsttransformEnemyPositions[i].position, lsttransformEnemyPositions[i].rotation);
        }
    }

    void subExecuteBasedOnCurrentMap()
    {
        if (MapManager.strCurrentMap == "Ancient Grove")
        {
            MapManager.subActivateWalls(MapManager.strCurrentMap);
        }
        else if (MapManager.strCurrentMap == "Forsaken Field")
        {
            MapManager.subActivateWalls(MapManager.strCurrentMap);
        }
        else if (MapManager.strCurrentMap == "Swept Cathedral")
        {
            MapManager.subActivateWalls(MapManager.strCurrentMap);
        }
        else if (MapManager.strCurrentMap == "Silent Ghetto")
        {
            MapManager.subActivateWalls(MapManager.strCurrentMap);
        }
        else if (MapManager.strCurrentMap == "Hope Prelude")
        {
            MapManager.subActivateWalls(MapManager.strCurrentMap);
        }
    }
} 
//======================================================================================================================