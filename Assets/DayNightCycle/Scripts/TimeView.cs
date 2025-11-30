using System;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace DayNightCycle
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timeText;

        [SerializeField] Light sun;
        [SerializeField] Light moon;
        [SerializeField] AnimationCurve lightIntensityCurve;
        [SerializeField] float maxSunIntensity = 1;
        [SerializeField] float maxMoonIntensity = 0.5f;

        [SerializeField] Color dayAmbientLight;
        [SerializeField] Color nightAmbientLight;
        [SerializeField] Volume volume;
        [SerializeField] Material skyboxMaterial;

        [SerializeField] RectTransform dial;
        float initialDialRotation;

        ColorAdjustments colorAdjustments;

        TimeService _timeService;

        [Inject]
        public void Construct(TimeService service)
        {
            Debug.LogError("TimeService constructed");
            _timeService = service;
            _timeService.OnTimeUpdated += UpdateTime;

            initialDialRotation = dial.rotation.eulerAngles.z;
        }

        private void OnDestroy()
        {
            skyboxMaterial.SetFloat("_Blend", 0);
            _timeService.OnTimeUpdated -= UpdateTime;
        }
        
        public void UpdateTime()
        {
            if (timeText != null)
            {
                timeText.text = _timeService.CurrentTime.ToString("HH:mm");
            }
            
            RotateSun();
            UpdateLightSettings();
            UpdateSkyBlend();
        }
        
        void RotateSun()
        {
            float rotation = _timeService.CalculateSunAngle();
            sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
            moon.transform.rotation = Quaternion.AngleAxis(rotation + 180, Vector3.right);
            dial.rotation = Quaternion.Euler(0, 0, rotation - initialDialRotation);
        }
        
        void UpdateLightSettings()
        {
            float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
            float lightIntensity = lightIntensityCurve.Evaluate(dotProduct);

            sun.intensity = Mathf.Lerp(0, maxSunIntensity, lightIntensity);
            moon.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightIntensity);

            if (colorAdjustments == null) return;
            colorAdjustments.colorFilter.value = Color.Lerp(nightAmbientLight, dayAmbientLight, lightIntensity);
        }
        
        void UpdateSkyBlend()
        {
            float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.up);
            float blend = Mathf.Lerp(0, 1, lightIntensityCurve.Evaluate(dotProduct));
            skyboxMaterial.SetFloat("_Blend", blend);
        }
    }
}