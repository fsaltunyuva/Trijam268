using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public float acids = 100;
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private GameObject rainParticleSystemGameObject;
    [SerializeField] private Slider acidSlider;
    [SerializeField] private float acidDecreaseRate = 1f;
    [SerializeField] Agent agentScript;
    [SerializeField] private GameObject winPanel, losePanel;
    
    private float initialEmissionRate = 0;

    public bool stopDecrease = false;

    private void Start()
    {
        rain = rainParticleSystemGameObject.GetComponent<ParticleSystem>();
        initialEmissionRate = rain.emission.rateOverTime.constant;
        
        var emission = rain.emission;
        emission.rateOverTime = 0;
    }

    void Update()
    {
        if (!agentScript.gameOver)
        {
            if(Input.GetKey(KeyCode.Space)) // Check if space is pressed 
            {
                if (acids > 0) // Check if there are acids left
                {
                    if(!stopDecrease)
                        acids -= acidDecreaseRate * Time.deltaTime; // Decrease acids
                    var emission = rain.emission;
                    emission.rateOverTime = initialEmissionRate;
                }
                else
                {
                    var emission = rain.emission;
                    emission.rateOverTime = 0;
                }
            }
            else
            {
                var emission = rain.emission;
                emission.rateOverTime = 0;
            }
            
            if(acids <= 0) // Check if acids are less than or equal to 0
            {
                stopDecrease = true; // Stop decreasing acids
                StartCoroutine(WaitBeforeDisplayingLosePanel()); // Wait for 1.5 seconds before displaying lose panel
            }
        
            acidSlider.value = acids; // Update slider value
        }
    }
    
    IEnumerator WaitBeforeDisplayingLosePanel()
    {
        yield return new WaitForSeconds(1.5f);
        losePanel.SetActive(true);
    }
    
    public void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
