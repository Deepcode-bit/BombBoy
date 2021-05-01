using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject healthBar;
    public GameObject bossHealthBar;
    public GameObject pauseMenu;
    public GameObject dialog;
    public GameObject endDialog;
    public Slider bossHealth;
    

    private void Awake()
    {         
        if (instance == null)
        {
            instance = this;
            bossHealthBar.SetActive(false);
            return;
        }
        Destroy(transform.gameObject);
    }

    private void Start()
    {
        var ng = PlayerPrefs.GetInt("ng", 1);
        //高周目玩家
        if (ng != 1) return;
        var index = SceneManager.GetActiveScene().buildIndex - 1;
        if (index < 0 || index > dialogText.Count - 1) return;
        dialog.GetComponent<Dialog>().dialogText = dialogText[index];
        dialog.SetActive(true);
    }

    public void UpdateHealth(float heart)
    {
        for (int i = 0; i < healthBar.transform.childCount; i++)
        {
            var state = heart > i;
            healthBar.transform.GetChild(i).gameObject.SetActive(state);
        }
    }

    public void SetBossHealth(float health)
    {
        bossHealthBar.SetActive(true);
        bossHealth.maxValue = health;
    }

    public void UpdateBossHealth(float health)
    {
        bossHealth.value = health;
    }

    public void GameOver()
    {
        var ng = PlayerPrefs.GetInt("ng", 1);
        endDialog.GetComponentInChildren<Text>().text=
            $"现在要进入第{ng+1}回的世界吗?\r\n如果现在不进入，下次游戏需要重打boss战(doge)";
        endDialog.SetActive(true);
    }

    #region Button Event
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Thread.BeginThreadAffinity();
        Time.timeScale = 1;
    }
    #endregion

    #region Game Text
    private List<List<string>> dialogText = new List<List<string>>()
    {
        new List<string>()
        {
        "操作指南:左下方区域为动态遥感，控制方向。\r\n右下方两个按钮分别控制跳跃和攻击。",
        "怪物指南:黄瓜怪,会咬人和吹灭炸弹，吹灭的炸弹可以通过引爆附近的炸弹进行点燃。"
        },
        new List<string>()
        {
            "怪物指南:鲸鱼,会吞掉炸弹，吃饱后会死亡并吐出吃掉的炸弹。",
            "怪物指南:光头怪，能踢飞炸弹和人。此关Boss"
        },
        new List<string>()
        {
            "操作指南:横向平台可以由下往上跳上去。",
            "怪物指南:大块头，会拾起炸弹朝玩家扔去。"
        },
        new List<string>()
        {
            "怪物指南:海盗船长，看到炸弹就跑，跑步速度贼快。"
        },
        new List<string>(),
        new List<string>()
        {
        },
        new List<string>()
        {
            "怪物指南:二营长的意大利炮(不是)\r\n船长的加农炮，没人知道是谁操纵着它，只看到它没日没夜地吐炸弹。"+
            "\r\n可通过炸弹进行摧毁"
        },
        new List<string>(),
        new List<string>(),
        new List<string>()
        {
            "怪物指南:靠常人难以压制的红眼。\r\n大块头大人，若不是喝了变若水..."
        }
    };
    #endregion
}
