using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCStats : CharacterStats{
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
            NavMeshAgent agent;
            agent=GetComponent<NavMeshAgent>();
            agent.enabled = false ;
            Sink(0.2f);
        }
    }
    
    void Sink(float speed)
    {
        Vector3 targetPos;
        targetPos = transform.position;
        targetPos.y -= speed;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    
}
