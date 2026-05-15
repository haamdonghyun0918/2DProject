using UnityEngine;

public class SuccessPopUp : UiBase
{
    [SerializeField] private UiButton button_retry;
    [SerializeField] private UiButton button_exit;

    private void Start()
    {
        button_retry.BindOnClickButtonEvent(OnRetryButtonClick);
        button_exit.BindOnClickButtonEvent(OnExitButtonClick);
    }
    private void OnRetryButtonClick()
    {
        TestProjectUiManager.Instance.CloseAllTestUis();
        TestProjectUiManager.Instance.OpenTestLoadingUi();
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}