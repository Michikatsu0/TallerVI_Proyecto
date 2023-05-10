using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoolDownDashBar : MonoBehaviour
{
    [SerializeField] private PlayerMechanicResponse mechanicResponse;
    [SerializeField] private float sliderTimeTransition;

    private Slider slider;
    public bool isDashing, barRegeneration;
    private float currentTimeRegeneration, currentValue, refVelocity;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        PlayerActionsResponse.ActionDashBarCoolDown += UpdatePlayerDashBar;
        slider.minValue = 0;
        slider.maxValue = mechanicResponse.playerSettings.dashCoolDown;
        slider.value = slider.maxValue;
        currentValue = slider.value;
    }

    private void Update()
    {
            slider.maxValue = mechanicResponse.playerSettings.dashCoolDown;

            if (isDashing) 
            {
                currentValue = 0;
                Invoke(nameof(StartRegeneration), mechanicResponse.playerSettings.dashDuration);
                slider.value -= Time.deltaTime / sliderTimeTransition;
            }
            else
            {
                if (barRegeneration)
                {
                    currentValue += Time.deltaTime;
                    slider.value = currentValue;
                    if (slider.value >= slider.maxValue)
                    {
                        barRegeneration = false;
                    }
                }
            }
        
    }

    private void StartRegeneration()
    {
        barRegeneration = true;
    }

    private void UpdatePlayerDashBar(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    private void OnDestroy()
    {
        PlayerActionsResponse.ActionDashBarCoolDown -= UpdatePlayerDashBar;
    }
}
