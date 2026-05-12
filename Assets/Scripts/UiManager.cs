using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject UiRoot;

    public static UiManager Instance { get; set; }
    //생성, 제거 딕셔너리
    private Dictionary<UiType, GameObject> _createdUiDic = new Dictionary<UiType, GameObject>();
    //활성, 비활성 -> SetActive
    private HashSet<UiType> _openUiDic = new HashSet<UiType>();

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        UiManagerExtension.OpenMainUi(this);
    }
    public void OpenUi(UiType uiType, GameObject uiObject)
    {
        if(_openUiDic.Contains(uiType) == false)
        {
            uiObject.SetActive(true);
            _openUiDic.Add(uiType);
        }
    }

    private void CloseUi(UiType uiType)
    {
        if(_openUiDic.Contains(uiType))
        {
            var uiObject = _createdUiDic[uiType];
            uiObject.SetActive(false);
            _openUiDic.Remove(uiType);
        }
    }

    private void CreateUi(UiType uiType)
    {
        if(_createdUiDic.ContainsKey(uiType) == false)
        {
            string path = this.GetUiPath(uiType);
            GameObject loadedObj = (GameObject)Resources.Load(path);
            GameObject gObj = Instantiate(loadedObj, UiRoot.transform);
            if(gObj != null)
            {
                _createdUiDic.Add(uiType, gObj);
            }
        }
    }

    public GameObject GetCreatedUi(UiType uiType)
    {
        if (_createdUiDic.ContainsKey(uiType) == false)
        {
            CreateUi(uiType);
        }
        return _createdUiDic[uiType];
    }
    public void CloseSpecificUi(UiType uiType)
    {
        CloseUi(uiType);
    }
}