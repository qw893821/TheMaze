using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackR : MonoBehaviour {
    FoWMask fm;
    bool isContacting;
	// Use this for initialization
	void Start () {
        fm = transform.parent.gameObject.GetComponent<FoWMask>();
        fm.posBR = transform.position;
        isContacting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isContacting)
        {
            fm.posBR= transform.position;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" )
        {
            ContactPoint contact = collision.contacts[0];
            SetPoint(contact.point);
            Debug.Log(contact.point);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == "Wall" && isContacting)
        {
            isContacting = false;
            GameManager.gm.PastToNext(this.transform.gameObject);
        }
    }

    public void SetPoint(Vector3 point)
    {
        isContacting = true;
        fm.posBR = point;
    }
}
