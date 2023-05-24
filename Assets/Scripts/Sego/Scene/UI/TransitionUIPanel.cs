using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionUIPanel : MonoBehaviour
{
    public static TransitionUIPanel Instance;
    public Animator animator;
    // Start is called before the first frame update
    private void Awake()
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
