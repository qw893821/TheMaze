using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontR : MonoBehaviour {
    public FoWMask fm;
    bool isContacting;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.pos2 = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.pos2 = transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && !isContacting)
        {
            isContacting = true;
            ContactPoint contact = collision.contacts[0];
            fm.pos2 =contact.point;        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wall" && isContacting)
        {
            Debug.Log("exit");
            isContacting = false;
        }
    }
}
