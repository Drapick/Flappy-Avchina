using UnityEngine;

public class PlayerStateBridge : MonoBehaviour
{
    [Header("Инициализация при старте")]
    [Tooltip("В какой режим перевести игрока сразу при загрузке этой сцены")]
    [SerializeField] private PlayerState startState = PlayerState.Menu;

    private void Start()
    {
        // Применяем стартовый стейт при запуске сцены
        ChangeState(startState);
    }

    /// <summary>
    /// Прямой метод смены стейта через enum (для вызова из кода).
    /// </summary>
    public void ChangeState(PlayerState newState)
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetState(newState);
        }
        else
        {
            Debug.LogWarning($"[PlayerStateBridge] PlayerStateManager.Instance не найден! Не удалось переключить на {newState}.");
        }
    }

    // --- Удобные методы для привязки к кнопкам UI (OnClick) в Инспекторе ---

    public void TriggerMenuState()
    {
        ChangeState(PlayerState.Menu);
    }

    public void TriggerGetReadyState()
    {
        ChangeState(PlayerState.GetReady);
    }

    public void TriggerGameplayState()
    {
        ChangeState(PlayerState.Gameplay);
    }
}