using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class DebugLightController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    private Button _button;
    private bool _lightsEnabled = true;

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        _button.onClick.AddListener(ToggleAllLights);
        UpdateButtonVisuals();
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(ToggleAllLights);
        }
    }

    public void ToggleAllLights()
    {
        _lightsEnabled = !_lightsEnabled;

        foreach (var registry in LightSourceRegistry.AllRegistries)
        {
            if (registry != null)
            {
                registry.SetLightState(_lightsEnabled);
            }
        }

        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        if (buttonText != null)
        {
            buttonText.text = _lightsEnabled ? "Lights: ON" : "Lights: OFF";
        }
    }
}