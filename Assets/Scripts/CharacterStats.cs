using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats:MonoBehaviour {
    protected int health;
    public bool inRange;
    public bool isDead;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public int satisfaction;
    public Relationship rs;
    public enum Relationship
    {
        opponent,
        friend,
        neutral
    }

    public virtual void ChangeRelation()
    {
        if (satisfaction >= 100)
        {
            rs = Relationship.friend;
        }
    }
}
