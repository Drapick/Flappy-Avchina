using UnityEngine;

public enum PlayerState
{
    Menu,       // Меню: анимация парения, физика отключена
    GetReady,   // Ожидание старта: анимация ожидания, управление выключено
    Gameplay    // Полет: аниматор выключен, управление и физика активны
}

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }

    [Header("Компоненты")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private FLAvController controller;
    [SerializeField] private FlappyAvchinaRotation rotationScript;

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

        // Автоматически находим компоненты на самом объекте
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (col == null) col = GetComponent<Collider2D>();
        if (controller == null) controller = GetComponent<FLAvController>();
        if (rotationScript == null) rotationScript = GetComponent<FlappyAvchinaRotation>();
    }

    /// <summary>
    /// Переключает состояние игрока между меню, ожиданием и полетом.
    /// </summary>
    public void SetState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Menu:
                SetAnimationMode("ToMenu");
                break;

            case PlayerState.GetReady:
                SetAnimationMode("ToGetReady");
                break;

            case PlayerState.Gameplay:
                SetGameplayMode();
                break;
        }
    }

    // Режим анимаций (Меню / Ожидание)
    private void SetAnimationMode(string triggerName)
    {
        // 1. Делаем тело кинематическим, чтобы гравитация не роняла птицу во время анимации
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        // 2. Выключаем столкновения и управление
        if (col) col.enabled = false;
        if (controller) controller.enabled = false;
        if (rotationScript) rotationScript.enabled = false;

        // 3. Включаем аниматор и запускаем нужный клип
        if (animator)
        {
            animator.enabled = true;
            animator.SetTrigger(triggerName);
        }
    }

    // Режим реального геймплея
    private void SetGameplayMode()
    {
        // 1. Отключаем аниматор, чтобы он не мешал падению и прыжкам
        if (animator) animator.enabled = false;

        // 2. Включаем стандартную физику гравитации
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 3. Активируем столкновения и управление полетом
        if (col) col.enabled = true;
        if (controller) controller.enabled = true;
        if (rotationScript) rotationScript.enabled = true;
    }
}
