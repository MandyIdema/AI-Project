using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //======== HEALTH =============
    public int maxHealth;

    [SerializeField]
    private int currentHealth;
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

        healthbar.fillAmount = this.currentHealth / 100f;
        
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
            currentHealth -= 10;
            Debug.Log("Collided");

        }
    }





}
