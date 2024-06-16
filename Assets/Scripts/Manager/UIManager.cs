using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI ����, ���Ӱ� UI���� ���� ���� Ŭ����
public class UIManager : MonoSingleton<UIManager>
{
    // ���α׷��� �����ϸ� GameStart Button UI
    // ������ ����Ǹ� ���� ���� UI
    // ������ HeadUp Display UI

    public Button GameStartButton = null;
    public Button BattleEndButton = null;
    public Slider PlayerHpBar = null;

    public void OnClickGameStartButton()
    {
        SystemManager.Instance.SetState(GameState.GAME);
    }

    public void OnClickBattleEndButton()
    {
        SystemManager.Instance.SetState(GameState.END);
    }

    public void SetPlayerHpBar(float curHp)
    {
        PlayerHpBar.value = curHp;
    }
}
