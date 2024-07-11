using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider hpSlider;
    public Slider mpSlider;
    public Text hpText;
    public Text mpText;

    private Player player;

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerCreated += OnPlayerCreated;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerCreated -= OnPlayerCreated;
    }

    private void OnPlayerCreated(Player createdPlayer)
    {
        player = createdPlayer;

        if (player == null)
        {
            Debug.LogError("Player object not found.");
            return;
        }

        player.OnHealthChanged += UpdateHP; // 이벤트 등록
        player.OnManaChanged += UpdateMP; // 이벤트 등록
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        UpdateHP();
        UpdateMP();
    }

    private void UpdateHP()
    {
        if (player != null)
        {
            hpSlider.value = player.currentHealth / (float)player.maxHealth;
            hpText.text = $"{player.currentHealth} / {player.maxHealth}";
        }
    }

    private void UpdateMP()
    {
        if (player != null)
        {
            mpSlider.value = player.currentMana / (float)player.maxMana;
            mpText.text = $"{player.currentMana} / {player.maxMana}";
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= UpdateHP; // 이벤트 해제
            player.OnManaChanged -= UpdateMP; // 이벤트 해제
        }
    }
}
