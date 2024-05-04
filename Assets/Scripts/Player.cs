using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public float acids = 100;
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private GameObject rainParticleSystemGameObject;
    private float initialEmissionRate = 0;
    [SerializeField] private Slider acidSlider;
    [SerializeField] private float acidDecreaseRate = 1f;
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
        
        acidSlider.value = acids; // Update slider value
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collided with rain");
    }
}
