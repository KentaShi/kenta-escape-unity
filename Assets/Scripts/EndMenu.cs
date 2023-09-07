using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] Text playerDeathsText;
    [SerializeField] Text finalScoreText;

    private void Start()
    {
        
       
        
    }
    public void Restart()
    {
        FindObjectOfType<GameSesion>().ResetGameSession();
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
