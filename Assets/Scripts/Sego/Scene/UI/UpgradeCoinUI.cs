using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeCoinUI : MonoBehaviour
{
    public static UpgradeCoinUI Instance;
    private TMP_Text coinText;



    private void Start()
    {
        Instance = this;
        coinText = GetComponentInChildren<TMP_Text>();
        coinText.text = upgradesManager.CoinQuantity.ToString();

    }


    public void changeCoinText()
    {
        coinText.text = upgradesManager.CoinQuantity.ToString();

    }
}
