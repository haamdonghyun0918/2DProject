using UnityEngine;

public class MainUi : UiBase
{
    [SerializeField] private UiButton StartButton;
    [SerializeField] private UiButton CharacterButton;
    [SerializeField] private UiButton ExitButton;

    private void OnEnable()
    {
        if (StartButton != null)
        {
            StartButton.BindOnClickButtonEvent(OnStartButtonClick);
        }
        if (CharacterButton != null)
        {
            CharacterButton.BindOnClickButtonEvent(OnCharacterButtonClick);
        }
        if (ExitButton != null)
        {
            ExitButton.BindOnClickButtonEvent(OnExitButtonClick);
        }
    }
    private void OnStartButtonClick()
    {
        UiManager.Instance.OpenGameStartUi();
        UiManager.Instance.CloseMainUi();
    }
    private void OnCharacterButtonClick()
    {
        UiManager.Instance.OpenCharacterUi();
        UiManager.Instance.CloseMainUi();
    }
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}