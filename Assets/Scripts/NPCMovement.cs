using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour {
    Vector3 target;
    NavMeshAgent agent;
    Animator anim;
    NPCStats ns;
    float speed;
    bool targeted;
    GameObject targetGO;
    public GameObject targetingGO;

	// Use this for initialization
	void Start () {
        //targetGO = GameObject.FindGameObjectWithTag("Player");
        agent = transform.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        speed=1.0f;
        anim = GetComponent<Animator>();
        ns = GetComponent<NPCStats>();
        targeted = false;
    }
	
	// Update is called once per frame
	void Update () {
        //Movement();
        //UpdateMesh();
        targetGO = transform.GetComponent<NPCStats>().targetGO;
    }

    private void LateUpdate()
    {
        UpdateMesh();
        ChangeAnim();
    }

    void Movement()
    {
        target = targetGO.transform.position;
        target.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void UpdateMesh()
    {
        //fix the conflict between animation and navmesh

        if (GameManager.gm.gs != GameStats.other&&agent.isActiveAndEnabled&& !agent.pathPending)
        {

            if (transform.tag == "Neutral")
            {
                if (!targeted)
                {
                    agent.destination = new Vector3(Random.Range(-10f, 10f), 1.2f, Random.Range(-10f, 10f));
                    agent.stoppingDistance = 1f;
                    targeted = true;
                }
            }
            else
            {
                target = targetGO.transform.position;
                target.y = 1.2f;
                agent.destination = target;
                if (transform.tag == "Teammember")
                {
                    agent.stoppingDistance = 3f;
                }
            }
            Vector3 agentPosFix;
            //agentPosFix = agent.nextPosition;
            //Debug.Log(agent.nextPosition);
            agentPosFix = Repath(agent);
            agentPosFix.y = 1.2f;
            agent.nextPosition = agentPosFix;
            //trun npc to the direction they want to go
            transform.LookAt(agentPosFix);
            transform.position = agentPosFix;
        }
    }

    void ChangeAnim()
    {
        //https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        //check the destination
        if (!ns.isDead)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath)
                    {
                        return;
                    }
                    targeted = false;
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
        else { Debug.Log(ns.isDead); }
    }
    //convert the navmeshagent direction to hori/vert only. meet player's moveing rule
    Vector3 Repath(NavMeshAgent agent)
    {
        Vector3 target;
        Vector3 dir;
        float angle;
        target = agent.nextPosition;
        dir = target - transform.position;
        dir.y = 0;
        //dir.y = dir.y + 1.2f;
        //unity angle value will be 0-180
        angle = Vector3.Angle(Vector3.forward, dir);
        //this condition missing some condition check. so target is not working
        if (angle <= 45)
        {
            //target = Vector3.forward * dir.magnitude;
            dir = Vector3.forward * dir.magnitude;
        }
        else if (angle >= 135)
        {
            //target = Vector3.back * dir.magnitude;
            dir = Vector3.forward * -dir.magnitude;
        }
        else
        {
            float newAngle;
            newAngle = Vector3.Angle(Vector3.left, dir);
            if (newAngle < 45)
            {
                //target = Vector3.right * dir.magnitude;
                dir = Vector3.left * dir.magnitude;
            }
            else if (newAngle > 135)
            {
                //target = Vector3.left * dir.magnitude;
                dir = Vector3.left * -dir.magnitude;
            }
        }
        target = dir + transform.position;
        return target;
    }
}
