using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryStal : MonoBehaviour {

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
}
