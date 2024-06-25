using System.Text.RegularExpressions;

public static class Utility
{
    public const int MinNicknameLength = 3;
    public const int MaxNicknameLength = 10;

    public static bool IsValidNickname(string nickname)
    {
        return !string.IsNullOrWhiteSpace(nickname) &&
            nickname.Length >= MinNicknameLength &&
            nickname.Length <= MaxNicknameLength &&
            Regex.IsMatch(nickname, "^[a-zA-Z0-9]+$");
    }
}
