using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void Tutorial()
    {
        //2 is the tutorial scene
        SceneManager.LoadScene(2);
    }

    public void StartGame()
    {
        //1 is the game scene
        SceneManager.LoadScene(1);
    }
}
