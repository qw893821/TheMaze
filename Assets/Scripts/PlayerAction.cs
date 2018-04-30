using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Mode
{
    attack,
    chat
}
public class PlayerAction : MonoBehaviour {
    
    bool inRange;
    PlayerStats ps;
    public float timer;
    public GameObject inRangeEnemy;
    RaycastHit hit;
    //public List<GameObject> Enemylist;
    public Mode playerMode;
    public Camera camera;
	// Use this for initialization
	void Start () {
        timer = 0;
        inRange = false;
        ps = transform.GetComponent<PlayerStats>();
        playerMode = Mode.chat;
	}
	
	// Update is called once per frame
	void Update () { 
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeMode();
        }
    }
    
    private void LateUpdate()
    {
        Action();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Character")
        {
            //inRange = true;
            inRangeEnemy = col.transform.gameObject;
            if (!GameManager.gm.currentList.Contains(inRangeEnemy))
            {
                GameManager.gm.currentList.Add(inRangeEnemy);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Character")
        {
            //inRange = false;
            GameManager.gm.currentList.Remove(col.gameObject);
        }
    }

    void attack()
    {
        if (playerMode == Mode.attack && !GameManager.gm.overUI /*&& inRange*/)
        {
                GameManager.gm.gs = GameStats.attack;
        }
        
    }
    public void Chat()
    {
        if (playerMode == Mode.chat && GameManager.gm.currentTargetGO)
        {
            Vector3 targetPos;
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
            GameManager.gm.currentTargetGO = TargetPicker();
            attack();
            Chat();
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
                    GameObject currentGO;
                    NPCStats currentNPCs;
                    currentGO = GameManager.gm.currentTargetGO;
                    currentNPCs = currentGO.GetComponent<NPCStats>();
                    if (currentNPCs.rs != Relationship.friend)
                    {
                        currentNPCs.currentHealth -= ps.attackPower;
                        currentNPCs.rs = Relationship.opponent;
                        if (!currentNPCs.opponentList.Contains(transform.gameObject))
                        {
                            currentNPCs.opponentList.Add(transform.gameObject);
                        }
                    }
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
        GameObject targetGO;
        targetGO = GameManager.gm.currentTargetGO;
        targetGO.GetComponent<NPCStats>().satisfaction += 10;
        GameManager.gm.gs=GameStats.other;
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
    //warning button 1 use
    public void FaceTowardChar()
    {
        foreach (GameObject go in GameManager.gm.cTargetList)
        {
            NPCStats ns;
            ns = go.GetComponent<NPCStats>();
            if (ns.rs == Relationship.neutral || ns.rs == Relationship.friend)
            {
                /*Vector3 dir;
                float angle;
                dir = go.transform.position - transform.position;
                angle = Vector3.Angle(transform.forward, dir);
                if (angle >= 135)
                {
                    transform.Rotate(0f,180f,0f);
                }
                else if (angle > 45 && angle < 135)
                {
                    float newAngle;
                    newAngle = Vector3.Angle(transform.right, dir);
                    if (newAngle >= 135)
                    {
                        transform.Rotate(0, -90f, 0);
                    }
                    else { transform.Rotate(0, 90f, 0); }
                }
                */
                CheckDir(go);
            }
        }
    }
    //warning button2 use
    public void FaceToWardOpponent()
    {
        foreach (GameObject go in GameManager.gm.cTargetList)
        {
            NPCStats ns;
            ns = go.GetComponent<NPCStats>();
            if (ns.rs == Relationship.neutral || ns.rs == Relationship.friend)
            {
                CheckDir(go);
            }
        }
    }

    void CheckDir(GameObject go)
    {
        Vector3 dir;
        float angle;
        dir = go.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, dir);
        if (angle >= 135)
        {
            transform.Rotate(0f, 180f, 0f);
        }
        else if (angle > 45 && angle < 135)
        {
            float newAngle;
            newAngle = Vector3.Angle(transform.right, dir);
            if (newAngle >= 135)
            {
                transform.Rotate(0, -90f, 0);
            }
            else { transform.Rotate(0, 90f, 0); }
        }
    }
}
