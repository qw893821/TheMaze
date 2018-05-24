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
        if (GameManager.gm.player.GetComponent<PlayerAction>().playerMode != Mode.chat)
        {
            transform.gameObject.SetActive(false);
        }
	}
    private void OnEnable()
    {
        anim.SetTrigger("Enable");
    }
}
