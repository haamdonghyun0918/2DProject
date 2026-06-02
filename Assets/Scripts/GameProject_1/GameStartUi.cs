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
        //SelectedCharacterId에 저장하여 게임에 진입하였을 때, GameDataManager에서 해당 캐릭터의 카드와 능력치를 가져올 수 있음
        // 이 값들은 게임 들어갈 때, BeforeGameStartUi에 작성되어 있음
        UiManager.Instance.SelectedCharacterId = "character_punch_01";
        UiManager.Instance.OpenBeforeGameStartUi();
    }
    public void OnStartColtInGame()
    {
        //SelectedCharacterId에 저장하여 게임에 진입하였을 때, GameDataManager에서 해당 캐릭터의 카드와 능력치를 가져올 수 있음
        // 이 값들은 게임 들어갈 때, BeforeGameStartUi에 작성되어 있음
        UiManager.Instance.SelectedCharacterId = "character_gun_01";
        UiManager.Instance.OpenBeforeGameStartUi();
    }
    public void OnStartKaelenInGame()
    {
        //SelectedCharacterId에 저장하여 게임에 진입하였을 때, GameDataManager에서 해당 캐릭터의 카드와 능력치를 가져올 수 있음
        // 이 값들은 게임 들어갈 때, BeforeGameStartUi에 작성되어 있음
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