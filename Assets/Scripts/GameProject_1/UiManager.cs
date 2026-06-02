using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // 시작될 캔버스 선택하는 부분
    [SerializeField] Canvas Canvas_GameCanvas;
    // 싱글턴을 사용하여 이 게임에서 하나밖에 없는 UiManager를 만듦
    public static UiManager Instance { get; set; }
    // 선택된 캐릭터
    public string SelectedCharacterId { get; set; }
    // 영웅에서 선택한 캐릭터 (도감처럼 카드들을 보기 위하여)
    public string ViewCharacterId { get; set; }
    // 한 번이라도 만들어진 UI들을 기억해 두는 보관함
    private Dictionary<UiType, UiBase> _createdUiDic = new Dictionary<UiType, UiBase>();
    // 현재 화면에 켜져 있는 UI들의 목록만 따로 관리하기 때문에 중복이 없으므로 해쉬셋을 사용
    private HashSet<UiType> _openUiDic = new HashSet<UiType>();

    private void Awake()
    {
        if (Instance == null) Instance = this; // Instance가 없다면 현재 자기 자신을 할당한다.
        else Destroy(gameObject); //이미 존재한다면 중복의 위험이 있으므로 파괴한다.
    }

    private void Start()
    {
        this.GameStart(); // UiManagerExtension에 있는 GameStart 메서드를 가져온다 => 가능한 이유는 UiManagerExtension에서 UiManager를 호출하기 때문에
    }
    // UI를 켜달라는 UiManager의 핵심기능
    public UiBase OpenUi(UiRootType uiRootType, UiType uiType, bool isInitialHide = false)
    {
        var openedUi = GetCreatedUi(uiRootType, uiType);
        bool isSetActiveOnOpen = (isInitialHide == false);

        if (_openUiDic.Contains(uiType) == false)
        {
            openedUi.gameObject.SetActive(isSetActiveOnOpen);
            _openUiDic.Add(uiType);
        }
        return openedUi;
    }
    // UI를 꺼달라는 UiManager의 핵심기능 => 닫기 즉 비활성화만 할 뿐이지, 삭제(Destroy)하는 것이 아님
    public void CloseUi(UiRootType uiRootType, UiType uiType)
    {
        if (_openUiDic.Contains(uiType))
        {
            var openedUi = _createdUiDic[uiType];
            openedUi.gameObject.SetActive(false);
            _openUiDic.Remove(uiType);
        }
    }

    // Canvas가 나타날 위치 조정 => 하나의 캔버스에서 나타나게 할꺼여서 하나의 캔버스의 트랜스폼을 가짐
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
    // 만들어지지 않은 UI를 메모리에 새로 Instantiate 하는 역할을 한다.
    private void CreateUi(UiRootType uiRootType, UiType uiType)
    {
        if (_createdUiDic.ContainsKey(uiType) == false)
        {
            string path = this.GetUiPath(uiRootType, uiType); // GetUiPath => 경로 가져오기 UiManagerExtension에서 구현되어 있음
            GameObject loadedObj = (GameObject)Resources.Load(path);
            Transform root = GetRootTransform(uiRootType);
            GameObject gObj = Instantiate(loadedObj, root);
            
            if (gObj != null)
            {
                var uiBase = gObj.GetComponent<UiBase>();
                _createdUiDic.Add(uiType, uiBase);
            }
        }
    }

    // 만들어진 UI를 반환합니다. 만약 없다면 CreateUi를 통하여 만듭니다. => 만들어진 Ui를 가져오는 메서드
    private UiBase GetCreatedUi(UiRootType uiRootType, UiType uiType)
    {
        if (_createdUiDic.ContainsKey(uiType) == false)
        {
            CreateUi(uiRootType, uiType);
        }
        return _createdUiDic[uiType];
    }
}