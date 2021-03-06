﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryStal : MonoBehaviour {
    public float rValue;
    public bool ruin;
    float regenTimer;
    float regenTime;
    Renderer[] renderers;
    public GameObject guardian;
    NPCStats ns;
            
    private void Start()
    {
        rValue = 50f;
        ruin = false;
        regenTime = 30f;
        regenTimer = 0f;
        renderers = transform.GetComponentsInChildren<Renderer>();
        if (guardian)
        {
            ns = guardian.GetComponent<NPCStats>();
        }
    }

    private void Update()
    {
        SelfAction();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerSample")
        {
            GameManager.gm.EnableFarm();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag=="PlayerSample")
        {
            GameManager.gm.DisableFarm();
        }
    }

    public void Farming()
    {
        rValue -= 10f;
        if (guardian&&ns.satisfaction < 0)
        {
            if (ns)
            {
                ns.opponentList.Add(GameManager.gm.player);
                Debug.Log("added");
                //force the player in typeb range
                ns.currentInRangeList.Add(GameManager.gm.player);
            }

        }
    }

    void SelfAction()
    {
        if (rValue <= 0)
        {
            ruin = true;
            foreach (Renderer rd in renderers)
            {

                rd.enabled = false;
            }
            
        }
        if (ruin)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenTime)
            {
                rValue = 50f;
                regenTimer = 0f;
                ruin = false;
                Debug.Log("back");
                foreach (Renderer rd in renderers)
                {
                    rd.enabled = true;
                }
            }
        }
    }
}
