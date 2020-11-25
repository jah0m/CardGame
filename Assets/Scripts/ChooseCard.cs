using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCard : MonoBehaviour
{
    public GameObject cardSet1;
    public GameObject cardSet2;
    public GameObject cardSet3;
    public Text cardSetName1;
    public Text cardSetName2;
    public Text cardSetName3;

    public GameObject cardPrefab;
    
    public List<int> set1Cards = new List<int>();
    public List<int> set2Cards = new List<int>();
    public List<int> set3Cards = new List<int>();

    GameController gameController;
    EnemyArea enemyArea;
    private int id;

    private Dictionary<int, int> deck1 = new Dictionary<int, int>();
    private Dictionary<int, int> deck2 = new Dictionary<int, int>();

    /// <summary>
    /// typeId: 1 :江湖 2：魔教 3：
    /// </summary>
    /// <param name="typeId"></param>
    void Start()
    {
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
          
        ///牌库1
        deck1.Add(1, 20);
        deck1.Add(2, 3);
        deck1.Add(3, 1);
        deck1.Add(4, 3);
        deck1.Add(5, 3);
        deck1.Add(6, 3);
        deck1.Add(7, 1);
        deck1.Add(8, 2);
        deck1.Add(9, 3);
        deck1.Add(10, 1);

        //牌库2

        deck2.Add(201,5);
        deck2.Add(202,5);
        deck2.Add(203,3);
        deck2.Add(204,2);
        deck2.Add(205,5);

        
        //SetCardSet(1, 1);
        //SetCardSet(2, 1);
        //SetCardSet(3, 2);

    }

    public void Choose()
    {
        gameObject.SetActive(true);
        Invoke("AddCardSet", 0.1f);
        
    }

    private void AddCardSet()
    {
        if (gameController.关卡 <= 2)
        {
            SetCardSet(1, 1, 1);
            SetCardSet(2, 1, 1);
            SetCardSet(3, 1, 1);
        }
        else if (gameController.关卡 <= 4)
        {
            SetCardSet(1, 1, 1);
            SetCardSet(2, 1, 1);
            SetCardSet(3, 2, 1);
        }
    }

    public void SetCardSet(int cardSetId, int typeId, int times) //输入卡牌类型id
    {
        for(int i = 0; i < times; i++)
        {
            AddCard(cardSetId, typeId);
        }
    }

    public void AddSet(int cardSetId)
    {
        if (cardSetId == 1)
        {
            foreach(int i in set1Cards)
            {
                if (gameController.tempDeck.ContainsKey(i))
                {
                    gameController.tempDeck[i] += 1;
                }
                else
                {
                    gameController.tempDeck.Add(i, 1);
                }
            }
        }
        else if (cardSetId == 2)
        {
            foreach (int i in set2Cards)
            {
                if (gameController.tempDeck.ContainsKey(i))
                {
                    gameController.tempDeck[i] += 1;
                }
                else
                {
                    gameController.tempDeck.Add(i, 1);
                }
            }
        }
        else if (cardSetId == 3)
        {
            foreach (int i in set3Cards)
            {
                if (gameController.tempDeck.ContainsKey(i))
                {
                    gameController.tempDeck[i] += 1;
                }
                else
                {
                    gameController.tempDeck.Add(i, 1);
                }
            }

        
        }

        //
        for (int i = 0; i < cardSet1.transform.childCount; i++)
            GameObject.Destroy(cardSet1.transform.GetChild(i).gameObject);
        for (int i = 0; i < cardSet2.transform.childCount; i++)
            GameObject.Destroy(cardSet2.transform.GetChild(i).gameObject);
        for (int i = 0; i < cardSet3.transform.childCount; i++)
            GameObject.Destroy(cardSet3.transform.GetChild(i).gameObject);
        set1Cards.Clear();
        set2Cards.Clear();
        set3Cards.Clear();
        gameObject.SetActive(false);
        enemyArea.改关();
    }

    public void AddCard(int cardSetId, int typeId)
    {
        GameObject card = Instantiate(cardPrefab);
        if (typeId == 1)
        {
            int count = 0;
            foreach (int c in deck1.Keys)
            {
                count += deck1[c];
            }
            int index = Random.Range(0, count);
            int sum = 0;
            foreach (int i in deck1.Keys)
            {
                sum += deck1[i];
                if (index < sum)
                {
                    id = i;
                    break;
                }
            }
        }
        else if(typeId == 2)
        {
            int count = 0;
            foreach (int c in deck2.Keys)
            {
                count += deck2[c];
            }
            int index = Random.Range(0, count);
            int sum = 0;
            foreach (int i in deck2.Keys)
            {
                sum += deck2[i];
                if (index < sum)
                {
                    id = i;
                    break;
                }
            }
        }
        if (cardSetId == 1)
        {
            card.transform.SetParent(cardSet1.transform);
            set1Cards.Add(id);
        }
        else if (cardSetId == 2)
        {
            card.transform.SetParent(cardSet2.transform);
            set2Cards.Add(id);
        }
        else if (cardSetId == 3)
        {
            card.transform.SetParent(cardSet3.transform);
            set3Cards.Add(id);
        }
        card.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        card.GetComponent<Card>().Init(id);
        card.GetComponent<Card>().choose = true;
    }
 
}
