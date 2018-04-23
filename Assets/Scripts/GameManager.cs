using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    public GameObject chatUI;
    //current select target gameobject;
    public GameObject currentTargetGO;
    //cursor texture
    public Texture2D cursorBattle;
    public Texture2D cursorChat;
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
        chatUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        TimeController();
        ChatUIUpdate();
        ChangeCursor();
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

    //update chatUI pos
    void ChatUIUpdate()
    {
        if (chatUI.activeSelf)
        {
            chatUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(gm.currentTargetGO.transform.position.x, gm.currentTargetGO.transform.position.y + 1.2f, gm.currentTargetGO.transform.position.z));
        }
    }

    void ChangeCursor()
    {
        Vector2 hotspot = Vector2.zero;
        CursorMode cursorMode = CursorMode.Auto;
        Cursor.SetCursor(cursorBattle,hotspot,cursorMode);
    }
}
