using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    [SerializeField] public bool lose, win, joystick, pause, pausePanel, pauseButtom, loading, dashButton;
    private Joystick leftJoystick, rightJoystick;
    Vector2 center = new Vector2(0.5f, 0.5f);

    private void Awake()
    {
        instance = this;
        leftJoystick = panelList[4].GetComponent<Joystick>();
        rightJoystick= panelList[5].GetComponent<Joystick>();
    }

    private void Update()
    {
        if (joystick)
        {
            panelList[4].SetActive(true);
            panelList[5].SetActive(true);
        }
        else
        {
            panelList[4].SetActive(false);
            panelList[5].SetActive(false);
        }
         
        if (dashButton)
            panelList[6].SetActive(true);
        else
            panelList[6].SetActive(false);

        if (pauseButtom)
            panelList[2].SetActive(true);
        else
            panelList[2].SetActive(false);

        if (pausePanel)
            panelList[3].SetActive(true);
        else
            panelList[3].SetActive(false);

        if (pause)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();

            pausePanel = true;
            pauseButtom = false;
            joystick = false;
            dashButton = false;

            Time.timeScale = 0;
        }
        else
        {
            pausePanel = false;
            joystick = true;
            pauseButtom = true;
            dashButton = true;

            Time.timeScale = 1;
        }

        if (lose)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();

            panelList[0].SetActive(true);
            joystick = false;
            pauseButtom = false;
        }
        else
        {
            panelList[0].SetActive(false);
        }

        if (win)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();

            panelList[1].SetActive(true);
            pauseButtom = false;
            joystick = false;
        }
        else
        {
            panelList[1].SetActive(false);
        }
    }

    public void ResetTheLevel()
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
