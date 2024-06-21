using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string playerName;
    private int playerLevel;
    private DateTime savedTime;
    private PlayerClass playerClass;

    public void Initialize(GameData gameData)
    {
        this.playerName = gameData.name;
        this.playerLevel = gameData.level;
        this.savedTime = gameData.savedTime;
        this.playerClass = gameData.playerClass;

        // Initialize player-specific settings here
        Debug.Log($"Player {playerName} initialized with level {playerLevel} as {playerClass} at {savedTime}");
    }
}
