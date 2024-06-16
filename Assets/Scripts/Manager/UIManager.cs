using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 관리, 게임과 UI간의 연동 관리 클래스
public class UIManager : MonoSingleton<UIManager>
{
    // 프로그램이 시작하면 GameStart Button UI
    // 전투가 종료되면 전투 종료 UI
    // 전투중 HeadUp Display UI

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
