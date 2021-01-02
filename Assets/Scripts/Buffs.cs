using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buffs : MonoBehaviour
{
    public GameObject buffPrefab;
    
    public void AddBuff(int Id, int Type, int Round) //type: 1为玩家buff ，2为队友或敌人buff
    {
        GameObject buff = Instantiate(buffPrefab);
        buff.transform.SetParent(transform);
        buff.transform.localScale = new Vector3(1f, 1f, 1f);
        if(Id == 1)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "逍";
        }
        else if (Id == 2)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "回";
        }
        else if (Id == 3)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "铁";
        }
        else if (Id == 4)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "清";
        }
        else if (Id == 5)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "护";
        }
        else if (Id == 201)
        {
            buff.GetComponent<Image>().sprite = Resources.Load("buff/晕", typeof(Sprite)) as Sprite;
            buff.transform.GetChild(1).GetComponent<Text>().text = Round.ToString();
        }
        else if(Id == 206)
        {
            buff.transform.GetChild(0).GetComponent<Text>().text = "庸";
        }

    }
    public void ClearAllBuff()
    {
        for (int i = 0; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);
    }


}
