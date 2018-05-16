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
    
    //show chat ui
    public void Chat()
    {
        if (playerMode == Mode.chat && GameManager.gm.currentTargetGO)
        {
            
            Vector3 targetPos;
            targetPos = Camera.main.WorldToScreenPoint(new Vector3(GameManager.gm.currentTargetGO.transform.position.x, GameManager.gm.currentTargetGO.transform.position.y+1.2f,GameManager.gm.currentTargetGO.transform.position.z));
            GameManager.gm.chatUI.SetActive(true);
            GameManager.gm.chatUI.transform.position = targetPos;
            //called this when ui is enabled
            GameManager.gm.InstBTN();
        }       
    }

    //pick a target
    GameObject TargetPicker()
    {
        GameObject targetGO;
        RaycastHit hit;
        int layerMask = (1 << 9)|(1<<12);
        layerMask = ~layerMask;
        targetGO = null;
        Ray camRay = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out hit,5f,layerMask))
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

    //actions which effect time. currently move and attack could effect time
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
    //switch between chat/attack mode using right click
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

    //switch mode using button
    public void ModeAtk()
    {
        if (playerMode == Mode.chat)
        {
            playerMode = Mode.attack;
        }
    }
    //switch mode using button
    public void ModeChat()
    {
        if (playerMode == Mode.attack)
        {
            playerMode = Mode.chat;
        }
    }

    //test chat mode button red
    public void ButtonAggressive()
    {
        GameObject targetGO;
        NPCStats ns;
        targetGO = GameManager.gm.currentTargetGO;
        ns = targetGO.GetComponent<NPCStats>();
        if (ns.ps == Personality.typeA)
        {
            ns.satisfaction -= 10;
        }
        else if (ns.ps == Personality.typeC)
        {
            ns.opponentList.Add(transform.gameObject);
        }
        else if (ns.ps == Personality.typeB)
        {
            ns.satisfaction += 10;
        }
        GameManager.gm.gs=GameStats.other;
        GameManager.gm.UpdateEmoji(targetGO.GetComponent<NPCStats>().satisfaction, targetGO.GetComponent<NPCStats>().currentHealth);
        GameManager.gm.BtnShuffle();
    }

    //test chat mode button green
    public void ButtonFriendness()
    {
        GameObject targetGO;
        NPCStats ns;
        targetGO = GameManager.gm.currentTargetGO;
        ns = targetGO.GetComponent<NPCStats>();
        if (ns.ps == Personality.typeA)
        {
            ns.satisfaction += 10;
        }
        else if (ns.ps == Personality.typeC)
        {
            ns.satisfaction -= 10;
        }
        GameManager.gm.gs = GameStats.other;
        GameManager.gm.UpdateEmoji(targetGO.GetComponent<NPCStats>().satisfaction, targetGO.GetComponent<NPCStats>().currentHealth);
        //next lines are for test use
        GameManager.gm.BtnShuffle();
    }
    
    //test of button white
    public void ButtonNeutral()
    {
        GameManager.gm.BtnShuffle();
    }
    //test of button yellow
    public void ButtonTrade()
    {
        NPCStats ns;
        GameObject targetGO;
        targetGO = GameManager.gm.currentTargetGO;
        ns = targetGO.GetComponent<NPCStats>();
        if (ns.satisfaction > 50)
        {
            //test use
            //actual code should show trade uis
            GameManager.gm.BtnShuffle();
        }
        else
        {
            switch (ns.ps){
                case Personality.typeC:
                    if (!ns.ignoredList.Contains(transform.gameObject))
                    {
                        ns.ignoredList.Add(transform.gameObject);
                    }
                    if (ns.targetGO == transform.gameObject)
                    {
                        ns.targetGO = null;
                    }
                    break;
                case Personality.typeA:
                    ns.satisfaction -= 10;
                    break;
                case Personality.typeB:
                    //show trade ui
                    break;
            }
            
        }
        GameManager.gm.UpdateEmoji(targetGO.GetComponent<NPCStats>().satisfaction, targetGO.GetComponent<NPCStats>().currentHealth);
        
    }
    //Mode Button image change
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

    //check direction of gameobject
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
