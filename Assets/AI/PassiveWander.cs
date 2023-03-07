using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveWander : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        Wandering();
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
}
