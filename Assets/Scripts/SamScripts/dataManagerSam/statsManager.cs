using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statsManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static int muertes; //MuertesPrefs
    public static int saltosDados;//SaltosPrefs 
    public static int dashesDados;//DashesPrefs
    public static int da�oHecho;//Da�oPrefs
    public static int enemigosDerrotados;//EnemigosPrefs
    public static int balasDisparadas;

    void Awake()
    {
        PlayerPrefs.GetInt("MuertesPrefs", muertes);
        PlayerPrefs.GetInt("SaltosPrefs", saltosDados);
        PlayerPrefs.GetInt("DashesPrefs", dashesDados);
        PlayerPrefs.GetInt("Da�oPrefs", da�oHecho);
        PlayerPrefs.GetInt("EnemigosPrefs", enemigosDerrotados);
        PlayerPrefs.GetInt("balasDisparadasPref", balasDisparadas);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void OnGameOver()
    {
        PlayerPrefs.SetInt("MuertesPrefs",muertes);
        PlayerPrefs.SetInt("SaltosPrefs", saltosDados); //cuando el juego llega a game over, actualiza la variable que lo guarda
        PlayerPrefs.SetInt("DashesPrefs", dashesDados);
        PlayerPrefs.SetInt("Da�oPrefs", da�oHecho);
        PlayerPrefs.SetInt("EnemigosPrefs", enemigosDerrotados);
        PlayerPrefs.SetInt("balasDisparadasPref", balasDisparadas);
        PlayerPrefs.Save();
    }

}
