using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static int muertes; //MuertesPrefs
    public static int saltosDados;//SaltosPrefs 
    public static int dashesDados;//DashesPrefs
    public static int dañoHecho;//DañoPrefs
    public static int enemigosDerrotados;//EnemigosPrefs
    public static int balasDisparadas;//balasDisparadasPref

    [SerializeField] Canvas statsCanvas;
    [SerializeField] TMP_Text Jumps;
    [SerializeField] TMP_Text Deaths;
    [SerializeField] TMP_Text Dashes;
    [SerializeField] TMP_Text Damage;
    [SerializeField] TMP_Text EnemiesKilled;
    [SerializeField] TMP_Text BulletsShot;




    void Awake()
    {
        muertes = PlayerPrefs.GetInt("MuertesPrefs", muertes);
        saltosDados = PlayerPrefs.GetInt("SaltosPrefs", saltosDados);
        dashesDados = PlayerPrefs.GetInt("DashesPrefs", dashesDados);
        dañoHecho = PlayerPrefs.GetInt("DañoPrefs", dañoHecho);
        enemigosDerrotados =PlayerPrefs.GetInt("EnemigosPrefs", enemigosDerrotados);
        balasDisparadas =PlayerPrefs.GetInt("balasDisparadasPref", balasDisparadas);

    }
    private void Start()
    {
        statsCanvas.enabled = false;
    }

    public static void OnGameOver()
    {
        PlayerPrefs.SetInt("MuertesPrefs",muertes);
        PlayerPrefs.SetInt("SaltosPrefs", saltosDados); //cuando el juego llega a game over, actualiza la variable que lo guarda
        PlayerPrefs.SetInt("DashesPrefs", dashesDados);
        PlayerPrefs.SetInt("DañoPrefs", dañoHecho);
        PlayerPrefs.SetInt("EnemigosPrefs", enemigosDerrotados);
        PlayerPrefs.SetInt("balasDisparadasPref", balasDisparadas);
        PlayerPrefs.Save();
    }

    public void showStats()
    {
        statsCanvas.enabled = true;
        Jumps.text = saltosDados.ToString()+" Times jumped ";
        Deaths.text  = muertes.ToString()+" Deaths";
        Dashes.text = dashesDados.ToString() + " Dashes performed";
        Damage.text = dañoHecho.ToString() + " Damage done";
        EnemiesKilled.text = enemigosDerrotados.ToString() + " Enemies taken down";
        BulletsShot.text = balasDisparadas.ToString() + " Bullets shot";
    }

}
