using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UI
    public GameObject menuPanel;
    public new GameObject camera;
    public GameObject killCounter;
    //íACR
    public Image weapon_icon;
    public Sprite[] wepon;
    public Sprite[] wepon_interval;
    //XLACR
    public  Image skill_icon;
    public Sprite[] skill;

    //Q[óÔ
    public bool open_Option = false;

    //S[ð
    public Text currentKill;
    public Text goalSpawnKill;
    //Xe[^X\L
    public Text now_hpNum;
    public Text max_hpNum;
    public Text now_apNum;
    public Text max_apNum;
    public RectTransform hp;
    bool hpSlide;
    bool hpSlide2;
    public RectTransform ap;
    bool apSlide;
    bool apSlide2;

    //·µh~
    public bool input = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        camera.SetActive(true);
        killCounter.SetActive(true);

        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();
        open_Option = false;

        hpSlide = false;
        hpSlide2 = false;
        apSlide = false;
        apSlide2 = false;

        Icon();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager;
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();
        playerController = obj.GetComponent<PlayerController>();

        //IvVÇ
        //EscÅì
        if(gameManager.gamePlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !input)
            {
                switch (open_Option)
                {
                    case false:
                        open_Option = true;
                        break;
                    case true:
                        open_Option = false;
                        break;
                }
                input = true;
            }

            //{^Åì
            if (!open_Option)
            {
                menuPanel.SetActive(false);
                camera.SetActive(true);
                Time.timeScale = 1;
            }
            else
            {
                menuPanel.SetActive(true);
                camera.SetActive(false);
                Time.timeScale = 0;
            }
        }
        
        //·µh~
        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }

        if(!gameManager.gamePlay)
        {
            camera.SetActive(false);
        }

        if (playerController.currentHp < 100.0f && !hpSlide)
        {
            hp.position += new Vector3(0.01f, 0, 0);
            hpSlide = true;
        }
        else if (playerController.currentHp >= 100.0f && hpSlide)
        {
            hp.position -= new Vector3(0.01f, 0, 0);
            hpSlide = false;
        }

        if (playerController.currentAp < 100.0f && !apSlide)
        {
            ap.position += new Vector3(0.01f, 0, 0);
            apSlide = true;
        }
        else if (playerController.currentAp >= 100.0f && apSlide)
        {
            ap.position -= new Vector3(0.01f, 0, 0);
            apSlide = false;
        }

        currentKill.text = playerController.kill_enemy.ToString();
        goalSpawnKill.text = playerController.goalspawn.ToString();

        now_hpNum.text = playerController.currentHp.ToString();
        max_hpNum.text = playerController.maxHp.ToString();
        now_apNum.text = playerController.currentAp.ToString();
        max_apNum.text = playerController.maxAp.ToString();

        if (playerController.kill_enemy >= 5)
            killCounter.SetActive(false);

        Icon();
    }

    //gpõðACRÆµÄ\¦
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.interval || playerController.apLost) 
        {
            //í
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNUCKLE];
                    break;
            }
        }
        else if (!playerController.interval)
        {
            //í
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNUCKLE];
                    break;
            }
        }
       
        //t^øÊ
        //AP2{
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2{
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //UÍ2{
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //í_[W2{
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //Ú®1.5{EUÍ0.75{
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //Ú®0.75{EUÍ1.5{
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //ÁïAPEUÍ2{
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //í_[W2{E^_[W0.5{
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //í_[W0.5{E^_[W2{ 0
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[8];
        }
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }
}
