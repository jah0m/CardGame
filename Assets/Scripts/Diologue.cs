using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Diologue : MonoBehaviour
{


    public TMP_Text textLable;
    public Image faceImage;
    List<string> textList = new List<string>();
    Event eve;
    public GameObject talkObj;
    public Sprite face1, face2, face3;
    int id;
    int talkId;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        eve = talkObj.GetComponent<Event>();
        faceImage.sprite = face1;
        GetTxetFormFile();
       
      
    }

    // Update is called once per frame
    void Update()
    {
        Choose();
    }
    public void Choose()
    {
        textList.Clear();
        for (int i = 1; i <= 6; i++)
        {
            textList.Add(csvController.GetInstance().getString(i, talkId - 1));
        }
        textLable.text = textList[id - 1];
      
    }

    public void GetTxetFormFile() //读取文本逐行播放
    {
        textList.Clear();
        csvController.GetInstance().loadFile(Application.dataPath + "/res", "文本框内容.csv");

        //根据索引读取csvController中的list（csv文件的内容）数据
   
    }
    
    public void Choose(int Id,  int TalkId)
    {
        gameObject.SetActive(true);
        id = Id;
        talkId = TalkId;

    }
    public void Btn()
    {
           gameObject.SetActive(false);
    }
}
