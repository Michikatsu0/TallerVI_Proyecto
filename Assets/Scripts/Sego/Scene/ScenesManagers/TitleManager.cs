using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{ 
    public static TitleManager Instance;
    [SerializeField] private float transitionDelay;
    [SerializeField] private int tutorial;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private BaseUISettings audioSettings;
    private void Start()
    {
        tutorial = PlayerPrefs.GetInt("TutorialComplete");
        TransitionUIPanel.Instance.FadeIn();
        audioSource.clip = audioSettings.titleClips[Random.Range(0, audioSettings.titleClips.Count)];
        audioSource.volume = Mathf.Lerp(0, 0.5f, 1f);
        audioSource.spatialBlend = 0.5f;
        audioSource.Play();
    }

    void Update()
    {
        OnTapScreenTitle();
    }

    public void OnTapScreenTitle()
    {
        if (Input.touchCount > 0 && tutorial == 0)
        {
            PlayerPrefs.SetInt("LoadingSceneIndexToLoad", 3);
            StartCoroutine(TransitionToNextScene((int)SceneIndexes.LOADING));
        }
        else if (Input.touchCount > 0 && tutorial != 0)
        {
            SceneManager.LoadSceneAsync((int)SceneIndexes.LOBBY);
        }
    }
    public IEnumerator TransitionToNextScene(int sceneIndex)
    {
        TransitionUIPanel.Instance.FadeOut();
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 1f);
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }
}
