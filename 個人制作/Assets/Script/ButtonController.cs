using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    int mode = 0;

    //タイトルに移動
    public void LoadTitle()
    {
        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Title;

    }
    //ステージ1に移動
    public void LoadMain1()
    {
        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main1;

    }
    //ステージ2に移動
    public void LoadMain2()
    {
        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main2;
    }
    //ステージ3に移動
    public void LoadMain3()
    {
        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main3;
    }
    //ホームに移動
    public void LoadHome()
    {
        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Home;
    }
    //ゲームを再開する
    public void BackGame()
    {
        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Back;
    }
    //ゲーム終了
    public void EndGame()
    {
        StartCoroutine("MoveDelay", 1.0f);
        mode = (int)Mode.GameEnd;

    }
    private IEnumerator MoveDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (mode == (int)Mode.Title)
            SceneManager.LoadScene("title");
        if (mode == (int)Mode.Home)
            SceneManager.LoadScene("home");
        if (mode == (int)Mode.Main1)
            SceneManager.LoadScene("main1");
        if (mode == (int)Mode.Main2)
            SceneManager.LoadScene("main2");
        if (mode == (int)Mode.Main3)
            SceneManager.LoadScene("main3");
        if (mode == (int)Mode.Back)
        {
            MainUIScript mainUIScript;
            GameObject obj = GameObject.Find("MainUI");
            mainUIScript = obj.GetComponent<MainUIScript>();

            mainUIScript.open_Option = false;
        }
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    //押したボタンの判別用
    public enum Mode
    {
        Title,
        Main1,
        Main2,
        Main3,
        Home,
        Back,
        GameEnd
    }
}
