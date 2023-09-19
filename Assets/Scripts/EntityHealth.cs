using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;
    public int health {get {return currentHealth; }}


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        // Might need to change this if we want damage to be preserved between fights?
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(int amount)
    // Trying to make this so that when called, health can be taken or restored
    {
        if (amount < 0)
        {
            // Conditional for if damage is taken
            // Could include animation stuff here?
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Mathf.Clamp(xValue, xMin, xMax)
        Debug.Log(currentHealth + "/" + maxHealth);
        
    }
}
