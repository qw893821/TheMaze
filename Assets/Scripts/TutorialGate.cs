using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGate : MonoBehaviour {

    // Use this for initialization
    CryStal cs;

    // Update is called once per frame
    public GameObject gate;
    private void Start()
    {
        cs=transform.GetComponent<CryStal>();
    }
    private void Update()
    {
        if (cs.rValue==0)
        {
            gate.SetActive(false);
        }
    }
}
