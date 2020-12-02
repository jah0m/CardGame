using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckPrint : MonoBehaviour
{
    public TMP_Text text;
    CardController cardController;
    void Start()
    {
        cardController = GameObject.Find("cards").GetComponent<CardController>();
    }

    void Update()
    {
        Init(cardController.deck);
    }

    public void Init(Dictionary<int, int> Deck)
    {
        string cardName = "";
        text.text = "";
        foreach(int id in Deck.Keys)
        {
            if (id == 1)
            {
                cardName = "普通剑法";
            }
            else if (id == 2)
            {
                cardName = "回春丹";
            }
            else if (id == 3)
            {
                cardName = "无中生有";
            }
            else if (id == 4)
            {
                cardName = "逍遥步";
            }
            else if (id == 5)
            {
                cardName = "回春散";
            }
            else if (id == 6)
            {
                cardName = "快斩";
            }
            else if (id == 7)
            {
                cardName = "破釜沉舟";
            }
            else if (id == 8)
            {
                cardName = "铁布衫";
            }
            else if (id == 9)
            {
                cardName = "闷棍";
            }
            else if (id == 10)
            {
                cardName = "清心静气";
            }
            else if (id == 11)
            {
                cardName = "结阵";
            }
            else if (id == 101)
            {
                cardName = "旋风斩";
            }
            else if (id == 201)
            {
                cardName = "魔教掌法";
            }
            else if (id == 202)
            {
                cardName = "嗜血掌法";
            }
            else if (id == 203)
            {
                cardName = "魔教剑法";
            }
            else if (id == 204)
            {
                cardName = "魔功护体";
            }
            else if (id == 205)
            {
                cardName = "噬魂剑法";
            }
            text.text += cardName + " " + Deck[id] + "\n";
        }
    }
}
