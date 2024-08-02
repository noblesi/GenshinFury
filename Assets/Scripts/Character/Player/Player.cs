using UnityEngine;

public class Player : BaseCharacter
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerData playerData;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider weaponCollider;
    public float attackCooldown = 1f;
    private float lastAttackTime;
    private bool isAttacking = false;
    private bool hasDealtDamage = false;

    public PlayerData PlayerData => playerData;

    private int experience;
    private int level = 1;
    private int experienceToNextLevel = 100;

    public event System.Action<int> OnExperienceChanged;
    public event System.Action<int> OnLevelChanged;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    protected override void Start()
    {
        base.Start();
        EnsureComponents();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"{gameObject.name}에 Animator 컴포넌트가 없습니다.");
            }
        }
    }

    public void Initialize(PlayerData data)
    {
        this.playerData = data;
        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;
        maxMana = data.maxMana;
        currentMana = data.currentMana;
        experience = 0;
        level = data.level;
        experienceToNextLevel = CalculateExperienceToNextLevel(level);
    }

    private void EnsureComponents()
    {
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    private void Update()
    {
        HandleInventoryInput();
        CheckAttack();
    }

    protected void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ToggleUI(UIType.Inventory);
        }
    }

    private void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            Debug.Log("Player attacks.");
            PlayAnimation(animator, "Attack");
            lastAttackTime = Time.time;
            StartAttack();
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        hasDealtDamage = false;
    }

    public void EndAttack()
    {
        isAttacking = false;
        DeactivateWeaponCollider();
    }

    public void ActivateWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
        }
    }

    public void DeactivateWeaponCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public int CalculateDamage()
    {
        return (int)(playerData.strength * 2) +
               (int)(playerData.dexterity * 1.5f) +
               (int)(playerData.intelligence * 1.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (other.CompareTag("Enemy") && isAttacking && !hasDealtDamage)
        {
            int damage = CalculateDamage();
            other.GetComponent<IDamageable>().TakeDamage(damage);
            hasDealtDamage = true;
        }
    }

    public void Heal(int amount)
    {
        ChangeHealth(Mathf.Min(maxHealth, currentHealth + amount));
        Debug.Log($"{gameObject.name} healed for {amount}. Current health: {currentHealth}");
    }

    public void RestoreMana(int amount)
    {
        ChangeMana(Mathf.Min(maxMana, currentMana + amount));
        Debug.Log($"{gameObject.name} restored {amount} mana. Current mana: {currentMana}");
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        OnExperienceChanged?.Invoke(experience);

        while (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        experience -= experienceToNextLevel;
        level++;
        experienceToNextLevel = CalculateExperienceToNextLevel(level);

        playerData.strength += 5;
        playerData.dexterity += 5;
        playerData.intelligence += 5;

        playerData.maxHealth = CalculateMaxHealth(playerData);
        playerData.maxMana = CalculateMaxMana(playerData);

        OnLevelChanged?.Invoke(level);
        Debug.Log($"Leveled up to level {level}!");
    }

    private int CalculateMaxHealth(PlayerData playerData)
    {
        return 100 + (playerData.strength * 2) + (int)(playerData.dexterity * 1.5f) + (level * 10);
    }

    private int CalculateMaxMana(PlayerData playerData)
    {
        return 50 + (playerData.intelligence * 2) + (level * 5);
    }

    private int CalculateExperienceToNextLevel(int currentLevel)
    {
        return Mathf.FloorToInt(100 * Mathf.Pow(1.5f, currentLevel - 1));
    }

    protected override void Die()
    {
        base.Die();
        PlayAnimation(animator, "Die");
    }
}
