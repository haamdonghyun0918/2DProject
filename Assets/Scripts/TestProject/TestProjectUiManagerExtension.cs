using UnityEngine;

public enum TestUiRootType
{
    None = 0,
    StartUi,
    GameUi
}
public enum TestUiType
{
    TestLoadingUi,
    TestStartUi,
    GameSceneUi,
    SuccessPopUp,
    FailedPopUp

}
public static partial class TestProjectUiManagerExtension
{
    public static string GetTestUiPath(this TestProjectUiManager testProjectuiManager, TestUiRootType testUiRootType, TestUiType testUiType)
    {
        string path = string.Empty;
        path = $"Prefabs/TestProject/{testUiRootType}/{testUiType}";
        return path;
    }
    public static void StartTestLoadingUiOnGameStart(this TestProjectUiManager testProjectuiManager)
    {
        testProjectuiManager.OpenTestLoadingUi();
    }
    public static void OpenTestLoadingUi(this TestProjectUiManager testProjectUiManager)
    {
        var uiBase = testProjectUiManager.TestOpenUi(TestUiRootType.StartUi, TestUiType.TestLoadingUi);
        if (uiBase == null)
        {
            Debug.LogWarning("Ui가 생성되지 않았습니다");
            return;
        }
    }
    public static void OpenTestStartUi(this TestProjectUiManager testProjectUiManager)
    {
        var uiBase = testProjectUiManager.TestOpenUi(TestUiRootType.StartUi, TestUiType.TestStartUi);
        if (uiBase == null)
        {
            Debug.LogWarning("Ui가 생성되지 않았습니다");
            return;
        }
    }
    public static void OpenGameSceneUi(this TestProjectUiManager testProjectUiManager)
    {
        var uiBase = testProjectUiManager.TestOpenUi(TestUiRootType.GameUi, TestUiType.GameSceneUi);
        if (uiBase == null)
        {
            Debug.LogWarning("Ui가 생성되지 않았습니다");
        }
    }
    public static void OpenSuccessPopup(this TestProjectUiManager testProjectUiManager)
    {
        testProjectUiManager.TestOpenUi(TestUiRootType.GameUi, TestUiType.SuccessPopUp);
    }
    public static void OpenFailedPopUp(this TestProjectUiManager testProjectUiManager)
    {
        testProjectUiManager.TestOpenUi(TestUiRootType.GameUi, TestUiType.FailedPopUp);
    }
    public static void CloseStartUi(this TestProjectUiManager testProjectUiManager)
    {
        testProjectUiManager.CloseTestUi(TestUiRootType.StartUi, TestUiType.TestStartUi);
    }
    public static void CloseLoadingUi(this TestProjectUiManager testProjectuiManager)
    {
        testProjectuiManager.CloseTestUi(TestUiRootType.StartUi, TestUiType.TestLoadingUi);
    }
    public static void CloseAllTestUis(this TestProjectUiManager uiManager)
    {
        uiManager.CloseTestUi(TestUiRootType.StartUi, TestUiType.TestLoadingUi);
        uiManager.CloseTestUi(TestUiRootType.StartUi, TestUiType.TestStartUi);
        uiManager.CloseTestUi(TestUiRootType.GameUi, TestUiType.GameSceneUi);
        uiManager.CloseTestUi(TestUiRootType.GameUi, TestUiType.SuccessPopUp);
        uiManager.CloseTestUi(TestUiRootType.GameUi, TestUiType.FailedPopUp);
    }
}