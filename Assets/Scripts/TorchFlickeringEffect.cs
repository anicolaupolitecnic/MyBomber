using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFlickeringEffect : MonoBehaviour {
    public Light flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1.0f;

    void Start() {
        if (flickeringLight == null) {
            flickeringLight = GetComponent<Light>();
            if (flickeringLight == null) {
                enabled = false;
                return;
            }
        }
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker() {
        while (true) {
            // Randomly change the intensity of the light
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            flickeringLight.intensity = randomIntensity;

            // Wait for a random duration before flickering again
            yield return new WaitForSeconds(Random.Range(0.1f, flickerSpeed));

            // Restore the original intensity
            flickeringLight.intensity = maxIntensity;

            // Wait for a random duration before flickering again
            yield return new WaitForSeconds(Random.Range(0.1f, flickerSpeed));
        }
    }
}
