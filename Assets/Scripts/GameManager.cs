using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public enum GameStats
{
    walking,
    attack,
    turn,
    farming,
    healing,
    other
}

public enum Personality
{
    typeA,
    typeB,
    typeC
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
    PlayerStats ps;
    //action ui
    GameObject attackBtn;
    public Image attackBtnImg;
    public Sprite attOn;
    public Sprite attOff;
    GameObject chatBtn;
    public Image chatBtnImg;
    public Sprite chatOn;
    public Sprite chatOff;
    GameObject farmBtn;
    public Image farmBtnImg;
    public Sprite farmOn;
    public Sprite farmOff;
    GameObject healingBtn;
    //game chat ui
    public GameObject chatUI;
    GameObject waitBTNGO;
    //password ui
    public GameObject pwUI;
    public GameObject pwInput;
    public List<Sprite> pwImgs;//sequence:  rgbbw
    //trade ui
    public GameObject tradeUI;
    private Slider _tradeSlider;
    public float sliderValue { get { return _tradeSlider.value; }
    set { _tradeSlider.maxValue = value; }
    }
    //being target warning ui
    public GameObject warningUI1;
    public GameObject warningUI2;
    //img being attacked
    public Image flashImg;
    public Color flashColor {
        get { return flashImg.color; }
        set { flashImg.color=value; }
    }
    //text ui animation
    public Animator textUIAnim;

    //current select target gameobject;
    public GameObject currentTargetGO;
    //cursor texture
    public Texture2D cursorBattle;
    public Texture2D cursorChat;
    public Texture2D cursorFarm1;
    public Texture2D cursorFarm2;
    Mode currentMode;
    //NPCs currently target player
    public List<GameObject> cTargetList;
    //NPC Target Position list 
    //public List<GameObject> targetList;
    //NPC emotion sprite list
    //public List<Sprite> emojiList;
    //map camera
    GameObject cameraGO;
    Vector3 cameraOffSet;
    //some button to test
    public List<GameObject> btns;
    List<string> cList;
    //current player facing direction
    public string facing;
    //health and resource bar
    public GameObject healthBar;
    Slider healthSlider;
    public GameObject resourceBar;
    Text resourceText;

    //number sprite
    public Sprite[] sprites;
    //game end 
    public GameObject endScreen;
    Animator endAnim;
    //fow related
    public FoWMask fowMask;
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
        player = GameObject.Find("Player");
        pa = player.GetComponent<PlayerAction>();
        ps = player.GetComponent<PlayerStats>();
        currentList = new List<GameObject>();
        cTargetList = new List<GameObject>();
        attackBtn = GameObject.Find("Attack");
        attackBtnImg = attackBtn.GetComponent<Image>();
        chatBtn = GameObject.Find("Chat");
        chatBtnImg = chatBtn.GetComponent<Image>();
        farmBtn = GameObject.Find("Farm");
        farmBtnImg = farmBtn.GetComponent<Image>();
        farmBtn.GetComponent<Button>().interactable = false;
        healingBtn = GameObject.Find("Rest");
        waitBTNGO = GameObject.Find("WaitingButtons");
        Debug.Log(waitBTNGO);
        waitBTNGO.SetActive(false);
        chatUI.SetActive(false);
        currentMode = pa.playerMode;
        warningUI1.SetActive(false);
        warningUI2.SetActive(false);
        //get reference of slider before disable the tradeUI
        _tradeSlider = tradeUI.GetComponentInChildren<Slider>();
        tradeUI.SetActive(false);
        textUIAnim = GameObject.Find("InforBox").GetComponent<Animator>();
        cList = new List<string>();
        //eList = new EmojiManager();
        //targetList = new List<GameObject>();
        cameraGO = GameObject.Find("MapCamera");
        cameraOffSet = cameraGO.transform.position - player.transform.position;
        facing = "Vertical";
        flashColor = Color.clear;
        healthSlider = healthBar.GetComponent<Slider>();
        resourceText = resourceBar.GetComponentInChildren<Text>();
        endAnim = endScreen.GetComponent<Animator>();
        
	}

    private void Awake()
    {
        //enable fowmask by force
        
    }
    // Update is called once per frame
    void Update () {
        TimeController();
        ChatUIUpdate();
        HealingBtn();
        ChangeCursor();
        Warning();
        MapCameraFollow();
        HealthBarUpdate();
        ResourceBarUpdate();
        PWUIUpdate();
        fowMask.enabled = true;
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
            case GameStats.farming:
                Time.timeScale = 1f;
                break;
            case GameStats.healing:
                Time.timeScale = 1f;
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
        if (currentTargetGO&&currentTargetGO.tag=="Character")
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
        else if (!currentTargetGO)
        {
            tradeUI.SetActive(false);
            chatUI.SetActive(false);
        }
    }
    //change current curose type based on player action mode
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
            else if (pa.playerMode == Mode.farm)
            {
                Vector2 hotspot = Vector2.zero;
                CursorMode cursorMode = CursorMode.Auto;
                Cursor.SetCursor(cursorFarm1, hotspot, cursorMode);
            }
            currentMode = pa.playerMode;
        }
    }


    //warning player, when some NPC is targeting player.
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

    //update emoji show on the UI
    public void InstBTN()
    {
        waitBTNGO.SetActive(true);
        NPCStats ns;
        ns = currentTargetGO.GetComponent<NPCStats>();
        cList = ns.list;
        
        for(int i = 0; i < cList.Count; i++)
        {
            GameObject go;
            go = GameObject.Find(cList[i]);
            go.transform.parent = GameObject.Find("ButtonSlot"+(i+1).ToString()).transform;
            go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        if (ns.slotNum > 4)
        {
            chatUI.transform.Find("ButtonSlot1").GetComponentInChildren<Button>().interactable = false;
            chatUI.transform.Find("ButtonSlot2").GetComponentInChildren<Button>().interactable = false;
        }
        else {
            foreach(Transform tran in chatUI.transform)
            {
                tran.gameObject.GetComponentInChildren<Button>().interactable = true;
            }
            /*chatUI.transform.Find("ButtonSlot1").GetComponentInChildren<Button>().interactable = true;
            chatUI.transform.Find("ButtonSlot2").GetComponentInChildren<Button>().interactable = true;*/
        }
        waitBTNGO.SetActive(false);
    }


    //this function shif btn ui
    public void BtnShuffle()
    {
        if (GameManager.gm.chatUI.activeSelf)
        {
            waitBTNGO.SetActive(true);
            NPCStats ns;
            ns = currentTargetGO.GetComponent<NPCStats>();
            GameObject currentGO;
            Transform parentTrans;
            int num;
            currentGO = EventSystem.current.currentSelectedGameObject;
            parentTrans = currentGO.transform.parent;
            char[] c = parentTrans.name.ToCharArray();
            //should convert char value to a numeric value to make it work
            num = (int)char.GetNumericValue(c[c.Length - 1]);
            //currentGO.transform.parent = GameObject.Find("WaitingButtons").transform;
            currentGO.transform.parent = waitBTNGO.transform;
            //should change the recttransform position to make it work properly
            currentGO.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            GameObject newBtn;
            newBtn = GameObject.Find(ns.Shuffle(num, currentGO));
            newBtn.SetActive(true);
            //disable the waiting button ui
            waitBTNGO.SetActive(false);
            newBtn.transform.parent = parentTrans;
            newBtn.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
    }

    void MapCameraFollow()
    {
        cameraGO.transform.position = player.transform.position+cameraOffSet; 
    }

    public void EnableFarm()
    {
        farmBtn.GetComponent<Button>().interactable = true;
    }

    public void DisableFarm()
    {
        farmBtn.GetComponent<Button>().interactable = false;
    }

    void HealingBtn()
    {
        if (ps.resource <= 5f)
        {
            healingBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            healingBtn.GetComponent<Button>().interactable = true;
        }
    }

    public string PastToNext(GameObject go)
    {
        string name;
        char[] nameChar;
        nameChar = go.name.ToCharArray();
        switch (facing){
            case "Vertical":
                if (nameChar[0] == 'B')
                {
                    name = "F" + nameChar[1];
                }
                else { name= "B" + nameChar[1]; }
                break;
            case "Horizontal":
                if (nameChar[1] == 'L')
                {
                    name = nameChar[0]+"B";

                }
                else { name = nameChar[0]+"L"; }
                break;
            default:
                name = "player";
                break;
        }
        return name;
    }

    public void ShowPW(NPCStats ns)
    {
        //5 is current pw length, 6 is blank icon
        for(int i = 0; i < 5; i++)
        {
            if (ns.pwList[i] == "Red")
            {
                pwUI.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[0];
            }
            else if (ns.pwList[i] == "Green")
            {
                pwUI.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[1];
            }
            else if (ns.pwList[i] == "Blue")
            {
                pwUI.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[2];
            }
            else if (ns.pwList[i] == "Black")
            {
                pwUI.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[3];
            }
            else if (ns.pwList[i] == "White")
            {
                pwUI.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[4];
            }
            //inserted pw list
            if (ns.insertedList[i] == "Red")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[0];
            }
            else if (ns.insertedList[i] == "Green")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[1];
            }
            else if (ns.insertedList[i] == "Blue")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[2];
            }
            else if (ns.insertedList[i] == "Black")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[3];
            }
            else if (ns.insertedList[i] == "White")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[4];
            }
            else { pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[5]; }
        }
    }

    //player enter pw by click btn
    public void EnterPW(string str,NPCStats ns)
    {
        //-1, because the program running sequence
        int i;
        i = ns.slotNum - 1;
            if (str == "Red")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[0];
            ns.insertedList[i]="Red";
            }
            else if (str == "Green")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[1];
            ns.insertedList[i] = "Green";
        }
            else if (str == "Blue")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[2];
            ns.insertedList[i] = "Blue";
        }
            else if (str == "Black")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[3];
            ns.insertedList[i] = "Black";
        }
            else if (str == "White")
            {
                pwInput.transform.GetChild(i).GetComponent<Image>().sprite = pwImgs[4];
            ns.insertedList[i] = "White";
        }
    }
    public void Trade()
    {
        NPCStats ns;
        ns = currentTargetGO.GetComponent<NPCStats>();
        ns.resource += _tradeSlider.value;
        ns.satisfaction += (int)_tradeSlider.value;
    }

    void HealthBarUpdate()
    {
        healthSlider.value = ps.currentHealth;
    }

    void ResourceBarUpdate()
    {
        int[] digitals=new int[3];
        int originalInt;
        originalInt = (int)ps.resource;
        SpliteNum(originalInt, digitals);
        for(int i = 0; i < digitals.Length; i++)
        {
            
            resourceBar.transform.GetChild(i+1).GetComponent<Image>().sprite = sprites[digitals[i]];
        }
    }

    void SpliteNum(int num, int[] nums)
    {
        //this method may have problem.
        /*if (num >= 10)
        {
            nums.Add(num % 10);
            num = (num - num % 10) / 10;
            SpliteNum(num,nums);
        }
        else { nums.Add(num);
            return;
        }
        */
        nums[0] = num % 10;
        nums[1] = ((num - nums[0]) / 10) % 10;
        nums[2]=((num - nums[0]) / 10-nums[1])/10% 10;
    }


    void PWUIUpdate()
    {
        if (!currentTargetGO||currentTargetGO.tag != "Character")
        {
            foreach(Transform trans in pwUI.transform)
            {
                trans.GetComponent<Image>().sprite = pwImgs[5];
            }
        }
        if (!currentTargetGO || currentTargetGO.tag != "Character")
        {
            foreach(Transform trans in pwInput.transform)
            {
                trans.GetComponent<Image>().sprite = pwImgs[5];
            }
        }
    }

    void WinScreen()
    {
        endAnim.SetTrigger("win");

    }
    
    void LoseScreen()
    {
        endAnim.SetTrigger("lose");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
