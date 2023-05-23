using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the values stored here are the levels from 1 to 4 of the given upgrades, so in the player settings they will be multiplied by a given value
/// </summary>
public class upgradesManager : MonoBehaviour
{
    // Start is called before the first frame update

     public static int ThemaxTimeInvincible,  ThetimeToRegenerate, TheregenerationSpeed, RegularMovementSpeed, TheCrouchedSpeed,  TheDashStrenght, TheDashCoolDown, ThemaxHealth, jumpQuantity;

    //UpgradePricesScaleLineally

    [SerializeField] public float TimeInvMod, TimeRegenMod, RegenSpeedMod, MovementSpeedMod, CrouchSpeedMod, DashStrenghtMod, DashCDMod, MaxHealthMod, JumpQuantityMod;
    public static float TimeInvChange, TimeRegenChange, RegenSpeedChange, MovementSpeedChange, CrouchSpeedChange, DashStrenghtChange, DashCDChange, MaxHealthChange, JumpQuantityChange;

    public static int levelCounter; //total accesive levels
    public static int CoinQuantity; //coinsPref

    private void Awake()
    {
        #region initializeUpgradesLvl
        TimeInvChange = TimeInvMod*ThemaxTimeInvincible;
        TimeRegenChange = TimeRegenMod * ThetimeToRegenerate;
        RegenSpeedChange = RegenSpeedMod * TheregenerationSpeed;
        MovementSpeedChange = MovementSpeedMod * RegularMovementSpeed;
        CrouchSpeedChange = CrouchSpeedMod * TheCrouchedSpeed;
        DashStrenghtChange = DashStrenghtMod * TheDashStrenght;
        DashCDChange = DashCDMod * TheDashCoolDown;
        MaxHealthChange = MaxHealthMod * ThemaxHealth;
        JumpQuantityChange = JumpQuantityMod * jumpQuantity;

        #endregion

        #region initializePrefs
        CoinQuantity = PlayerPrefs.GetInt("CoinsPref", CoinQuantity);

        RegularMovementSpeed = PlayerPrefs.GetInt("MovementSpeedPref", RegularMovementSpeed);
        TheCrouchedSpeed= PlayerPrefs.GetInt("CrouchSpeedPref", TheCrouchedSpeed);

        TheDashStrenght = PlayerPrefs.GetInt("DashStrenghtPref", TheDashStrenght);
        TheDashCoolDown = PlayerPrefs.GetInt("DashCoolDownPref", TheDashCoolDown);

        ThemaxTimeInvincible = PlayerPrefs.GetInt("MaxTimeInvinviblePref", ThemaxTimeInvincible);

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

        PlayerPrefs.SetFloat("MovementSpeedPref", RegularMovementSpeed);
        PlayerPrefs.SetFloat("CrouchSpeedPref", TheCrouchedSpeed);

        PlayerPrefs.SetFloat("DashStrenghtPref", TheDashStrenght);
        PlayerPrefs.SetFloat("DashCoolDownPref", TheDashCoolDown);
        PlayerPrefs.SetFloat("MaxTimeInvinviblePref", ThemaxTimeInvincible);

        PlayerPrefs.SetFloat("TimeToRegeneratePref", ThetimeToRegenerate);
        PlayerPrefs.SetFloat("RegenerationSpeedPref", TheregenerationSpeed);

        PlayerPrefs.SetInt("jumpQuantityPrefs", jumpQuantity);
        PlayerPrefs.SetInt("maxHealthPrefs", ThemaxHealth);

        PlayerPrefs.SetInt("levelCounterPrefs", levelCounter);

        PlayerPrefs.Save();
    }
}
