﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    RaycastHit hit;
    //public List<GameObject> Enemylist;
    Mode playerMode;
    public Camera camera;
	// Use this for initialization
	void Start () {
        timer = 0;
        inRange = false;
        ps = transform.GetComponent<PlayerStats>();
        playerMode = Mode.chat;
        //Enemylist = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        //attack();
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeMode();
        }
        //Chat();
    }
    
    private void LateUpdate()
    {
        //attack();
        Action();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Character")
        {
            inRange = true;
            inRangeEnemy = col.transform.gameObject;
            //Enemylist.Add(inRangeEnemy);
            //Enemylist.Add(col.gameObject);
            GameManager.gm.currentList.Add(inRangeEnemy);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Character")
        {
            inRange = false;
            //Enemylist.Remove(col.gameObject);
            GameManager.gm.currentList.Remove(col.gameObject);
        }
    }

    void attack()
    {
        if (playerMode == Mode.attack && !GameManager.gm.overUI /*&& inRange*/)
        {
                GameManager.gm.gs = GameStats.attack;
            /*if (GameManager.gm.gs == GameStats.attack)
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
                }
            }*/
        }
        
    }
    public void Chat()
    {
        
        if (playerMode == Mode.chat && GameManager.gm.currentTargetGO)
        {
            Vector3 targetPos;
            //targetPos= Camera.main.WorldToScreenPoint(targetGO.transform.position);
            targetPos = Camera.main.WorldToScreenPoint(new Vector3(GameManager.gm.currentTargetGO.transform.position.x, GameManager.gm.currentTargetGO.transform.position.y+1.2f,GameManager.gm.currentTargetGO.transform.position.z));
            GameManager.gm.chatUI.SetActive(true);
            GameManager.gm.chatUI.transform.position = targetPos;
            
        }
            
    }
    GameObject TargetPicker()
    {
        GameObject targetGO;
        RaycastHit hit;
        targetGO = null;
        Ray camRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out hit))
        {
            if (hit.collider.tag == "Character")
            {
                //if (playerMode == Mode.chat && inRangeEnemy)
                //{
                GameObject hitTarget;
                hitTarget = hit.transform.gameObject;
                if (GameManager.gm.currentList.Contains(hitTarget))
                {
                    targetGO = hitTarget;
                }
                //}
            }
        }
        return targetGO;
    }
    public void Action()
    {
        UISwitch();
        if (Input.GetButtonDown("Fire1")&&!GameManager.gm.overUI)
        {
            attack();
            Chat();
            GameManager.gm.currentTargetGO = TargetPicker();
        }
        if (GameManager.gm.gs == GameStats.attack)
        {
            timer = timer + Time.deltaTime;
                if (timer >= ps.attackSpeed)
                {
                //if (inRange && inRangeEnemy)
                //{
                
                if (GameManager.gm.currentTargetGO)
                {
                    Debug.Log("Attack");
                    GameManager.gm.currentTargetGO.GetComponent<NPCStats>().currentHealth -= ps.attackPower;
                    //inRangeEnemy.GetComponent<NPCStats>().currentHealth -= ps.attackPower;
                    //}
                }
                    timer = 0;
                    GameManager.gm.gs = GameStats.other;
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

    public void ModeAtk()
    {
        if (playerMode == Mode.chat)
        {
            playerMode = Mode.attack;
        }
    }

    public void ModeChat()
    {
        if (playerMode == Mode.attack)
        {
            playerMode = Mode.chat;
        }
    }

    public void Test()
    {
        //GameObject targetGO;
        //Debug.Log(TargetPicker());
        //targetGO = TargetPicker().transform.parent.gameObject;
        //targetGO = EventSystem.current.currentSelectedGameObject.transform.root.gameObject;
        //Debug.Log(targetGO);

        //targetGO.GetComponent<NPCStats>().satisfaction += 10;
        GameManager.gm.gs=GameStats.other;
        //TeamManager.tm.AddMember(targetGO);
        Debug.Log(GameManager.gm.gs);
    }

    void UISwitch()
    {
        switch (playerMode)
        {
            case Mode.attack:
                GameManager.gm.attackBtnImg.sprite = GameManager.gm.attOn;
                GameManager.gm.chatBtnImg.sprite = GameManager.gm.chatOff;
                break;
            case Mode.chat:
                GameManager.gm.attackBtnImg.sprite = GameManager.gm.attOff;
                GameManager.gm.chatBtnImg.sprite = GameManager.gm.chatOn;
                break;
        }
    }
}
