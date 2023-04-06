using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    //Count the amount of objects
    public GameObject passiveobject;
    public GameObject enemyobject;
    public GameObject Plant;


    void Update()
    {
        //Counting the amount of of this prefab are in the scene
        if (GameObject.FindGameObjectsWithTag("Passive").Length < 1)
        {
            Instantiate(passiveobject);
        }

        if (GameObject.FindGameObjectsWithTag("Agressive").Length < 1)
        {
            Instantiate(enemyobject);
        }

        if (GameObject.FindGameObjectsWithTag("Plant").Length < 1)
        {
            Instantiate(Plant);
        }

        if (GameObject.FindGameObjectsWithTag("Passive").Length < 5)
        {
            Instantiate(passiveobject);
        }

    }

    void Spawn()
    {
        int spawnPointX = Random.Range(-300, 300);
        int spawnPointY = Random.Range(-150, 360);
        Vector3 spawnPosition = new Vector2(spawnPointX, spawnPointY);

        Instantiate(Plant, spawnPosition, Quaternion.identity);
    }
}
