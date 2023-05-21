using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public enum AlertSystemStates
{
    Alert_LVL0,
    Alert_LVL1,
    Alert_LVL2,
}

public class SurveillanceCamResponse : MonoBehaviour
{
    [SerializeField] private SurveillanceSettings surveillanceSettings;

    [SerializeField] private Transform playerTarget, searchTarget;
    [SerializeField] private Image fillImage;
    [SerializeField] private Rig playerAimRig;
    [SerializeField] private Rig searchAimRig;
    [SerializeField] private Slider sliderTimeAlert;

    private List<BaseEnemyController> enemysAround = new List<BaseEnemyController>();
    private Color color;
    private Vector3 searchTargetPos;
    private Animator animator;
    
    private float timeStartAlert, timeEndAlert, searchTime, currentDistance;
    private bool onAlert;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.5f;
        audioSource.volume = 0.1f;
        searchTargetPos.y = transform.position.y;
        searchTargetPos.z = transform.position.z;
        SphereScanner();
        animator = GetComponent<Animator>();
        playerTarget = GameObject.Find("Player_Armature_CharacterController").transform;
        sliderTimeAlert.maxValue = surveillanceSettings.timeToStartAlert;
        sliderTimeAlert.value = timeStartAlert;
    }

    void Update()
    {
        ColorChanger();
        TimingDistanceAlertManager();
        SurveillanceCamFuntion();
        
    }

    void ColorChanger()
    {
        Color healthBarColor = Color.Lerp(surveillanceSettings.sliderColors[1], surveillanceSettings.sliderColors[0], sliderTimeAlert.value / sliderTimeAlert.maxValue);
        fillImage.color = healthBarColor;
    }

    void SphereScanner()
    {
        var colliders = Physics.OverlapSphere(transform.position + surveillanceSettings.center, surveillanceSettings.radiusSignRange, surveillanceSettings.isEnemy);
        foreach (var col in colliders)
        {
            var baseEnemy = col.gameObject.GetComponentInParent<BaseEnemyController>();
            if (baseEnemy != null && !enemysAround.Contains(baseEnemy))
            {
                enemysAround.Add(baseEnemy);
            }
        }
    }

    void SendSingAlert()
    {
        foreach(var enemyAlly in enemysAround)
        {
            enemyAlly.OnAlert = true;
        }
    }

    void SurveillanceCamFuntion()
    {
        if (onAlert)
        {
            animator.SetBool("OnAlert", true);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 0f, surveillanceSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 1f, surveillanceSettings.lerpAimWeight * Time.deltaTime);
            color = Color.Lerp(color, Color.red, surveillanceSettings.lerpTransitionColor * Time.deltaTime);
            surveillanceSettings.glassCam.SetColor("_EmissionColor", color);
            SendSingAlert();
            searchTime = 0;
        }
        else
        {
            animator.SetBool("OnAlert", false);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 1f, surveillanceSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 0f, surveillanceSettings.lerpAimWeight * Time.deltaTime);
            color = Color.Lerp(color, Color.green, surveillanceSettings.lerpTransitionColor * Time.deltaTime);
            surveillanceSettings.glassCam.SetColor("_EmissionColor", color);

            searchTime += Time.deltaTime;

            if (searchTime >= surveillanceSettings.timeToSearchPos)
            {
                searchTime = 0;
                searchTargetPos.y = transform.position.y + UnityEngine.Random.Range(0.0f, surveillanceSettings.searchYlimit);
                searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(-surveillanceSettings.alertDistance, surveillanceSettings.alertDistance);
            }

            searchTarget.position = Vector3.Lerp(searchTarget.position, searchTargetPos, surveillanceSettings.lerpSearchPosTarget * Time.deltaTime);
        }
        
    }

    void TimingDistanceAlertManager()
    {
        currentDistance = Vector3.Distance(transform.position, playerTarget.position);
        if (onAlert)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = surveillanceSettings.audioClips[0];
                audioSource.loop = true;
                audioSource.Play();
            }

            sliderTimeAlert.gameObject.SetActive(true);
            
            if (currentDistance <= surveillanceSettings.alertDistance)
            {
                sliderTimeAlert.maxValue = surveillanceSettings.timeToEndAlert;
                sliderTimeAlert.value = sliderTimeAlert.maxValue;
                if (timeEndAlert >= surveillanceSettings.timeToEndAlert) return;
                    timeEndAlert += Time.deltaTime;
            }
            else
            {
                if (timeEndAlert <= 0)
                {
                    onAlert = false;
                    sliderTimeAlert.maxValue = surveillanceSettings.timeToStartAlert;
                    sliderTimeAlert.value = sliderTimeAlert.maxValue;
                    return;
                }
                timeEndAlert -= Time.deltaTime;
            }
            sliderTimeAlert.value = Mathf.Lerp(sliderTimeAlert.value, Mathf.Clamp(timeEndAlert, 0, surveillanceSettings.timeToEndAlert), sliderTimeAlert.value/ sliderTimeAlert.maxValue);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.loop = false;
                audioSource.Stop();
            }

            if (currentDistance <= surveillanceSettings.alertDistance)
            {
                sliderTimeAlert.gameObject.SetActive(true);
                timeStartAlert += Time.deltaTime;
                
                if (timeStartAlert >= surveillanceSettings.timeToStartAlert)
                {
                    timeStartAlert = 0;
                    onAlert = true;
                    timeEndAlert = surveillanceSettings.timeToEndAlert;
                    sliderTimeAlert.maxValue = surveillanceSettings.timeToEndAlert;
                    sliderTimeAlert.value = sliderTimeAlert.maxValue;
                }
               
            }
            else
            {
                timeStartAlert -= Time.deltaTime;
                if (timeStartAlert <= 0)
                {
                    sliderTimeAlert.gameObject.SetActive(false);
                    timeStartAlert = 0;
                }
               
            }
            sliderTimeAlert.value = Mathf.Lerp(sliderTimeAlert.value, Mathf.Clamp(timeStartAlert, 0, surveillanceSettings.timeToStartAlert), surveillanceSettings.lerpTransitionSlider);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + surveillanceSettings.center, surveillanceSettings.radiusSignRange);
    }
}