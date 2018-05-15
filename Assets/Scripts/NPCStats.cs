using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCStats : CharacterStats {
    public int currentHealth;
    Animator anim;
    CapsuleCollider capCol;
    public Button chatBtn1;
    public Button chatBtn2;
    public Button chatBtn3;
    public GameObject canvGO;
    public Canvas canv;
    //character following timer
    float timer;
    float ignoreTime;
    //properity of line
    LineRenderer line;
    int width;
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
    // Use this for initialization
    void Start() {
        health = 100;
        currentHealth = health;
        resource = 100;
        rDecreaseRate = 1;
        anim = GetComponent<Animator>();
        isDead = false;
        line = transform.Find("TargetLine").GetComponent<LineRenderer>();
        width = 1;
        attackRange = 3f;
        attackPower = 10;
        attackSpeed = 1f;
        capCol = GetComponent<CapsuleCollider>();
        //chatBtn1 = transform.Find("Neutral").GetComponent<Button>();
        //chatBtn2 = transform.Find("Aggressive").GetComponent<Button>();
        //chatBtn3 = transform.Find("Friendness").GetComponent<Button>();
        //canv = transform.Find("UICanv").GetComponent<Canvas>();
        canv = canvGO.GetComponent<Canvas>();
        canv.enabled = false;
        rs = Relationship.neutral;
        //opponentList = new List<GameObject>();
        friendList = new List<GameObject>();
        ignoredList = new List<GameObject>();
        currentInRangeList = new List<GameObject>();
        ignoreTime = 5.0f;
        leader = null;
        flashColor = new Color(1.0f,0f,0f,0.0f);
        flashSpeed = 10f;
        img = GameObject.Find("ScreenFlash").GetComponent<Image>();
        selectionList = new List<string>() { "Aggressive", "Friendness","Neutral","Trade","Test1","Test2" };
        wSelectionList = new List<string>();
        cSelectionList = new List<string>();
        InstSelectionList();

    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update() {
        Die();
        ChangeRelation();
        ChangeTarget();
        ResourceReduce();
        //LineUpdate();
    }
    private void LateUpdate()
    {
        LineUpdate();
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
        if (other.tag == "Player" || other.tag == "Character")
        {
            //inRange = true;
            FoV(other.transform.gameObject);
        }
    }

    public override void FoV(GameObject go)
    {
        base.FoV(go);
    }

    private float IgnoreTimer()
    {
        if (ignoredList.Contains(targetGO) || (targetGO.tag != "Character" && targetGO.tag != "Player"))
        {
            return timer = 0;
        }
        return timer += Time.deltaTime;

    }

    public override void ChangeTarget()
    {
        base.ChangeTarget();
        float distance;
        distance = Vector3.Distance(transform.position, targetGO.transform.position);
        if (opponentList.Contains(targetGO) && distance <= attackRange)
        {
            Attack(targetGO);
            anim.SetBool("inRange", true);
        }
        //will fail to match because playre do not have ps properity
        else if (IgnoreTimer() >= ignoreTime && (targetGO.tag == "Character" || targetGO.tag == "Player") && !opponentList.Contains(targetGO))
        {
            if (NPCMatch())
            {
                timer = 0;
                targetGO = null;
            }
            else if (!NPCMatch())
            {
                anim.SetBool("inRange", false);
                ignoredList.Add(targetGO);
                //ChangeTarget();
                //for test use. hard to find a place hold prevTargetoGO, use null to avoid err
                targetGO = null;
                //prevTargetGO = null;
                timer = 0;
            }
        }
    }

    void LineUpdate()
    {
        if (targetGO)
        {
            if (targetGO.tag == "Character" || targetGO.tag == "Player")
            {
                line.enabled = true;
                Vector3 target;
                target = targetGO.transform.position - transform.position;
                line.SetPosition(1, -target);
            }
            else { line.enabled = false; }
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
                ns.currentHealth -= attackPower;
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
                ps.currentHealth -= attackPower;
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
                flashColor = new Color(1.0f, 0f, 0f, 0.5f);
            }
        }
        flashColor = Color.Lerp(flashColor, Color.clear, flashSpeed * Time.deltaTime);
        img.color = flashColor;

        
    }

    void Attack(GameObject go)
    {
        if (go.tag == "Character")
        {
            AttackOpponent(go);
        }
        else if (go.tag == "Player")
        {
            AttackPlayer(go);
        }
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

    void ResourceReduce()
    {
        resource -= rDecreaseRate * Time.deltaTime;
    }

    void InstSelectionList()
    {
        //pick three element from array
        for(int i = 0; i < 3; i++)
        {
            int num;
            num = Random.Range(0, selectionList.Count);
            cSelectionList.Add(selectionList[num]);
            selectionList.RemoveAt(num);
        }
        wSelectionList = selectionList;
    }

    public void Shuffle(int i,GameObject go)
    {
        Debug.Log(i);
        int num;
        num = Random.Range(0, wSelectionList.Count - 1);
        cSelectionList[i] = wSelectionList[num];
        wSelectionList.RemoveAt(num);
        wSelectionList.Add(go.name);
    }

}
