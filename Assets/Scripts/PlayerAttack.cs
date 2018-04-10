using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    bool inRange;
	// Use this for initialization
	void Start () {
        inRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
       // if()
    }
}
