using UnityEngine;

public enum UiRootType
{
    None = 0,
    BaseUi,
    CharacterUi
}
public enum UiType
{
    LoadingUi,
    MainUi,
    CharacterUi,
    CharacterInfoPopUp
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
    public static void OpenCharacterInfoPopUp(this UiManager uiManager)
    {
        uiManager.OpenUi(UiRootType.CharacterUi, UiType.CharacterInfoPopUp);
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
    public static void CloseCharacterInfoPopUp(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.CharacterUi, UiType.CharacterInfoPopUp);
    }
}