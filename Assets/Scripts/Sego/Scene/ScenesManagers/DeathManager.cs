using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    [SerializeField] private List<GameObject> uIObjectList = new List<GameObject>();
    [SerializeField] private float timeDeathScene, delayTransitionTime;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        TransitionUIPanel.Instance.FadeIn();
        Invoke(nameof(DelayDeath), timeDeathScene);
        PlayerPrefs.SetInt("LoadingSceneIndexToLoad",7);
        PlayerPrefs.Save();
    }
    void DelayDeath()
    {
        StartCoroutine(DeathCoroutine());
    }
    IEnumerator DeathCoroutine()
    {
        TransitionUIPanel.Instance.FadeOut();
        yield return new WaitForSeconds(delayTransitionTime);
        SceneManager.LoadSceneAsync((int)SceneIndexes.LOADING, LoadSceneMode.Single);
        yield return null;
    }
}