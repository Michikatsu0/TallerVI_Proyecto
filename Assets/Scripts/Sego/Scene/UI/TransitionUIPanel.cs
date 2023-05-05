using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUIPanel : MonoBehaviour
{
    public static TransitionUIPanel Instance;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }
    public void FadeIn()
    {
        animator.SetBool("OnFade", true);
    }

    public void FadeOut()
    {
        animator.SetBool("OnFade", false);
    }


}
