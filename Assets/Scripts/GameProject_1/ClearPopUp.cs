using UnityEngine;

public class ClearPopUp : UiBase
{
    [SerializeField] private UiButton button_Exit;
    [SerializeField] private UiButton button_Card;
    [SerializeField] private UiButton button_Rest;
    public void OnEnable()
    {
        button_Exit.BindOnClickButtonEvent(OnClickPopUpExit);
        button_Card.BindOnClickButtonEvent(OnClickReward);
        button_Rest.BindOnClickButtonEvent(OnClickHeal);
    }
    public void OnClickPopUpExit()
    {
        UiManager.Instance.OpenGameMainScene();
        UiManager.Instance.CloseStageUi();
        UiManager.Instance.CloseClearPopUp();
    }
    public void OnClickReward()
    {

    }
    public void OnClickHeal()
    {

    }
}