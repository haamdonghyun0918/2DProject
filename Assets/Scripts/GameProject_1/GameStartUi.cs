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
        UiManager.Instance.SelectedCharacterId = "character_punch_01";
        UiManager.Instance.OpenBeforeGameStartUi();
    }
    public void OnStartColtInGame()
    {
        UiManager.Instance.SelectedCharacterId = "character_gun_01";
        UiManager.Instance.OpenBeforeGameStartUi();
    }
    public void OnStartKaelenInGame()
    {
        UiManager.Instance.SelectedCharacterId = "character_knife_01";
        UiManager.Instance.OpenBeforeGameStartUi();
    }
    public void OnBackMain()
    {
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseBeforeGameStartUi();
        UiManager.Instance.CloseGameStartUi();
    }
}