using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestProjectUiManager : MonoBehaviour
{
    [SerializeField] Canvas TestUiRoot;
    public static TestProjectUiManager Instance { get; set; }
    //생성, 제거 딕셔너리
    private Dictionary<TestUiType, UiBase> _createdUiDic = new Dictionary<TestUiType, UiBase>();
    //활성, 비활성 -> SetActive
    private HashSet<TestUiType> _openTestUiDic = new HashSet<TestUiType>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        this.StartTestLoadingUiOnGameStart();
    }
    public UiBase TestOpenUi(TestUiRootType testUiRootType, TestUiType testUiType, bool isInitialHide = false)
    {
        var openedTestUi = GetTestCreatedUi(testUiRootType, testUiType);
        bool isSetActiveOnOpen = (isInitialHide == false);

        if (_openTestUiDic.Contains(testUiType) == false)
        {
            openedTestUi.gameObject.SetActive(isSetActiveOnOpen);
            _openTestUiDic.Add(testUiType);
        }

        return openedTestUi;
    }

    public void CloseTestUi(TestUiRootType testUiRootType, TestUiType testUiType)
    {
        if (_openTestUiDic.Contains(testUiType))
        {
            var openedTestUi = _createdUiDic[testUiType];
            openedTestUi.gameObject.SetActive(false);
            _openTestUiDic.Remove(testUiType);
        }
    }

    private Transform GetTestRootTransform(TestUiRootType testUiRootType)
    {
        Transform testroot = null;
        switch (testUiRootType)
        {
            case TestUiRootType.StartUi:
                testroot = TestUiRoot.transform;
                break;
            case TestUiRootType.GameUi:
                testroot = TestUiRoot.transform;
                break;
        }
        return testroot;
    }

    private void CreateTestUi(TestUiRootType testUiRootType, TestUiType testUiType)
    {
        if (_createdUiDic.ContainsKey(testUiType) == false)
        {
            string path = this.GetTestUiPath(testUiRootType, testUiType);
            GameObject loadedTestObj = (GameObject)Resources.Load(path);
            Transform testroot = GetTestRootTransform(testUiRootType);
            GameObject gObj = Instantiate(loadedTestObj, testroot);
            if (gObj != null)
            {
                var uiBase = gObj.GetComponent<UiBase>();
                _createdUiDic.Add(testUiType, uiBase);
            }
        }
    }

    private UiBase GetTestCreatedUi(TestUiRootType testUiRootType, TestUiType testUiType)
    {
        if (_createdUiDic.ContainsKey(testUiType) == false)
        {
            CreateTestUi(testUiRootType, testUiType);
        }
        return _createdUiDic[testUiType];
    }

}