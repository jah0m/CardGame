using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class Story : MonoBehaviour
{


    [Header("UI组件")]
    public TMP_Text textLable;
    public Image faceImage;
    [Header("wenben")]
    public TextAsset textFile;
    public int index;
    public GameObject talkObj;
  
    Event talk;




    List<string> textList = new List<string>();

    void Start()
    {
        talk = talkObj.GetComponent<Event>();
        GetTxetFormFile(textFile);
    }

    // Update is called once per frame
    void Update()
    {   if(index<=20)
        {
            if (Input.GetMouseButtonUp(0))
            {
                textLable.text = textList[index];
                index++;
            }
            if ( Input.GetMouseButtonUp(0) && index == textList.Count)
            {
                gameObject.SetActive(false);
                talkObj.SetActive(true);
                talk.Init(1);
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





