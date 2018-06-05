using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats: CharacterStats{
    public BoxCollider box;
    //change is true when team member changes
    public bool change;
    float flashspeed;
    float flashTimer;
	// Use this for initialization
	void Start () {
        change = false;
        attackPower = 10;
        attackRange = 1.5f;
        attackSpeed = 1f;
        currentHealth = 100;
        resource = 100;
        rDecreaseRate = 1f;
        flashspeed = 1f;
        flashTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        ResourceReduce();
	}

    void ChangeRange()
    {
        box.size =new Vector3(attackRange, 1f, attackRange);
        change = false;
    }

    public override void ResourceReduce()
    {
        base.ResourceReduce();
        if (resource <= 0)
        {
            resource = 0;
            flashTimer += Time.deltaTime;
            if (flashTimer >= 1.0f)
            {
                GameManager.gm.flashColor = new Color(1.0f,0.0f,0.0f);
                flashTimer = 0;
            }
            
        }
        GameManager.gm.flashColor = Color.Lerp(GameManager.gm.flashColor, Color.clear, flashspeed * Time.deltaTime);
    }

}
