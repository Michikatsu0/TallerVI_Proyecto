using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCoins : MonoBehaviour
{
    [SerializeField] TMP_Text CoinText;
    void Update()
    {
        CoinText.text = upgradesManager.CoinQuantity.ToString();
    }
}
