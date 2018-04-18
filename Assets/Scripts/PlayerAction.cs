using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAction : MonoBehaviour {
    enum Mode
    {
        attack,
        chat
    }
    bool inRange;
    PlayerStats ps;
    public float timer;
    public GameObject inRangeEnemy;
    Mode playerMode;
	// Use this for initialization
	void Start () {
        timer = 0;
        inRange = false;
        ps = transform.GetComponent<PlayerStats>();
        playerMode = Mode.chat;
	}
	
	// Update is called once per frame
	void Update () {
        attack();
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeMode();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Character")
        {
            inRange = true;
            inRangeEnemy = col.transform.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
       // if()
    }

    void attack()
    {
        if (playerMode == Mode.attack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameManager.gm.gs = GameStats.attack;

            }
            if (GameManager.gm.gs == GameStats.attack)
            {

                timer = timer + Time.deltaTime;
                if (timer >= ps.attackSpeed)
                {
                    if (inRange && inRangeEnemy)
                    {
                        inRangeEnemy.GetComponent<NPCStats>().currentHealth -= ps.attackPower;
                    }
                    timer = 0;
                    GameManager.gm.gs = GameStats.other;
                    Debug.Log("attack");
                }
            }
        }
    }
    
    public void ChangeMode()
    {
        //if (Input.GetButtonDown("Fire2"))
        //{
            if (playerMode == Mode.chat)
            {
                playerMode = Mode.attack;
                
            }
            else if (playerMode == Mode.attack)
            {
                playerMode = Mode.chat;
            }
        //}
    }

    
}
