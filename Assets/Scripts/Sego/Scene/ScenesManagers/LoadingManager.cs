using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private float transitionDelay;
    [SerializeField] private Slider slider;
    [SerializeField] private TransitionUIPanel transitionUIPanel;
    [SerializeField] private List<Animator> animators = new List<Animator>();
    private int sceneIndex;
    private bool flagOneTouech;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        flagOneTouech = true;
        transitionUIPanel.FadeIn();
        sceneIndex = PlayerPrefs.GetInt("LoadingSceneIndexToLoad");

    }

    void Update()
    {
        if (slider.value == 1)
        {
            animators[0].SetBool("IsHide", true);
            animators[1].SetBool("IsShow", true);
            if (Input.touchCount > 0 && flagOneTouech)
            {
                flagOneTouech = false;
                StartCoroutine(TransitionNextScene());
            }
        }
    }

    private IEnumerator TransitionNextScene()
    {
        transitionUIPanel.FadeOut();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }

}
