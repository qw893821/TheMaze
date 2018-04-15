using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    Vector3 target;
    NavMeshAgent agent;
    GameObject playerGO;
    Animator anim;
    EnemyStats es;
    float speed;
	// Use this for initialization
	void Start () {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        agent = transform.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        //agent.updateRotation = false;
        speed=1.0f;
        anim = GetComponent<Animator>();
        es = GetComponent<EnemyStats>();
    }
	
	// Update is called once per frame
	void Update () {
        //Movement();
        //UpdateMesh();
        
    }

    private void LateUpdate()
    {
        UpdateMesh();
        ChangeAnim();
    }

    void Movement()
    {
        target = playerGO.transform.position;
        target.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void UpdateMesh()
    {
        //fix the conflict between animation and navmesh
        if (GameManager.gm.gs != GameStats.other&&agent.isActiveAndEnabled)
        {
            target=playerGO.transform.position;
            target.y = 1.2f;
            agent.destination = target;
            Vector3 agentPosFix;
            agentPosFix = agent.nextPosition;
            agentPosFix.y += 1.2f;
            agent.nextPosition = agentPosFix;
            transform.position = agentPosFix;
            
            
        }
    }

    void ChangeAnim()
    {
        //https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        //check the destination
        if (!es.isDead)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath)
                    {
                        return;
                    }
                    anim.SetBool("inCombat",true);
                    anim.SetBool("walking",false);
                }
                else
                {
                    anim.SetBool("walking",true);
                    anim.SetBool("inCombat", false);
                }
            }
            
        }
        else { Debug.Log(es.isDead); }
    }
}
