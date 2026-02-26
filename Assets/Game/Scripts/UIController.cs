using UnityEngine;

public sealed class UIController : MonoBehaviour
{
    [SerializeField] private ScreenManager screens;
    [SerializeField] private GameController game;

    public void OnStart()
    {
        screens.OpenPage(ScreenId.GameplayHud);
        game.StartRun();
    }

    public void OnOpenRecords() => screens.OpenPage(ScreenId.Records);
    public void OnOpenPrivacy() => screens.OpenPage(ScreenId.PrivacyPolicy);

    public void OnBackToMenu()
    {
        game.StopRun();
        screens.OpenPage(ScreenId.MainMenu);
    }

    public void OnPause()
    {
        if (!screens.IsPageOpen(ScreenId.GameplayHud)) return;

        game.SetPaused(true);
        screens.OpenOverlay(ScreenId.Pause);
    }

    public void OnResume()
    {
        screens.CloseOverlay(ScreenId.Pause);
        game.SetPaused(false);
    }

    public void OnExitFromPause()
    {
        screens.CloseOverlay(ScreenId.Pause);
        OnBackToMenu();
    }

    public void OnGameOverShown()
    {
        screens.OpenOverlay(ScreenId.GameOver);
    }

    public void OnPlayAgain()
    {
        screens.CloseOverlay(ScreenId.GameOver);
        screens.OpenPage(ScreenId.GameplayHud);
        game.StartRun();
    }
}