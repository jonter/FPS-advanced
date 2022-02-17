using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanelLogic : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;

    private void Start()
    {
        deathPanel.SetActive(false);
        FindObjectOfType<PlayerHealth>().onDeath += GameOver;
    }

    void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        deathPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
