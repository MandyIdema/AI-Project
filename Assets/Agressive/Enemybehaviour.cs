using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Start()
    {
    }


    void FixedUpdate()
    {
        //Follow the passive object
        distance = Vector2.Distance(transform.position, passive.transform.position);
        Vector2 direction = passive.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, passive.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        else
        {
            Wandering();
        }

  

    }

    void Wandering()
    {
 
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

    void FollowPassive()
    {


    }
}
