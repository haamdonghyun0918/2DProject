using UnityEngine;

public class TestStartUi : UiBase
{
    [SerializeField] private UiButton StartButton;
    [SerializeField] private UiButton ExitButton;

    private void OnEnable()
    {
        if(StartButton != null)
        {
            StartButton.BindOnClickButtonEvent(OnStartButtonClick);
        }
        if(ExitButton != null)
        {
            ExitButton.BindOnClickButtonEvent(OnExitButtonClick);
        }
    }
    private void OnStartButtonClick()
    {
        TestProjectUiManager.Instance.OpenGameSceneUi();
        TestProjectUiManager.Instance.CloseStartUi();
    }
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
