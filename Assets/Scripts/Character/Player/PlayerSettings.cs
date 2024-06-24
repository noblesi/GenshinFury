public class PlayerSettings
{
    private static PlayerSettings _instance;
    public static PlayerSettings Instance => _instance ?? (_instance = new PlayerSettings());

    public string Nickname { get; private set; }
    public PlayerClass PlayerClass { get; private set; }

    private PlayerSettings() { }

    public void SetNickname(string nickname)
    {
        Nickname = nickname;
    }

    public void SetPlayerClass(PlayerClass playerClass)
    {
        PlayerClass = playerClass;
    }
}
