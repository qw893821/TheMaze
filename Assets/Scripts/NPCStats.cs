using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCStats : CharacterStats {
    public int currentHealth;
    public Animator anim;
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
    // Use this for initialization
    void Start() {
        health = 100;
        currentHealth = health;
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

    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Die();
        ChangeRelation();
        ChangeTarget();
        LineUpdate();
    }
    private void LateUpdate()
    {

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
        if (other.tag == "Player"||other.tag=="Character")
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
        if (ignoredList.Contains(targetGO)||(targetGO.tag!="Character"&&targetGO.tag!="Player"))
        {
            return timer = 0;
        }
        return timer += Time.deltaTime;
        
    }

    public override void ChangeTarget()
    {
        base.ChangeTarget();
        
        if (opponentList.Contains(targetGO))
        {
            Attack(targetGO);
        }
        else if (IgnoreTimer() >= ignoreTime&&(targetGO.tag=="Character"||targetGO.tag=="Player")&&!opponentList.Contains(targetGO))
        {
            ignoredList.Add(targetGO);
            //ChangeTarget();
            //for test use. hard to find a place hold prevTargetoGO, use null to avoid err
            targetGO = null;
            //prevTargetGO = null;
            timer = 0;
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
        NPCStats ns;
        ns = go.GetComponent<NPCStats>();
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            ns.currentHealth -= attackPower;
            attackTimer = 0;
        }
    }

    void AttackPlayer(GameObject go)
    {
        PlayerStats ps;
        ps=go.GetComponent<PlayerStats>();
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            ps.currentHealth -= attackPower;
            attackTimer = 0;
        }
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
}
