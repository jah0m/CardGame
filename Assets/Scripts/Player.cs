using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;



public class Player : MonoBehaviour
{
    public int hp;
    public int mp;
    public int atk;

    public int missRate;//闪避几率

    public Text hpText;
    public Text mpText;
    public Text xpText;
    public Text floatText;
    public Text atkText;
    
    public Text skillText;

    public Image hpBar;
    private Image mpBar;
    private Image xpBar;
    public  int level;
    public int xp;
    private int MaxXp;

    public Transform fires;
    public GameObject firePrefab;

    public int skillId;

    GameController gameController;
    public GameObject buffsObj;
    Buffs buffsIcon;
    public GameObject Comeback_;
    Reborn reborn;
    public Dictionary<string, int> buffs = new Dictionary<string, int>();

    public GameObject chosseSkill;

    public bool blood;

    public CardController cardController;

    public float 减伤;
    Team team;
    public bool 庸医 = false;
    Animator anim;
    EnemyArea enemyArea;

    Vector3 pos;
    public GameObject guidesObj;

    public bool attacking; //正在攻击


    void Start()
    {
       
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        reborn = Comeback_.GetComponent<Reborn>();
        //hpBar = GameObject.Find("hpBar").GetComponent<Image>();
        //mpBar = GameObject.Find("mpBar").GetComponent<Image>();
        xpBar = GameObject.Find("xpBar").GetComponent<Image>();
        xpText = GameObject.Find("xp").GetComponent<Text>();
        team = GameObject.Find("team").GetComponent<Team>();
        buffsIcon = buffsObj.GetComponent<Buffs>();

        cardController = GameObject.Find("cards").GetComponent<CardController>();
        anim = GetComponent<Animator>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
        //guidesObj = GameObject.Find("guides");
    }
    public void Init(int Hp, int Mp, int Atk)
    {
        hp = Hp;
        mp = Mp;
        atk = Atk;
        level = 1;
        xp = 0;

        SetHp(0);
        SetMp(0);
        SetAtk(0);
    }

    public void ChangePos()
    {
        anim.SetBool("Shining", true);
        team.ChangePos(transform.parent.gameObject, true);
    }

    public void SetHp(int num)
    {
        hp += num;
        if (hp > gameController.playerMaxHp) hp = gameController.playerMaxHp;
        hpText.text = hp + "/" + gameController.playerMaxHp;
        hpBar.fillAmount = (float)hp / gameController.playerMaxHp * 0.846f + 0.164f;
        if(hp<=0)
        {
            gameController.gameOver = true;
            foreach (Transform card in cardController.transform)
            {
                Destroy(card.gameObject);
            }
            reborn.back();
        }

        if (num > 0)
        {
            SetFloat(num.ToString(), new Color(0, 255, 0));
        }
        else if (num < 0)
        {
            SetFloat(num.ToString(), new Color(255, 0, 0));
        }
        if(num < 0 && gameController.guideId == 3)
        {
            guidesObj.SetActive(true);
            guidesObj.GetComponent<Guide>().Init(3);
        }
        if (num > 0 && gameController.guideId == 4)
        {
            guidesObj.SetActive(true);
            guidesObj.GetComponent<Guide>().Init(4);
        }

    }
    public void SetXp(int num)
    {
        xp += num;
        
        if (level == 1)
        {
            MaxXp = 6;
        }
        if(level == 2)
        {
            MaxXp = 10;
        }
        //xpText.text = "经验值:" + xp + "/" + MaxXp;
        if (level == 3)
        {
            xpText.text =  "MAX";
        }
        //xpBar.fillAmount = (float)xp / MaxXp;
        UpdateMp();
     
    }

    public void SetAtk(int num)
    {
        atk += num;
        atkText.text = atk.ToString();
    }

    public void SetMp(int num)
    {
        mp += num;
        if (mp > gameController.playerMaxMp) mp = gameController.playerMaxMp;
        foreach(Transform fire in fires)
        {
            Destroy(fire.gameObject);
        }
        for(int i = 0; i < mp; i++)
        {
            GameObject fire = Instantiate(firePrefab);
            fire.transform.SetParent(fires);
            fire.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        //mpText.text = mp + "/" + gameController.playerMaxMp;
        //mpBar.fillAmount = (float)mp / gameController.playerMaxMp;

    }
    public void UpdateMp()
    {
        if(level == 1 && xp >=MaxXp)
        {
            level = 2;
            xp = 0;
            gameController.playerMaxMp = 5;
            SetXp(0);
            
        }
        else if(level == 2&& xp>= MaxXp)
        {
            level = 3;
            xp = 0;
            gameController.playerMaxMp = 7;
            SetXp(0);
        }
     
        gameController.playerMpReply = gameController.playerMaxMp;


    }

    public void SetBuff(int Id, int Round)
    {
        if (Id == 1)
        {
            if (!buffs.ContainsKey("逍遥步"))
            {
                buffs.Add("逍遥步", Round);
            }
            else
            {
                buffs["逍遥步"] = Round;
            }
        }
        if (Id == 2)
        {
            if (!buffs.ContainsKey("回春散"))
            {
                buffs.Add("回春散", Round);
            }
            else
            {
                buffs["回春散"] = Round;
            }
        }
        if (Id == 3)
        {
            if (!buffs.ContainsKey("铁布衫"))
            {
                buffs.Add("铁布衫", Round);
            }
            else
            {
                buffs["铁布衫"] = Round;
            }
        }
        if (Id == 4)
        {
            if (!buffs.ContainsKey("清心静气"))
            {
                buffs.Add("清心静气", Round);
            }
            else
            {
                buffs["清心静气"] = Round;
            }
        }
        if (Id == 5)
        {
            if (!buffs.ContainsKey("魔功护体"))
            {
                buffs.Add("魔功护体", Round);
            }
            else
            {
                buffs["魔功护体"] = Round;
            }
        }
        if(Id == 206 )
        {

            if (!buffs.ContainsKey("庸医"))
            {
                buffs.Add("庸医", Round);
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
                if (buffs.ElementAt(i).Key == "逍遥步")
                {

                    missRate = 65;
                    buffsIcon.AddBuff(1, 1, round);
                }
                else if (buffs.ElementAt(i).Key == "回春散")
                {

                    SetHp(10);
                    buffsIcon.AddBuff(2, 1, round);
                }
                else if (buffs.ElementAt(i).Key == "铁布衫")
                {

                    减伤 = 0.5f;
                    buffsIcon.AddBuff(3, 1, round);
                }
                else if (buffs.ElementAt(i).Key == "清心静气")
                {

                    SetMp(10);
                    cardController.SlowAddCard();
                    buffsIcon.AddBuff(4, 1, round);
                }
                else if (buffs.ElementAt(i).Key == "魔功护体")
                {

                    减伤 = 1f;
                    buffsIcon.AddBuff(5, 1, round);
                }
                else if (buffs.ElementAt(i).Key == "庸医")
                {

                    庸医 = true;
                    buffsIcon.AddBuff(206, 1, round);
                }
            }
            else if (buffs[buffs.ElementAt(i).Key] == 0)//如果剩余回合为0则取消buff效果并移除buff
            {
                if (buffs.ElementAt(i).Key == "逍遥步")
                {
                    missRate = 0;
                }
                else if (buffs.ElementAt(i).Key == "回春散")
                {
                    SetHp(10);
                }
                else if (buffs.ElementAt(i).Key == "铁布衫")
                {
                    减伤 = 0;
                }
                else if (buffs.ElementAt(i).Key == "清心静气")
                {

                    SetMp(10);
                    cardController.SlowAddCard();
                }
                else if (buffs.ElementAt(i).Key == "魔功护体")
                {
                    减伤 = 0;
                }
                else if (buffs.ElementAt(i).Key == "庸医")
                {
                    庸医 = false;
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
    public void SetFloat(string info, Color color)
    {
        floatText.text = info;
        floatText.color = color;
        Invoke("ClearFloat", 1f);
    }

    public void ClearFloat()
    {
        floatText.text = "";
    }

    public void Hurt(int num)
    {
        int a = Random.Range(1, 101);
        if(a > missRate)
        {
            SetHp(-(int)(num * (1 - 减伤)));
        }
        else
        {
            SetFloat("闪避", new Color(0, 45, 255));
        }
    }

    public void InitSkill(int Id)
    {
        skillId = Id;
        if (Id == 1)
        {
            skillText.text = "内力疗伤";
        }
        if (Id == 2)
        {
            skillText.text = "嗜血";
            blood = true;
        }
        if (Id == 3)
        {
            skillText.text = "生命透支";
        }
        chosseSkill.SetActive(false);
        gameController.selectSkill = true;
        gameController.StartGame();//游戏开始
    }

    public void UseSkill()
    {
        if (skillId == 1)
        {
            SetHp(2);
           
        }
        if (skillId == 2)
        {
            blood = true;
        }
        if (skillId == 3)
        {
            int costHp = (int)(hp * 0.2);
            if (costHp < 5) costHp = 5;
            SetHp(-costHp);
            cardController.SlowAddCard();
        }
    }

    public void Attack()
    {
        pos = transform.position;
        GameObject target = enemyArea.GetFirst();
        transform.position = new Vector3(target.transform.position.x - 80, pos.y, pos.z);
        attacking = true;
        Invoke("AttackAni", 0.3f);
    }
    
    void AttackAni()
    {
        anim.SetTrigger("Attack");
    }

    public void BackToPos()
    {
        Debug.Log("back");
        transform.position = pos;
        attacking = false;
    }
}
