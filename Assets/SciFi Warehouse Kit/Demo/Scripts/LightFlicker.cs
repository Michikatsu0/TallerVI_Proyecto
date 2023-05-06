﻿ using UnityEngine;
 using System.Collections;
 
 public class LightFlicker : MonoBehaviour
 {
     public float maximumDim;
     public float maximumBoost;
     public float speed;
     public float strength;
     private Light source;
     private float initialIntensity;
     bool lol;
     public void Reset()
     {
         maximumDim = 0.2f;
         maximumBoost = 0.2f;
         speed = 0.1f;
         strength = 250;
     }
 
     public void Start()
     {
         source = GetComponent<Light>();
         initialIntensity = source.intensity;
         StartCoroutine(Flicker());
     }
 
 
     private IEnumerator Flicker()
     {
         while (!lol)
         {
             source.intensity = Mathf.Lerp(source.intensity, Random.Range(initialIntensity - maximumDim, initialIntensity + maximumBoost), strength * Time.deltaTime);
             yield return new WaitForSeconds(speed);
         }
     }
 }



