using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    // UI要素
    public GameObject menuPanel;   //一時停止画面
    public GameObject killCounter; //KILL画像
    public GameObject pause;   //「ポーズ」
    public GameObject reunion; //「再開」
    public GameObject escIcon; //Esc画像
    public new GameObject camera; //カメラオブジェクト
    public Image weaponIcon; //武器アイコン
    public Image skillIcon;  //変化アイコン
    public Image changeIcon; //変更ゲージ
    public Text currentKill; //現在の倒した敵数
    public Text goalSpawnKill; //目標敵数
    public Text chaseEnemy; //「チェイス判定に入っている敵数」
    public int chaseEnemyNum; //チェイス判定に入っている敵数
    public int randomSkill; //変更適用の判別用
    public float changeTime;   //変化が起きるまでの時間
    public float change;       //計測用
    public bool changeConsent; //変化開始フラグ
    public GameObject keyR;    //変化するボタン画像

    // 武器とスキルのアイコン
    public Sprite[] weapon; //攻撃可能
    public Sprite[] weapon_interval; //攻撃不可
    public Sprite[] skills; //変更アイコン

    //入力関連
    public bool changeInput; //任意変化

    //直前の変化を保存
    public int beforeStatus;


    // ゲームの状態を管理
    public GameManager gameManager;
    public PlayerController playerController;

    //武器判別用
    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初期設定
        menuPanel.SetActive(false);
        reunion.SetActive(false);
        keyR.SetActive(false);
        changeConsent = false;
        changeInput = false;
        beforeStatus = -1;
        changeIcon.fillAmount = 0; //変更可能ゲージの進捗度を0にする
        chaseEnemyNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ポーズやメニューの切り替え
        if (!gameManager.gameClear && !gameManager.gameOver)
        {
            HandleMenu(); // メニュー表示の処理

            //一時停止中は動かない
            if (!gameManager.openOption) 
            {
                //ステータス変化
                ChangeExtLuck();
            }
        }
        else
        {
            camera.SetActive(false);
            escIcon.SetActive(false);
        }

        // ゴール条件達成でKILLカウントを非表示に
        if (gameManager.killEnemy >= gameManager.goalSpawn) 
            killCounter.SetActive(false);

        //ステータス変化ゲージの状態を更新
        changeIcon.fillAmount = change / changeTime;

        // KILLカウントとゴールの条件を更新
        UpdateKillCount();
        //チェイス中の敵数更新
        UpdateChaseCount();
        //インターバル中のアイコン制御
        UpdateWeaponIcon();
        // アイコンを更新
        UpdateSkillIcon();
        //アイコン変更
        GameObject.Find("Skill").GetComponent<Image>().sprite = skills[randomSkill];
        //現在の変化を保存
        beforeStatus = randomSkill;
    }

    //使用装備をアイコンとして表示
    void UpdateWeaponIcon()
    {
        // プレイヤーの武器の状態に応じてアイコンを変更
        if (playerController.apLost)
        {
            // APが足りない場合、インターバル用の武器アイコンを表示
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon_interval[playerController.weapon];
        }
        else
        {
            // 通常の武器アイコンを表示
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon[playerController.weapon];
        }
    }

    // メニューの表示と非表示を切り替える処理
    private void HandleMenu()
    {
        if (gameManager.openOption)
        {
            // オプションメニューを表示
            Cursor.lockState = CursorLockMode.None; // カーソルを自由に動かせるようにする
            menuPanel.SetActive(true);
            pause.SetActive(false);
            reunion.SetActive(true);
            camera.SetActive(false);
        }
        else
        {
            // 通常ゲーム画面に戻る
            Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央にロック
            menuPanel.SetActive(false);
            reunion.SetActive(false);
            pause.SetActive(true);
            camera.SetActive(true);
        }
    }

    void ChangeExtLuck()
    {
        if (!changeConsent)
        {
            change++;

            //ゲージがたまったとき
            if (change >= changeTime)
            {
                changeConsent = true;
                keyR.SetActive(true);
                change = changeTime;
            }
        }
        else
        {
            //入力があったとき
            if (Input.GetKeyDown(KeyCode.R) && !changeInput)
            {
                //直前の変化以外の変化が起きるまで抽選
                do
                {
                    playerController.skill = Random.Range(1, 100);

                    // アイコンを更新
                    UpdateSkillIcon();
                } while (beforeStatus == randomSkill);
                beforeStatus = randomSkill;
                keyR.SetActive(false);
                changeConsent = false;
                changeInput = true;
                change = 0.0f;
                //アイコン変更
                GameObject.Find("Skill").GetComponent<Image>().sprite = skills[randomSkill];
            }
        }

        if (Input.GetKeyUp(KeyCode.R) && changeInput)
        {
            changeInput = false;
        }
    }

    // プレイヤーのスキルに応じてスキルアイコンを更新
    void UpdateSkillIcon()
    {
        //AP2倍
        if (playerController.skill <= 20)
        {
            randomSkill = 0;
        }
        //HP2倍
        else if (playerController.skill <= 40)
        {
            randomSkill = 1;
        }
        //攻撃力2倍
        else if (playerController.skill <= 50)
        {
            randomSkill = 2;
        }
        //被ダメージ2倍
        else if (playerController.skill <= 60)
        {
            randomSkill = 3;
        }
        //移動1.5倍・攻撃力0.75倍
        else if (playerController.skill <= 70)
        {
            randomSkill = 4;
        }
        //移動0.75倍・攻撃力1.5倍
        else if (playerController.skill <= 80)
        {
            randomSkill = 5;
        }
        //消費AP・攻撃力2倍
        else if (playerController.skill <= 90)
        {
            randomSkill = 6;
        }
        //被ダメージ2倍・与ダメージ0.5倍
        else if (playerController.skill <= 95)
        {
            randomSkill = 7;
        }
        //被ダメージ0.5倍・与ダメージ2倍
        else if (playerController.skill <= 100)
        {
            randomSkill = 8;
        }
    }

    // KILLカウントを更新する処理
    private void UpdateKillCount()
    {
        currentKill.text = gameManager.killEnemy.ToString(); // 現在のKILL数
        goalSpawnKill.text = gameManager.goalSpawn.ToString(); // ゴール出現に必要なKILL数
    }

    //チェイス中の敵数更新
    private void UpdateChaseCount()
    {
        chaseEnemy.text = chaseEnemyNum.ToString(); //チェイス中の敵数
    }
}
