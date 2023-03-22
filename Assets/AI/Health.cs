using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //======== HEALTH =============
    public int maxHealth;

    [SerializeField]
    private float currentHealth;
    public Image healthbar;



    void Start()
    {
        currentHealth = maxHealth;
        
        
    }

    void FixedUpdate()
    {
        TakeDamage();


    }

    void TakeDamage()
    {

        if(this.currentHealth <= 0)
        {
            Death();
        }

        healthbar.fillAmount = this.currentHealth / maxHealth;
        
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
            currentHealth -= 5f * Time.deltaTime;
            Debug.Log("Collided");

        }
    }





}
