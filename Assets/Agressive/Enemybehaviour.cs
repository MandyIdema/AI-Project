using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemybehaviour : MonoBehaviour
{
    //======== WANDERING =============

    [SerializeField]
    float speed;

    [SerializeField]
    float range;

    [SerializeField]
    float maxDistance;

    Vector2 wayPoint;

    public bool wanderingBehaviour;

    //========= FOLLOW PASSIVE ============
    [SerializeField]
    public GameObject passive;

    private float distance;

    public float distanceBetween;

    public bool followPassiveBehaviour;

    //========= ATTACK PASSIVE ============

    [SerializeField]
    public static bool Attack;


    //======== ENERGY =============

    public int maxEnergy;
    private int currentEnergy;
    public Image Energybar;


    void Start()
    {
        Attack = false;
        SetNewDestination();
    }


    void Update()
    {
        //Follow the passive object
        distance = Vector2.Distance(transform.position, passive.transform.position);
        Vector2 direction = passive.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        print(passive.transform.position);
        if ((distance < distanceBetween) && passive)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, passive.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            Attack = true;
            Debug.Log("Hunting");
        }
        else
        {
            Wandering();
            Debug.Log("Wandering around");
        }

            

    }

    void Wandering()
    {
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

    void RunCheck()
    {
        Energybar.fillAmount = currentEnergy;

        if (Attack && currentEnergy >= 0)
        {
            speed = 10;
            currentEnergy -= 10;
        }
        else
        {
            speed = 3;
            Attack = false;
            Debug.Log("Not enough stamina");
        }
    }

}
