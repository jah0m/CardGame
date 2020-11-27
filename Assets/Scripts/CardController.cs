using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardController : MonoBehaviour
{
    public GameObject cardPrefab;//卡牌预制体
    private int cardCount;//手牌数
    
    private int id; //卡牌id

    public bool isMixing = false;//是否正在融合
    GameController gameController;

    public Dictionary<int, int> deck = new Dictionary<int, int>(); //牌库 
    private int deckIndex;
    public int deckCount;//牌库中剩余的卡牌数
    private int sum;
    public Text deckCountText;
    public List<int> handCards = new List<int>();//手牌
    public Dictionary<string, int> handCardsCount = new Dictionary<string, int>(); //计算手牌中各类牌的数量，用以计算融合

    public bool canMix; //卡牌融合功能是否开启

    Player player;//玩家物体


    void Start()
    {
        handCardsCount.Add("普通剑法", 0);
        player = GameObject.Find("player").GetComponent<Player>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
    }

    void Update()
    {

    }
    public  void ClearCard()
    {
        for (int i = 0; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);
    }
    public void AddCardByInput() //通过输入获得卡牌或调整玩家属性（GM）
    {
        string input = GameObject.Find("InputText").GetComponent<Text>().text;
        if(input == "hp")
        {
            player.SetHp(100);
        }
        else if(input == "mp")
        {
            player.SetMp(50);
        }
        else
        {
            int inputNum = int.Parse(input);
            AddCard(inputNum);
        }
        
    }

    public void AddCard(int Id) // 抽牌 规则，如果参数为0则随机抽牌吧并且消耗牌库内的牌，如果参数为卡牌id则抽取指定的牌，不消耗牌库内的牌
    {
        if (Id != 0) id = Id;
        else
        {
            CheckDeckCount();
            if (deckCount <= 0)
            {
               deck = new Dictionary<int, int>(gameController.tempDeck);
            }
            deckIndex = Random.Range(0, deckCount); //牌库索引
            sum = 0;
            foreach(int i in deck.Keys)
            {
                sum += deck[i];
                if(deckIndex < sum)
                {
                    id = i;
                    break;
                }
            }
            
            //id = deck.ElementAt(deckIndex).Key; //抽牌id等于从牌库中随机选取的卡牌id
            deck[id] -= 1; //对应卡牌的卡牌数-1
            if (deck[id] == 0) deck.Remove(id); //如果剩余卡牌数为0，则删掉这张牌
        }
     
        cardCount = transform.childCount;
        if (cardCount >= 7) return;
        GameObject card = Instantiate(cardPrefab);
        card.transform.SetParent(transform);
        card.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        card.GetComponent<Card>().Init(id);
        
        //MixCard();
        CheckDeckCount();
    }

    public void CheckDeckCount()//检查牌库数
    {
        deckCount = 0;
        foreach (int c in deck.Keys)
        {
            deckCount += deck[c];
        }//获取deck中剩余的卡牌数
        deckCountText.text = "剩余卡牌数:" + deckCount;
    }

    public void CheckHandCards() //遍历手牌
    {
        handCards = new List<int>();
        handCardsCount["普通剑法"] = 0;
        foreach (Transform child in transform)
        {
            handCards.Add(child.GetComponent<Card>().id);
        }
        foreach(int i in handCards)
        {
            if(i == 1)
            {
                handCardsCount["普通剑法"] += 1;
            }
        }
    }

    public void MixCard(int CardId)//卡牌融合
    {
        CheckHandCards();
        if (canMix == false) return;
        if(CardId == 1) //如果CardId为1则判断合成普通剑法
        {
            if (handCardsCount["普通剑法"] >= 3)
            {
                int count = 0;
                if (isMixing == true) return;
                isMixing = true;
                foreach (Transform x in transform)
                {
                    if (x.GetComponent<Card>().id == 1)
                    {
                        //x.GetComponent<Image>().color = Color.blue;
                        x.GetComponent<Card>().desText.text = "融合中";
                        Destroy(x.gameObject, 0.6f);
                        count += 1;
                    }
                    if (count == 3) break;
                }
                Invoke("AddCard101", 0.8f);//旋风斩
            }
        }
        
        
    }

    
    public void SlowAddCard() //延迟抽牌
    {
        Invoke("addCard", 0.5f);
    }

    public void addCard()
    {
        AddCard(0);
    }
    public void AddCard101()
    {
        AddCard(101);
        isMixing = false;
    }

    
}
