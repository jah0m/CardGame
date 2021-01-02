using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private int hp;
    private int atk;
    private int maxHp;
    public Text hpText;
    public Text floatText;
    public Text buffText;
    public Image hpBar;
    

    Player player;
    Team team;
    GameController gameController;
    public Animator anim;
    EnemyArea enemyArea;
    public Text name;

    private bool isDog;

    public bool diz; //眩晕

    public Dictionary<string, int> buffs = new Dictionary<string, int>();
    public GameObject buffsObj;
    Buffs buffsIcon;

    GameObject guidesObj;

    Vector3 pos;

    void Start()
    {
        isDog = false;
        player = GameObject.Find("player").GetComponent<Player>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        anim = GetComponent<Animator>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
        team = GameObject.Find("team").GetComponent<Team>();
        buffsIcon = buffsObj.GetComponent<Buffs>();
        guidesObj = GameObject.Find("guides");
    }
    void Update()
    {
       
    }
    public void Init(int Hp, int Atk, string Name)
    {
        maxHp = Hp;
        hp = Hp;
        atk = Atk;
        name.text = Name;

        SetHp(0);
    }

    public void SetHp(int num)
    {
        hp += num;
        if (hp <= 0)
        {
            hp = 0;
            anim.SetTrigger("Death");
            Destroy(gameObject, 1f);
        }
        hpText.text = hp + "/" + maxHp;
        hpBar.fillAmount = (float)hp / maxHp;
        if (num > 0)
        {
            SetFloat(1, num.ToString(), new Color(0, 255, 0));
        }
        else if (num < 0)
        {
            SetFloat(1, num.ToString(), new Color(255, 0, 0));
        }

        if(name.text == "铁匠阿牛" && num < 0 &&gameController.guideId == 2)
        {
            guidesObj.SetActive(true);
            guidesObj.GetComponent<Guide>().Init(2);
        }
    }
    public void SetFloat(int Type, string info, Color color)//type : 1伤害, 2buff
    {
        if (Type == 1)
        {
            floatText.text = info;
            floatText.color = color;
        }
        else if (Type == 2)
        {
            buffText.text = info;
            buffText.color = color;
        }
        Invoke("ClearFloat", 1f);
    }

    public void ClearFloat()
    {
        floatText.text = "";
        buffText.text = "";
    }

    public void Hurt(int num)
    {
        anim.SetTrigger("Hurt");
        SetHp(-num);
        
    }

    public void Attack()//攻击动画
    {
        if (name.text == "铁匠阿牛" && hp <= 30 && isDog == false)
        {
            isDog = true;
            enemyArea.AddDog("enemy1","阿黄1号", 10, 5, new Color(255, 255, 255));
            enemyArea.AddDog("enemy2","阿黄2号", 10, 5, new Color(255, 255, 255));
            guidesObj.SetActive(true);
            guidesObj.GetComponent<Guide>().Init(7);
        }
        if (diz == true)//是否眩晕
        {
            AttackOver();
            return;
        }

        pos = transform.localPosition;
        Debug.Log("now" + pos);
        GameObject target = team.GetFirst();
        transform.localPosition = new Vector3(target.transform.localPosition.x + 80, pos.y, pos.z);//瞬移
        Debug.Log("after" + transform.localPosition);
        Invoke("AttackAni", 0.8f);
    }

    private void AttackAni()
    {
        anim.SetBool("Attack", true);
    }
    private void Hit()//帧时间，命中时给予伤害
    {

        team.SetBuff(201, 2);
        team.Hurt(atk);
    }

    private void AttackOver()
    {
        anim.SetBool("Attack", false);
        enemyArea.isAtk = 0;
        Invoke("BackToPos", 0.5f);
    }

    private void BackToPos()
    {
        transform.localPosition = pos;
        Invoke("End1Attack", 0.5f);
    }

    private void End1Attack()
    {
        enemyArea.End1Attack();
    }

    public void SetBuff(int Id, int Round)
    {
        if (Id == 201)
        {
            SetFloat(2, "眩晕", new Color(255, 0, 0));
            if (!buffs.ContainsKey("眩晕"))
            {
                buffs.Add("眩晕", Round);
            }
            else
            {
                buffs["眩晕"] = Round;
            }
        }
        CheckBuff();
    }

    public void CheckBuff()
    {
        buffsIcon.ClearAllBuff();
        List<int> rm = new List<int>();
        for (int i = 0; i < buffs.Count; i++)//遍历字典
        {
            if (buffs[buffs.ElementAt(i).Key] > 0)//如果剩余回合不为零则设置buff效果
            {
                int round = buffs[buffs.ElementAt(i).Key];
                if (buffs.ElementAt(i).Key == "眩晕")
                {
                    diz = true;
                    buffsIcon.AddBuff(201, 2, round);
                }
            }
            else if (buffs[buffs.ElementAt(i).Key] == 0)//如果剩余回合为0则取消buff效果并移除buff
            {
                if (buffs.ElementAt(i).Key == "眩晕")
                {
                    diz = false;
                }
                rm.Add(i);
            }
        }
        for (int i = 0; i < rm.Count; i++)
        {
            buffs.Remove(buffs.ElementAt(i).Key);
        }
    }

    public void UpdateBuff()
    {
        if (buffs != null)//如果buffs不为空，则让buffs内的所有buff回合数减一
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[buffs.ElementAt(i).Key] -= 1;
            }
            CheckBuff();
        }
    }
}
