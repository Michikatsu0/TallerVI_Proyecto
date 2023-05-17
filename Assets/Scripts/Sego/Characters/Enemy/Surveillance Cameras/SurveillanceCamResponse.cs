using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
    public static Action<AlertSystemStates> SurveillanceAlertSecurity;

    [SerializeField] private AlertSystemStates alertSystemState;
    [SerializeField] private float timeToStartAlert, timeToAlertEnd, lerpAimWeight;
    [SerializeField] private Material glassCam;
    [SerializeField] private Animator rigController;

    private Animator animator;
    private Rig aimConstraint;
    private Color color;
    private float startTime, endTime;
    public bool canAlert = true, startAlert,onAlert;

    void Start()
    {
        animator= GetComponent<Animator>();
        aimConstraint = GetComponentInChildren<Rig>();
        SphereScanner();
    }

    // Update is called once per frame
    void Update()
    {
        TimingAlert();

        SurveillanceCamFuntion();
        

    }

    [SerializeField] private float radiusRange;
    private List<GameObject> enemysAround = new List<GameObject>();
    private RaycastHit hit;
 
    void SphereScanner()
    {
        var colliders = Physics.SphereCastAll(transform.position, radiusRange, Vector3.zero);
        foreach(var col in colliders)
        {
            if (col.transform.gameObject.CompareTag("Enemy"))
                enemysAround.Add(col.transform.gameObject);
        }
       
    }

    void SurveillanceCamFuntion()
    {
        if (onAlert)
        {
            animator.SetBool("OnAlert", true);
            rigController.enabled = false;
            aimConstraint.weight = Mathf.Lerp(aimConstraint.weight, 1f, lerpAimWeight * Time.deltaTime);
            color = Color.Lerp(Color.red, Color.green, Mathf.Clamp(startTime, 0, timeToStartAlert) / timeToStartAlert);
            glassCam.SetColor("_EmissionColor", color);
        }
        else
        {
            animator.SetBool("OnAlert", false);
            aimConstraint.weight = Mathf.Lerp(aimConstraint.weight, 0f, lerpAimWeight * Time.deltaTime);
            rigController.enabled = true;
            color = Color.Lerp(Color.green, Color.red, Mathf.Clamp(startTime, 0, timeToStartAlert) / timeToStartAlert);
            glassCam.SetColor("_EmissionColor", color);
        }
        
    }

    void TimingAlert()
    {
        if (startAlert)
        {
            startTime += Time.deltaTime;
            if (startTime >= timeToStartAlert)
            {
                onAlert = true;
            }
        }
        else
        {
            startTime += Time.deltaTime;
            if (startTime >= timeToAlertEnd)
            {
                onAlert = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            startAlert = true;
            startTime = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player")) 
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            startAlert = false;
            startTime = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusRange);
    }
}
