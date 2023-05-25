using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCoins : MonoBehaviour
{
    [SerializeField] TMP_Text CoinText;
    public static TMP_Text CoinTextStatic;
    private void Start()
    {
        CoinTextStatic = CoinText;
        CoinTextStatic.text = upgradesManager.CoinQuantity.ToString();

    }
    void Update()
    {
    }
    public static void changeCoinText()
    {
        CoinTextStatic.text = upgradesManager.CoinQuantity.ToString();

    }
}
