using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public Image healthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        TakeDamage();
    }

    void TakeDamage()
    {

        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        healthbar.fillAmount = currentHealth / 100f;
    }

    void Damage()
    {

    }

}
