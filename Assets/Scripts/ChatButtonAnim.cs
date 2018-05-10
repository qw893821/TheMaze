using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatButtonAnim : MonoBehaviour {
    Animator anim;
    
    // Use this for initialization
    void Start () {
        anim = transform.gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
        anim.SetTrigger("Enable");
    }
}
