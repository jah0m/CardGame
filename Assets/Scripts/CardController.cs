using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CardController : MonoBehaviour
{
    public GameObject cardPrefab;//卡牌预制体
    public int cardCount;//手牌数
    
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

    public GameObject deckObj;

    Player player;//玩家物体
    


    void Start()
    {
        handCardsCount.Add("普通剑法", 0);
        player = GameObject.Find("player").GetComponent<Player>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
    }

    void Update()
    {
        //SetPos();
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
        card.GetComponent<Canvas>().sortingOrder = cardCount ;
        card.transform.SetParent(transform);
        card.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        card.GetComponent<Card>().Init(id);

        //MixCard();
        CheckDeckCount();
        SetPos();
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

    public void ShowDeck()
    {
        deckObj.SetActive(true);
    }
    public void HideDeck()
    {
        deckObj.SetActive(false);
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

    public void SetPos()
    {
        Dictionary<int, Transform> cards = new Dictionary<int, Transform>();
        cardCount = transform.childCount;
        if(cardCount == 1)
        {
            Transform card1 = transform.GetChild(0);
            card1.localPosition = new Vector3(0, 0, 1);
            card1.localEulerAngles = new Vector3(0, 0, 0);
            card1.GetComponent<Canvas>().sortingOrder = 1;
        }
        else if(cardCount == 2)
        {
            Transform card1 = transform.GetChild(0);
            Transform card2 = transform.GetChild(1);
            
            card1.localPosition = new Vector3(-50, 0, 1);
            card1.localEulerAngles = new Vector3(0, 0, 3);
            card1.GetComponent<Canvas>().sortingOrder = 1;
            card2.localPosition = new Vector3(50, 0, 1);
            card2.localEulerAngles = new Vector3(0, 0, -3);
            card2.GetComponent<Canvas>().sortingOrder = 2;
        }
        else if(cardCount == 3)
        {
            Transform card1 = transform.GetChild(0);
            Transform card2 = transform.GetChild(1);
            Transform card3 = transform.GetChild(2);

            cards.Add(1, card1);
            cards.Add(2, card2);
            cards.Add(3, card3);

            foreach(int key in cards.Keys)
            {
                cards[key].localPosition = new Vector3( (key - 2) * 100, Mathf.Abs(key - 2) * -5, 1);
                cards[key].localEulerAngles = new Vector3( 0, 0, (key - 2) * -5);
                cards[key].GetComponent<Canvas>().sortingOrder = key;
            }
        }
        else if(cardCount == 4)
        {
            Transform card1 = transform.GetChild(0);
            Transform card2 = transform.GetChild(1);
            Transform card3 = transform.GetChild(2);
            Transform card4 = transform.GetChild(3);

            cards.Add(1, card1);
            cards.Add(2, card2);
            cards.Add(3, card3);
            cards.Add(4, card4);

            foreach (int key in cards.Keys)
            {
                cards[key].localPosition = new Vector3((key - 2.5f) * 90, (int)Mathf.Abs(key - 2.5f) * -10, 1);
                cards[key].localEulerAngles = new Vector3(0, 0, (key - 2.5f) * -5);
                cards[key].GetComponent<Canvas>().sortingOrder = key;
            }
        }
        else if (cardCount == 5)
        {
            Transform card1 = transform.GetChild(0);
            Transform card2 = transform.GetChild(1);
            Transform card3 = transform.GetChild(2);
            Transform card4 = transform.GetChild(3);
            Transform card5 = transform.GetChild(4);

            cards.Add(1, card1);
            cards.Add(2, card2);
            cards.Add(3, card3);
            cards.Add(4, card4);
            cards.Add(5, card5);

            foreach (int key in cards.Keys)
            {
                cards[key].localPosition = new Vector3((key - 3) * 80, Mathf.Abs((float)key - 3f) * -5, 1);
                cards[key].localEulerAngles = new Vector3(0, 0, (key - 3) * -5);
                cards[key].GetComponent<Canvas>().sortingOrder = key;
            }
            card1.localPosition = new Vector3(-160 , -16, 1);
            card5.localPosition = new Vector3(160, -16, 1);
        }
        else if (cardCount == 6)
        {
            cards.Add(1, transform.GetChild(0));
            cards.Add(2, transform.GetChild(1));
            cards.Add(3, transform.GetChild(2));
            cards.Add(4, transform.GetChild(3));
            cards.Add(5, transform.GetChild(4));
            cards.Add(6, transform.GetChild(5));

            foreach (int key in cards.Keys)
            {
                if(Mathf.Abs(key - 3.5f) < 1)
                {
                    cards[key].localPosition = new Vector3((key - 3.5f) * 80, 0, 1);
                }
                else
                {
                    cards[key].localPosition = new Vector3((key - 3.5f) * 80, (int)Mathf.Abs((float)key - 3.5f) * -16 + 8, 1);
                }
                cards[key].localEulerAngles = new Vector3(0, 0, (key - 3.5f) * -5);
                cards[key].GetComponent<Canvas>().sortingOrder = key;
            }
        }
        else if (cardCount == 7)
        {
            cards.Add(1, transform.GetChild(0));
            cards.Add(2, transform.GetChild(1));
            cards.Add(3, transform.GetChild(2));
            cards.Add(4, transform.GetChild(3));
            cards.Add(5, transform.GetChild(4));
            cards.Add(6, transform.GetChild(5));
            cards.Add(7, transform.GetChild(6));

            foreach (int key in cards.Keys)
            {
                cards[key].localPosition = new Vector3((key - 4) * 80, Mathf.Abs(key - 4) * -5, 1);
                cards[key].localEulerAngles = new Vector3(0, 0, (key - 4) * -5);
                cards[key].GetComponent<Canvas>().sortingOrder = key;
            }
            transform.GetChild(1).localPosition = new Vector3(-160, -18, 1);
            transform.GetChild(5).localPosition = new Vector3(160, -18, 1);
            transform.GetChild(0).localPosition = new Vector3(-240, -38, 1);
            transform.GetChild(6).localPosition = new Vector3(240, -38, 1);

        }
    }
    
}
