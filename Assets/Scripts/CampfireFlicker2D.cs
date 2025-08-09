using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class CampfireFlicker2D : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Light2D light2D;

    [Header("Base Settings")]
    [SerializeField] private float baseIntensity = 1.2f;
    [SerializeField] private float baseInnerRadius = 1.0f;
    [SerializeField] private float baseOuterRadius = 3.0f;

    [Header("Variance")]
    [SerializeField] private float intensityVariance = 0.35f;
    [SerializeField] private float radiusVariance = 0.25f;

    [Header("Flicker Speed")]
    [SerializeField] private float speed = 2.0f;      // higher = faster changes
    [SerializeField] private float smoothness = 8.0f;  // higher = smoother response

    [Header("Optional Warmth Shift")]
    [SerializeField] private bool shiftColor = true;
    [SerializeField] private Color coolColor = new Color(1.0f, 0.78f, 0.55f);
    [SerializeField] private Color warmColor = new Color(1.0f, 0.53f, 0.25f);

    private float seedI;
    private float seedR;

    private void Reset()
    {
        light2D = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        if (!light2D) light2D = GetComponent<Light2D>();

        seedI = Random.value * 1000f;
        seedR = Random.value * 1000f;

        // Initialize to base values
        light2D.intensity = baseIntensity;
        light2D.pointLightInnerRadius = baseInnerRadius;
        light2D.pointLightOuterRadius = baseOuterRadius;
    }

    private void Update()
    {
        if (!light2D) return;

        float t = Time.time;

        // Perlin noise 0..1 -> -1..1
        float nI = Mathf.PerlinNoise(seedI, t * speed) * 2f - 1f;
        float nR = Mathf.PerlinNoise(seedR, t * speed * 0.8f) * 2f - 1f;

        float targetIntensity = baseIntensity + nI * intensityVariance;
        float targetInner = baseInnerRadius + nR * radiusVariance;
        float targetOuter = baseOuterRadius + nR * radiusVariance * 1.5f;

        // Clamp
        if (targetIntensity < 0f) targetIntensity = 0f;
        if (targetInner < 0f) targetInner = 0f;
        if (targetOuter <= targetInner) targetOuter = targetInner + 0.01f;

        // Smooth towards targets
        float s = Time.deltaTime * smoothness;
        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, s);
        light2D.pointLightInnerRadius = Mathf.Lerp(light2D.pointLightInnerRadius, targetInner, s);
        light2D.pointLightOuterRadius = Mathf.Lerp(light2D.pointLightOuterRadius, targetOuter, s);

        if (shiftColor)
        {
            float k = Mathf.InverseLerp(baseIntensity - intensityVariance, baseIntensity + intensityVariance, light2D.intensity);
            light2D.color = Color.Lerp(coolColor, warmColor, k);
        }
    }
}
