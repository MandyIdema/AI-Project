using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveWander : MonoBehaviour
{

    //======== WANDERING =============

    [SerializeField]
    float speed;

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

    //======== ENERGY =============

    public int maxEnergy;
    private int currentEnergy;
    public Image Energybar;


    void Start()
    {
        SetNewDestination();
        Run = false;

        currentEnergy = maxEnergy;
    }

    void FixedUpdate()
    {
        //Flee from enemy

        Wandering();
        RunCheck();

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


    void RunCheck()
    {
        Energybar.fillAmount = currentEnergy;

        if (Enemybehaviour.Attack && currentEnergy >= 0)
        {
            speed = 10;
            currentEnergy -= 10;
        }
        else
        {
            speed = 3;
            Run = false;
            Debug.Log("Not enough stamina");
        }
    }

}
