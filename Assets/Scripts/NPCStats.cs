using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCStats : CharacterStats {
    Animator anim;
    CapsuleCollider capCol;
    /*public Button chatBtn1;
    public Button chatBtn2;
    public Button chatBtn3;
    */
    //character following timer
    float timer;
    float ignoreTime;
    
    public Personality ps;
    //flash screen color
    public Image img;
    Color flashColor;
    float flashSpeed;

    //NPCs selection button related properity
    List<string> selectionList;
    List<string> cSelectionList;//current list
    public List<string> wSelectionList;//selection in waitlist
    public List<string> list { get { return cSelectionList; } }
    //password of npc
    List<string> pwPool;
    public List<string> pwList;
    //current inserted pw
    public List<string> insertedList;
    //current slot for password
    int cSlot;
    public int slotNum
    {
        get { return cSlot; }
    }
    // Use this for initialization
    void Start() {
        currentHealth = 50;
        resource = 100;
        rDecreaseRate = 1;
        anim = GetComponent<Animator>();
        isDead = false;
        
        attackRange = 3f;
        attackPower = 10;
        attackSpeed = 1f;
        capCol = GetComponent<CapsuleCollider>();
        //not local button, canv no longer useful
        //canv = canvGO.GetComponent<Canvas>();
        //canv.enabled = false;
        rs = Relationship.neutral;
        //opponentList = new List<GameObject>();
        friendList = new List<GameObject>();
        ignoredList = new List<GameObject>();
        currentInRangeList = new List<GameObject>();
        ignoreTime = 5.0f;
        
        flashColor = new Color(1.0f,0f,0f,0.0f);
        flashSpeed = 10f;
        img = GameObject.Find("ScreenFlash").GetComponent<Image>();
        selectionList = new List<string>() { "Red", "Green","Blue","Black","White" };
        //instal pwpool here;
        pwPool = new List<string>();
        foreach(string str in selectionList)
        {
            pwPool.Add(str);
        }
        wSelectionList = new List<string>();
        cSelectionList = new List<string>();
        InstSelectionList();
        PWGenerater();
        cSlot = 0;
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update() {
        Die();
        ChangeRelation();
        ChangeTarget();
        //npc now have no resource limition
        //ResourceReduce();
        //LineUpdate();
    }
    
    void Die()
    {
        if (currentHealth <= 0)
        {
            anim.SetTrigger("dead");
            //anim.Play("dying",-1);
            isDead = true;
            capCol.enabled = false;
            if (isDead)
            {
                anim.SetBool("walking", false);
                anim.SetBool("inCombat", false);
                Destroy(this.gameObject, 3.0f);
            }
            transform.tag = "Dead";
            GameManager.gm.cTargetList.Remove(this.transform.gameObject);
            GameManager.gm.currentList.Remove(this.transform.gameObject);
            NavMeshAgent agent;
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            Sink(0.2f);
        }
    }

    void Sink(float speed)
    {
        Vector3 targetPos;
        targetPos = transform.position;
        targetPos.y -= speed;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public override void ChangeRelation()
    {
        base.ChangeRelation();
    }

    //when a character enters the trigger, add character to a list
    private void OnTriggerEnter(Collider other)
    {
        //only triggered by player 
        if (other.tag == "Player" )
        {
            //inRange = true;
            FoV(other.transform.gameObject);
        }
    }

    public override void FoV(GameObject go)
    {
        if (ps == Personality.typeA&&satisfaction<0)
        {
            base.FoV(go);
            opponentList.Add(go);
        }
        
    }

    public override void ResourceReduce()
    {
        base.ResourceReduce();
    }

    /*private float IgnoreTimer()
    {
        if (ignoredList.Contains(targetGO) || (targetGO.tag != "Character" && targetGO.tag != "Player"))
        {
            return timer = 0;
        }
        return timer += Time.deltaTime;

    }*/


    public override void ChangeTarget()
    {
        base.ChangeTarget();
        float distance;
        if (ps == Personality.typeA)
        {
            SatificationTest(GameManager.gm.player);
        }
        distance = Vector3.Distance(transform.position, targetGO.transform.position);
        if (opponentList.Contains(targetGO) && distance <= attackRange)
        {
            Attack(targetGO);
            anim.SetBool("inRange", true);
        }
        
    }

    

    void AttackOpponent(GameObject go)
    {
        if (!isDead)
        {
            NPCStats ns;
            ns = go.GetComponent<NPCStats>();
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed)
            {
                ns.Damaged(attackPower);
                attackTimer = 0;

                //add this gameobject to the target's list
                if (!ns.opponentList.Contains(transform.gameObject))
                {
                    if (ns.opponentList.Contains(transform.gameObject))
                    {
                        ns.opponentList.Remove(transform.gameObject);
                    }
                    ns.opponentList.Add(transform.gameObject);
                }

            }
        }
    }

    void AttackPlayer(GameObject go)
    {
        if (!isDead) { 
            PlayerStats ps;
            ps = go.GetComponent<PlayerStats>();
            attackTimer += Time.deltaTime;
            
            if (attackTimer >= attackSpeed)
            {
                ps.Damaged(attackPower);
                attackTimer = 0;
                //there is no need for manipulate player opponentList. but do this for further use.
                if (!ps.opponentList.Contains(transform.gameObject))
                {
                    if (ps.opponentList.Contains(transform.gameObject))
                    {
                        ps.opponentList.Remove(transform.gameObject);
                    }
                    ps.opponentList.Add(transform.gameObject);

                }
                GameManager.gm.flashColor = new Color(1.0f, 0f, 0f, 0.5f);
            }
        }
        GameManager.gm.flashColor = Color.Lerp(GameManager.gm.flashColor, Color.clear, flashSpeed * Time.deltaTime);
        //GameManager.gm.img.color = flashColor;

        
    }

    void Attack(GameObject go)
    {
        AttackPlayer(go);
    }

    bool NPCMatch()
    {
        int randomValue;
        randomValue=Random.Range(0,100);
        //positive match. add to friend list
        if (randomValue < Value(targetGO))
        {
            if (!friendList.Contains(targetGO))
            {
                friendList.Add(targetGO);
            }
            return true;
        }
        else {
            Debug.Log("fail match");
            randomValue = Random.Range(0,100);
            //this is a negtive match. add to opponentList
            if (randomValue < 50)
            {
                if (!opponentList.Contains(targetGO))
                {
                    opponentList.Add(targetGO);
                }
                return true;
            }
            return false;
        }
    }
    //get a value based on character's personality, when the same personality, character are easy to match when close
    int Value(GameObject go)
    {
        
            Personality targetPS;
            targetPS = go.GetComponent<NPCStats>().ps;
            if (ps == targetPS)
            {
                return 80;
            }
            else if (Mathf.Abs((int)ps - (int)targetPS) == 1)
            {
                return 50;
            }
            else { return 20; }
    }

    

    void InstSelectionList()
    {
        //pick three element from array
        for(int i = 0; i < 2; i++)
        {
            int num;
            num = Random.Range(0, selectionList.Count);
            cSelectionList.Add(selectionList[num]);
            selectionList.RemoveAt(num);
        }
        wSelectionList = selectionList;
    }

    public string Shuffle(int i,GameObject go)
    { 
        int num;
        num = Random.Range(0, wSelectionList.Count - 1);
        cSelectionList[i-1] = wSelectionList[num];
        wSelectionList.RemoveAt(num);
        wSelectionList.Add(go.name);
        return cSelectionList[i-1];
    }

    public override void Damaged(int v)
    {
        base.Damaged(v);
    }

    void PWGenerater()
    {
        if (pwPool.Count > 0)
        { 
            int i = Random.Range(0,pwPool.Count-1);
            pwList.Add(pwPool[i]);
            pwPool.RemoveAt(i);
            PWGenerater();
            return;
        }
        else {
            return; }
    }

    public bool PWMatch(GameObject go)
    {
        //if have entered 5 pw, then end enter
        if (cSlot == 4)
        {
            GameManager.gm.chatUI.SetActive(false);
        }
        if (go.name != pwList[cSlot])
        {
            Debug.Log("false");
            cSlot++;
            
            return false;
        } 
        else 
        {
            Debug.Log("true");
            cSlot++;
            
            return true;
        }

    }

    
}
