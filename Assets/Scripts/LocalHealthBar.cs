using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHealthBar : MonoBehaviour {
    NPCStats ns;
    RectTransform rt;
	// Use this for initialization
	void Start () {
        ns = transform.GetComponentInParent<NPCStats>();
        rt = transform.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rt.sizeDelta=new Vector2(Mathf.Lerp(0.01f,1.0f,(ns.currentHealth)/50),0.2f);
        
	}
}
