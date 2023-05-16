using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;
    [SerializeField] private float delayDeathTime;
    [SerializeField] private float delayLoadingTime;
    [SerializeField] private float delayTransitionTime;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        TransitionUIPanel.Instance.FadeIn();
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {

        yield return new WaitForSeconds(delayDeathTime);
        yield return new WaitForSeconds(delayLoadingTime);
        yield return new WaitForSeconds(delayTransitionTime);
        
    }
}
