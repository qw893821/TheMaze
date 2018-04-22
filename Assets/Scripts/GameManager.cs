using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<GameObject> currentList;
    GameObject player;
    PlayerAction pa;
    GameObject attackBtn;
    public Image attackBtnImg;
    public Sprite attOn;
    public Sprite attOff;
    GameObject chatBtn;
    public Image chatBtnImg;
    public Sprite chatOn;
    public Sprite chatOff;
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
        player = GameObject.FindGameObjectWithTag("Player");
        pa = player.GetComponent<PlayerAction>();
        currentList = new List<GameObject>();
        attackBtn = GameObject.Find("Attack");
        attackBtnImg = attackBtn.GetComponent<Image>();
        chatBtn = GameObject.Find("Chat");
        chatBtnImg = chatBtn.GetComponent<Image>();
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
