using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDecreaseRate = 10f; // 초당 감소량
    [SerializeField] private float staminaRecoveryRate = 5f; // 초당 회복량
    private float stamina;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    private float currentSpeed;

    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    [SerializeField] private int experienceToNextLevel = 100;

    private void Awake()
    {
        stamina = maxStamina;
        currentSpeed = runSpeed; // 기본 이동 속도는 Run
    }

    public float MaxStamina
    {
        get => maxStamina;
        set => maxStamina = value;
    }

    public float StaminaDecreaseRate
    {
        get => staminaDecreaseRate;
        set => staminaDecreaseRate = value;
    }

    public float StaminaRecoveryRate
    {
        get => staminaRecoveryRate;
        set => staminaRecoveryRate = value;
    }

    public float Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
            if (stamina < 0)
                stamina = 0;
            if (stamina > maxStamina)
                stamina = maxStamina;
        }
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public float SprintSpeed
    {
        get => sprintSpeed;
        set => sprintSpeed = value;
    }

    public float CurrentSpeed
    {
        get => currentSpeed;
        set => currentSpeed = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int Experience
    {
        get => experience;
        set
        {
            experience = value;
            if (experience >= experienceToNextLevel)
            {
                LevelUp();
            }
        }
    }

    public int ExperienceToNextLevel
    {
        get => experienceToNextLevel;
        set => experienceToNextLevel = value;
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.FloorToInt(experienceToNextLevel * 1.1f);
        // 레벨업 시 추가 효과를 여기에 추가할 수 있습니다.
    }

    public void ApplySpeedBuff(float multiplier, float duration)
    {
        currentSpeed *= multiplier;
        StartCoroutine(RemoveSpeedBuffAfterDelay(multiplier, duration));
    }

    private IEnumerator RemoveSpeedBuffAfterDelay(float multiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        currentSpeed /= multiplier;
    }

    public void SaveData(string filePath)
    {
        GameData data = new GameData
        {
            stamina = stamina,
            currentSpeed = currentSpeed,
            positionX = transform.position.x,
            positionY = transform.position.y,
            positionZ = transform.position.z,
            rotationX = transform.rotation.x,
            rotationY = transform.rotation.y,
            rotationZ = transform.rotation.z,
            rotationW = transform.rotation.w,
            level = level,
            experience = experience
        };

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }

    public void LoadData(string filePath)
    {
        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                GameData data = (GameData)serializer.Deserialize(stream);

                stamina = data.stamina;
                currentSpeed = data.currentSpeed;
                transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
                transform.rotation = new Quaternion(data.rotationX, data.rotationY, data.rotationZ, data.rotationW);
                level = data.level;
                experience = data.experience;
            }
        }
    }
}
