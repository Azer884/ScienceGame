using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;

    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;

    [SerializeField] private Light globalLight;

    private int minutes;

    public int Minutes
    {
        get { return minutes; }
        set { minutes = value; OnMinutesChange(value); }
    }

    private int hours = 7;

    public int Hours
    {
        get { return hours; }
        set { hours = value; OnHoursChange(value); }
    }

    private int days;

    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    private float tempSecond;

    private GameObject[] flowers;
    private GameObject[] roots;

    private Texture2D initialTexture1;
    private Texture2D initialTexture2;
    private float initialBlend;

    private void Start()
    {
        flowers = GameObject.FindGameObjectsWithTag("Flower");
        roots = GameObject.FindGameObjectsWithTag("Root");
        NightItemsSpawner(false);
        SetInitialLightRotation();

        // Store initial shader values
        initialTexture1 = (Texture2D)RenderSettings.skybox.GetTexture("_Texture1");
        initialTexture2 = (Texture2D)RenderSettings.skybox.GetTexture("_Texture2");
        initialBlend = RenderSettings.skybox.GetFloat("_Blend");
    }

    private void OnApplicationQuit()
    {
        // Restore initial shader values
        RenderSettings.skybox.SetTexture("_Texture1", initialTexture1);
        RenderSettings.skybox.SetTexture("_Texture2", initialTexture2);
        RenderSettings.skybox.SetFloat("_Blend", initialBlend);
    }

    private void Update()
    {
        tempSecond += Time.deltaTime;

        // Rotate the light based on time progression
        float rotationSpeed = 360f / (10f * 60f); // 360 degrees per game day (10 real-life minutes in a day)
        globalLight.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.World);

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }
    }

    private void SetInitialLightRotation()
    {
        // Set the initial light rotation based on the current hour and minute
        float initialRotation = ((hours * 60 + minutes) / 1440f) * 360f;
        globalLight.transform.rotation = Quaternion.Euler(initialRotation, 0, 0);
    }

    private void OnMinutesChange(int value)
    {
        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 10f));
            StartCoroutine(LerpLight(graddientNightToSunrise, 10f));
            NightItemsSpawner(false);
        }
        else if (value == 8)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 10f));
            StartCoroutine(LerpLight(graddientSunriseToDay, 10f));
        }
        else if (value == 18)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(graddientDayToSunset, 10f));
        }
        else if (value == 22)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(graddientSunsetToNight, 10f));
            NightItemsSpawner(true);
        }
    }

    private void NightItemsSpawner(bool spawn)
    {
        foreach (GameObject flower in flowers)
        {
            flower.SetActive(spawn);
        }
        foreach (GameObject root in roots)
        {
            root.SetActive(spawn);
        }
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
