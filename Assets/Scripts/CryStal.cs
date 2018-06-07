using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryStal : MonoBehaviour {
    public float rValue;
    public bool ruin;
    float regenTimer;
    float regenTime;
    Renderer[] renderers;
    
            
    private void Start()
    {
        rValue = 50f;
        ruin = false;
        regenTime = 20f;
        regenTimer = 0f;
        renderers = transform.GetComponentsInChildren<Renderer>();
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
