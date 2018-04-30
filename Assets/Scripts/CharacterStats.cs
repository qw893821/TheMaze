using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Relationship
{
    opponent,
    friend,
    neutral
}
public class CharacterStats:MonoBehaviour {
    protected int health;
    public bool isDead;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public int satisfaction;
    public Relationship rs;
    public GameObject targetGO;
    protected GameObject prevTargetGO;
    public List<GameObject> opponentList;
    public List<GameObject> friendList;
    public List<GameObject> ignoredList;
    public List<GameObject> currentInRangeList;
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
                targetGO = GameManager.gm.targetList[Random.Range(0, GameManager.gm.targetList.Count)];
            }
            else {
                    return; }
            //return;
        }
        //when there are element in current list
        else if (currentInRangeList.Count != 0)
        {
            //when ignoredlist has element
            //if (ignoredList.Count != 0)
            //{
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
                //when every ignoredList is not in the currentInRangeList,find a random one as target
            targetGO = currentInRangeList[Random.Range(0, currentInRangeList.Count)];
           // }
            //else { targetGO = currentInRangeList[Random.Range(0, currentInRangeList.Count)]; }
        }
    }

   /*public virtual void FoV(GameObject go)
    {
        //if (inRange)
        //{
            if (ignoredList.Contains(go))
            {
                //inRange = false;
                return;
            }
            //else if (targetGO.tag != "Player"||!targetGO )
            else if(!ignoredList.Contains(go)||!targetGO)
            {
                Vector3 dir;
                float angle;
                dir =go.transform.position - transform.position;
                angle = Vector3.Angle(dir, transform.forward);
                if (angle <= 45f)
                {
                    if (!prevTargetGO)
                    {
                        prevTargetGO = targetGO;
                    }
                    targetGO = go;
                }
            }
        //}
    }*/

    public virtual void FoV(GameObject go)
    {
        Vector3 dir;
        float angle;
        dir = go.transform.position - transform.position;
        angle = Vector3.Angle(dir, transform.forward);
        if (angle <= 45)
        {
            currentInRangeList.Add(go);
        }
    }
}
