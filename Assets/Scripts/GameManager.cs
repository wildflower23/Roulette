using UnityEngine;

public class GameManager
{
    public enum GameActivity { Home, Playing, Spinnning, Pause, Setting, calculation }
    public static GameActivity gameActivity = GameActivity.Home;

    public static void SetGameActivity(GameActivity setActivity)
    {
        gameActivity = setActivity;
    }
}
