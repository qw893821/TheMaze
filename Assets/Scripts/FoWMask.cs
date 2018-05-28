using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoWMask : MonoBehaviour {
    Vector3 offSet;
	// Use this for initialization
	void Start () {
        offSet = transform.position- GameManager.gm.player.transform.position ;
	}
	
	// Update is called once per frame
	void Update () {
        MaskFollow();
	}

    void MaskFollow()
    {
        transform.position = GameManager.gm.player.transform.position + offSet;
    }
}
