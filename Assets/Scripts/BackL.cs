using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackL : MonoBehaviour {
    public FoWMask fm;
    bool isContacting;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.pos4 = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.pos4 = transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && !isContacting)
        {
            isContacting = true;
            ContactPoint contact = collision.contacts[0];
            fm.pos4 =contact.point;        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Wall" && isContacting)
        {
            isContacting = false;
        }
    }
}
