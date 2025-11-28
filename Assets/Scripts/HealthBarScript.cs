using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBarSlider;
    public Text healthBarValueText;

    [SerializeField] PlayerHealthScript playerHealthScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // set health bar text
        healthBarValueText.text = playerHealthScript.health.ToString() + "/" + playerHealthScript.maxHealth.ToString();

        // set slider values
        healthBarSlider.value = playerHealthScript.health;
        healthBarSlider.maxValue = playerHealthScript.maxHealth;
    }
}
