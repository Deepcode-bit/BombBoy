using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanle;
    public bool gameOver;
    public List<BaseEnemy> enemies;
    private ExitDoor exitDoor;
    private PlayerController player;

    public void RemoveEnemies(BaseEnemy enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0) exitDoor?.Open();
        //游戏通关
        if (enemies.Count == 0 && SceneManager.GetActiveScene().buildIndex == 10 && !player.isDead)
        {
            UIManager.instance.GameOver();
        }
    }
    public void AddEnemies(BaseEnemy enemy)
    {
        if (enemies == null) enemies = new List<BaseEnemy>();
        enemies.Add(enemy);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            exitDoor = FindObjectOfType<ExitDoor>();
            player = FindObjectOfType<PlayerController>();
            return;
        }
        Destroy(instance);
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverPanle.SetActive(true);
    }

    public void NextLevle()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (buildIndex >= SceneManager.sceneCountInBuildSettings) return;
        //保存数据
        PlayerPrefs.SetFloat("player_health", player.health);
        PlayerPrefs.SetInt("level", buildIndex);
        //加载场景
        SceneManager.LoadScene(buildIndex);
    }


    #region Button Event
    public void ReSetScene()
    {
        PlayerPrefs.SetFloat("player_health", 3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UIManager.instance.ResumeGame();
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetFloat("player_health",3f);
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        var level = PlayerPrefs.GetInt("level", 1);
        SceneManager.LoadScene(level);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void MainMenu() => SceneManager.LoadScene(0);

    public void NextRound()
    {
        var ng = PlayerPrefs.GetInt("ng", 1);
        PlayerPrefs.SetInt("ng", ng + 1);
        PlayerPrefs.SetInt("level", 1);
        SceneManager.LoadScene(1);
    }
    #endregion
}
