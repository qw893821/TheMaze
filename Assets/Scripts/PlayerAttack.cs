using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    bool inRange;
    PlayerStats ps;
    public float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
        inRange = false;
        ps = transform.GetComponent<PlayerStats>();
	}
	
	// Update is called once per frame
	void Update () {
        attack();

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
       // if()
    }

    void attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameManager.gm.gs = GameStats.attack;
            
        }
        if(GameManager.gm.gs == GameStats.attack)
        {
            timer = timer + Time.deltaTime;
            if (timer >= ps.attackSpeed)
            {
                Debug.Log("attack");
                timer = 0;
                GameManager.gm.gs = GameStats.other;
            }
        }
    }
}
