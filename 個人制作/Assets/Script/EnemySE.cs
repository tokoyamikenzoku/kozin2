using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySE : SeScript
{
    //スクリプト取得
    public EnemyBear bear;
    public GolemController golem;

    //被弾
    public AudioClip[] damage;
    public AudioClip[] powerDamage;
    public AudioClip[] downDamage;

    //死亡
    public AudioClip bearDeath;
    public bool isBearDeath;
    public AudioClip golemDeath;
    public bool isGolemDeath;

    // Start is called before the first frame update
    new void Start()
    {
        isHit = false;
        isBearDeath = false;
        isGolemDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        //敵が被弾したとき
        //死亡判定になっていないとき
        //複数回鳴らないようにする
        if ((bear.isDamage && !bear.death) || (golem.isDamage && !golem.death))
        {
            if (!isHit && !gameManager.isDamageSe) 
            {
                if (playerController.attack > playerController.firstAttack) //被ダメ増加
                {
                    se.PlayOneShot(powerDamage[playerController.weapon]);
                }
                else if (playerController.attack < playerController.firstAttack) //被ダメ減少
                {
                    se.PlayOneShot(downDamage[playerController.weapon]);
                }
                else //通常
                {
                    se.PlayOneShot(damage[playerController.weapon]);
                }
                isHit = true;
                gameManager.isDamageSe = true;
            }
            
        }
        else
        {
            isHit = false;
            gameManager.isDamageSe = false;
        }

        //熊死亡
        if (bear.death) 
        {
            //死亡時のSE
            //鳴っている最中ならば他の敵が死亡しても鳴らさない
            if (!isBearDeath && !gameManager.isDeathSe) 
            {
                se.PlayOneShot(bearDeath);
                isBearDeath = true;
                gameManager.isDeathSe = true;
            }
        }
        else
        {
            isBearDeath = false;
            gameManager.isDeathSe = false;
        }

        //ゴーレム死亡
        if (golem.death) 
        {
            //死亡時のSE
            //鳴っている最中ならば他の敵が死亡しても鳴らさない
            if (!isGolemDeath && !gameManager.isDeathSe)
            {
                se.PlayOneShot(golemDeath);
                isGolemDeath = true;
                gameManager.isDeathSe = true;
            }
        }
        else
        {
            isGolemDeath = false;
            gameManager.isDeathSe = false;
        }
    }
}
