using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeManager : MonoBehaviour
{
    [SerializeField] string upgradeName;
    [SerializeField] int Price;
    [SerializeField] int PriceScalling;


    [SerializeField] Button activeButton;

    [SerializeField] Canvas targetCanvas;
    [SerializeField] Canvas OwnCanvas;

    int numberOfUpgrade;
    void Start()
    {
        if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
        {
            activeButton.enabled = false;
        } 
    }
    #region buyFunctions
    public void BuyMaxLife()
    {
        if (upgradesManager.CoinQuantity > Price*PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price*PriceScalling;
            upgradesManager.ThemaxHealth++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyMaxJumps()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.jumpQuantity++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyRegenSpeed()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.TheregenerationSpeed++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }

    public void BuyRegenTime()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.ThetimeToRegenerate++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
        }
        if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
        {
            activeButton.enabled = false;
        }
    }
    public void BuyTimeInvincible()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.ThemaxTimeInvincible++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }

    }
    public void BuyDashCd()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.TheDashCoolDown++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyDashForce()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.TheDashStrenght++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }
    public void BuyCrouchSpeed()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.TheCrouchedSpeed++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
            if (PlayerPrefs.GetInt(upgradeName, 0) >= 4)
            {
                activeButton.enabled = false;
            }
        }
    }

    public void BuyMovementSpeed()
    {
        if (upgradesManager.CoinQuantity > Price * PriceScalling)
        {
            upgradesManager.CoinQuantity -= Price * PriceScalling;
            upgradesManager.RegularMovementSpeed++;
            upgradesManager.SaveGame();
            PriceScalling = PriceScalling * upgradesManager.ThemaxHealth;
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
        OwnCanvas.enabled = false;
    }
}