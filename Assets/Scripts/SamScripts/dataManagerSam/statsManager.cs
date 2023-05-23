using System.Collections;
using System.Collections.Generic;
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
   

    void Awake()
    {
        muertes = PlayerPrefs.GetInt("MuertesPrefs", muertes);
        saltosDados = PlayerPrefs.GetInt("SaltosPrefs", saltosDados);
        dashesDados = PlayerPrefs.GetInt("DashesPrefs", dashesDados);
        dañoHecho = PlayerPrefs.GetInt("DañoPrefs", dañoHecho);
        enemigosDerrotados =PlayerPrefs.GetInt("EnemigosPrefs", enemigosDerrotados);
        balasDisparadas =PlayerPrefs.GetInt("balasDisparadasPref", balasDisparadas);

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
        PlayerPrefs.SetInt("DañoPrefs", dañoHecho);
        PlayerPrefs.SetInt("EnemigosPrefs", enemigosDerrotados);
        PlayerPrefs.SetInt("balasDisparadasPref", balasDisparadas);
        PlayerPrefs.Save();
    }

}
