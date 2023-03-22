using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Enemybehaviour : MonoBehaviour
{
    [Header("--- Movement ---")]
    //======== WANDERING =============

    [SerializeField]
    float speed;

    [SerializeField]
    float runningSpeed;

    float originalSpeed;

    [SerializeField]
    float range;

    [SerializeField]
    float maxDistance;

    Vector2 wayPoint;

    public bool wanderingBehaviour;

    [Header("--- Chasing ---")]

    //========= FOLLOW PASSIVE ============

    public List<GameObject> passiveObjects = new List<GameObject>();

    [SerializeField] private Transform[] passiveTransforms;

    private float distance;

    public float distanceBetween;

    public bool followPassiveBehaviour;

    //========= ATTACK PASSIVE ============

    [SerializeField]
    public static bool Attack;
    public float AttackPower;

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


    void Start()
    {
        SetNewDestination();

        currentHunger = maxHunger;

        currentEnergy = maxEnergy;

        currentHealth = maxHealth;

        originalSpeed = speed;
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


    private void OnCollisionStay2D(Collision2D collision)
    {
        currentHunger += AttackPower * Time.deltaTime;
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



}
