using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    [SerializeField] private List<GameObject> panelList = new List<GameObject>();

    [SerializeField] public bool lose, win, joystick, pause, pausePanel, pauseButton, loading, dashButton, dashBar, changeGun, interactableUi, panelAnim;

    private Joystick leftJoystick, rightJoystick;
    private int triggerId;
    private void Awake()
    {
        ProbsActionResponse.InteractableUI += InteractableUI;
        Instance = this;
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

        if (changeGun)
            panelList[8].SetActive(true);
        else
            panelList[8].SetActive(false);

        if (interactableUi)
            panelList[9].SetActive(true);
        else
            panelList[9].SetActive(false);


        if (pause)
        {
            leftJoystick.ResetJoysticks();
            rightJoystick.ResetJoysticks();

            pausePanel = true;
            pauseButton = false;
            joystick = false;
            dashButton = false;

            Time.timeScale = 0;
        }
        else
        {
            pausePanel = false;
            joystick = true;
            pauseButton = true;
            dashButton = true;

            Time.timeScale = 1;
        }


        if (lose)
        {
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
    
        if (panelAnim)
        {
            PanelAnimationOn();
            panelList[10].SetActive(true);
            panelList[11].SetActive(true);
        }
        else
        {
            PanelAnimationOff();
            panelList[10].SetActive(false);
            panelList[11].SetActive(false);
        }
    }

    public void PanelAnimationOn()
    {
        for (int i = 4; i < panelList.Count; i++)
        {
            panelList[i].SetActive(false);
        }
    }

    public void PanelAnimationOff()
    {
        for (int i = 4; i < panelList.Count - 2; i++)
        {
            if (i == 9) return;
            panelList[i].SetActive(true);
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

    public void InteractableUI(bool interactableUi, int id)
    {
        this.interactableUi = interactableUi;
        this.triggerId = id;
    }

    public void InteractableButton()
    {
        ProbsActionResponse.InteractableButtonUI?.Invoke(true, triggerId);
        Invoke(nameof(ResetInteractableButton), 0.1f);
    }
    private void ResetInteractableButton()
    {
        ProbsActionResponse.InteractableButtonUI?.Invoke(false, triggerId);
    }
}
