using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// 玩家基本属性
    /// </summary>
    public int playerAtk; 
    public int playerMaxHp;
    public int playerMaxMp;
    public int playerMpReply;

    public GameObject bg;
    public int turn = 0;//回合，玩家回合：1，敌人回合：2  中间回合：0

    public int 关卡;
    public int 抽卡数;

    public bool gameOver = false;

    //public Transform cards;
    CardController cardController;

    private Transform playerTransform;

    public bool playerIsAtk = false;

    public Text tips;

    Player player;
    Team team;

    Enemy enemy;

    EnemyArea enemyArea;
          

    public int round = 0;

    private int count;
    private int times;//重复执行次数

    public bool selectSkill;

    public bool isChangePos = false;
    public bool isAdding = false;
    GameObject tempCard;

    public Dictionary<int, int> tempDeck = new Dictionary<int, int>(); //临时牌库，用于存储当前玩家的牌库

    public Transform mainMenu;

    public GameObject guides;

    public bool guiding; //正在指导
    public int guideId = 1;
    public bool addCardLock;

    
    void Start()
    {
        selectSkill = false;
        playerAtk = 5;
        playerMaxHp = 50;
        playerMaxMp = 3;
        playerMpReply = playerMaxMp;

        cardController = GameObject.Find("cards").GetComponent<CardController>();
        playerTransform = GameObject.Find("player").transform;
        player = playerTransform.GetComponent<Player>();
        team = GameObject.Find("team").GetComponent<Team>();

        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
        
        player.Init(playerMaxHp, 0, playerAtk);//初始化玩家攻击力，生命值等

        tempCard = GameObject.Find("tempCard");
        
    }
    public void StartGame()
    {
       
        设定关卡(关卡);
        selectSkill = true;

    }
    /// <summary>
    /// 玩家回合开始
    /// </summary>
    /// 
    public void StartTurn()
    {
        SetTips("你的回合", new Color(255, 255, 255));
        if(player.level == 1)
        {
            抽卡数 = 2;
        }
        else
        {
            抽卡数 = 2;
        }
        
        round += 1;
        enemyArea.checkDie = false;
        
        player.UpdateBuff(); //每回合开始时刷新buff
        enemyArea.UpdateBuff();
        team.UpdateBuff();

        playerIsAtk = false;//玩家是否已攻击
        turn = 1;
        
        player.SetMp(playerMpReply);
        if (round == 1)
        {
            count = 0;
            times = 3;
            InvokeRepeating("AddCard", 0.5f, 0.5f);
        }
        else
        {
            if (addCardLock) return;
            count = 0;
            times = 抽卡数;
            InvokeRepeating("AddCard", 0.5f, 0.5f);
        }
    }

    private void AddCard()
    {
        isAdding = true;
        foreach (Transform card in tempCard.transform)//将指着的牌缩小为正常大小
        {
            card.GetComponent<Card>().Exit();
        }
        count += 1;
        cardController.AddCard(0);
        if (count == times)
        {
            isAdding = false;
            CancelInvoke();
        }
            
    }

    public void SetTips(string info, Color color)
    {
        tips.text = info;
        tips.color = color;
        Invoke("ClearTips", 1f);
    }

    public void ClearTips()
    {
        tips.text = "";
    }

   
    
    /// <summary>
    /// 玩家回合结束
    /// </summary>
    public void EndTurn()
    {
        

        if (turn != 1) return; //如果不是玩家回合，则无法结束回合
        turn = 2;

        
        if(player.level<3)
        {
            player.SetXp(player.mp);
            player.SetMp(-player.mp);
        }

        if (team.Count() != 1) {//如果有队友就队友攻击，如果没有队友就直接让敌人攻击
            team.Attack();
        }
        else
        {
            enemyArea.Attack();
        }
        
        isChangePos = true;
        //enemyArea.Attack();
        
    }

    public void Guide()
    {
        mainMenu.gameObject.SetActive(false);
        cardController.deck.Add(1, 5);
        cardController.deck.Add(2, 2);
        selectSkill = true;
        设定关卡(0);
    }

    public void 设定关卡(int num)
    {
        关卡 = num;
        player.level = 1;
        player.xp = 0;
        player.SetXp(0);
        round = 0;
        cardController.ClearCard();
        SetTips("改变位置", new Color(255, 255, 255));
        isChangePos = false;
        //Invoke("关卡" + 关卡, 0f);
        if (关卡 == 0) 教程();
        if (关卡 == 1) 关卡1();
        if (关卡 == 2) 关卡2();
        if (关卡 == 3) 关卡3();
        if (关卡 == 4) 关卡4();
        if (关卡 == 5) 关卡5();
        if (关卡 == 6) 关卡6();

        Invoke("StartTurn", 0.3f);
    }
    /// <summary>
    /// 关卡设定
    /// </summary>
    /// 

    public void 教程()
    {
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_1", typeof(Sprite)) as Sprite;
        tempDeck = new Dictionary<int, int>(cardController.deck);//将当前的牌库保存到临时牌库
        //team.AddDog("tm2", "队友2", 50, 8, new Color(255, 255, 255));

        enemyArea.AddTj("enemy3", "铁匠阿牛", 50, 10);
        cardController.canMix = false;
        guides.SetActive(true);
        guiding = true;
    }

    public void 关卡1()
    {
        SetTips("第1关", new Color(0, 55, 255));
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_1", typeof(Sprite)) as Sprite;
    
        tempDeck =  new Dictionary<int, int> (cardController.deck);//将当前的牌库保存到临时牌库

        team.AddDog("tm2", "队友2", 50, 8, new Color(255, 255, 255));
        //team.AddDog("tm1", "队友1", 50, 8, new Color(255, 255, 255));

        enemyArea.AddDog("enemy3", "野狗", 10, 8, new Color(255, 255, 255));
        cardController.canMix = false;
        
    }

    public void 关卡2()
    {
        SetTips("第2关", new Color(0, 55, 255));
        cardController.deck = new Dictionary<int, int>(tempDeck);//将临时牌库的牌复制回牌库中。
        cardController.CheckDeckCount();
        enemyArea.AddDog("enemy2", "野狗", 30, 8, new Color(255, 255, 255));
        enemyArea.AddDog("enemy3", "野狗", 30, 8, new Color(255, 255, 255));
        cardController.canMix = true;
    }

    public void 关卡3()
    {
        SetTips("第3关", new Color(0, 55, 255));
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_2", typeof(Sprite)) as Sprite;
        cardController.deck = new Dictionary<int, int>(tempDeck);
        cardController.CheckDeckCount();
        enemyArea.AddDog("enemy1", "野狗", 30, 8, new Color(255, 255, 255));
        enemyArea.AddDog("enemy2", "野狗", 30, 8, new Color(255, 255, 255));
        enemyArea.AddDog("enemy3", "野狗王", 50, 10, new Color(0, 0, 0));
        cardController.canMix = true;
    }
    public void 关卡4()
    {
        SetTips("第3关", new Color(0, 55, 255));
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_2", typeof(Sprite)) as Sprite;
        cardController.deck = new Dictionary<int, int>(tempDeck);
        cardController.CheckDeckCount();
        enemyArea.AddTj("enemy3", "铁匠阿牛", 100, 15);
        cardController.canMix = true;
    }
    public void 关卡5()
    {
        SetTips("第3关", new Color(0, 55, 255));
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_2", typeof(Sprite)) as Sprite;
        cardController.deck = new Dictionary<int, int>(tempDeck);
        cardController.CheckDeckCount();
        enemyArea.AddTj("enemy3", "铁匠阿牛", 100, 15);
        cardController.canMix = true;
    }
    public void 关卡6()
    {
        SetTips("第3关", new Color(0, 55, 255));
        bg.GetComponent<Image>().sprite = Resources.Load("background/bg_2", typeof(Sprite)) as Sprite;
        cardController.deck = new Dictionary<int, int>(tempDeck);
        cardController.CheckDeckCount();
        enemyArea.AddTj("enemy3", "铁匠阿牛", 100, 15);
        cardController.canMix = true;
    }
    public void 重新开始()
    {
        SceneManager.LoadScene(0);
    }

}
