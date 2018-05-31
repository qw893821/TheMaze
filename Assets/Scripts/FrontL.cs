﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontL : MonoBehaviour {
    public FoWMask fm;
    bool isContacting;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.pos1 = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.pos1 = transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall"&&!isContacting)
        {
            isContacting = true;
            ContactPoint contact = collision.contacts[0];
            Debug.Log(contact.point);
            fm.pos1 =contact.point;        }
    }

   /* private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("out");
            
        }
    }
    */
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall" && isContacting)
        {
            Debug.Log("Exit");
            isContacting = false;
        }
    }
}
