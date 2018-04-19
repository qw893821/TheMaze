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
        Chat();
    }
    
    private void LateUpdate()
    {
        attack();
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
            if (Input.GetButtonDown("Fire1"))
            {
                GameManager.gm.gs = GameStats.attack;
                Debug.Log("trigger");

            }
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
            }
        }
    }
    public void Chat()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray camRay = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit))
            {
                if (playerMode == Mode.chat && inRangeEnemy)
                {
                    Canvas canv;
                    canv = inRangeEnemy.GetComponent<NPCStats>().canv;
                    if (!canv.enabled)
                    {
                        canv.enabled = true;
                    }
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

}
