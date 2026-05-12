using UnityEngine;

public enum UiType
{
    MainUi,
    CharacterUi,
    GamePopup
}
public static partial class UiManagerExtension
{
    public static string GetUiPath(this UiManager uiManager, UiType uiType)
    {
        string path = string.Empty;
        switch (uiType)
        {
            case UiType.MainUi:
                path = "Prefabs/Ui/MainUi";
                break;
        }
        return path;
    }

    public static void OpenMainUi(UiManager uiManager)
    {
        var openUitype = UiType.MainUi;
        var gObj = uiManager.GetCreatedUi(openUitype);

        if(gObj != null)
        {
            uiManager.OpenUi(openUitype, gObj);
        }
    }
}