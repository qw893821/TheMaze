using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameEnd : MonoBehaviour {
    public Text text;
	// Use this for initialization
	void Start () {
        text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
            text.enabled = true;
        }
    }
}
