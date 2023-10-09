using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveBehaviour : MonoBehaviour
{


    private float randomNumber;
    //Determine a number between 0 and 1 to determine Male or Female

    [Header("--- Gender specification ---")]

    [Tooltip("50:50 ratio on gender specification. True = Female, False = Male")]
    public bool Gender;
    //True = female ------- False = Male

    public GameObject Bunny;
    public bool isCreated;


    [Header("--- Movement ---")]

    //======== WANDERING =============

    [Tooltip("Sets the speed")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float speed;

    [Tooltip("Sets the running speed")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float runningSpeed;

    float originalSpeed;

    public bool Run;

    [Tooltip("Range in which the bunny can detect plants or has to run from a predator")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float range;

    [Tooltip("Maximumdistance between the bunny and the predator")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float maxDistance;

    Vector2 wayPoint;



    public bool wanderingBehaviour;


    [Header("--- Fleeing ---")]

    //======== ENEMY =============

    
    public GameObject Enemy;

    private float distance;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float distanceBetween;

    public GameObject Target;

    [Header("--- Health ---")]

    //======== HEALTH =============
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float maxHealth;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float Damage;

    [SerializeField]
    private float currentHealth;
    public Image healthbar;


    [Header("--- Energy ---")]
    //======== ENERGY =============

    [Tooltip("Maximum amount of energy")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float maxEnergy;
    private float currentEnergy;
    public Image Energybar;

    [Tooltip("How fast the energy bar drains")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float Stamina;

    [Tooltip("How fast your energy regenerates")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float regenerateStamina;

    [Header("--- Hunger ---")]

    //======== HUNGER =============
    [Tooltip("Maximum amount of hunger")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float maxHunger;
    private float currentHunger;
    public Image Hungerbar;

    [Tooltip("Minimum level the hunger needs to be in order to seek for food")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float minimumHungerlevel;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float hungryLevel;

    public GameObject Plant;
    public GameObject FoodTarget;
    private float Fooddistance;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float FooddistanceBetween;


    void Start()
    {
        randomNumber = Random.Range(0, 100);
        //Get random number for gender specification

        GenderSpecification();

        SetNewDestination();

        currentHunger = maxHunger;

        currentHealth = maxHealth;

        currentEnergy = maxEnergy;

        originalSpeed = speed;

        isCreated = false;

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

        void GenderSpecification()
        {
        if (randomNumber > 50)
        {
            Gender = true; //FEMALE
        }
        else
        {
            Gender = false; // MALE
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

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == ("Agressive"))
        {
            currentHealth -= Damage * Time.deltaTime;
            Debug.Log("Collided");

        }

        FoodTarget = FindClosestFoodSource();

        if (collision.gameObject.tag == ("Plant"))
        {

            Destroy(FoodTarget);
            currentHunger += 100 * Time.deltaTime;

        }

        //Spawn a new bunny BUT wait a few seconds before making a new one
        //A female bunny can not pop out a child every second... that would be cruel
        if(currentHunger > hungryLevel)
        {
            if (collision.gameObject.tag == ("Passive"))
            {
                if (Gender == true)
                {
                    if (collision.gameObject.GetComponent<PassiveBehaviour>().Gender == false)
                    {
                        StartCoroutine(waitTime(5.0f));
                    }
                }

            }
        }

    }



    IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isCreated)
        {
            Instantiate(Bunny, this.transform.position, this.transform.rotation);
            isCreated = true;
        }
        

    }


}
