﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCStats : CharacterStats {
    public int currentHealth;
    public Animator anim;
    CapsuleCollider capCol;
    public Button chatBtn1;
    public Button chatBtn2;
    public Button chatBtn3;
    public GameObject canvGO;
    public Canvas canv;
    float timer;
    float ignoreTime;
    // Use this for initialization
    void Start() {
        health = 100;
        currentHealth = health;
        anim = GetComponent<Animator>();
        isDead = false;
        capCol = GetComponent<CapsuleCollider>();
        //chatBtn1 = transform.Find("Neutral").GetComponent<Button>();
        //chatBtn2 = transform.Find("Aggressive").GetComponent<Button>();
        //chatBtn3 = transform.Find("Friendness").GetComponent<Button>();
        //canv = transform.Find("UICanv").GetComponent<Canvas>();
        canv = canvGO.GetComponent<Canvas>();
        canv.enabled = false;
        rs = Relationship.neutral;
        opponentList = new List<GameObject>();
        friendList = new List<GameObject>();
        ignoredList = new List<GameObject>();
        currentInRangeList = new List<GameObject>();
        ignoreTime = 5.0f; 
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Die();
        ChangeRelation();
        ChangeTarget();
        Debug.Log(timer);
    }
    private void LateUpdate()
    {

    }
    void Die()
    {
        if (currentHealth <= 0)
        {
            anim.SetTrigger("dead");
            //anim.Play("dying",-1);
            isDead = true;
            capCol.enabled = false;
            if (isDead)
            {
                anim.SetBool("walking", false);
                anim.SetBool("inCombat", false);
                Destroy(this.gameObject, 3.0f);
            }
            transform.tag = "Dead";
            GameManager.gm.currentList.Remove(this.transform.gameObject);
            NavMeshAgent agent;
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
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

    public override void ChangeRelation()
    {
        base.ChangeRelation();
    }

    //when a character enters the trigger, add character to a list
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"||other.tag=="Character")
        {
            //inRange = true;
            FoV(other.transform.gameObject);
        }
    }

    public override void FoV(GameObject go)
    {
        base.FoV(go);
    }

    private float IgnoreTimer()
    {
        if (ignoredList.Contains(targetGO)||(targetGO.tag!="Character"&&targetGO.tag!="Player"))
        {
            return timer = 0;
        }
        return timer += Time.deltaTime;
        
    }

    public override void ChangeTarget()
    {
        base.ChangeTarget();
        
        if (opponentList.Contains(targetGO))
        {
            return;
        }
        else if (IgnoreTimer() >= ignoreTime&&(targetGO.tag=="Character"||targetGO.tag=="Player")&&rs!=Relationship.opponent)
        {
            ignoredList.Add(targetGO);
            targetGO = prevTargetGO;
            prevTargetGO = null;
            timer = 0;
        }
        
        
    }
}
