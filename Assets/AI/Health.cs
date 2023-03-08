using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //======== HEALTH =============
    public int maxHealth;
    private int currentHealth;


    //======== ENERGY =============

    public int maxEnergy;
    private int currentEnergy;
    public Image Energybar;

    float totalDistance;
    Vector2 oldpos;
    Vector2 Currentpos;


    public Image healthbar;

    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        oldpos = transform.position;
        
    }

    void FixedUpdate()
    {
        TakeDamage();
        Damage();

        Currentpos = transform.position;
        oldpos = transform.position;
    }

    void TakeDamage()
    {

        if(currentHealth <= 0)
        {
            Death();
        }

        healthbar.fillAmount = currentHealth / 100f;
        
    }

    void Death()
    {
        this.gameObject.transform.position = new Vector2(30,30);

        // Destroy(this.gameObject);
        print("I died :(");
    }

    void Damage()
    {

        if (this.gameObject.tag == "Passive")
        {
            if (Enemybehaviour.Attack)
            {
                currentHealth -= 10;
            }
        }

    }



    public void EnergyUse()
    {
        if(currentEnergy  >= 0)
        {
            currentEnergy -= 10;
            Energybar.fillAmount = currentEnergy;
        }
        else
        {
            Debug.Log("Not enough stamina");
        }
    }

}
