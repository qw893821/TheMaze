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
    public bool inRange;
    public bool isDead;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public int satisfaction;
    public Relationship rs;
    public GameObject targetGO;
    public List<GameObject> opponentList;
    public List<GameObject> friendList;
    private void Start()
    {
        inRange = false;
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
        if (!inRange&&!targetGO)
        {
            targetGO = GameManager.gm.targetList[Random.Range(0,GameManager.gm.targetList.Count)];
        }
    }

   public virtual void FoV()
    {
        if (inRange)
        {
            if (targetGO.tag != "Player"||!targetGO)
            {
                Vector3 dir;
                float angle;
                dir = GameManager.gm.player.transform.position - transform.position;
                angle = Vector3.Angle(dir, transform.forward);
                Debug.Log(angle);
                if (angle <= 45f)
                {
                    targetGO = GameManager.gm.player;
                }
            }
        }
    }
}
