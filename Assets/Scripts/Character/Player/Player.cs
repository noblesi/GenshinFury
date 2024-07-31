using UnityEngine;

public class Player : BaseCharacter
{
    public static Player Instance { get; private set; }

    [SerializeField] private PlayerData playerData;
    [SerializeField] private Animator animator;
    public float attackCooldown = 1f;
    private float lastAttackTime;

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

    private void EnsureComponents()
    {
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    public virtual void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        maxHealth = playerData.maxHealth;
        currentHealth = playerData.maxHealth;
        maxMana = playerData.maxMana;
        currentMana = playerData.maxMana;
    }

    private void Update()
    {
        HandleInventoryInput();
    }

    protected void HandleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ToggleUI(UIType.Inventory);
        }
    }

    public int CalculateDamage()
    {
        return (int)(playerData.strength * 2) +
               (int)(playerData.dexterity * 1.5f) +
               (int)(playerData.intelligence * 1.2f);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"{gameObject.name} healed for {amount}. Current health: {currentHealth}");
    }

    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
        Debug.Log($"{gameObject.name} restored {amount} mana. Current mana: {currentMana}");
    }

    public void PickUpItem(Item item, int amount)
    {
        bool added = Inventory.Instance.AddItem(item, amount);
        if (added)
        {
            Debug.Log($"Picked up {amount} of {item.itemName}");
        }
        else
        {
            Debug.Log("Inventory is full");
        }
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
        experienceToNextLevel = Mathf.FloorToInt(experienceToNextLevel * 1.5f);
        playerData.strength += 5;
        playerData.dexterity += 5;
        playerData.intelligence += 5;
        OnLevelChanged?.Invoke(level);
        Debug.Log($"Leveled up to level {level}!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(other.GetComponent<Enemy>().attackDamage);
        }
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
    }
}
