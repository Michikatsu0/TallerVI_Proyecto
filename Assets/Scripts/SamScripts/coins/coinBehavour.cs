using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class coinBehavour : MonoBehaviour
{
    public GameObject elJugador;
   public int rarozongo =0;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        //transform.DOMove(transform.position, 10f);

        

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(3, 0, 0);
        //transform.position = transform.position + (elJugador.transform.position - transform.position);
      
        transform.position =  Vector3.Lerp(transform.position, elJugador.transform.position, 100);
        Debug.Log(rarozongo);
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.CompareTag("Player"))
        {
            Debug.Log("this is fine");
            CoinManager1.coinQuantity++;
            Destroy(gameObject);
        }
    }
}
