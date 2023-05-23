using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthResponse : MonoBehaviour
{
    [SerializeField] private StatsSettings statsSettings;

    public float currentHealth;

    private float blinkTimer, intensity, tmpTimeToRegenerate,regenerateValue = 0, currentVolumen;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private CharacterController characterController;
    private AudioSource audioSource;  
    private RagdollResponse ragdoll;
    private Slider healthSlider;
    private Image fillImage, playerImage;
    private TextMeshProUGUI textMeshPro;
    private bool canRegenerate = true, isRegenerating, deathScript, audioBlowFlag = true, audioRegeFlag = true;
    
    private void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        healthSlider = GameObject.Find("Health_Bar").GetComponent<Slider>();

        fillImage = healthSlider.fillRect.gameObject.GetComponent<Image>();

        playerImage = GameObject.Find("Player_Image").GetComponent<Image>();

        textMeshPro = healthSlider.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        ragdoll = GetComponentInChildren<RagdollResponse>();
        characterController = GetComponent<CharacterController>();

        currentHealth = statsSettings.maxHealth;

        healthSlider.maxValue = statsSettings.maxHealth;
        healthSlider.value = currentHealth;

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0.5f;
        audioSource.loop = false;
    }

    private void Update()
    {

        blinkTimer -= Time.deltaTime;

        intensity = (Mathf.Clamp01(blinkTimer / statsSettings.blinkDuration) * statsSettings.blinkIntensity) + 1.0f;

        if (intensity == 1 && !isRegenerating)
            playerImage.color = Color.white * intensity;
        else
            playerImage.color = Color.red * intensity;

        if (blinkTimer > -0.01f)
            BlinkColorChanger();

        healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, statsSettings.transitionDamageLerp * Time.deltaTime);
        int  stringHealth = (int)healthSlider.value;
        textMeshPro.text = stringHealth.ToString();

        IsDeath();

        if (deathScript) return;

        UIColorChanger();

        if (currentHealth < 10.0f && !isRegenerating && canRegenerate)
        {
            audioSource.PlayOneShot(statsSettings.deathClips[4], 1f);
            Invoke(nameof(DelayRegeneration), tmpTimeToRegenerate);
            canRegenerate = false;
        }

        if (isRegenerating)
        {
            currentVolumen = 1.0f;
            if (!audioSource.isPlaying)
            {
                audioSource.clip = statsSettings.deathClips[5];
                audioSource.Play();
            }
            playerImage.color = Color.green * 1;
            regenerateValue += Time.deltaTime; 
            currentHealth = regenerateValue * statsSettings.regenerationSpeed;

            if (healthSlider.value >= 10.0f)
            {
                if (audioSource.isPlaying)
                    currentVolumen = 0.0f;

                audioRegeFlag = true;
                canRegenerate = true;
                isRegenerating = false;
                regenerateValue = 0;
                tmpTimeToRegenerate = statsSettings.timeToRegenerate;
            }


        }
        else
        {
            if (audioSource.isPlaying)
            {
                currentVolumen = 1.0f;
            }
            
            currentVolumen = 0f;
        }

        regenerateValue = currentHealth;
    }
    void BlinkColorChanger()
    {
        skinnedMeshRenderer.materials[0].color = statsSettings.armatureColorsMaterial[0] * intensity;
        skinnedMeshRenderer.materials[1].color = statsSettings.armatureColorsMaterial[1] * intensity;
        skinnedMeshRenderer.materials[2].color = statsSettings.armatureColorsMaterial[2] * intensity;

        foreach (var material in statsSettings.armatureHelmetMaterials)
            material.color = Color.white * intensity;
    }
    void DelayRegeneration()
    {
        isRegenerating = true;
        regenerateValue = currentHealth;
    }

    public void TakeDamage(float amount) //Changes the current Health, public so enemydamage can access it. When damaged, starts the timer for invencibility
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, statsSettings.maxHealth);
        blinkTimer = statsSettings.blinkDuration;
        isRegenerating = false;
        canRegenerate = true;
    }

    void UIColorChanger()
    {
        Color healthBarColor = Color.Lerp(statsSettings.sliderColors[1], statsSettings.sliderColors[0], healthSlider.value / healthSlider.maxValue);
        fillImage.color = healthBarColor;
    }

    public void IsDeath()
    {
        if (currentHealth <= 0.0f)
            StartCoroutine(DeathCoroutine());

        if (transform.position.y <= -20)
            currentHealth = 0;
    }

    IEnumerator DeathCoroutine() //waits for the destruction of the player, use and adjust the time for a death animation
    {
        statsManager.muertes++; //añade uno al contador de muertes
        statsManager.OnGameOver();
        upgradesManager.SaveGame();
        audioSource.PlayOneShot(statsSettings.deathClips[UnityEngine.Random.Range(0, 4)],0.5f);
        deathScript = true;
        ragdoll.ActivateRagdolls();
        characterController.enabled = false;
        yield return new WaitForSeconds(statsSettings.deathTime);
        LevelUIManager.Instance.lose = true;
    }

}

