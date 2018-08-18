using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFlux : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI Text;

    // Use this for initialization
    void Start () {
        SetNextFlux();
    }

    float timeToFlux;
    float fluxValue;

    private void SetNextFlux()
    {
        timeToFlux = UnityEngine.Random.Range(0.1f, 0.3f);
        fluxValue = UnityEngine.Random.Range(0.15f, 0.65f);
    }

    void Update () {

        timeToFlux -= Time.deltaTime;

        if (timeToFlux <= 0)
        {
            Text.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, fluxValue);
            SetNextFlux();
        }
    }
}
