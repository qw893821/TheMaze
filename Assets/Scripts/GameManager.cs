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
    public GameObject player;
    PlayerAction pa;
    GameObject attackBtn;
    public Image attackBtnImg;
    public Sprite attOn;
    public Sprite attOff;
    GameObject chatBtn;
    public Image chatBtnImg;
    public Sprite chatOn;
    public Sprite chatOff;
    //game chat ui
    public GameObject chatUI;
    //being target warning ui
    public GameObject warningUI1;
    public GameObject warningUI2;
    //current select target gameobject;
    public GameObject currentTargetGO;
    //cursor texture
    public Texture2D cursorBattle;
    public Texture2D cursorChat;
    Mode currentMode;

    //NPCs currently target player
    public List<GameObject> cTargetList;
    //NPC Target Position list 
    public List<GameObject> targetList;
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
        cTargetList = new List<GameObject>();
        attackBtn = GameObject.Find("Attack");
        attackBtnImg = attackBtn.GetComponent<Image>();
        chatBtn = GameObject.Find("Chat");
        chatBtnImg = chatBtn.GetComponent<Image>();
        chatUI.SetActive(false);
        currentMode = pa.playerMode;
        warningUI1.SetActive(false);
        warningUI2.SetActive(false);
        //targetList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        TimeController();
        ChatUIUpdate();
        ChangeCursor();
        Warning();
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
    //call this in PlayerAction to make sure it was use after the Action()
    public void ChatUIUpdate()
    {
        if (currentTargetGO)
        {
            //anlge of player and target
            float playerToTarget;
            playerToTarget = Vector3.Angle(player.transform.forward, (currentTargetGO.transform.position - player.transform.position));
            if (chatUI.activeSelf)
            {
                if (playerToTarget < 60)
                {
                    chatUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(gm.currentTargetGO.transform.position.x, gm.currentTargetGO.transform.position.y + 1.2f, gm.currentTargetGO.transform.position.z));
                }
                else { chatUI.SetActive(false); }
            }
        }
    }

    void ChangeCursor()
    {
        if (currentMode != pa.playerMode)
        {
            if (pa.playerMode == Mode.attack)
            {
                Vector2 hotspot = Vector2.zero;
                CursorMode cursorMode = CursorMode.Auto;
                Cursor.SetCursor(cursorBattle, hotspot, cursorMode);
            }
            else if (pa.playerMode == Mode.chat)
            {
                Vector2 hotspot = Vector2.zero;
                CursorMode cursorMode = CursorMode.Auto;
                Cursor.SetCursor(cursorChat, hotspot, cursorMode);
            }
            currentMode = pa.playerMode;
        }
    }

    public void Warning()
    {
        if (warningUI2.activeSelf || warningUI1.activeSelf)
        {
            if (cTargetList.Count == 0)
            {
                warningUI1.SetActive(false);
                warningUI2.SetActive(false);
            }
        }
        if (!warningUI1.activeSelf||!warningUI2.activeSelf)
        {
            foreach(GameObject go in cTargetList)
            {
                NPCStats ns;
                ns = go.GetComponent<NPCStats>();
                if (ns.targetGO==player)
                {
                    if (ns.opponentList.Contains(player))
                    {
                        warningUI1.SetActive(true);
                    }
                    else { warningUI1.SetActive(true); }
                }
            }
        }
    }
}
