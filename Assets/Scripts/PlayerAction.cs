using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Mode
{
    attack,
    chat,
    farm
}
public class PlayerAction : MonoBehaviour {
    
    bool inRange;
    public PlayerStats ps;
    public float timer;
    public GameObject inRangeEnemy;
    RaycastHit hit;
    //public List<GameObject> Enemylist;
    public Mode playerMode;
    public Camera camera;
    public float farmSpeed;
	// Use this for initialization
	void Start () {
        timer = 0;
        inRange = false;
        ps = transform.GetComponent<PlayerStats>();
        playerMode = Mode.chat;
        farmSpeed = 10f;
    }
	
	// Update is called once per frame
	void Update () {
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
    //consume time
    void Attack()
    {
        if (playerMode == Mode.attack && !GameManager.gm.overUI /*&& inRange*/)
        {
                GameManager.gm.gs = GameStats.attack;
        }
        
    }
    
    //show chat ui, not consume time
    public void Chat()
    {
        if (playerMode == Mode.chat && GameManager.gm.currentTargetGO&& GameManager.gm.currentTargetGO.tag=="Character")
        {
            
            Vector3 targetPos;
            targetPos = Camera.main.WorldToScreenPoint(new Vector3(GameManager.gm.currentTargetGO.transform.position.x, GameManager.gm.currentTargetGO.transform.position.y+1.2f,GameManager.gm.currentTargetGO.transform.position.z));
            GameManager.gm.chatUI.SetActive(true);
            GameManager.gm.chatUI.transform.position = targetPos;
            //called this when ui is enabled
            GameManager.gm.InstBTN();
        }       
    }
    //consume time
    public void Farm()
    {
        if (playerMode == Mode.farm && !GameManager.gm.overUI/*&& GameManager.gm.currentTargetGO*/)
        {
            GameManager.gm.gs = GameStats.farming;
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
                GameObject hitTarget;
                hitTarget = hit.transform.gameObject;
                if (GameManager.gm.currentList.Contains(hitTarget))
                {
                    targetGO = hitTarget;
                }
            }
            else if (hit.collider.tag == "Resource")
            {
                Debug.Log("reach");
                targetGO = hit.transform.gameObject;
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
            Attack();
            Chat();
            Farm();
        }
        if (GameManager.gm.gs == GameStats.attack&& GameManager.gm.gs != GameStats.farming)
        {
            timer = timer + Time.deltaTime;
            if (timer >= ps.attackSpeed)
            {
                if (GameManager.gm.currentTargetGO.tag=="NPC")
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
                }
                timer = 0;
                GameManager.gm.gs = GameStats.other;
            }
        }
        //some place change the gs to other. so this is not working
        //problem is at the PlayerMovement script
        if (GameManager.gm.gs == GameStats.farming/*&& GameManager.gm.gs != GameStats.attack*/ )
        {
            timer += Time.deltaTime;
            if (timer>=1f)
            {
                if (GameManager.gm.currentTargetGO&&GameManager.gm.currentTargetGO.tag == "Resource")
                {
                    ps.resource +=farmSpeed;
                    //do farming
                    
                }
                else { Debug.Log("fail farm"); }
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
            playerMode = Mode.attack;
    }
    //switch mode using button
    public void ModeChat()
    {
            playerMode = Mode.chat;
    }
    //switch mode to farm
    public void ModeFarm()
    {
        playerMode = Mode.farm;
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
        //GameManager.gm.gs=GameStats.other;
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
       // GameManager.gm.gs = GameStats.other;
        GameManager.gm.UpdateEmoji(targetGO.GetComponent<NPCStats>().satisfaction, targetGO.GetComponent<NPCStats>().currentHealth);
        //next lines are for test use
        GameManager.gm.BtnShuffle();
    }
    
    //test of button white
    public void ButtonNeutral()
    {
        GameObject targetGO;
        NPCStats ns;
        targetGO = GameManager.gm.currentTargetGO;
        ns = targetGO.GetComponent<NPCStats>();
        if (ns.ps == Personality.typeA)
        {
            ns.satisfaction += 5;
        }
        else if (ns.ps == Personality.typeC)
        {
            ns.satisfaction += 5;
        }
        else if (ns.ps == Personality.typeB)
        {
            ns.satisfaction += 10;
        }
        
        GameManager.gm.UpdateEmoji(targetGO.GetComponent<NPCStats>().satisfaction, targetGO.GetComponent<NPCStats>().currentHealth);
        GameManager.gm.BtnShuffle();
    }
    //test of button yellow
    public void ButtonTrade()
    {
        GameManager.gm.tradeUI.SetActive(true);
        GameManager.gm.sliderValue = ps.resource;
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

    public void TradeResource()
    {
        ps.resource -= GameManager.gm.sliderValue;
        GameManager.gm.Trade();
        GameManager.gm.tradeUI.SetActive(false);
        
    }

    public void RejectTrade()
    {
        GameManager.gm.tradeUI.SetActive(false);
    }
}
