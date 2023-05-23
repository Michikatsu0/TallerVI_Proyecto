using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public enum StatesGameLoop
{
    Game,
    Pause
}

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    public static Action<StatesGameLoop> ActionShootWeaponTrigger;
    public StatesGameLoop stateGame = StatesGameLoop.Game;

    public int level;
    [SerializeField] private float delayToRestart;
    [SerializeField] private UISettings uISettings;
    [SerializeField] private List<GameObject> uIObjectList = new List<GameObject>();
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] public bool lose, win, joystick, pause, pausePanel, pauseButton, healthBar, dashButton, dashBar, switchWeaponButton, weaponBar, score, flagTransition;

    private Joystick leftJoystick, rightJoystick;
    private AudioSource camUIAudioSource;
   

    private void Start()
    {
        camUIAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        camUIAudioSource.spatialBlend = 0.5f;
        camUIAudioSource.volume = 1f;
        Instance = this;
        TransitionUIPanel.Instance.FadeIn();
        leftJoystick = uIObjectList[2].GetComponent<Joystick>();
        rightJoystick = uIObjectList[3].GetComponent<Joystick>();
        Invoke(nameof(DisabledPanelTransition), 1f); 
        pausePanel = false;
        pauseButton = false;
        joystick = false;
        dashButton = false;
        healthBar = false;
        switchWeaponButton = false;
        dashBar = false;
        weaponBar = false;
        score = false;
    }

    private void DisabledPanelTransition()
    {
        TransitionUIPanel.Instance.transform.gameObject.SetActive(false);
        flagTransition = true;
    }

    private void Update()
    {
        if (joystick)
        {
            uIObjectList[2].SetActive(true);
            uIObjectList[3].SetActive(true);
        }
        else
        {
            uIObjectList[2].SetActive(false);
            uIObjectList[3].SetActive(false);
        }

        if (pauseButton)
            uIObjectList[0].SetActive(true);
        else
            uIObjectList[0].SetActive(false);

        if (pausePanel)
            uIObjectList[1].SetActive(true);
        else
            uIObjectList[1].SetActive(false);

        if (dashButton)
            uIObjectList[4].SetActive(true);
        else
            uIObjectList[4].SetActive(false);

        if (dashBar)
            uIObjectList[5].SetActive(true);
        else
            uIObjectList[5].SetActive(false);

        if (switchWeaponButton)
            uIObjectList[6].SetActive(true);
        else
            uIObjectList[6].SetActive(false);

        if (healthBar)
            uIObjectList[7].SetActive(true);
        else
            uIObjectList[7].SetActive(false);

        if (weaponBar)
            uIObjectList[8].SetActive(true);
        else
            uIObjectList[8].SetActive(false);

        if (score)
            uIObjectList[9].SetActive(true);
        else
            uIObjectList[9].SetActive(false);

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
            score = false;
            ActionShootWeaponTrigger?.Invoke(stateGame);
        }
        else if (!pause && flagTransition)
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
            score = true;
            ActionShootWeaponTrigger?.Invoke(stateGame);
        }


        if (lose)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();
            pausePanel = false;
            pauseButton = false;
            joystick = false;
            dashButton = false;
            healthBar = false;
            switchWeaponButton = false;
            dashBar = false;
            weaponBar = false;
            score = false;
            TransitionUIPanel.Instance.FadeOut();
            SceneManager.LoadScene((int)SceneIndexes.DEATH, LoadSceneMode.Single);
        }
        if (win)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();  
            pauseButton = false;
            joystick = false;
        }
   
    }

    public void ResetTheLevel()
    {
        camUIAudioSource.PlayOneShot(uISettings.uICanvasClips[0]);
        TransitionUIPanel.Instance.transform.gameObject.SetActive(true);
        TransitionUIPanel.Instance.animator.SetBool("Flag", true);
        TransitionUIPanel.Instance.FadeOut();
        Invoke(nameof(DelayReset), delayToRestart);
    }
    void DelayReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseTheGame()
    {
        pause = true;
        camUIAudioSource.PlayOneShot(uISettings.uICanvasClips[1]);
    }
    public void ContinueTheGame()
    {
        pause = false;
        camUIAudioSource.PlayOneShot(uISettings.uICanvasClips[2]);
    }

    public void QuitApplication()
    {
        Application.Quit();
    } 
}
