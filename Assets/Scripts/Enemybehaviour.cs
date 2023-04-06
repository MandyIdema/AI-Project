using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Enemybehaviour : MonoBehaviour
{
    private float randomNumber;
    //Determine a number between 0 and 1 to determine Male or Female

    [Header("--- Gender specification ---")]

    [Tooltip("50:50 ratio on gender specification. True = Female, False = Male")]
    public bool Gender;
    //True = female ------- False = Male

    public GameObject Fox;
    public bool isCreated;

    [Header("--- Movement ---")]
    //======== WANDERING =============

    [Tooltip("Sets the speed")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float speed;

    [Tooltip("Sets the running speed")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float runningSpeed;

    float originalSpeed;

    [Tooltip("Range in which the bunny can detect plants or has to run from a predator")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float range;

    [Tooltip("Maximumdistance between the bunny and the predator")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float maxDistance;

    Vector2 wayPoint;

    public bool wanderingBehaviour;

    [Header("--- Chasing ---")]

    //========= FOLLOW PASSIVE ============

    public List<GameObject> passiveObjects = new List<GameObject>();

    [SerializeField] private Transform[] passiveTransforms;

    private float distance;

    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float distanceBetween;

    public bool followPassiveBehaviour;

    //========= ATTACK PASSIVE ============

    [SerializeField]
    public static bool Attack;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    public float AttackPower;

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


    void Start()
    {
        randomNumber = Random.Range(0, 100);
        Debug.Log(randomNumber);

        GenderSpecification();

        SetNewDestination();

        currentHunger = maxHunger;

        currentEnergy = maxEnergy;

        currentHealth = maxHealth;

        originalSpeed = speed;

        isCreated = false;
    }


    void Update()
    {
        barCheck();
        
        Target = FindClosestPassive();

        addPassivetoList();

        hungerDecrease();

        TakeDamage();

        //Calculate the distance between the predator and prey
        distance = Vector2.Distance(transform.position, Target.transform.position);

        Vector2 direction = Target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        //If the distance is small enough, chase the prey
        //The predator needs to be hungry as well in order to hunt (or else there is no use for hunting)

             if (distance < distanceBetween && currentHunger < minimumHungerlevel) 
            {
                transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
                Attack = true;
                Running();
                Debug.Log("Hunting");
            }
            else
            {
                //If you're not chasing then wander around :)
                Wandering();
                regenerateEnergy();
                Debug.Log("Wandering around");
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
        speed = originalSpeed;
        Attack = false;
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

        if(currentHunger < 0)
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
        if(currentEnergy < maxEnergy)
        {
            currentEnergy += regenerateStamina * Time.deltaTime;
        }
    }

    void Running()
    {
        if (currentEnergy >= 0)
        {
            speed = runningSpeed;
            currentEnergy -= Stamina * Time.deltaTime;
            print(currentEnergy);
        }
        else
        {
            //Returning to the originalspeed
            speed = originalSpeed;
            Debug.Log("Not enough stamina");
        }
    }

    void addPassivetoList()
    {
        foreach (GameObject passive in GameObject.FindGameObjectsWithTag("Passive"))
        {
            if (!passiveObjects.Contains(passive)){
                passiveObjects.Add(passive);
            }
        }

    }

    public GameObject FindClosestPassive()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Passive");
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        currentHunger += AttackPower * Time.deltaTime;

        if (currentHunger > hungryLevel)
        {
            if (collision.gameObject.tag == ("Passive"))
            {
                if (Gender == true)
                {
                    if (collision.gameObject.GetComponent<PassiveBehaviour>().Gender == false)
                    {
                        StartCoroutine(waitTime(1.0f));
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
            Instantiate(Fox, this.transform.position, this.transform.rotation);
            isCreated = true;
        }


    }

}
