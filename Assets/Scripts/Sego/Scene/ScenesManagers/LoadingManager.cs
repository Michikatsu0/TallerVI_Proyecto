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
    [SerializeField] private List<Animator> animators = new List<Animator>();
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private BaseUISettings audioSettings;

    private int sceneIndex;
    private bool flagOneTouech;
    private void Awake()
    {
        Instance = this;
        sceneIndex = PlayerPrefs.GetInt("LoadingSceneIndexToLoad");
    }

    void Start()
    {
        flagOneTouech = true;
        TransitionUIPanel.Instance.FadeIn();
        audioSource.clip = audioSettings.titleClips[Random.Range(0, audioSettings.titleClips.Count)];
        audioSource.volume = Mathf.Lerp(0, 0.5f, 1f);
        audioSource.spatialBlend = 0.5f;
        audioSource.Play();
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
        TransitionUIPanel.Instance.FadeOut();
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 1f);
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }

}
