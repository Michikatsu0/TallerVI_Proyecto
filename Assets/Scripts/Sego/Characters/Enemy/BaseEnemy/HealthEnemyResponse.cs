using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthEnemyResponse : MonoBehaviour
{
    [SerializeField] public StatsEnemySettings statsEnemySettings;
    [SerializeField] private Slider healthSlider;

    private AudioSource audioSource;
    private Image fillImage;
    [HideInInspector] public float currentHealth, maxHealth;
    private float blinkTimer, intensity;
    [HideInInspector] public bool deathScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxHealth = statsEnemySettings.maxHealth;
        currentHealth = maxHealth;
        healthSlider.maxValue= maxHealth;
        healthSlider.minValue=0;
        healthSlider.value = maxHealth;
        fillImage = healthSlider.fillRect.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        blinkTimer -= Time.deltaTime;
        intensity = (Mathf.Clamp01(blinkTimer / statsEnemySettings.blinkDuration) * statsEnemySettings.blinkIntensity) + 1.0f;
        if (blinkTimer > -0.01f)
            BlinkColorChanger();
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, statsEnemySettings.transitionDamageLerp * Time.deltaTime);
        if (deathScript) return;
        IsDeath();
        UIColorChanger();
    }

    public void TakeDamage(float amount, Vector3 direction) //Changes the current Health, public so enemydamage can access it. When damaged, starts the timer for invencibility
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        blinkTimer = statsEnemySettings.blinkDuration;
    }

    void BlinkColorChanger()
    {
        statsEnemySettings.turretMaterial.color = Color.white * intensity;
    }

    void UIColorChanger()
    {
        Color healthBarColor = Color.Lerp(statsEnemySettings.sliderColors[1], statsEnemySettings.sliderColors[0], healthSlider.value / healthSlider.maxValue);
        fillImage.color = healthBarColor;
    }

    public void IsDeath()
    {
        if (currentHealth <= 0.0f)
            StartCoroutine(DeathCoroutine());
    }


    IEnumerator DeathCoroutine() //waits for the destruction of the player, use and adjust the time for a death animation
    {
        audioSource.PlayOneShot(statsEnemySettings.deathClips[UnityEngine.Random.Range(0, 5)], 1f);
        deathScript = true;
        
        
        yield return new WaitForSeconds(statsEnemySettings.deathTime);
          
    }
}
