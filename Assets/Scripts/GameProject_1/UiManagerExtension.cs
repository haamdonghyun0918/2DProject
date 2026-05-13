using UnityEngine;

public enum UiRootType
{
    None = 0,
    BackGroundUi,
    BaseUi,
    CharacterBaseUi,
    CharacterInfoBaseUi
}
public enum UiType
{
    LoadingBackGroundUi,
    MainBackGroundUi,
    CharacterBackGroundUi,
    LoadingUi,
    MainUi,
    CharacterUi,
    CharacterInfoUi
}
public static partial class UiManagerExtension
{
    public static string GetUiPath(this UiManager uiManager, UiRootType uiRootType, UiType uiType)
    {
        string path = string.Empty;
        path = $"Prefabs/Ui/{uiRootType}/{uiType}";
        return path;
    }
    public static void ShowStartupUIOnGameStart(this UiManager uiManager)
    {
        uiManager.OpenLoadingUi();
        uiManager.OpenLoadingBackGroundUi();
    }
    public static void OpenLoadingUi(this UiManager uiManager)
    {
        var uiBase = uiManager.OpenUi(UiRootType.BaseUi, UiType.LoadingUi);
        if(uiBase == null)
        {
            Debug.LogWarning("Ui가 생성되지 않았습니다");
            return;
        }
    }
    public static void OpenLoadingBackGroundUi(this UiManager uiManager)
    {
        var uiBase = uiManager.OpenUi(UiRootType.BackGroundUi, UiType.LoadingBackGroundUi);
        if (uiBase == null)
        {
            Debug.LogWarning("Ui가 생성되지 않았습니다");
            return;
        }
    }
    public static void CloseLoadingUi(this UiManager uiManager)
    {
        uiManager.CloseUi(UiRootType.BaseUi, UiType.MainUi);
    }
}