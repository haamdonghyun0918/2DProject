using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] Canvas Canvas_GameCanvas;

    public static UiManager Instance { get; set; }
    //생성, 제거 딕셔너리
    private Dictionary<UiType, UiBase> _createdUiDic = new Dictionary<UiType, UiBase>();
    //활성, 비활성 -> SetActive
    private HashSet<UiType> _openUiDic = new HashSet<UiType>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        this.GameStart();
    }
    public UiBase OpenUi(UiRootType uiRootType, UiType uiType, bool isInitialHide = false)
    {
        var openedUi = GetCreatedUi(uiRootType, uiType);
        bool isSetActiveOnOpen = (isInitialHide == false);

        if(_openUiDic.Contains(uiType) == false)
        {
            openedUi.gameObject.SetActive(isSetActiveOnOpen);
            _openUiDic.Add(uiType);
        }

        return openedUi;
    }

    public void CloseUi(UiRootType uiRootType, UiType uiType)
    {
        if(_openUiDic.Contains(uiType))
        {
            var openedUi = _createdUiDic[uiType];
            openedUi.gameObject.SetActive(false);
            _openUiDic.Remove(uiType);
        }
    }

    private Transform GetRootTransform(UiRootType uiRootType)
    {
        Transform root = null;
        switch (uiRootType)
        {
            case UiRootType.BaseUi:
                root = Canvas_GameCanvas.transform;
                break;
            case UiRootType.CharacterUi:
                root = Canvas_GameCanvas.transform;
                break;
            case UiRootType.GameUi:
                root = Canvas_GameCanvas.transform;
                break;
        }
        return root;
    }

    private void CreateUi(UiRootType uiRootType, UiType uiType)
    {
        if(_createdUiDic.ContainsKey(uiType) == false)
        {
            string path = this.GetUiPath(uiRootType, uiType);
            GameObject loadedObj = (GameObject)Resources.Load(path);
            Transform root = GetRootTransform(uiRootType);
            GameObject gObj = Instantiate(loadedObj, root);
            if(gObj != null)
            {
                var uiBase = gObj.GetComponent<UiBase>();
                _createdUiDic.Add(uiType, uiBase);
            }
        }
    }

    private UiBase GetCreatedUi(UiRootType uiRootType, UiType uiType)
    {
        if (_createdUiDic.ContainsKey(uiType) == false)
        {
            CreateUi(uiRootType, uiType);
        }
        return _createdUiDic[uiType];
    }

}