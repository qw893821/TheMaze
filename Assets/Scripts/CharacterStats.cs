using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Relationship
{
    opponent,
    friend,
    neutral
}
public class CharacterStats : MonoBehaviour {
    protected float health;
    public bool isDead;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public float attackTimer;
    public int satisfaction;
    public Relationship rs;
    public GameObject targetGO;
    //two position for npc to nave;
    public GameObject[] poss;
    //public GameObject leader;
    protected GameObject prevTargetGO;
    public List<GameObject> opponentList;
    public List<GameObject> friendList;
    public List<GameObject> ignoredList;
    public List<GameObject> currentInRangeList;
    //resource realted date
    public float resource;
    protected float rDecreaseRate;
    public float currentHealth {
        get { return health; }
        set { health = value; }
    }
    private void Start()
    {
        currentInRangeList = new List<GameObject>();
    }

    public virtual void ChangeRelation()
    {
        if (satisfaction >= 100)
        {
            rs = Relationship.friend;
        }
    }

    public virtual void ChangeTarget()
    {
        if ((currentInRangeList.Count==0))//when there is no character/player in range and currently no target, "!targetGo" will cause some problem
        {
            if (!targetGO)
            {
                LoadPointPicker();
            }
            else {
                    return;
            }
        }
        //when there are element in current list
        else if (currentInRangeList.Count != 0)
        {
            
            //when ignoredlist has element
            //go thought currentInRangeList and find one as target
            for(int i=0; i < ignoredList.Count; i++)
                {
                    if (currentInRangeList.Contains(ignoredList[i]))
                    {
                        currentInRangeList.Remove(ignoredList[i]);
                        ChangeTarget();
                    }
                    else { continue; }
                }
            for(int i = 0; i < currentInRangeList.Count; i++)
            {
                if (opponentList.Contains(currentInRangeList[i]))
                {
                    targetGO = currentInRangeList[i];
                    return;
                }
            }
            //when every ignoredList is not in the currentInRangeList,find a random one as target
            if (currentInRangeList.Count != 0)//weird err
            {
                targetGO = currentInRangeList[Random.Range(0, currentInRangeList.Count)];
            }
        }
    }

    public GameObject LoadPointPicker()
    {
        float distance1, distance2;
        distance1 = Vector3.Distance(transform.position, poss[0].transform.position);
        distance2 = Vector3.Distance(transform.position, poss[1].transform.position);
        if (distance1 < distance2)
        {
            targetGO = poss[1];
        }
        else { targetGO = poss[0]; }
        return targetGO;
    }

    public virtual void FoV(GameObject go)
    {
        if (!currentInRangeList.Contains(go))
        {
            Vector3 dir;
            float angle;
            dir = go.transform.position - transform.position;
            angle = Vector3.Angle(dir, transform.forward);
            if (angle <= 45)
            {
                currentInRangeList.Add(go);
                if (go.tag == "Player")
                {
                    if (!GameManager.gm.cTargetList.Contains(transform.gameObject))
                    {
                        GameManager.gm.cTargetList.Add(transform.gameObject);
                    }
                }
            }
        }
        //else { return; }
        
    }

    public virtual void ResourceReduce()
    {
        if (resource >0) {
            resource -= rDecreaseRate * Time.deltaTime;
        }
        //when there is no resource
        else { health -= rDecreaseRate * Time.deltaTime; }
    }
    
    public virtual void Damaged(int v)
    {
        currentHealth -= v;
    }

    public virtual void SatificationTest(GameObject go)
    {
        if (satisfaction >= 0)
        {
            opponentList.Remove(go);
        }
    }
}
