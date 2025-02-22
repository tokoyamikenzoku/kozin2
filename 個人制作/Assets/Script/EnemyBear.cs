using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyBear : ObjectMove
{
    //索敵範囲
    public Collider searchArea;

    //移動関連
    public Transform[] goals; //徘徊ポイント
    public Transform player;  //プレイヤーの位置
    private int destNum;      //向かう場所
    public int goalNum;       //徘徊箇所数
    public bool isChase;      //追跡フラグ
    private int chaseTime;    //追跡解除用のカウント
    public float speed;       //移動速度
    public bool isCount;      //チェイス中カウントフラグ

    //戦闘関連
    bool attack = false;          //攻撃フラグ
    public float surpriseAttack;  //不意打ち被ダメ倍率
    public bool notDamage;        //索敵範囲を当たり判定にしないようにする

    //武器の当たり判定
    public Collider weaponCollider;

    //HPバー関連
    public new Transform camera; //カメラの位置

    //ドロップアイテム
    public GameObject healItem;

    //エフェクト
    public GameObject hitEffect;
    public GameObject deathEffect;

    //スクリプト取得
    public MainUIScript mainUI;

    // Start is called before the first frame update
    new void Start()
    {
        weaponCollider.enabled = false;
        death = false;
        move = 0.0f;
        isCount = false;

        //現在の値を最大値と同じにする
        currentHp = maxHp;
    }

    void nextGoal()
    {
        //目的地を抽選
        destNum = Random.Range(0, goalNum);
        //目的地まで移動
        agent.destination = goals[destNum].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //チェイス中の敵数カウントを増やす
        if (isChase && !isCount && !death) 
        {
            mainUI.chaseEnemyNum++;
            isCount = true;
        }
        //カウント済みの状態の敵のチェイス判定が切れると、カウントを減らす
        else if (!isChase && isCount) 
        {
            mainUI.chaseEnemyNum--;
            isCount = false;
        }
        //プレイヤーに一定距離近づくと、攻撃する
        if ((transform.position.x - player.transform.position.x) < 0.5f &&
            (transform.position.z - player.transform.position.z) < 0.5f &&
            isChase && !attack && !isAttack)
        {
            attack = true;
            isAttack = true;
            animator.SetTrigger("attack");

            Invoke("IsAttack", 0.5f);
            Invoke("NotAttack", 1.0f);
            Invoke("CanAttack", 2.0f);
            Invoke("NotWeapon", 0.3f);
        }
        //チェイス中は、移動速度と移動アニメーションを変更
        //攻撃中は移動しない
        else if (isChase && !isAttack)
        {
            //チェイス中は、索敵範囲を消去
            searchArea.enabled = false;
            move = 2.0f;
            agent.speed = 15;
            chaseTime++;
            // 対象のオブジェクトを追いかける
            agent.destination = player.transform.position;
        }
        //徘徊モード
        //目的地に一定距離近づくと、再度目的地の抽選を行う
        else if (agent.remainingDistance < 2.0f)
        {
            //徘徊時は、索敵範囲を出す
            searchArea.enabled = true;
            chaseTime = 0;
            move = 1.0f;
            agent.speed = speed;
            nextGoal();
        }

        //ゴールが生成されると、自動的にチェイスモードに移行する
        if (gameManager.spawn)
        {
            isChase = true;
        }

        //ゲームの決着がつくと、動きを止める
        if (gameManager.gameOver || gameManager.gameClear)
        {
            agent.speed = 0;
            move = 0.0f;
        }

        //チェイススタートから一定時間が経つと、徘徊モードに戻る
        if (chaseTime >= 300 && !gameManager.spawn && isChase) 
        {
            isChase = false;
        }

        ////体力が0以下になると、死亡アニメーションを表示しオブジェクトを消去
        if (currentHp <= 0.0f && !death)
        {
            if (isCount)
            {
                mainUI.chaseEnemyNum--;
                isCount = false;
            }
            animator.SetTrigger("death");
            agent.speed = 0;
            death = true;
            Invoke("Death", 0.6f);
        }

        animator.SetFloat("EnemySpeed", move, 0.1f, Time.deltaTime);

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;
        hpSlider.transform.LookAt(camera.transform);
    }

    // CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
    public void OnDetectObject(Collider other)
    {
        //索敵範囲を当たり判定に加えない
        if (other.CompareTag("weapon"))
        {
            notDamage = true;
        }
    }

    public void OnLoseObject(Collider other)
    {
        if (other.CompareTag("weapon"))
        {
            notDamage = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
        if (other.CompareTag("Player"))
        {
            isChase = true;
            chaseTime = 0;
        }

        //weaponタグのオブジェクトに触れると発動
        if (other.CompareTag("weapon") && !isDamage && !notDamage)   
        {
            GetComponent<Animator>().SetTrigger("damage");

            if(!isChase)
            {
                //現在のHPからダメージを引く
                currentHp -= playerController.attack * surpriseAttack;
            }
            else
            {
                //現在のHPからダメージを引く
                currentHp -= playerController.attack;
            }
            //エフェクトを生成する
            GameObject effects = Instantiate(hitEffect) as GameObject;
            //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
            effects.transform.position = gameObject.transform.position;
            isDamage = true;
            weaponCollider.enabled = false;
            //プレイヤーの攻撃が当たると、プレイヤーの方向を向く
            isChase =true;
            Invoke("NotDamage", 0.5f);

        }
    }

    void Death()
    {
        //死亡処理
        //エフェクトを生成する
        GameObject effects = Instantiate(deathEffect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effects.transform.position = gameObject.transform.position;
        //KILLカウントを増やす
        gameManager.killEnemy++;
        //回復アイテム生成
        Instantiate(healItem, transform.position, Quaternion.identity);
        //オブジェクトを消去する
        Destroy(gameObject);
    }
    //攻撃可能
    void CanAttack()
    {
        attack = false;
    }
    //武器コライダーつける
    void IsAttack()
    {
        weaponCollider.enabled = true;
    }
    //武器コライダー消す
    void NotWeapon()
    {
        weaponCollider.enabled = false;
    }

    
}
