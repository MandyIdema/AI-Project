using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public GameObject AI;
    public float RangeA;
    public float RangeB;

    void Update()
    {
        
    }

    void wander()
    {
        transform.position = Vector2.MoveTowards(transform.position, AI.transform.position, 1 * Time.deltaTime);
        if (Vector2.Distance(transform.position, AI.transform.position) <= 0.5f)
        {
            AI.transform.position += new Vector3(Random.Range(RangeA, RangeB),0, Random.Range(RangeA, RangeB));
        }
    }

}
