using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStats
{
    walking,
    attack,
    turn,
    other
}
public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager gm
    {
        get { return _instance; }
    }
    public GameStats gs;
    public bool overUI;
	// Use this for initialization
	void Start () {
        if (gm == null)
        {
            _instance = this;
        }
        else if (gm != null)
        {
            Destroy(this);
        }
        gs = GameStats.other;
        overUI = false ;
	}
	
	// Update is called once per frame
	void Update () {
        TimeController();

    }

    void TimeController()
    {
        switch (gs){
            case GameStats.other:
                Time.timeScale = 0;
                break;
            case GameStats.attack:
                Time.timeScale = 1;
                break;
            case GameStats.walking:
                Time.timeScale = 1;
                break;
            default:
                Time.timeScale = 0;
                break;
        }
    }
}
