using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicBarScript : MonoBehaviour
{
    public Slider healthBarSlider;
    public Text healthBarValueText;

    [SerializeField] PlayerMagicScript playerMagicScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // set health bar text
        healthBarValueText.text = playerMagicScript.currentMP.ToString() + "/" + playerMagicScript.maxMP.ToString();

        // set slider values
        healthBarSlider.value = playerMagicScript.currentMP;
        healthBarSlider.maxValue = playerMagicScript.maxMP;
    }
}
