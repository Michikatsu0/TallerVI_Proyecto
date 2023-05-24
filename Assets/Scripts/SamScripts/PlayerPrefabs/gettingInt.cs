using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gettingInt : MonoBehaviour
{
    public int testeandoTest;
    [SerializeField] TMP_Text miTexto;
    // Start is called before the first frame update
    void Start()
    {
       testeandoTest = PlayerPrefs.GetInt("testVariable");
        miTexto.text = testeandoTest.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
