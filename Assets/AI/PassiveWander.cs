using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveWander : MonoBehaviour
{
  

    [Header("--- Movement ---")]

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


    [Header("--- Fleeing ---")]

    //======== ENEMY =============

    public GameObject Enemy;

    private float distance;

    public float distanceBetween;

    public GameObject Target;

    [Header("--- Health ---")]

    //======== HEALTH =============
    public float maxHealth;
    public float Damage;

    [SerializeField]
    private float currentHealth;
    public Image healthbar;


    [Header("--- Energy ---")]
    //======== ENERGY =============

    public float maxEnergy;
    private float currentEnergy;
    public Image Energybar;

    public float Stamina;
    public float regenerateStamina;

    [Header("--- Hunger ---")]

    //======== HUNGER =============
    public float maxHunger;
    private float currentHunger;
    public Image Hungerbar;

    public float minimumHungerlevel;
    public float hungryLevel;

    public GameObject Plant;
    public GameObject FoodTarget;
    private float Fooddistance;
    public float FooddistanceBetween;


    void Start()
    {
        SetNewDestination();

        currentHunger = maxHunger;

        currentHealth = maxHealth;

        currentEnergy = maxEnergy;

        originalSpeed = speed;

        Run = false;

    }

    void Update()
    {
        barCheck();

        hungerDecrease();

        TakeDamage();


        //Flee from enemy

        Target = FindClosestEnemy();
        FoodTarget = FindClosestFoodSource();

        //Calculate the distance between the predator and prey
        distance = Vector2.Distance(transform.position, Target.transform.position);
        Fooddistance = Vector2.Distance(transform.position, FoodTarget.transform.position);


        Vector2 direction = Target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector2 Fooddirection = FoodTarget.transform.position - transform.position;
        Fooddirection.Normalize();
        float Foodangle = Mathf.Atan2(Fooddirection.y, Fooddirection.x) * Mathf.Rad2Deg;

        //If the distance is small enough, RUN
        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, -speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            Debug.Log("Fleeing");
            Running();
        }
        else
        {
            if(currentHunger > 75)
            {
                //If you're not chasing then wander around :)
                Wandering();
                regenerateEnergy();
                Debug.Log("Wandering around");
            }
            else
            {
                if (Fooddistance < FooddistanceBetween)
                {
                    transform.position = Vector2.MoveTowards(this.transform.position, FoodTarget.transform.position, speed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(Vector3.forward * Foodangle);
                    Debug.Log("Searching for food");
                }
                else
                {
                    Wandering();
                    regenerateEnergy();
                    Debug.Log("Wandering around");
                }
            }
        
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


    void barCheck()
    {
        Energybar.fillAmount = currentEnergy / maxEnergy;
        Hungerbar.fillAmount = currentHunger / maxHunger;
        healthbar.fillAmount = currentHealth / maxHealth;
    }

    void TakeDamage()
    {

        if (this.currentHealth <= 0)
        {
            Death();
        }

        if (currentHunger < 0)
        {
            currentHealth -= Damage * Time.deltaTime;
        }

    }

    void Death()
    {
        Destroy(this.gameObject);

        //Destroy
        print("I died :(");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == ("Agressive"))
        {
            currentHealth -= Damage * Time.deltaTime;
            Debug.Log("Collided");

        }
    }

    void hungerDecrease()
    {
        currentHunger -= hungryLevel * Time.deltaTime;
    }

    void regenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += Stamina * Time.deltaTime;
        }
    }

    void Running()
    {
        if (currentEnergy >= 0)
        {
            speed = runningSpeed;
            currentEnergy -= regenerateStamina * Time.deltaTime;
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

    public GameObject FindClosestFoodSource()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Plant");
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


    // EATING PLANTS

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FoodTarget = FindClosestFoodSource();

        if (collision.gameObject.tag == ("Plant"))
        {

            Destroy(FoodTarget);
            currentHunger += 50 * Time.deltaTime;

        }
    }

}
