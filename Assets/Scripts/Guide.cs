using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Guide : MonoBehaviour
{
    GameController gameController;
    public TMP_Text text;
    CardController cardController;

    void Start()
    {
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        cardController = GameObject.Find("cards").GetComponent<CardController>();
    }

    public void Init(int Id)
    {
        if (Id == 2)
        {
            text.text = "很好，你对铁匠阿牛造成5点伤害，这取决于你左下角的攻击力。" +
                "当你没有牌可以使用时，结束回合，将轮到敌方行动。";
        }
        if (Id == 3)
        {
            text.text = "你受到了伤害，使用一张回春丹进行回复吧。";
            cardController.AddCard(2);
            gameController.addCardLock = true;
        }
        if (Id == 4)
        {
            text.text = "很好，你已经会用了基础卡牌，记住了，每张卡牌都会消耗一定的内力值。也就是左下角的红色火焰。";
        }
        if(Id == 5)
        {
            text.text = "接下来，你可以使用一张伙伴卡，伙伴卡会为你召唤一个伙伴。你的伙伴会在你的回合结束时攻击";
            cardController.AddCard(501);
        }
        if(Id == 6)
        {
            text.text = "很不错，现在来战胜铁匠啊牛吧。";
            gameController.addCardLock = false;
        }
        if(Id == 7)
        {
            text.text = "不好了，铁匠阿牛召唤了两条狗。" +
                "不过没关系，等到手里有3张江湖剑法的时候，右键其中一张，将会合并成威力强大的招式。";
            cardController.canMix = true;
        }
    }
    public void Skip()
    {
        gameController.guiding = false;
        gameController.guideId++;
        if(gameController.guideId == 5)
            Init(5);
        else
            gameObject.SetActive(false);
    }
}
