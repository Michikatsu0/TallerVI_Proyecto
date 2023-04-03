using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelMenu : MonoBehaviour
{
    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    public bool lose, win, pause, pauseButtom = true;

    private void Update()
    {


        if (lose)
            panelList[0].SetActive(true);
        else
            panelList[0].SetActive(false);

        if (win)
        {
            panelList[1].SetActive(true);
            pauseButtom = false;
        }
        else
            panelList[1].SetActive(false);

        
         
        if (pauseButtom)
            panelList[2].SetActive(true);
        else
            panelList[2].SetActive(false);

        if (pause)
        {
            panelList[3].SetActive(true);
            Time.timeScale = 0;
        }
        else
        { 
            panelList[3].SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseTheGame()
    {
        pause = true;
    }
    public void ContinueTheGame()
    {
        pause = false;
    }
}
