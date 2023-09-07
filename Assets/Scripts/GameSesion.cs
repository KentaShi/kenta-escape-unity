using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSesion : MonoBehaviour
{
    [SerializeField]  int playerDeaths = 0;
    [SerializeField] TextMeshProUGUI deathsText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] AudioClip bgAudioClip;

    [SerializeField] int score = 0;

    public static AudioSource bgAudioSource;
    void Awake()
    {
        int numOfGameSessions = FindObjectsOfType<GameSesion>().Length;
        
        if (numOfGameSessions > 1)
        {
            Destroy(gameObject);
        }else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {

        bgAudioSource = gameObject.GetComponent<AudioSource>();
        bgAudioSource.clip = bgAudioClip;
        bgAudioSource.Play();

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Start Game" || currentSceneName == "End Game")
        {
            deathsText.text = "";
            scoreText.text = "";
        }
        deathsText.text = "You died " + playerDeaths.ToString();
        scoreText.text = score.ToString();  
    }
   

    public int GetPlayerDeaths()
    {
        return playerDeaths;
    }
    public int GetScore()
    {
        return score;
    }
    public void ProcessPlayerDeath()
    {
         TakeLife();
    }
    void TakeLife()
    {
        playerDeaths++;
        deathsText.text = "You died " + playerDeaths.ToString();
        Invoke("LoadCurrentScene", 1f);   
    }
    public void PickUp(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }
    void LoadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetGameSession()
    {
        //FindObjectOfType<ScenePersist>().ResetScenePersist();
        //SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
