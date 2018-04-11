using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    Vector3 target;
    GameObject playerGO;
	// Use this for initialization
	void Start () {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        
	}
	
	// Update is called once per frame
	void Update () {
        
        Movement();
	}

    void Movement()
    {
        target = playerGO.transform.position;
        target.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, target, 1.0f * Time.deltaTime);
    }
}
