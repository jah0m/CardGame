using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class jq : MonoBehaviour
{


    [Header("UI组件")]
    public TMP_Text textLable;
    public Image faceImage;
    [Header("wenben")]
    public TextAsset textFile;
    public int index;
    public GameObject chooseskill;
    public GameObject jwq;


    List<string> textList = new List<string>();

    void Start()
    {
        GetTxetFormFile(textFile);
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {   if(index<20)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textLable.text = textList[index];
                index++;
            }
            if (Input.GetKeyDown(KeyCode.Space) && index == textList.Count)
            {
                chooseskill.SetActive(true);
                jwq.SetActive(false);
            }
        }
    
    }



    void GetTxetFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        var lineDate = file.text.Split('\n');


        foreach (var line in lineDate)
        {
            textList.Add(line);

        }

    }

}


