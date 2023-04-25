using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    [SerializeField]
    private new Light light;
    [SerializeField]
    private float timeActive;
    [SerializeField]
    private float timeInactive;
    private float intensity;
    bool loop = true;
    private void Awake() 
    {
        light = GetComponent<Light>();
        intensity = light.intensity;
        StartCoroutine(Blinking());
    }
    public IEnumerator Blinking()
    {
        while(loop)
        {
            light.intensity = 0;
            yield return new WaitForSeconds(timeInactive);
            light.intensity = intensity;
            yield return new WaitForSeconds(timeActive);
        }
        StopCoroutine(Blinking());
    }
}
