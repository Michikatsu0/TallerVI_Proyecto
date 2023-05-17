using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradesManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static int jumpQuantity;

    public static float RegularMovementSpeed, TheCrouchedSpeed, TheJumpSpeed, TheStrenghtSpeed, TheDashSpeed, TheDashDuration, TheDashStreght, TheDashCoolDown;


     public static int ThemaxHealth;
     public static float ThemaxTimeInvincible, ThedeathTime, ThetimeToRegenerate, TheregenerationSpeed;

    public static int levelCounter; //total accesive levels



    public static int CoinQuantity; //coinsPref

    private void Awake()
    {
        PlayerPrefs.GetInt("CoinsPref", CoinQuantity);

        PlayerPrefs.GetFloat("MovementSpeedPref", RegularMovementSpeed);
        PlayerPrefs.GetFloat("CrouchSpeedPref", TheCrouchedSpeed);
        PlayerPrefs.GetFloat("JumpSpeedPref", TheJumpSpeed);
        PlayerPrefs.GetFloat("StrenghtSpeedPref", TheStrenghtSpeed);
        PlayerPrefs.GetFloat("DashDpeedPref", TheDashSpeed);
        PlayerPrefs.GetFloat("DashDurationPref", TheDashDuration);
        PlayerPrefs.GetFloat("DashStrenghtPref", TheDashStreght);
        PlayerPrefs.GetFloat("DashCoolDownPref", TheDashCoolDown);
        PlayerPrefs.GetFloat("MaxTimeInvinviblePref", ThemaxTimeInvincible);
        PlayerPrefs.GetFloat("DeathTimePref", ThedeathTime);
        PlayerPrefs.GetFloat("TimeToRegeneratePref", ThetimeToRegenerate);
        PlayerPrefs.GetFloat("RegenerationSpeedPref", TheregenerationSpeed);

        PlayerPrefs.GetInt("jumpQuantityPrefs", jumpQuantity);
        PlayerPrefs.GetInt("maxHealthPrefs", ThemaxHealth);
        PlayerPrefs.GetInt("levelCounterPrefs", levelCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void OnGameOver()//se guardan al final de un juego
    {
        PlayerPrefs.SetInt("CoinsPref", CoinQuantity);

        PlayerPrefs.SetFloat("MovementSpeedPref", RegularMovementSpeed);
        PlayerPrefs.SetFloat("CrouchSpeedPref", TheCrouchedSpeed);
        PlayerPrefs.SetFloat("JumpSpeedPref", TheJumpSpeed);
        PlayerPrefs.SetFloat("StrenghtSpeedPref", TheStrenghtSpeed);
        PlayerPrefs.SetFloat("DashDpeedPref", TheDashSpeed);
        PlayerPrefs.SetFloat("DashDurationPref", TheDashDuration);
        PlayerPrefs.SetFloat("DashStrenghtPref", TheDashStreght);
        PlayerPrefs.SetFloat("DashCoolDownPref", TheDashCoolDown);
        PlayerPrefs.SetFloat("MaxTimeInvinviblePref", ThemaxTimeInvincible);
        PlayerPrefs.SetFloat("DeathTimePref", ThedeathTime);
        PlayerPrefs.SetFloat("TimeToRegeneratePref", ThetimeToRegenerate);
        PlayerPrefs.SetFloat("RegenerationSpeedPref", TheregenerationSpeed);

        PlayerPrefs.SetInt("jumpQuantityPrefs", jumpQuantity);
        PlayerPrefs.SetInt("maxHealthPrefs", ThemaxHealth);
        PlayerPrefs.SetInt("levelCounterPrefs", levelCounter);



        PlayerPrefs.Save();
    }
}
