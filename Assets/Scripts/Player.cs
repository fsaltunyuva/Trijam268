using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float acids = 100;
    [SerializeField] private ParticleSystem rain;
    [SerializeField] private GameObject rainParticleSystemGameObject;
    public float initialEmissionRate = 0;

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
                acids -= 1 * Time.deltaTime; // Decrease acids
                //rain.Play(); // Play particle system (Not working for some reason)
                var emission = rain.emission;
                emission.rateOverTime = initialEmissionRate;
            }
            else
            {
                //rain.Stop(); // Stop particle system (Not working for some reason)
                var emission = rain.emission;
                emission.rateOverTime = 0;
            }
        }
        else
        {
            // rain.Stop(); // Stop particle system (Not working for some reason)
            var emission = rain.emission;
            emission.rateOverTime = 0;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collided with rain");
    }
}
