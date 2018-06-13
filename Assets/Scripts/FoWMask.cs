using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoWMask : MonoBehaviour {
    Vector3 offSet;
    // Use this for initialization
    void Awake () {
        Debug.Log(GameManager.gm.player.transform.position);
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
