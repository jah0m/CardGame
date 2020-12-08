using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class Event : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI组件")]
    public TMP_Text textLable;
    public Image faceImage;
    public Text chooseLable1;
    public Text chooseLable2;
    public Text chooseLable3;
    [Header("wenben")]
    public TextAsset textFile1;
    public TextAsset textFile2;
    public TextAsset textFile3;
    public int index;
    [Header("头像")]
    public Sprite face1, face2, face3;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    Player player;
    GameController gameController;
    public CardController cardController;
    public int talkId;
    List<string> textList = new List<string>();
    public GameObject Story;
    public GameObject talkObj;
    public GameObject diologueObj;
    Diologue diologue;

    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();
        cardController = GameObject.Find("cards").GetComponent<CardController>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        diologue = diologueObj.GetComponent<Diologue>();
        index = 0;
    }
    public void Init(int TalkId)
    {
        talkId = TalkId;
        GetTxetFormFile();
        if (talkId == 1)
        {
            faceImage.sprite = face1;
        }
        if (talkId == 2)
        {
            faceImage.sprite = face3;
        }
        if (talkId == 3)
        {
            faceImage.sprite = face3;
        }
        if(talkId ==4)
        {
            faceImage.sprite = face3;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (index < 6)
            {
                textLable.text += textList[index];
                textLable.text += textList[index + 1];
                index++;
                index++;
            }
            if (index == 6)
            {
                chooseLable1.text = textList[6];
                button1.SetActive(true);
                chooseLable2.text = textList[8];
                button2.SetActive(true);
                if (textList[10]!="")
                {
                    chooseLable3.text = textList[10];
                    button3.SetActive(true);
                }
            }
        }
    }


    void GetTxetFormFile() //读取文本逐行播放
    {
        textList.Clear();
        index = 0;
        csvController.GetInstance().loadFile(Application.dataPath + "/res", "游戏剧情.csv");
        //根据索引读取csvController中的list（csv文件的内容）数据
        for (int i = 1; i <= 6; i++)
        {
            textList.Add(csvController.GetInstance().getString(talkId, i));
            textList.Add("\n");
        }
    }

    public void Choose(int Id)
    {
        chooseLable1.text = "";
        chooseLable2.text = "";
        chooseLable3.text = "";
        textLable.text = "";
        //diologue.Choose(Id, talkId);
        if (talkId == 1)
        {
            if (Id == 1)
            {
                player.skillId = 1;
                player.skillText.text = "内力疗伤";
            }
            if (Id == 2)
            {
                player.skillId = 2;
                player.skillText.text = "嗜血";
                player.blood = true;
            }
            if (Id == 3)
            {
                player.skillId = 3;
                player.skillText.text = "生命透支";
            }
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            Init(2);


        }
        else if (talkId == 2)
        {
            if (Id == 1)
            {
                
            }
            if (Id == 2)
            {
                
            }
            if (Id == 3)
            {
                
            }
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            Init(3);

        }
        else if (talkId == 3)
        {
            if (Id == 1)
            {
            }
            if (Id == 2)
            {
            }
            if (Id == 3)
            {
            }
            button1.SetActive(false);
            button2.SetActive(false);
            button3.SetActive(false);
            Init(4);
        }
        else if (talkId == 4)
        {
            if (Id == 1)
            {
                cardController.deck.Add(2, 3);
                cardController.deck.Add(1, 3); 
            }
            if (Id == 2)
            {
                cardController.deck.Add(2, 3);
                cardController.deck.Add(1, 3);
            }
            if (Id == 3)
            {
                cardController.deck.Add(2, 3);
                cardController.deck.Add(1, 3);
            }
            gameObject.SetActive(false);
            gameController.StartGame();//游戏开始
        }


    }
}


    

   



