using UnityEditor.PackageManager;
using UnityEngine;

public enum UiRootType
{
    None = 0,
    BaseUi,
    CharacterUi,
    GameUi
}
public enum UiType
{
    LoadingUi,
    MainUi,
    CharacterUi,
    CharacterPunchInfoPopUp,
    CharacterGunInfoPopUp,
    CharacterKnifeInfoPopUp,
    GameStartUi,
    BeforeGameStartUi,
    GameMainScene,
    GetCardInventory,
    Scene_Stage
}
public static partial class UiManagerExtension
{
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
    public static void OpenMainUi(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.BaseUi, UiType.MainUi);
    }
    public static void OpenCharacterUi(this UiManager uiManager)
    {
       uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterUi);
    }
    public static void OpenCharacterPunchInfoPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterPunchInfoPopUp);
    }
    public static void OpenCharacterGunInfoPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterGunInfoPopUp);
    }
    public static void OpenCharacterKnifeInfoPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterKnifeInfoPopUp);
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
        uiManager.OpenUi(UiRootType.GameUi, UiType.Scene_Stage);
    }
    
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
    public static void CloseCharacterPunchInfoPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterPunchInfoPopUp);
    }
    public static void CloseCharacterGunInfoPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterGunInfoPopUp);
    }
    public static void CloseCharacterKnifeInfoPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterKnifeInfoPopUp);
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
        uiManager.CloseUi(UiRootType.GameUi, UiType.Scene_Stage);
    }
}