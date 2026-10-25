using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkinChangeButton : MonoBehaviour
{
    [Header("Имя спрайта для смены")]
    [SerializeField] private string targetSkinName = "02_shlapa_skin";

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickSkinButton);
    }

    private void OnClickSkinButton()
    {
        if (PlayerSkinManager.Instance != null)
        {
            PlayerSkinManager.Instance.ChangeSkin(targetSkinName);
        }
        else
        {
            Debug.LogWarning("[SkinChangeButton] PlayerSkinManager.Instance не найден на сцене!");
        }
    }
}