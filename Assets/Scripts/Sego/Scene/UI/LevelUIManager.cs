using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public enum StatesGameLoop{
    Game,
    Pause
}

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    public static Action<StatesGameLoop> ActionShootWeaponTrigger;

    public StatesGameLoop stateGame = StatesGameLoop.Game;

    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    [SerializeField] public bool lose, win, joystick, pause, pausePanel, pauseButton, healthBar, dashButton, dashBar, switchWeaponButton, weaponBar;

    private Joystick leftJoystick, rightJoystick;

    private void Start()
    {
        Instance = this;
        TransitionUIPanel.Instance.FadeIn();
        leftJoystick = panelList[4].GetComponent<Joystick>();
        rightJoystick = panelList[5].GetComponent<Joystick>();
        Invoke(nameof(DisabledPanelTransition), 1f);
    }

    private void DisabledPanelTransition()
    {
        TransitionUIPanel.Instance.transform.gameObject.SetActive(false);
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

        if (pauseButton)
            panelList[2].SetActive(true);
        else
            panelList[2].SetActive(false);

        if (pausePanel)
            panelList[3].SetActive(true);
        else
            panelList[3].SetActive(false);

        if (dashButton)
            panelList[6].SetActive(true);
        else
            panelList[6].SetActive(false);

        if (dashBar)
            panelList[7].SetActive(true);
        else
            panelList[7].SetActive(false);

        if (switchWeaponButton)
            panelList[8].SetActive(true);
        else
            panelList[8].SetActive(false);

        if (healthBar)
            panelList[9].SetActive(true);
        else
            panelList[9].SetActive(false);

        if (weaponBar)
            panelList[10].SetActive(true);
        else
            panelList[10].SetActive(false);

        if (pause)
        {
            stateGame = StatesGameLoop.Pause;

            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();

            pausePanel = true;
            pauseButton = false;
            joystick = false;
            dashButton = false;
            healthBar = false;
            switchWeaponButton= false;
            dashBar = false;
            weaponBar = false;

            ActionShootWeaponTrigger?.Invoke(stateGame);
        }
        else
        {
            stateGame = StatesGameLoop.Game;

            pausePanel = false;
            joystick = true;
            pauseButton = true;
            dashButton = true;
            healthBar= true;
            switchWeaponButton = true;
            dashBar = true;
            weaponBar = true;


            ActionShootWeaponTrigger?.Invoke(stateGame);
        }


        if (lose)
        {
            TransitionUIPanel.Instance.FadeOut();
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();
            panelList[0].SetActive(true);
            joystick = false;
            pauseButton = false;
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
            pauseButton = false;
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
