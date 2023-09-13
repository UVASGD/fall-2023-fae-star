using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreathingLights : MonoBehaviour
{
    [SerializeField] Light2D[] lights;
    [SerializeField] [Range(0f, 1f)] float minPercent;
    [SerializeField] float totalBreathTime;

    private float[] lightOuterRadii;
    private float[] lightInnerRadii;
    private float[] lightIntensity;
    private float[] lightPercentageOuter;
    private float[] lightPercentageIntensity;
    private float[] lightPercentageInner;

    void Awake()
    {
        lightOuterRadii = new float[lights.Length];
        lightInnerRadii = new float[lights.Length];
        lightPercentageOuter = new float[lights.Length];
        lightPercentageInner = new float[lights.Length];
        lightIntensity = new float[lights.Length];
        lightPercentageIntensity = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            lightOuterRadii[i] = lights[i].pointLightOuterRadius;
            lightInnerRadii[i] = lights[i].pointLightInnerRadius;
            lightPercentageOuter[i] = lightOuterRadii[i] * minPercent;
            lightPercentageInner[i] = lightInnerRadii[i] * minPercent;

            lightIntensity[i] = lights[i].intensity;
            lightPercentageIntensity[i] = lightIntensity[i] * minPercent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float progress = (-Mathf.Cos((Time.time * 2 * Mathf.PI) / totalBreathTime) + 1f) / 2f;

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].pointLightOuterRadius = Mathf.Lerp(lightOuterRadii[i], lightPercentageOuter[i], progress);
            lights[i].pointLightInnerRadius = Mathf.Lerp(lightInnerRadii[i], lightPercentageInner[i], progress);
            lights[i].intensity = Mathf.Lerp(lightIntensity[i], lightPercentageIntensity[i], progress);
        }
    }
}
