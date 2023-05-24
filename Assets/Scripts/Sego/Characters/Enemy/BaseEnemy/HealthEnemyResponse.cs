using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthEnemyResponse : MonoBehaviour
{
    [SerializeField] public StatsEnemySettings statsEnemySettings;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private bool humanoide;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AudioSource audioSource;
    private Image fillImage;
    private CoinSpawner coinSpawner;
    [HideInInspector] public float currentHealth, maxHealth;
    private float blinkTimer, intensity;
    [HideInInspector] public bool deathScript, onHit;

    void Start()
    {
        if (humanoide)
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        maxHealth = statsEnemySettings.maxHealth;
        currentHealth = maxHealth;
        healthSlider.maxValue= maxHealth;
        healthSlider.minValue = 0;
        healthSlider.value = maxHealth;
        fillImage = healthSlider.fillRect.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        blinkTimer -= Time.deltaTime;
        intensity = (Mathf.Clamp01(blinkTimer / statsEnemySettings.blinkDuration) * statsEnemySettings.blinkIntensity) + 1.0f;
        if (blinkTimer > -0.1f)
            BlinkColorChanger();
        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, statsEnemySettings.transitionDamageLerp * Time.deltaTime);
        if (deathScript) return;
        IsDeath();
        UIColorChanger();
    }

    public void TakeDamage(float amount, Vector3 direction) 
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        blinkTimer = statsEnemySettings.blinkDuration;
        onHit = true;
    }

    void BlinkColorChanger()
    {
        if (humanoide)
        {
            skinnedMeshRenderer.materials[0].color = statsEnemySettings.armatureColorMaterials[0] * intensity;
            skinnedMeshRenderer.materials[1].color = statsEnemySettings.armatureColorMaterials[1] * intensity;
            skinnedMeshRenderer.materials[2].color = statsEnemySettings.armatureColorMaterials[2] * intensity;
        }
        else
            statsEnemySettings.effectMaterial[0].color = Color.white * intensity;
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

    IEnumerator DeathCoroutine() 
    {
        deathScript = true;
        
        yield return new WaitForSeconds(statsEnemySettings.deathTime);
        audioSource.PlayOneShot(statsEnemySettings.deathClips[UnityEngine.Random.Range(0, 5)], 1f);
        healthSlider.gameObject.SetActive(false);
        coinSpawner.SpawnCoins();
    }
}
