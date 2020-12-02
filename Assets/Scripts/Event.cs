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
    public Text chooseLavle2;
    public Text chooseLavle3;
    [Header("wenben")]
    public TextAsset textFile1;
    public TextAsset textFile2;
    public TextAsset textFile3;
    public int index;
    [Header("头像")]
    public Sprite face1, face2,face3;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    Player player;
    GameController gameController;
    public CardController cardController;
    public int talkId ;
    List<string> textList = new List<string>();
    
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();
        cardController = GameObject.Find("cards").GetComponent<CardController>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        index = 0;



    }
    public void Init(int TalkId)
    {
        talkId = TalkId;
        if (talkId == 1)
        {
            GetTxetFormFile(textFile1);
            faceImage.sprite = face1;
         }
        if (talkId == 2)
        {
            GetTxetFormFile(textFile2);
            faceImage.sprite = face1;
         }
        if (talkId == 3)
        {
            GetTxetFormFile(textFile3);
            faceImage.sprite = face3;
        }

    }

    // Update is called once per frame
    void Update()           
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
         
            if (index < 6)
            {
                textLable.text += textList[index];
                textLable.text += textList[index+1];
                index++;
                index++;
            }
             
            if (index == 6 )
            {
                chooseLable1.text = textList[6];
                button1.SetActive(true);
                chooseLavle2.text = textList[8];
                button2.SetActive(true);
                chooseLavle3.text = textList[10];
                 button3.SetActive(true);
         
                
            }
        }
    }
    

    void GetTxetFormFile(TextAsset file) //读取文本逐行播放
    {
        textList.Clear();
        index = 0;
        var lineDate = file.text.Split('\n');
        foreach (var line in lineDate)
        {   
            textList.Add(line);
            textList.Add("\n");
        }
    }
    
    public void choose(int Id)
    {
        if (talkId == 0)
        {
            
        }
        if (talkId == 1)
        {   if(Id ==1)
            {
                player.skillId = 1;
                player.skillText.text = "内力疗伤";
                gameController.selectSkill = true;
                this.chooseLable1.text = "";
                this.textLable.text = "";
                Init(2);
               

            }
            if(Id ==2 )
            {
                player.skillId = 2;
                player.skillText.text = "嗜血";
                player.blood = true;
                gameController.selectSkill = true;
                this.chooseLable1.text = "";
                this.textLable.text = "";
                Init(2);

            }
            if(Id == 3 )
            {
                player.skillId = 3;
                player.skillText.text = "生命透支";
                gameController.selectSkill = true;
                this.chooseLable1.text = "";
                this.textLable.text = "";
                Init(2);

            }
            if (talkId == 2)
            {
                if (Id == 1)
                {
                    cardController.deck.Add(1, 6); //15张普通剑法
                    cardController.deck.Add(2, 1); //5张回春丹
                    cardController.deck.Add(3, 1); //3张无中生有
                    cardController.deck.Add(4, 2); //5张逍遥步
                    gameObject.SetActive(false);
                    gameController.StartGame();//游戏开始

                }
                if (Id == 2)
                {
                    cardController.deck.Add(1, 6); //15张普通剑法
                    cardController.deck.Add(2, 1); //5张回春丹
                    cardController.deck.Add(3, 1); //3张无中生有
                    cardController.deck.Add(4, 2); //5张逍遥步
                    gameObject.SetActive(false);
                    gameController.StartGame();//游戏开始
                }
                if (Id == 3)
                {
                    cardController.deck.Add(1, 6); //15张普通剑法
                    cardController.deck.Add(2, 1); //5张回春丹
                    cardController.deck.Add(3, 1); //3张无中生有
                    cardController.deck.Add(4, 2); //5张逍遥步
                    gameObject.SetActive(false);
                    gameController.StartGame();//游戏开始
                }

            }
        }
       
        
    }

   



}