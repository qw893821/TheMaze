using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackBar : MonoBehaviour {
    NPCStats ns;
    RectTransform rt;
	// Use this for initialization
	void Start () {
        ns = transform.GetComponentInParent<NPCStats>();
        rt = transform.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        BarUpdate();

    }

    void BarUpdate()
    {
        if (ns.satisfaction >= 0)
        {
            rt.sizeDelta = new Vector2(1,0.1f);
        }
        else if (ns.satisfaction < -50)
        {
            Debug.Log("-50");
            rt.sizeDelta = new Vector2(0,0.1f);
        }
        else
        {
            
            rt.sizeDelta = new Vector2(((ns.satisfaction+50.0f)/50.0f),0.1f);
        }
    }
}
