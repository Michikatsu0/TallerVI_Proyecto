using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the values stored here are the levels from 1 to 4 of the given upgrades, so in the player settings they will be multiplied by a given value
/// </summary>
public class upgradesManager : MonoBehaviour
{
    //vidaMáxima, total saltos, velocidad regeneración, Tiempo para regenerarar, enfriamiento del dash, fuerza del dash, fuerza del salto, vida regenerable

     public static int RegenerableLife=0,  ThetimeToRegenerate = 0, TheregenerationSpeed = 0,  TheDashStrenght = 0, TheDashCoolDown = 0, ThemaxHealth = 0, jumpQuantity = 0, JumpStrenght = 0;

    //UpgradePricesScaleLineally
    
    [SerializeField] public float RegenerableLifeMod, TimeRegenMod, RegenSpeedMod, DashStrenghtMod, DashCDMod, MaxHealthMod, JumpQuantityMod, jumpStrenghtMod;
    public static float RegenerableLifeChange, TimeRegenChange, RegenSpeedChange, DashStrenghtChange, DashCDChange, MaxHealthChange, JumpQuantityChange, jumpStrenghtChange;

    public static int levelCounter; //total accesive levels
    public static int CoinQuantity = 0; //coinsPref

    private void Awake()
    {
        DontDestroyOnLoad(this);
        #region initializeUpgradesLvl
        RegenerableLifeChange = RegenerableLifeMod * RegenerableLife;
        TimeRegenChange = TimeRegenMod * ThetimeToRegenerate;
        RegenSpeedChange = RegenSpeedMod * TheregenerationSpeed;
        DashStrenghtChange = DashStrenghtMod * TheDashStrenght;
        DashCDChange = DashCDMod * TheDashCoolDown;
        MaxHealthChange = MaxHealthMod * ThemaxHealth;
        JumpQuantityChange = JumpQuantityMod * jumpQuantity;
        jumpStrenghtChange = jumpStrenghtMod * JumpStrenght;

        #endregion
        
        #region initializePrefs
        CoinQuantity = PlayerPrefs.GetInt("CoinsPref", CoinQuantity);



        TheDashStrenght = PlayerPrefs.GetInt("DashStrenghtPref", TheDashStrenght);
        TheDashCoolDown = PlayerPrefs.GetInt("DashCoolDownPref", TheDashCoolDown);

        RegenerableLife = PlayerPrefs.GetInt("MaxTimeInvinviblePref", RegenerableLife);
        JumpStrenght = PlayerPrefs.GetInt("JumpStrenghtPref", JumpStrenght);

        ThetimeToRegenerate = PlayerPrefs.GetInt("TimeToRegeneratePref", ThetimeToRegenerate);
        TheregenerationSpeed = PlayerPrefs.GetInt("RegenerationSpeedPref", TheregenerationSpeed);

        jumpQuantity = PlayerPrefs.GetInt("jumpQuantityPrefs", jumpQuantity);
        ThemaxHealth = PlayerPrefs.GetInt("maxHealthPrefs", ThemaxHealth);

        levelCounter = PlayerPrefs.GetInt("levelCounterPrefs", levelCounter);
        #endregion


    }

    public static void SaveGame()//se guardan al final de un juego
    {
        PlayerPrefs.SetInt("CoinsPref", CoinQuantity);
        PlayerPrefs.SetInt("DashStrenghtPref", TheDashStrenght);
        PlayerPrefs.SetInt("DashCoolDownPref", TheDashCoolDown);
        PlayerPrefs.SetInt("RegenerableLifePref", RegenerableLife);
        PlayerPrefs.SetInt("TimeToRegeneratePref", ThetimeToRegenerate);
        PlayerPrefs.SetInt("RegenerationSpeedPref", TheregenerationSpeed);
        PlayerPrefs.SetInt("JumpStrenghtPref", JumpStrenght);
        PlayerPrefs.SetInt("jumpQuantityPrefs", jumpQuantity);
        PlayerPrefs.SetInt("maxHealthPrefs", ThemaxHealth);

        PlayerPrefs.SetInt("levelCounterPrefs", levelCounter);

        PlayerPrefs.Save();
    }
}
