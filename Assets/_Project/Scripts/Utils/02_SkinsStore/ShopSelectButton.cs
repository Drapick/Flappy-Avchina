using UnityEngine;
using UnityEngine.UI;
using TMPro; // Уберите, если пока тестируете только через Debug.Log

[RequireComponent(typeof(Button))]
public class ShopSelectButton : MonoBehaviour
{
    [SerializeField] private ConveyorMover conveyor;
    [SerializeField] private TextMeshProUGUI statusText; // Опционально: текст на самой кнопке

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnEquipButtonClicked);
    }

    // Вызывается из ConveyorMover при старте и после каждого пролистывания
    public void RefreshState()
    {
        if (conveyor == null || SaveManager.Instance == null || SaveManager.Instance.Data == null) return;

        ConveyorSkinSlot activeSlot = conveyor.GetSelectedSlot();
        if (activeSlot == null || string.IsNullOrEmpty(activeSlot.CurrentSkinName)) return;

        string skinName = activeSlot.CurrentSkinName;
        bool isUnlocked = SaveManager.Instance.Data.unlockedSkins.Contains(skinName);

        if (isUnlocked)
        {
            Debug.Log($"[{skinName}] Можно выбрать");
            if (statusText != null) statusText.text = "";
        }
        else
        {
            int price = SkinData.GetPrice(skinName);
            Debug.Log($"[{skinName}] Цена: {price}");
            if (statusText != null) statusText.text = $"{price}";
        }
    }

    private void OnEquipButtonClicked()
    {
        if (conveyor == null || SaveManager.Instance == null || SaveManager.Instance.Data == null) return;

        ConveyorSkinSlot activeSlot = conveyor.GetSelectedSlot();
        if (activeSlot == null || string.IsNullOrEmpty(activeSlot.CurrentSkinName)) return;

        string skinName = activeSlot.CurrentSkinName;

        // 1. Если скин уже есть в списке купленных — надеваем
        if (SaveManager.Instance.Data.unlockedSkins.Contains(skinName))
        {
            PlayerSkinManager.Instance.EquipSkin(skinName, saveToFile: true);
        }
        // 2. Если скина нет в списке — пока логируем попытку покупки
        else
        {
            int price = SkinData.GetPrice(skinName);
            Debug.Log($"Попытка купить {skinName} за {price}");

            // Когда добавите валюту, покупка будет выглядеть так:
            // SaveManager.Instance.Data.unlockedSkins.Add(skinName);
            // SaveManager.Instance.SaveGame();
            // PlayerSkinManager.Instance.EquipSkin(skinName, saveToFile: true);
            // RefreshState();
        }
    }
}
