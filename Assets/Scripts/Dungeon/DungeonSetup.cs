using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy, Normal, Hard
}

public class DungeonSetup : MonoBehaviour
{
    public void SetupDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                SetupEasy();
                break;
            case Difficulty.Normal:
                SetupNormal();
                break;
            case Difficulty.Hard:
                SetupHard();
                break;
        }
    }

    private void SetupEasy()
    {

    }

    private void SetupNormal()
    {

    }

    private void SetupHard()
    {

    }
}
