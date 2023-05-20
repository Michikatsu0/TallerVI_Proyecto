using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{ 
    public static TitleManager Instance;
    [SerializeField] private UISettings audioSettings;
    [SerializeField] private float transitionDelay, lerpAudioTransition;
    private int tutorial, fly = 0;

    private AudioSource camUIAudioSource;
    private bool flagOneTouch = true;
    private void Start()
    {
        camUIAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        tutorial = PlayerPrefs.GetInt("TutorialComplete");
        camUIAudioSource.clip = audioSettings.titleClips[Random.Range(0, audioSettings.titleClips.Count)];
        camUIAudioSource.spatialBlend = 0.5f;
        camUIAudioSource.Play();
        TransitionUIPanel.Instance.FadeIn();
    }

    void Update()
    {
        OnTapScreenTitle();
        if (fly == 0)
            camUIAudioSource.volume = Mathf.Lerp(camUIAudioSource.volume, 0.5f, lerpAudioTransition * Time.deltaTime);
        else
            camUIAudioSource.volume = Mathf.Lerp(camUIAudioSource.volume, 0f, lerpAudioTransition * Time.deltaTime);
    }

    public void OnTapScreenTitle()
    {
        if (Input.touchCount > 0 && tutorial == 0 && flagOneTouch)
        {
            fly = 1;
            flagOneTouch = false;
            PlayerPrefs.SetInt("LoadingSceneIndexToLoad", 3);
            camUIAudioSource.PlayOneShot(audioSettings.uICanvasClips[2], 0.5f);
            StartCoroutine(TransitionToNextScene((int)SceneIndexes.LOADING));
        }
        else if (Input.touchCount > 0 && tutorial != 0)
        {
            flagOneTouch = false;
            SceneManager.LoadSceneAsync((int)SceneIndexes.LOBBY);
        }
    }
    public IEnumerator TransitionToNextScene(int sceneIndex)
    {
        TransitionUIPanel.Instance.FadeOut();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }
}
