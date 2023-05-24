using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private float transitionDelay, lerpAudioTransition;
    [SerializeField] private Slider slider;
    [SerializeField] private List<Animator> animators = new List<Animator>();
    [SerializeField] private AudioUISettings audioSettings;

    private AudioSource camUIAudioSource;
    private int sceneIndex;
    private bool flagOneTouch = true;

    private void Awake()
    {
        Instance = this;
        sceneIndex = PlayerPrefs.GetInt("LoadingSceneIndexToLoad");
    }

    void Start()
    {
        camUIAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        TransitionUIPanel.Instance.FadeIn();
        camUIAudioSource.clip = audioSettings.loadingClips[Random.Range(0, audioSettings.loadingClips.Count)];
        camUIAudioSource.spatialBlend = 0.5f;
        camUIAudioSource.Play();
    }

    void Update()
    {
        if (slider.value == 1)
        {
            animators[0].SetBool("IsHide", true);
            animators[1].SetBool("IsShow", true);
            if (Input.touchCount > 0 && flagOneTouch)
            {
                camUIAudioSource.PlayOneShot(audioSettings.uICanvasClips[2], 0.5f);
                flagOneTouch = false;
                StartCoroutine(TransitionNextScene());
            }
        }
        if (flagOneTouch)
            camUIAudioSource.volume = Mathf.Lerp(camUIAudioSource.volume, 0.5f, lerpAudioTransition * Time.deltaTime);
        else
            camUIAudioSource.volume = Mathf.Lerp(camUIAudioSource.volume, 0f, lerpAudioTransition * Time.deltaTime);
        
    }

    private IEnumerator TransitionNextScene()
    {
        TransitionUIPanel.Instance.FadeOut();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }

}
