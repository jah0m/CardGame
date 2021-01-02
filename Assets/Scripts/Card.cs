using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    private Vector3 pos;
    /// <summary>
    /// 卡牌id，名字，消耗内力，描述
    /// </summary>
    public int id;
    public string cardName;
    public int cost;
    public string cardInfo;

    private int count;
    private int times;

    public Text nameText;
    public Text desText;

    public bool choose;

    private int damage;
    
    private Transform playerTransform;
    Team team;
    Player player;
    Canvas canvas;
    EnemyArea enemyArea;
    GameObject guidesObj;

    GameController gameController;
    private CardController cardController;
    private GameObject tempCard;

    public int index;
    public int order;

    public UnityEvent rightClick;//右击事件

    /// <summary>
    /// 卡牌基础控制
    /// </summary>
    public void  Enter()
    {
        if (gameController.isAdding) return; //如果正在抽牌则无法放大
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        if (choose == true) return;
        //GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = false;
        index = transform.GetSiblingIndex();
        pos = transform.localPosition;
        //transform.localPosition = new Vector3(pos.x + 10, pos.y, pos.y);
        order = canvas.sortingOrder;
        canvas.sortingOrder = 10;
        //transform.SetParent(tempCard.transform);
    }

    public void Exit()
    {
        if (gameController.isAdding) return;
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        if (choose == true) return;
        canvas.sortingOrder = order;
        //GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        //transform.SetParent(cardController.transform);
        //transform.SetSiblingIndex(index);
    }

    public void Drag()
    {
        if (choose == true) return;
        //GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        transform.position = Input.mousePosition;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.SetParent(GameObject.Find("tempCard").transform);
        cardController.SetPos();

    }

    public void Up()
    {
        if (choose == true) return;
        //GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        Vector3 pos = transform.position;
        float pos_y = pos.y;
        float pos_x = pos.x;
        if(pos_y < 180 && pos_x > 1240)
        {
            Destroy(gameObject);
            cardController.SetPos();
        }
        if (pos_y > 260)
        {
            UseCard();
        }else
        {
            transform.SetParent(GameObject.Find("cards").transform);
            transform.SetSiblingIndex(index);
        }
        cardController.SetPos();
    }

    public void BackToHand()//回到手牌
    {
        transform.SetParent(GameObject.Find("cards").transform);
        transform.SetSiblingIndex(index);
        cardController.SetPos();
    }

    



    /// <summary>
    /// 游戏开始时
    /// </summary>
    void Start()
    {
        playerTransform = GameObject.Find("player").transform;
        player = playerTransform.GetComponent<Player>();
        team = GameObject.Find("team").GetComponent<Team>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();

        cardController = GameObject.Find("cards").GetComponent<CardController>();

        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        rightClick.AddListener(new UnityAction(ButtonRightClick));//添加右击监听

        transform.GetComponent<Image>().color = new Color(255, 255, 255);
        tempCard = GameObject.Find("tempCard");
        canvas = GetComponent<Canvas>();
        order = canvas.sortingOrder;
        guidesObj = GameObject.Find("guides");

    }

    public void OnPointerDown(PointerEventData eventData)//当鼠标被按下时
    {
        if (eventData.button == PointerEventData.InputButton.Right)//监听右键
            rightClick.Invoke();
    }
    private void ButtonRightClick() //右击后触发的方法
    {
        Exit();
        cardController.MixCard(id);
    }
    
    public void Init(int Id)//读取卡牌文本
    {
        id = Id;
        csvController.GetInstance().loadFile(Application.dataPath + "/Resources", "卡牌集.csv");
        int i = 1;
        while(true){
            string cardId = csvController.GetInstance().getString(i, 0);
            if(cardId == id.ToString())
            {
                cardName = csvController.GetInstance().getString(i, 1);
                cardInfo = csvController.GetInstance().getString(i, 2);
                cost = int.Parse(csvController.GetInstance().getString(i, 3));
                break;
            }
            i++;
        }
        nameText.text = cardName;
        desText.text = cardInfo;
    }
    public void AddCard()
    {
        cardController.AddCard(0);
    }

    public void UseCard()
    {
        if (gameController.turn != 1 || cardController.isMixing || player.attacking)//如果不是玩家回合就无法用牌
        {
            BackToHand();
            return;
        }
        if(player.mp < cost)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-cost);//消耗内力
        Invoke(cardName, 0f);
    }

    /// <summary>
    /// 卡牌方法
    /// </summary>
    public void 回春丹()///回春丹
    {
        gameController.SetTips("回春丹", new Color(0, 255, 0));
        if (player.庸医)
        {
            damage = player.atk;
            enemyArea.Hurt(damage);
        }
        else
        {
            player.SetHp(player.atk);
        }
        Destroy(gameObject);
    }

    public void 江湖剑法()//普通剑法
    {
        gameController.playerIsAtk = true;
        player.Attack();
        damage = player.atk;
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(player.atk * 0.3));//吸血效果
        }
        gameController.SetTips("普通剑法", new Color(255, 0, 0));
        Destroy(gameObject);
    }
    public void 庸医()
    {
        player.SetBuff(206,1);
        gameController.SetTips("庸医", new Color(0, 255, 0));
        Destroy(gameObject);
    }
    
    public void 无中生有()//无中生有
    {
        gameController.SetTips("无中生有", new Color(255, 181, 0));
        cardController.SlowAddCard();
        cardController.SlowAddCard();
        Destroy(gameObject);
    }

    public void 逍遥步()
    {
        gameController.SetTips("逍遥步", new Color(0, 45, 255));
        player.SetBuff(1, 2);
        Destroy(gameObject);
    }

    public void 回春散()
    {
        gameController.SetTips("回春散", new Color(0, 255, 0));
        player.SetBuff(2, 2);
        Destroy(gameObject);
    }

    public void 快斩()
    {
        gameController.SetTips("快斩", new Color(255, 0, 0));
        damage = player.atk / 2;
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(damage * 0.3));//吸血效果
        }
        cardController.SlowAddCard();
        Destroy(gameObject);
    }

    public void 破釜沉舟()
    {
        if (player.hp > gameController.playerMaxHp / 10)
        {
            gameController.SetTips("还没到用这张牌的地步", new Color(255, 255, 255));
            BackToHand();
            return;
        }
        gameController.SetTips("破釜沉舟", new Color(255, 0, 0));
        cardController.SlowAddCard();
        cardController.SlowAddCard();
        player.SetMp(20);
        Destroy(gameObject);
    }

    public void 铁布衫()
    {
        gameController.SetTips("铁布衫", new Color(0, 255, 0));
        player.SetBuff(3, 2);
        Destroy(gameObject);
    }

    public void 闷棍()
    {
        gameController.playerIsAtk = true;
        damage = player.atk;
        enemyArea.Hurt(damage);
        enemyArea.SetBuff(201, 1);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(damage * 0.3));
        }
        gameController.SetTips("闷棍", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 清心静气()
    {
        if (gameController.playerIsAtk == true)
        {
            gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
            BackToHand();
            return;
        }
        gameController.playerIsAtk = true;
        gameController.SetTips("清心静气", new Color(0, 255, 0));
        player.SetBuff(4, 1);
        Destroy(gameObject);
    }

    public void 结阵()
    {
        team.SetArmor(team.Count() * 5);
        Destroy(gameObject);
    }

    public void 旋风斩()
    {
        gameController.playerIsAtk = true;
        damage = player.atk + 10;
        enemyArea.AllHurt(damage);
        if(player.blood == true)
        {
            player.SetHp((int)(damage * 0.3) * enemyArea.transform.childCount);
        }
        gameController.SetTips("旋风斩", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 魔教掌法()
    {
        gameController.playerIsAtk = true;
        damage = gameController.playerMaxHp - player.hp;
        if (damage < 5) damage = 5;
        if (damage > 30) damage = 30;
        enemyArea.Hurt(damage);
        if (player.blood == true)
        {
            player.SetHp((int)(damage * 0.3));
        }
        gameController.SetTips("魔教掌法", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 嗜血掌法()
    {
        gameController.playerIsAtk = true;
        damage = player.atk;
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(player.atk * 0.3) + damage);//吸血效果
        }
        else
        {
            player.SetHp(damage);
        }
        gameController.SetTips("嗜血剑法", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 魔教剑法()
    {
        gameController.playerIsAtk = true;
        damage = (int)(player.hp * 0.4);
        player.SetHp(-damage / 2);
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(player.atk * 0.3));//吸血效果
        }
        gameController.SetTips("魔教剑法", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 魔功护体()
    {
        player.SetHp(-player.hp / 2);
        player.SetBuff(5, 1);
        gameController.SetTips("魔功护体", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 噬魂剑法()
    {
        gameController.playerIsAtk = true;
        damage = player.atk;
        enemyArea.Hurt(damage);
        player.SetMp(10);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(player.atk * 0.3));//吸血效果
        }
        gameController.SetTips("噬魂剑法", new Color(255, 0, 0));
        Destroy(gameObject);
    }

    public void 阿黄()
    {
        team.AddDog("tm2", "阿黄", 20, 5, new Color(255, 255, 255));
        Destroy(gameObject);
    }
}
