using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager1 : MonoBehaviour
{
    // Start is called before the first frame update

    public static int coinQuantity = 0;
    [SerializeField] TMP_Text coinText;
    public string probandoProbando;


    // Update is called once per frame
    void Update()
    {
        coinText.text = coinQuantity.ToString();
    }
}
