using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;  



public class storeManager : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] int Price;
    [SerializeField] int PriceScalling;
    [SerializeField] TMP_Text priceText;


    [SerializeField] Button activeButton;

    [SerializeField] Canvas targetCanvas;
    [SerializeField] Canvas OwnCanvas;

    private CanvasGroup myCanvasGroup;
    private CanvasGroup otherCanvas;

    [SerializeField] int targetScene;

    int numberOfUpgrade;
    void Start()
    {
        otherCanvas = targetCanvas.GetComponent<CanvasGroup>();
        myCanvasGroup = OwnCanvas.GetComponent<CanvasGroup>();

        if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
        {
            activeButton.enabled = false;
        } 
        PriceScalling = PriceScalling*PlayerPrefs.GetInt(upgradeName, 0);
        priceText.text = (Price+Price*PriceScalling*PlayerPrefs.GetInt(upgradeName, 0)).ToString();
        DOTween.Init();
            
    }
    #region buyFunctions
    public void BuyMaxLife()
    {
        if (upgradesManager.CoinQuantity > Price+PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price+PriceScalling;
            upgradesManager.ThemaxHealth++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            priceText.text = Price + PriceScalling.ToString();
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyMaxJumps()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.jumpQuantity++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.jumpQuantity;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyRegenSpeed()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.TheregenerationSpeed++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.TheregenerationSpeed;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }

    public void BuyRegenTime()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.ThetimeToRegenerate++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThetimeToRegenerate;
            priceText.text = Price + PriceScalling.ToString();

        }
        if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
        {
            activeButton.enabled = false;
        }
    }
    public void BuyRegenerableLife()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {   
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.RegenerableLife++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.RegenerableLife;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }

    }
    public void BuyDashCd()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.TheDashCoolDown++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.TheDashCoolDown;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyDashForce()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.TheDashStrenght++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.TheDashStrenght;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyJumpStrenght()
    {
        if (upgradesManager.CoinQuantity > Price + PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price + PriceScalling;
            upgradesManager.JumpStrenght++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.JumpStrenght;
            priceText.text = Price + PriceScalling.ToString();

            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }

    #endregion

    public void ChangeCanvas()
    {
        targetCanvas.enabled = true;
        otherCanvas.DOFade(1, 1); //targetCanvas
        myCanvasGroup.DOFade(0,1);  //mine
        OwnCanvas.enabled = false;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(targetScene);

    }
}