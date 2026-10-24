using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSourceRegistry : MonoBehaviour
{
    public static readonly HashSet<LightSourceRegistry> AllRegistries = new();

    // Кэшируем компоненты света на объекте и во всех его дочерних элементах
    private Light2D[] _lights;

    private void Awake()
    {
        AllRegistries.Add(this);
        // true — находить компоненты, даже если они изначально выключены
        _lights = GetComponentsInChildren<Light2D>(true);
    }

    private void OnDestroy()
    {
        AllRegistries.Remove(this);
    }

    public void SetLightState(bool state)
    {
        if (_lights == null) return;

        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
            {
                _lights[i].enabled = state;
            }
        }
    }
}