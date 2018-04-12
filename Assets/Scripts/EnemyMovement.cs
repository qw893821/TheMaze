using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    Vector3 target;
    NavMeshAgent agent;
    GameObject playerGO;
    float speed;
	// Use this for initialization
	void Start () {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        agent = transform.GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        //agent.updateRotation = false;
        speed=1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        UpdateMesh();
        Debug.Log(agent.nextPosition);
    }

    void Movement()
    {
        target = playerGO.transform.position;
        target.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void UpdateMesh()
    {
        if (GameManager.gm.gs != GameStats.other)
        {
            target=playerGO.transform.position;
            target.y = 1.2f;
            agent.destination = target;
            agent.nextPosition = transform.position;
        }
    }
}
