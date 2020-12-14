using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    public int tmCount;
    public GameObject dogTmPrefab;

    private GameObject tm1;
    private GameObject tm2;
    private GameObject tm3;

    public Text armorText;

    private int attack_index;
    public int isAtk = 0;

    public int playerIn = 3;//玩家在的位置

    public int armor = 0; //护甲
    
    GameController gameController;
    Player player;
    EnemyArea enemyArea;

    private GameObject changePos1;
    private int playerChange;

    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();
        gameController = GameObject.Find("gameController").GetComponent<GameController>();
        enemyArea = GameObject.Find("enemyArea").GetComponent<EnemyArea>();
        changePos1 = null;
    }

    void Update()
    {
        SetPos();
    }

    public int Count()
    {
        tmCount = transform.childCount;
        return tmCount;
    }

    public void ChangePos(GameObject obj, bool isPlayer)
    {
        if (gameController.isChangePos) return;
        if (changePos1 == null)
        {
            playerChange = 1;
            changePos1 = obj;
        }
        else
        {
            if (playerChange != 0 && !isPlayer)
            {
                if (obj.name == "tm1") playerIn = 1;
                if (obj.name == "tm2") playerIn = 2;
                if (obj.name == "tm3") playerIn = 3;
                playerChange = 0;
            }
            if (isPlayer)
            {
                if (changePos1.name == "tm1") playerIn = 1;
                if (changePos1.name == "tm2") playerIn = 2;
                if (changePos1.name == "tm3") playerIn = 3;
            }
            string tempName = changePos1.name;
            changePos1.name = obj.name;
            obj.name = tempName;
            changePos1 = null;
        }
    }
    public void AddDog(string index, string Name, int Hp, int Atk, Color color)
    {
        tmCount = transform.childCount;
        if (tmCount >= 3) return;
        GameObject tm = Instantiate(dogTmPrefab);
        tm.transform.SetParent(transform);
        tm.transform.localScale = new Vector3(1f, 1f, 1f);
        tm.name = index;
        tm.GetComponent<Teammate>().Init(Hp, Atk, Name);
        tm.GetComponent<Image>().color = color;
    }

    public void GetTm()
    {
        tm1 = GameObject.Find("tm1");
        tm2 = GameObject.Find("tm2");
        tm3 = GameObject.Find("tm3");
    }

    public void Attack()
    {
        if (tmCount == 1) EndAllAttack();
        GetTm();
        if (tm1 && playerIn != 1) attack_index = 1;
        else if (tm2 && playerIn != 2) attack_index = 2;
        else if (tm3 && playerIn != 3) attack_index = 3;

        if (attack_index == 1)
        {
            isAtk = 1;
            tm1.GetComponent<Teammate>().Attack();
        }
        if (attack_index == 2)
        {
            isAtk = 2;
            tm2.GetComponent<Teammate>().Attack();

        }
        if (attack_index == 3)
        {
            isAtk = 3;
            tm3.GetComponent<Teammate>().Attack();
        }
    }

    public void End1Attack()//一个队友攻击结束
    {
        if (attack_index == 1)
        {
            if (tm2 && playerIn != 2) attack_index = 2;
            else if (tm3 && playerIn != 3) attack_index = 3;
            else EndAllAttack();
        }
        else if (attack_index == 2)
        {
            if (tm3 && playerIn != 3) attack_index = 3;
            else EndAllAttack();
        }
        else if (attack_index == 3)
        {
            attack_index = 0;
            EndAllAttack();
        }

        if (attack_index == 1)
        {
            isAtk = 1;
            tm1.GetComponent<Teammate>().Attack();
        }

        if (attack_index == 2)
        {
            isAtk = 2;
            tm2.GetComponent<Teammate>().Attack();
        }
        if (attack_index == 3)
        {
            isAtk = 3;
            tm3.GetComponent<Teammate>().Attack();
        }
    }

    public void EndAllAttack()//所有队友攻击结束
    {
        attack_index = 0;
        isAtk = 0;
        Invoke("EnemyAttack", 1f);
    }

    private void EnemyAttack()
    {
        Debug.Log("enemyatk");
        enemyArea.Attack();
    }

    public void SetBuff(int Id, int Round)
    {
        GetTm();
        if (tm1)
        {
            if (playerIn == 1) player.SetBuff(Id, Round);
            else tm1.GetComponent<Teammate>().SetBuff(Id, Round);
        }
        else if (tm2)
        {
            if (playerIn == 2) player.SetBuff(Id, Round);
            else tm2.GetComponent<Teammate>().SetBuff(Id, Round);
        }
        else if (tm3)
        {
            if (playerIn == 3) player.SetBuff(Id, Round);
            else tm3.GetComponent<Teammate>().SetBuff(Id, Round);
        }
    }

    public void UpdateBuff()//刷新buff
    {
        GetTm();
        if (tm1 && playerIn != 1) tm1.GetComponent<Teammate>().UpdateBuff();
        if (tm2 && playerIn != 2) tm2.GetComponent<Teammate>().UpdateBuff();
        if (tm3 && playerIn != 3) tm3.GetComponent<Teammate>().UpdateBuff();
    }

    public void SetArmor(int num)//设定护甲
    {
        armor += num;
        armorText.text = armor.ToString();
    }
    public void Hurt(int num)
    {
        GetTm();
        int damage;
        if(num <= armor)
        {
            SetArmor(-num);
            return;
        }
        else
        {
            damage = num - armor;
            SetArmor(-armor);
        }
        if (tm1)
        {
            if (playerIn == 1) player.Hurt(damage);
            else tm1.GetComponent<Teammate>().Hurt(damage);
        }
        else if (tm2)
        {
            if (playerIn == 2) player.Hurt(damage);
            else tm2.GetComponent<Teammate>().Hurt(damage);
        }
        else if (tm3)
        {
            if (playerIn == 3) player.Hurt(damage);
            else tm3.GetComponent<Teammate>().Hurt(damage);
        }
    }

    public void AllHurt(int num)
    {
        GetTm();
        if (tm1 && playerIn != 1) tm1.GetComponent<Teammate>().Hurt(num);
        if (tm2 && playerIn != 2) tm2.GetComponent<Teammate>().Hurt(num);
        if (tm3 && playerIn != 3) tm3.GetComponent<Teammate>().Hurt(num);
    }

    public void SetPos()
    {
        GetTm();
        if (tm1)
        {
            if (isAtk != 1)
            {
                tm1.transform.localPosition = new Vector3(-100f, 0f, 0f);
            }


        }
        if (tm2)
        {
            if (isAtk != 2)
            {
                tm2.transform.localPosition = new Vector3(-200f, 0f, 0f);
            }

        }
        if (tm3)
        {
            if (isAtk != 3)
            {
                tm3.transform.localPosition = new Vector3(-300f, 0f, 0f);
            }

        }
    }
}
