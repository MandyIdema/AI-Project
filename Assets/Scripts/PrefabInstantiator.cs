using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    [Header("--- Objects ---")]
    //Count the amount of objects
    public GameObject passiveobject;
    public GameObject enemyobject;
    public GameObject plantobject;

    [Header("--- Instantiate amount of objects ---")]

    [Range(0.0f, 100.0f)]
    public float Bunny;

    [Range(0.0f, 100.0f)]
    public float Fox;

    [Range(0.0f, 100.0f)]
    public float Plant;

    private void Start()
    {
        SpawnBunny();
        SpawnFox();
        SpawnPlant();
    }


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
            Spawn();
        }

    }

    void SpawnBunny()
    {

        for (int i = 0; i < Bunny; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-40.0f, 40.0f), Random.Range(-20.0f, 20.0f));
            Instantiate(passiveobject, spawnPosition, Quaternion.identity);
        }
            
    }

    void SpawnFox()
    {

        

        for (int i = 0; i < Fox; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-40.0f, 40.0f), Random.Range(-20.0f, 20.0f));
            Instantiate(enemyobject, spawnPosition, Quaternion.identity);
        }

    }

    void SpawnPlant()
    {


        for (int i = 0; i < Plant; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-40.0f, 40.0f), Random.Range(-20.0f, 20.0f));
            Instantiate(plantobject, spawnPosition, Quaternion.identity);
        }

    }

    void Spawn()
    {
        int spawnPointX = Random.Range(-46, 21);
        int spawnPointY = Random.Range(40, -20);
        Vector2 spawnPosition = new Vector2(Random.Range(-40.0f, 40.0f), Random.Range(-20.0f, 20.0f));

        Instantiate(plantobject, spawnPosition, Quaternion.identity);
    }
}
