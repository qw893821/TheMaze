using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoWMask : MonoBehaviour {
    Vector3 offSet;
    GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        offSet = transform.position-GameManager.gm.player.transform.position;
    }
	
	void Update () {
        MaskFollow();
    }
    private void FixedUpdate()
    {
       // UpdateMeshPos();
    }

    void MaskFollow()
    {
        transform.position = GameManager.gm.player.transform.position + offSet;
    }

}
