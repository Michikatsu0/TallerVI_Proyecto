using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] float Xdurationx = 1f;
    [SerializeField] float Ydurationy = 1f;
    [SerializeField] float Zdurationz = 1f;

    //the ease or transition
    [SerializeField] Ease Xeasex = Ease.Linear;
    [SerializeField] Ease Yeasey = Ease.Linear;
    [SerializeField] Ease Zeasez = Ease.Linear;

    //here you can choose the directions you want to move
    [SerializeField] bool Xmovex = true;
    [SerializeField] bool Ymovey = true;
    [SerializeField] bool Zmovez = true;

    //the final position. In the future we can make an adjustment to
    //include the starting position so it will be easier to change
    [SerializeField] int Xpositionx = 0;
    [SerializeField] int Ypositiony = 0;
    [SerializeField] int Zpositionz = 0;

    //cuantity of loops, -1 is infinite loops
    [SerializeField] int Xloopcuantity = -1;
    [SerializeField] int Yloopcuantity = -1;
    [SerializeField] int Zloopcuantity = -1;

    //type of loope, yoyo is go and go back
    [SerializeField] LoopType Xlooptype = LoopType.Yoyo;
    [SerializeField] LoopType Ylooptype = LoopType.Yoyo;
    [SerializeField] LoopType Zlooptype = LoopType.Yoyo;
    Sequence mySequence = DOTween.Sequence();



    //Sequence mysequence 
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(); //initialize dotween
        enemymovement();//calls the enemy movement. Dotween does not need to be in update.  
    }

    // Update is called once per frame
    void Update()
    {

    }
    void enemymovement()
    {
        if (Xmovex) { transform.DOMoveX(Xpositionx, Xdurationx).SetEase(Xeasex).SetLoops(Xloopcuantity, Xlooptype); } //moves in x
        if (Ymovey) { transform.DOMoveY(Ypositiony, Ydurationy).SetEase(Yeasey).SetLoops(Yloopcuantity, Ylooptype); } //moves in y
        if (Zmovez) { transform.DOMoveZ(Zpositionz, Zdurationz).SetEase(Zeasez).SetLoops(Zloopcuantity, Zlooptype); } //moves in z

        mySequence.Append(transform.DOMoveX(Xpositionx, Xdurationx)).Append(transform.DORotate(new Vector3(0, 180, 0), 1));


    }
}
