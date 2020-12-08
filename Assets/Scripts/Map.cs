using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public GameObject pointPrefab;
    public int pointCount;
    GameObject pointsObj;
    public GameObject ply;//玩家小图标

    public int playerIn;
    GameObject point1;
    GameObject point2;
    GameObject point3;
    GameObject point4;
    GameObject point5;

    List<string> map1 = new List<string>();

    void Start()
    {
        pointsObj = GameObject.Find("points");
        map1.Add("小树林");
        map1.Add("村长家");
        map1.Add("市集");
        map1.Add("铁匠铺");
        map1.Add("村口");
        Init(1);
    }
    public void Init(int MapId)
    {
        playerIn = 1;
        if (MapId == 1)
        {
            for(int i = 0; i < 5; i++)
            {
                AddPoint(map1[i]);
            }
            GetPoint();
            ply.transform.SetParent(point1.transform);
            ply.transform.localPosition = new Vector3(0, 50, 0);
        }
        point1.GetComponent<Image>().color = new Color(255, 0, 0);
    }

    public void AddPoint(string Name)
    {
        GameObject point = Instantiate(pointPrefab);
        point.transform.SetParent(pointsObj.transform);
        point.name = "point" + pointsObj.transform.childCount.ToString();//point的名字等于他是第几个point
        Button btn = point.GetComponent<Button>();//动态添加button事件
        btn.onClick.AddListener(delegate ()
        {
            Move(point);
        });
        Text text = point.transform.GetChild(0).GetComponent<Text>();
        text.text = Name;
        point.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void GetPoint()//获取point
    {
        point1 = GameObject.Find("point1");
        point2 = GameObject.Find("point2");
        point3 = GameObject.Find("point3");
        point4 = GameObject.Find("point4");
        point5 = GameObject.Find("point5");
    }

    public void Move(GameObject pointObj) //玩家移动
    {
        if (pointObj.name != "point" + (playerIn + 1).ToString()) return;//玩家只能去到所在的下一个点；
        ply.transform.SetParent(pointObj.transform);
        playerIn++;
        ply.transform.localPosition = new Vector3(0, 50, 0);
        pointObj.GetComponent<Image>().color = new Color(255, 0, 0);
        //Invoke("display", 1f);
    }

    public void display()
    {
        gameObject.SetActive(false);
    }
}
