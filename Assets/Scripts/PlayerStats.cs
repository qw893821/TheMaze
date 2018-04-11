using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats: CharacterStats{
    public BoxCollider box;
    //change is true when team member changes
    public bool change;
	// Use this for initialization
	void Start () {
        change = false;
        attackPower = 10;
        attackRange = 1.5f;
        attackSpeed = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (change)
        {
            ChangeRange();
        }
	}

    void ChangeRange()
    {
        box.size =new Vector3(attackRange, 1f, attackRange);
        change = false;
    }
}
