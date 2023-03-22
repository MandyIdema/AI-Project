using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveWander : MonoBehaviour
{

    //======== WANDERING =============

    [SerializeField]
    float speed;

    [SerializeField]
    float runningSpeed;

    float originalSpeed;

    public bool Run;

    [SerializeField]
    float range;

    [SerializeField]
    float maxDistance;

    Vector2 wayPoint;



    public bool wanderingBehaviour;

    //======== ENEMY =============

    public GameObject Enemy;

    private float distance;

    public float distanceBetween;

    public GameObject Target;

    //======== ENERGY =============

    public float maxEnergy;
    private float currentEnergy;
    public Image Energybar;


    void Start()
    {
        SetNewDestination();
        Run = false;

        currentEnergy = maxEnergy;

        originalSpeed = speed;
        
        
    }

    void Update()
    {
        EnergybarCheck();


        //Flee from enemy

        Target = FindClosestEnemy();

        //Calculate the distance between the predator and prey
        distance = Vector2.Distance(transform.position, Target.transform.position);

        Vector2 direction = Target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //If the distance is small enough, RUN
        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, -speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            Debug.Log("Hunting");
            Running();
        }
        else
        {
            //If you're not chasing then wander around :)
            Wandering();
            regenerateEnergy();
            Debug.Log("Wandering around");
        }



    }

        void Wandering()
        {
            //AI wanders around the map
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, wayPoint) < range)
            {
                SetNewDestination();
            }
        }

        void SetNewDestination()
        {
            wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
        }

        void flee()
        {
            distance = Vector2.Distance(transform.position, Enemy.transform.position);
            Vector2 direction = Enemy.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (Enemybehaviour.Attack)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, Enemy.transform.position, -speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);

                Debug.Log("HELP");
            }
        }


    void EnergybarCheck()
    {
        Energybar.fillAmount = currentEnergy;
    }

    void regenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += 1f * Time.deltaTime;
        }
    }

    void Running()
    {
        if (currentEnergy >= 0)
        {
            speed = runningSpeed;
            currentEnergy -= 1f * Time.deltaTime;
        }
        else
        {
            //Returning to the originalspeed
            speed = originalSpeed;
            Run = false;
            Debug.Log("Not enough stamina");
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Agressive");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

}
