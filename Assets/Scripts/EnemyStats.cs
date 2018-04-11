using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats{
    public int currentHealth;
	// Use this for initialization
	void Start () {
        health = 100;
        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
        Die();
	}

    void Die()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
