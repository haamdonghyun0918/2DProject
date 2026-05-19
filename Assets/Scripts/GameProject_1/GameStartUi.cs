using UnityEngine;

public class GameStartUi : UiBase
{
    [SerializeField] private UiButton button_Vance;
    [SerializeField] private UiButton button_Colt;
    [SerializeField] private UiButton button_Kaelen;
    [SerializeField] private UiButton button_BackMain;

    private void OnEnable()
    {
        button_Vance.BindOnClickButtonEvent(OnStartVanceInGame);
        button_Colt.BindOnClickButtonEvent(OnStartColtInGame);
        button_Kaelen.BindOnClickButtonEvent(OnStartKaelenInGame);
        button_BackMain.BindOnClickButtonEvent(OnBackMain);
    }

    public void OnStartVanceInGame()
    {

    }
    public void OnStartColtInGame()
    {

    }
    public void OnStartKaelenInGame()
    {

    }
    public void OnBackMain()
    {
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseGameStartUi();
    }
}