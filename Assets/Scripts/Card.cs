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

    EnemyArea enemyArea;

    GameController gameController;
    private CardController cardController;
    private GameObject tempCard;

    private int index;

    public UnityEvent rightClick;//右击事件

    /// <summary>
    /// 卡牌基础控制
    /// </summary>
    public void  Enter()
    {
        transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        if (choose == true) return;
        GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = false;
        index = transform.GetSiblingIndex();
        transform.SetParent(tempCard.transform);
        
        
    }

    public void Exit()
    {
        GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        transform.SetParent(cardController.transform);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.SetSiblingIndex(index);
    }

    public void Drag()
    {
        if (choose == true) return;
        GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        transform.position = Input.mousePosition;
        transform.SetParent(GameObject.Find("tempCard").transform);

    }

    public void Up()
    {
        if (choose == true) return;
        GameObject.Find("cards").GetComponent<GridLayoutGroup>().enabled = true;
        Vector3 pos = transform.position;
        float pos_y = pos.y;
        float pos_x = pos.x;
        if(pos_y < 180 && pos_x > 1240)
        {
            Destroy(gameObject);
        }
        if (pos_y > 320)
        {
            UseCard();
        }else
        {
            transform.SetParent(GameObject.Find("cards").transform);
        }
        
    }

    public void BackToHand()//回到手牌
    {
        transform.SetParent(GameObject.Find("cards").transform);
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

    public void Init(int Id)
    {
        id = Id;
   
        if (id == 1)
        {
            cardName = "普通剑法";
            cardInfo = "对敌人造成(攻击力)伤害";
        }
        else if (id == 2)
        {
            cardName = "回春丹";
            cardInfo = "回复20生命值";
        }
        else if (id == 3)
        {
            cardName = "无中生有";
            cardInfo = "抽两张牌";
        }
        else if (id == 4)
        {
            cardName = "逍遥步";
            cardInfo = "获得buff逍遥步(闪避几率增加65%)两回合";
        }
        else if (id == 5)
        {
            cardName = "回春散";
            cardInfo = "立即回复10点生命值，每回合开始回复10点生命值，持续2回合";
        }
        else if (id == 6)
        {
            cardName = "快斩";
            cardInfo = "抽一张牌，对敌方造成(攻击力/2)的伤害，可额外攻击一次";
        }
        else if (id == 7)
        {
            cardName = "破釜沉舟";
            cardInfo = "抽两张牌，获得20内力(生命值大于10%时无法使用)";
        }
        else if (id == 8)
        {
            cardName = "铁布衫";
            cardInfo = "收到的伤害降低50%，持续2回合";
        }
        else if (id == 9)
        {
            cardName = "闷棍";
            cardInfo = "对敌人造成(攻击力)伤害。使敌人眩晕，下回合无法攻击";
        }
        else if (id == 10)
        {
            cardName = "清心静气";
            cardInfo = "本回合无法攻击。抽一张牌，回复10内力。下回合开始额外回复10内力，抽一张牌";
        }
        else if (id == 11)
        {
            cardName = "结阵";
            cardInfo = "获得(友方数量*5)的护甲";
        }
        else if (id == 101)
        {
            cardName = "旋风斩";
            cardInfo = "对所有敌人造成(当前攻击力)+10伤害";
        }
        else if (id == 201)
        {
            cardName = "魔教掌法";
            cardInfo = "对敌人造成已（损失生命值 * 0.5，最小为5，最大为30）伤害";
        }
        else if (id == 202)
        {
            cardName = "嗜血掌法";
            cardInfo = "对敌人造成（攻击力）伤害，回复造成的伤害";
        }
        else if (id == 203)
        {
            cardName = "魔教剑法";
            cardInfo = "消耗当前20%生命值，对敌人造成（消耗生命值*2）伤害";
        }
        else if (id == 204)
        {
            cardName = "魔功护体";
            cardInfo = "消耗当前50%生命值，免疫伤害1回合";
        }
        else if (id == 205)
        {
            cardName = "噬魂剑法";
            cardInfo = "对敌人造成(攻击力)伤害，回复10内力";
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
        if (gameController.turn != 1 || cardController.isMixing == true)//如果不是玩家回合就无法用牌
        {
            BackToHand();
            return;
        }
        if (id == 1) 普通剑法();
        if (id == 2) 回春丹();
        if (id == 3) 无中生有();
        if (id == 4) 逍遥步();
        if (id == 5) 回春散();
        if (id == 6) 快斩();
        if (id == 7) 破釜沉舟();
        if (id == 8) 铁布衫();
        if (id == 9) 闷棍();
        if (id == 10) 清心静气();
        if (id == 11) 结阵();

        if (id == 201) 魔教掌法();
        if (id == 202) 嗜血掌法();
        if (id == 203) 魔教剑法();
        if (id == 204) 魔功护体();
        if (id == 205) 噬魂剑法();


        if (id == 101) 旋风斩();
    }

    /// <summary>
    /// 卡牌方法
    /// </summary>
    public void 回春丹()///回春丹
    {
        if (player.mp < 1)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-1);
        gameController.SetTips("回春丹", new Color(0, 255, 0));
        player.SetHp(20);
        //cardController.SlowAddCard();
        
        Destroy(gameObject);
    }

    public void 普通剑法()//普通剑法
    {
        //   if (gameController.playerIsAtk == true)
        //{
        //      gameController.SetTips("本回合您已进行了攻击",new Color(255, 255, 255));
        //      BackToHand();
        //       return;
        //  }
        if (player.mp < 1)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-1);

        gameController.playerIsAtk = true;
        damage = player.atk;
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(player.atk * 0.3));//吸血效果
        }
        gameController.SetTips("普通剑法", new Color(255, 0, 0));
        Destroy(gameObject);
    }
    
    public void 无中生有()//无中生有
    {
        if (player.mp < 2)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-2);
        gameController.SetTips("无中生有", new Color(255, 181, 0));
        Invoke("AddCard", 0.5f);
        Invoke("AddCard", 0.5f);
        cardController.SlowAddCard();
        cardController.SlowAddCard();
        Destroy(gameObject);
    }

    public void 逍遥步()
    {
        if (player.mp < 1)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-1);
        gameController.SetTips("逍遥步", new Color(0, 45, 255));
        player.SetBuff(1, 2);
        //cardController.SlowAddCard();
        Destroy(gameObject);

    }

    public void 回春散()
    {
        if (player.mp < 1)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-1);
        //cardController.SlowAddCard();
        gameController.SetTips("回春散", new Color(0, 255, 0));
        player.SetBuff(2, 2);
        Destroy(gameObject);
    }

    public void 快斩()
    {
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}
        if (player.mp < 2)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-2);
        damage = player.atk / 2;
        enemyArea.Hurt(damage);
        if (player.blood == true) //判断是否吸血
        {
            player.SetHp((int)(damage * 0.3));//吸血效果
        }
        gameController.SetTips("快斩", new Color(255, 0, 0));
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
        if (player.mp < 1)
        {
            gameController.SetTips("您的内力值不足", new Color(255, 255, 255));
            BackToHand();    //如果法力值不足就回到手牌，不触发牌
            return;
        }
        player.SetMp(-1);
        gameController.SetTips("铁布衫", new Color(0, 255, 0));
        player.SetBuff(3, 2);
        Destroy(gameObject);

    }

    public void 闷棍()
    {
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}

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
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}

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
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}
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
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}
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
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}

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
        //if (gameController.playerIsAtk == true)
        //{
        //    gameController.SetTips("本回合您已进行了攻击", new Color(255, 255, 255));
        //    BackToHand();
        //    return;
        //}

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

    
}
