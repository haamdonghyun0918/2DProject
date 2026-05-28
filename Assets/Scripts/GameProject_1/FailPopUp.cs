using UnityEngine;

public class FailPopUp : UiBase
{
    [SerializeField] private UiButton button_Exit;

    public void OnEnable()
    {
        button_Exit.BindOnClickButtonEvent(OnClickPopUpExit);
    }
    public void OnClickPopUpExit()
    {
        UiManager.Instance.OpenGameMainScene();
        UiManager.Instance.CloseStageUi();
        UiManager.Instance.CloseFailPopUp();
    }
}