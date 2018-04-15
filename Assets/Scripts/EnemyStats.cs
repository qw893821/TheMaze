using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats{
    public int currentHealth;
    public Animator anim;
    CapsuleCollider capCol;
	// Use this for initialization
	void Start () {
        health = 10;
        currentHealth = health;
        anim = GetComponent<Animator>();
        isDead = false;
        capCol = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        Die();
	}

    void Die()
    {
        if (currentHealth <= 0)
        {
            anim.SetTrigger("dead");
            //anim.Play("dying",-1);
            isDead = true;
            capCol.enabled = false ;
            if (isDead)
            {
                anim.SetBool("walking", false);
                anim.SetBool("inCombat", false);
                Destroy(this.gameObject, 3.0f);
            }
        }
    }
}
