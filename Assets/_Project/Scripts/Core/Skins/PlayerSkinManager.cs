using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer targetRenderer;
    [SerializeField] private List<Sprite> skins = new List<Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (targetRenderer == null) targetRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Загружаем сохраненный скин при спавне игрока
        if (SaveManager.Instance != null && SaveManager.Instance.Data != null)
        {
            ChangeSkin(SaveManager.Instance.Data.currentSkin, saveToFile: false);
        }
    }

    public void ChangeSkin(string skinName, bool saveToFile = true)
    {
        Sprite newSkin = skins.Find(s => s != null && s.name == skinName);

        if (newSkin != null)
        {
            targetRenderer.sprite = newSkin;

            if (saveToFile && SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.currentSkin = skinName;
                SaveManager.Instance.SaveGame();
            }
        }
        else
        {
            Debug.LogWarning($"[PlayerSkinManager] Скин '{skinName}' не найден!");
        }
    }
}