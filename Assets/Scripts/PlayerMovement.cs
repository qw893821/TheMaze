using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Vector3 target;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        //RotatePlayer();

    }

    void Movement()
    {
        if (Input.GetKey("w"))
        {
            GameManager.gm.gs = GameStats.walking;
        }
        else if (GameManager.gm.gs == GameStats.other)
        {
            if (Input.GetKeyDown("d"))
            {
                transform.Rotate(0, 90, 0);
                GameManager.gm.gs = GameStats.turn;
                //target = Quaternion.AngleAxis(90, Vector3.forward) * transform.position;
            }
            if (Input.GetKeyDown("a"))
            {
                transform.Rotate(0, -90, 0);
                //target = Quaternion.AngleAxis(-90, Vector3.forward) * transform.position;
                GameManager.gm.gs = GameStats.turn;
                //Debug.Log(target);
            }
            if (Input.GetKeyDown("s"))
            {
                transform.Rotate(0, 180, 0);
                GameManager.gm.gs = GameStats.turn;
            }
        }
        else if(GameManager.gm.gs!=GameStats.attack )
        { GameManager.gm.gs = GameStats.other; }
        if(GameManager.gm.gs == GameStats.walking)
        {
            transform.position += transform.forward * Time.deltaTime;
        }
        if (transform.rotation.y == 0 || transform.rotation.y == -180)
        {
            GameManager.gm.facing = "Vertical";
        }
        else { GameManager.gm.facing = "Horizontal"; }
    }

    /*void RotatePlayer()
    {
        if (GameManager.gm.gs == GameStats.other) {
            if (Input.GetKey("d"))
            {
                GameManager.gm.gs = GameStats.turn;
                target = Quaternion.AngleAxis(90, Vector3.forward)*transform.position;
            }
            if (Input.GetKey("a"))
            {
                target = Quaternion.AngleAxis(-90, Vector3.forward) * transform.position;
                GameManager.gm.gs = GameStats.turn;
                Debug.Log(target);
            }
        }
        if (GameManager.gm.gs==GameStats.turn)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target, 0.1f,0.0f));
        }
    }*/
}
