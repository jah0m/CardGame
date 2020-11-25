using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Teammate : MonoBehaviour
{
    private int hp;
    private int atk;
    public Text hpText;
    public Text buffText;
    public Text floatText;
    public Text name;

    public GameObject buffsObj;
    Buffs buffsIcon;
    public Dictionary<string, int> buffs = new Dictionary<string, int>();
    public bool diz;//眩晕

    Player player;
    GameController gameController;
    public Animator anim;
    EnemyArea enemyArea;
    Team team;


    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        anim = GetComponent<Animator>();
        team = GameObject.Find("team").GetComponent<Team>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
        buffsIcon = buffsObj.GetComponent<Buffs>();
    }

    public void Init(int Hp, int Atk, string Name)
    {
        hp = Hp;
        atk = Atk;
        name.text = Name;

        SetHp(0);
    }

    public void ChangePos()
    {
        team.ChangePos(gameObject, false);
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
        hpText.text = "生命" + hp;
        if (num > 0)
        {
            SetFloat(1, num.ToString(), new Color(0, 255, 0));
        }
        else if (num < 0)
        {
            SetFloat(1, num.ToString(), new Color(255, 0, 0));
        }
    }
    public void SetFloat(int Type, string info, Color color)//type : 1伤害, 2buff
    {
        if(Type == 1)
        {
            floatText.text = info;
            floatText.color = color;
        }
        else if(Type == 2)
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
        if(diz == true)
        {
            AttackOver();
            return;
        }
        anim.SetBool("Attack", true);
    }

    private void Hit()//帧时间，命中时给予伤害
    {
        enemyArea.Hurt(atk);
    }

    private void AttackOver()
    {
        anim.SetBool("Attack", false);
        team.End1Attack();
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
                if (buffs.ElementAt(i).Key == "眩晕")
                {
                    diz = true;
                    buffsIcon.AddBuff(201, 2);
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
