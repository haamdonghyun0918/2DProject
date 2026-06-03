using Unity.VisualScripting;
using UnityEngine;

// Ui의 갯수가 많아지므로 폴더를 나눠서 배분 (오타 문제 해결)
public enum UiRootType
{
    None = 0,
    BaseUi,
    CharacterUi,
    GameUi
}
// 전체 Ui들을 폴더안에 넣어서 그 루트폴더 안에 있는 Ui들을 호출하게끔 enum 생성 (오타 문제 해결)
public enum UiType
{
    LoadingUi,
    MainUi,
    CharacterUi,
    CharacterInfoPopUp,
    GameStartUi,
    BeforeGameStartUi,
    GameMainScene,
    GetCardInventory,
    GameStageUi,
    ClearPopUp,
    FailPopUp,
    CardRewardStage,
    CardDictionaryUi,
    FinalClearUi
}

// UiManager 클래스의 코드를 직접 수정하지 않고 그 안에 있는 함수처럼 새로운 기능을 덧붙여주는 C#문법(partial)
public static partial class UiManagerExtension
{
    // GetUiPath 를 호출하면 문자열은 "Prefabs/Ui/GameUi/GameStartUi" 로 완성됩니다.
    public static string GetUiPath(this UiManager uiManager, UiRootType uiRootType, UiType uiType)
    {
        string path = string.Empty;
        path = $"Prefabs/Ui/{uiRootType}/{uiType}";
        return path;
    }
    public static void GameStart(this UiManager uiManager)
    {
        uiManager.OpenLoadingUi();
    }
    public static void OpenLoadingUi(this UiManager uiManager)
    {
        var uiBase = uiManager.OpenUi(UiRootType.BaseUi, UiType.LoadingUi);
        if (uiBase == null)
        {
            Debug.LogWarning("LoadingUi가 생성되지 않았습니다");
            return;
        }
    }

    // Open 메서드를 통하여 하나의 메서드를 만들어서 UiManager.Instance.Openxx로 바로 열 수 있게 만듦
    public static void OpenMainUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.BaseUi, UiType.MainUi);
    }
    public static void OpenCharacterUi(this UiManager uiManager)
    {
       uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterUi);
    }
    public static void OpenCharacterInfoPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterInfoPopUp);
    }
    public static void OpenGameStartUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.GameStartUi);
    }
    public static void OpenBeforeGameStartUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.BeforeGameStartUi);
    }
    public static void OpenGameMainScene(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.GameMainScene);
    }
    public static void OpenInventory(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.GetCardInventory);
    }
    public static void OpenStageUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.GameStageUi);
    }
    public static void OpenClearPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.ClearPopUp);
    }
    public static void OpenFailPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.FailPopUp);
    }
    public static void OpenCardRewardStage(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.CardRewardStage);
    }
    public static void OpenCardDictionaryUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CardDictionaryUi);
    }
    public static void OpenFinalClearUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.GameUi, UiType.FinalClearUi);
    }

    // Close 메서드를 통하여 하나의 메서드를 만들어서 UiManager.Instance.Closexx로 바로 닫을 수 있게 만듦
    public static void CloseLoadingUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.BaseUi, UiType.LoadingUi);
    }
    public static void CloseMainUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.BaseUi, UiType.MainUi);
    }
    public static void CloseCharacterUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterUi);
    }
    public static void CloseCharacterInfoPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterInfoPopUp);
    }
    public static void CloseGameStartUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.GameStartUi);
    }
    public static void CloseBeforeGameStartUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.BeforeGameStartUi);
    }
    public static void CloseGameMainScene(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.GameMainScene);
    }
    public static void CloseInventory(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.GetCardInventory);
    }
    public static void CloseStageUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.GameStageUi);
    }
    public static void CloseClearPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.ClearPopUp);
    }
    public static void CloseFailPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.FailPopUp);
    }
    public static void CloseCardRewardStage(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.CardRewardStage);
    }
    public static void CloseCardDictionaryUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CardDictionaryUi);
    }
    public static void CloseFinalClearUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.GameUi, UiType.FinalClearUi);
    }
}