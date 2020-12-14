using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyArea : MonoBehaviour
{
    public GameObject tjPrefab;
    public GameObject dogPrefab;
    public int enemyCount;

    private GameObject enemy1;
    private GameObject enemy2;
    private GameObject enemy3;
    private GameObject enemy;

    public GameObject chooseCard_;
    ChooseCard chooseCard;

    public int isAtk = 0;

    private int attack_index;

    public bool checkDie;
    GameController gameController;
    Player player;
    CardController cardController;

    

    void Start()
    {
        player = GameObject.Find("player").GetComponent<Player>();

        gameController = GameObject.Find("gameController").GetComponent<GameController>();

        chooseCard = chooseCard_.GetComponent<ChooseCard>();
        cardController = GameObject.Find("cards").GetComponent<CardController>();

        checkDie = false;
        
    }

    public void AddTj(string index, string Name, int Hp, int Atk)
    {
        enemyCount = transform.childCount;
        if (enemyCount >= 3) return;
        GameObject enemy = Instantiate(tjPrefab);
        enemy.transform.SetParent(transform);
        enemy.transform.localScale = new Vector3(1f, 1f, 1f);
        enemy.name = index;
        enemy.GetComponent<Enemy>().Init(Hp, Atk, Name);
    }
    public void AddDog(string index, string Name,int Hp,int Atk, Color color)
    {
        enemyCount = transform.childCount;
        if (enemyCount >= 3) return;
        GameObject enemy = Instantiate(dogPrefab);
        enemy.transform.SetParent(transform);
        enemy.transform.localScale = new Vector3(1f, 1f, 1f);
        enemy.name = index;
        enemy.GetComponent<Enemy>().Init(Hp, Atk, Name);
        enemy.GetComponent<Image>().color = color;
    }

    void Update()
    {
        SetPos();
        CheckDie();
        
    }
    public void CheckDie()
    {
        if (checkDie == true) return;
        if (gameController.selectSkill == false) return;
        if(transform.childCount == 0)
        {
            checkDie = true;
            foreach(Transform card in cardController.transform)
            {
                Destroy(card.gameObject);
            }
            chooseCard.Choose();
        }
    }

    public void Attack()
    {
        gameController.SetTips("敌人回合", new Color(255, 255, 255));
        GetEnemy();
        if (enemy1) attack_index = 1;
        else if (enemy2) attack_index = 2;
        else if (enemy3) attack_index = 3;

        Debug.Log(attack_index);

        if (attack_index == 1)
        {
            isAtk = 1;
            enemy1.GetComponent<Enemy>().Attack();
        }
        if (attack_index == 2)
        {
            isAtk = 2;
            enemy2.GetComponent<Enemy>().Attack();

        }
        if (attack_index == 3)
        {
            isAtk = 3;
            enemy3.GetComponent<Enemy>().Attack();
        }
    }

    public void End1Attack()
    {
        if (attack_index == 1)
        {
            if (enemy2) attack_index = 2;
            else if (enemy3) attack_index = 3;
            else EndAllAttack();
        }
        else if (attack_index == 2)
        {
            if (enemy3) attack_index = 3;
            else EndAllAttack();
        }
        else if (attack_index == 3)
        {
            attack_index = 0;
            EndAllAttack();
        }

        if (attack_index == 1) {
            isAtk = 1;
            enemy1.GetComponent<Enemy>().Attack();
        }
        
        if (attack_index == 2)
        {
            isAtk = 2;
            enemy2.GetComponent<Enemy>().Attack();
        }
        if (attack_index == 3)
        {
            isAtk = 3;
            enemy3.GetComponent<Enemy>().Attack();
        }
    }

 
    public void EndAllAttack()
    {
        attack_index = 0;
        gameController.StartTurn();
    }

    public void 改关()
    {
        if (transform.childCount == 0)
        {
            gameController.设定关卡(gameController.关卡 + 1);
        }
    }
    public void Hurt(int num)
    {
        GetEnemy();
        if (enemy1)
        {
            enemy1.GetComponent<Enemy>().Hurt(num);
        }
        else if(enemy2)
        {
            enemy2.GetComponent<Enemy>().Hurt(num);
        }
        else if (enemy3)
        {
            enemy3.GetComponent<Enemy>().Hurt(num);
        }
    }

    public void SetBuff(int Id, int Round)
    {
        GetEnemy();
        if (enemy1)
        {
            enemy1.GetComponent<Enemy>().SetBuff(Id, Round);
        }
        else if (enemy2)
        {
            enemy2.GetComponent<Enemy>().SetBuff(Id, Round);
        }
        else if (enemy3)
        {
            enemy3.GetComponent<Enemy>().SetBuff(Id, Round);
        }
    }

    public void UpdateBuff()
    {
        GetEnemy();
        if (enemy1) enemy1.GetComponent<Enemy>().UpdateBuff();
        if (enemy2) enemy2.GetComponent<Enemy>().UpdateBuff();
        if (enemy3) enemy3.GetComponent<Enemy>().UpdateBuff();
    }

    public void AllHurt(int num)
    {
        GetEnemy();
        if (enemy1) enemy1.GetComponent<Enemy>().Hurt(num);
        if (enemy2) enemy2.GetComponent<Enemy>().Hurt(num);
        if (enemy3) enemy3.GetComponent<Enemy>().Hurt(num);
    }

    public void GetEnemy()
    {
        enemy1 = GameObject.Find("enemy1");
        enemy2 = GameObject.Find("enemy2");
        enemy3 = GameObject.Find("enemy3");
    }
    


    public void SetPos()
    { 
        GetEnemy();
        if (enemy1)
        {
            if (isAtk != 1)
            {
                enemy1.transform.localPosition = new Vector3(100f, 0f, 0f);
            }
            

        }
        if (enemy2)
        {
            if (isAtk != 2)
            {
                enemy2.transform.localPosition = new Vector3(200f, 0f, 0f);
            }
            
        }
        if (enemy3)
        {
            if (isAtk != 3)
            {
                enemy3.transform.localPosition = new Vector3(300f, 0f, 0f);
            }
            
        }
    }
}
