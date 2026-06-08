using UnityEngine;
public class GameMainScene : UiBase
{
    [Header("재시작/종료")]
    [SerializeField] private UiButton button_GameOver;
    [SerializeField] private UiButton button_ReChoice;

    [Header("스테이지 버튼")]
    [SerializeField] private UiButton button_Stage1;
    [SerializeField] private UiButton button_Stage2;
    [SerializeField] private UiButton button_Stage3;
    [SerializeField] private UiButton button_Stage4;
    [SerializeField] private UiButton button_Stage5;
    [SerializeField] private UiButton button_FinalStage;

    [Header("플레이어 이미지")]
    [SerializeField] private GameCharacter gameCharacter;

    [Header("성공 이미지(1~5)")]
    [SerializeField] private GameObject img_Stage1_Success;
    [SerializeField] private GameObject img_Stage2_Success;
    [SerializeField] private GameObject img_Stage3_Success;
    [SerializeField] private GameObject img_Stage4_Success;
    [SerializeField] private GameObject img_Stage5_Success;

    [Header("실패 이미지(1~6)")]
    [SerializeField] private GameObject img_Stage1_Fail;
    [SerializeField] private GameObject img_Stage2_Fail;
    [SerializeField] private GameObject img_Stage3_Fail;
    [SerializeField] private GameObject img_Stage4_Fail;
    [SerializeField] private GameObject img_Stage5_Fail;
    [SerializeField] private GameObject img_Final_Fail;

    [Header("잠금 이미지")]
    [SerializeField] private GameObject block_Stage2;
    [SerializeField] private GameObject block_Stage3;
    [SerializeField] private GameObject block_Stage4;
    [SerializeField] private GameObject block_Stage5;
    [SerializeField] private GameObject block_Final;
    
    [Header("도움말 버튼")]
    [SerializeField] private UiButton button_Example;

    [Header("덱 버튼")]
    [SerializeField] private UiButton button_Deck;
    private void OnEnable()
    {
        button_GameOver.BindOnClickButtonEvent(OnClickGameOver);
        button_ReChoice.BindOnClickButtonEvent(OnClickGameRetry);

        button_Example.BindOnClickButtonEvent(OnClickExample);

        button_Deck.BindOnClickButtonEvent(OnClickDeck);

        button_Stage1.BindOnClickButtonEvent(OnClickStage1_Button);
        button_Stage2.BindOnClickButtonEvent(OnClickStage2_Button);
        button_Stage3.BindOnClickButtonEvent(OnClickStage3_Button);
        button_Stage4.BindOnClickButtonEvent(OnClickStage4_Button);
        button_Stage5.BindOnClickButtonEvent(OnClickStage5_Button);
        button_FinalStage.BindOnClickButtonEvent(OnClickFinalStage_Button);

        SetupSelectedCharacter();
        UpdateStageUI();
    }
    
    private void OnClickExample()
    {

    }

    private void OnClickDeck()
    {
        UiManager.Instance.OpenInventory();
    }

    private void OnClickStage1_Button()
    {
        OnClickStage(1);
    }
    
    private void OnClickStage2_Button()
    {
        if (StageManager.Instance.highestClearedStage >= 1)
        {
            OnClickStage(2);
        }
    }
    
    private void OnClickStage3_Button()
    {
        if (StageManager.Instance.highestClearedStage >= 2)
        {
            OnClickStage(3);
        }
    }
    private void OnClickStage4_Button()
    {
        if (StageManager.Instance.highestClearedStage >= 3)
        {
            OnClickStage(4);
        }
    }
    private void OnClickStage5_Button()
    {
        if (StageManager.Instance.highestClearedStage >= 4)
        {
            OnClickStage(5);
        }
    }
    private void OnClickFinalStage_Button()
    {
        if (StageManager.Instance.highestClearedStage >= 5)
        {
            OnClickStage(6);
        }
    }

    // StageManager에서 배열로 갖고 있던 스테이지의 결과값을 가지고 성공 이미지와 실패 이미지를 실행시키고, 잠금 이미지를 비활성화 합니다.
    private void UpdateStageUI()
    {
        if (StageManager.Instance == null) return;
        int[] results = StageManager.Instance.stageResults;
        int highest = StageManager.Instance.highestClearedStage;

        if (img_Stage1_Success != null) img_Stage1_Success.SetActive(results[1] == 1);
        if (img_Stage1_Fail != null) img_Stage1_Fail.SetActive(results[1] == 2);

        if (img_Stage2_Success != null) img_Stage2_Success.SetActive(results[2] == 1);
        if (img_Stage2_Fail != null) img_Stage2_Fail.SetActive(results[2] == 2);

        if (img_Stage3_Success != null) img_Stage3_Success.SetActive(results[3] == 1);
        if (img_Stage3_Fail != null) img_Stage3_Fail.SetActive(results[3] == 2);

        if (img_Stage4_Success != null) img_Stage4_Success.SetActive(results[4] == 1);
        if (img_Stage4_Fail != null) img_Stage4_Fail.SetActive(results[4] == 2);

        if (img_Stage5_Success != null) img_Stage5_Success.SetActive(results[5] == 1);
        if (img_Stage5_Fail != null) img_Stage5_Fail.SetActive(results[5] == 2);
        // 마지막 스테이지는 클리어 이미지 볼 필요 없이 초기화되고 바로 메인화면으로 직행
        if (img_Final_Fail != null) img_Final_Fail.SetActive(results[6] == 2);

        if (block_Stage2 != null) block_Stage2.SetActive(highest < 1);
        if (block_Stage3 != null) block_Stage3.SetActive(highest < 2);
        if (block_Stage4 != null) block_Stage4.SetActive(highest < 3);
        if (block_Stage5 != null) block_Stage5.SetActive(highest < 4);
        if (block_Final != null) block_Final.SetActive(highest < 5);
    }
    
    public void OnClickGameOver()
    {
        Application.Quit();
    }
    
    public void OnClickGameRetry()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.ResetStageData();
        }

        GameDataManager.Instance.ResetCharacterData();
        UiManager.Instance.OpenGameStartUi();
        UiManager.Instance.CloseGameMainScene();
        UiManager.Instance.CloseInventory();
    }
    
    public void OnClickStage(int stageNum)
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.currentStageNum = stageNum;
        }
        UiManager.Instance.OpenStageUi();
        UiManager.Instance.CloseInventory();
        UiManager.Instance.CloseGameMainScene();
    }
    
    private void SetupSelectedCharacter()
    {
        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData characterdata = GameDataManager.Instance.GetCharacterData(charId);

        if (characterdata == null) return;
        Debug.Log($"게임이 시작되었습니다!! 선택된 캐릭터는 {characterdata.Name}입니다!");

        if (gameCharacter != null)
        {
            gameCharacter.SetUp(characterdata);
        }
        else
        {
            Debug.LogError("캐릭터를 찾지 못했습니다!");
        }
    }
}