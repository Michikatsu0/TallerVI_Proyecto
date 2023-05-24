using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBarCoolDown : MonoBehaviour
{
    [SerializeField] private PlayerMechanicResponse mechanicResponse;
    private Slider slider;
    private bool isDashing, barRegeneration;
    private float sliderTimeTransition, currentTimeRegeneration, currentValue, refVelocity;

    void Start()
    {
        slider = GetComponent<Slider>();
        PlayerActionsResponse.ActionDashBarCoolDown += UpdatePlayerDashBar;
        slider.minValue = 0;
        slider.maxValue = mechanicResponse.FinalDashCd;
        slider.value = slider.maxValue;
        currentValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = mechanicResponse.FinalDashCd;

        if (isDashing)
        {
            currentValue = 0;
            sliderTimeTransition = 0.3f;
            Invoke(nameof(StartRegeneration), mechanicResponse.playerSettings.dashDuration);
            slider.value = Mathf.Lerp(slider.value, currentValue, sliderTimeTransition);
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
