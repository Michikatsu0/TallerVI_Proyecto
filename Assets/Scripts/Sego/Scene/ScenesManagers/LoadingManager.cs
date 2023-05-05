using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    public float transitionDelay;

    private int sceneIndex;
    private bool onContinueButton;
    private Animator animator;

    private void Awake()
    {
        TransitionUIPanel.Instance.FadeIn();
        Instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (onContinueButton)
        {
            StartCoroutine(nameof(TransitionNextScene));
        }
    }

    private IEnumerator TransitionNextScene()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        yield return null;
    }

    public void LoadingScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
    }

}
