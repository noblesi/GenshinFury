using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Slider hpSlider;
    public Slider mpSlider;
    public Text hpText;
    public Text mpText;

    private Player player;

    private void Start()
    {
        var players = FindObjectsOfType<Player>();
        player = players.FirstOrDefault(p => p is Archer || p is Warrior || p is Wizard);

        if (player == null)
        {
            Debug.LogError("Player object not found.");
            return;
        }

        UpdateHUD();
        player.OnHealthChanged += UpdateHP; // 이벤트 등록
        player.OnManaChanged += UpdateMP; // 이벤트 등록
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
