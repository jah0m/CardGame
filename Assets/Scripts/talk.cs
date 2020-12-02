using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class talk : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("UI组件")]
    public TMP_Text textLable;
    public Image faceImage;
    public Text chooseLable1;
    public Text chooseLavle2;
    
  
    [Header("wenben")]
    public TextAsset textFile1;
    public TextAsset textFile2;
    public TextAsset textFile3;
    public int index;

    [Header("头像")]
    public Sprite face1, face2;

    public GameObject button1;
    public GameObject button2;
    


    List<string> textList = new List<string>();
    

    void Start()
    {
        index = 0;
        int i = Random.Range(1, 2);
        if (i == 1)
        {
            GetTxetFormFile(textFile1);
            faceImage.sprite = face1;
    
        }
        if (i == 2)
        {
            index = 0;
            GetTxetFormFile(textFile2);
            faceImage.sprite = face2;
    
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
             
            if (index == 6)
             {
                chooseLable1.text = textList[6];
                chooseLavle2.text = textList[8];
                button1.SetActive(true);
                button2.SetActive(true);
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

    




    public void 选项1()
    {   
        gameObject.SetActive(false);
    }

    public void 选项2()
    {
        gameObject.SetActive(false);
    }


}