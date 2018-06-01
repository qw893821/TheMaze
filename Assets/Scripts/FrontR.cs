using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FrontR : MonoBehaviour {
    public FoWMask fm;
    bool isContacting;
    public string nextName;
    Vector3 point;
    ContactPoint lastContact;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.posFR = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.posFR = transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" )
        {
            isContacting = true;
            ContactPoint contact = collision.contacts[0];
            point = contact.point;
            fm.posFR = point;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        lastContact = collision.contacts[0];
        Debug.Log(lastContact);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wall" && isContacting)
        {
            isContacting = false;
            nextName=GameManager.gm.PastToNext(this.transform.gameObject);
            fm.GetType().GetProperty("pos"+nextName).SetValue(fm,lastContact.point,null);
        }
        
    }
}
