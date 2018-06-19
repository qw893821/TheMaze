using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleJAnim : MonoBehaviour {
    Animator anim;
	// Use this for initialization
	void Start () {
        anim = transform.GetComponent<Animator>();
        anim.SetBool("walking", true);
    }
	
	
}
