using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontL : MonoBehaviour {
    FoWMask fm;
    bool isContacting;
    ContactPoint lastContact;
    string nextName;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.posFL = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.posFL = transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isContacting = true;
            ContactPoint contact = collision.contacts[0];
            fm.posFL =contact.point;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        lastContact = collision.contacts[0];

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
            isContacting = false;
            nextName = GameManager.gm.PastToNext(this.transform.gameObject);
            fm.GetType().GetProperty("pos" + nextName).SetValue(fm, lastContact.point, null);
        }
    }
}
