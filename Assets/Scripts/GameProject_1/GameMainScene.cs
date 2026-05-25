using UnityEngine;
using UnityEngine.UI;
public class GameMainScene : UiBase
{
    [SerializeField] private UiButton button_GameOver;
    [SerializeField] private UiButton button_ReChoice;
    [SerializeField] private UiButton button_Stage1;
    [SerializeField] private UiButton button_Stage2;
    [SerializeField] private UiButton button_Stage3;
    [SerializeField] private UiButton button_Stage4;
    [SerializeField] private UiButton button_Stage5;
    [SerializeField] private UiButton button_FinalStage;

    [SerializeField] private GameCharacter gameCharacter;
    private void OnEnable()
    {
        button_GameOver.BindOnClickButtonEvent(OnClickGameOver);
        button_ReChoice.BindOnClickButtonEvent(OnClickGameRetry);
        button_Stage1.BindOnClickButtonEvent(OnClickStage);
        button_Stage2.BindOnClickButtonEvent(OnClickStage);
        button_Stage3.BindOnClickButtonEvent(OnClickStage);
        button_Stage4.BindOnClickButtonEvent(OnClickStage);
        button_Stage5.BindOnClickButtonEvent(OnClickStage);
        button_FinalStage.BindOnClickButtonEvent(OnClickStage);

        SetupSelectedCharacter();
    }
    private void Update()
    {
        ShowandHideInventory();
    }
    public void OnClickGameOver()
    {
        Application.Quit();
    }
    public void OnClickGameRetry()
    {
        UiManager.Instance.OpenGameStartUi();
        UiManager.Instance.CloseGameMainScene();
    }
    public void OnClickStage()
    {
        UiManager.Instance.OpenStageUi();
        UiManager.Instance.CloseGameMainScene();
    }
    public void ShowandHideInventory()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UiManager.Instance.OpenInventory();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            UiManager.Instance.CloseInventory();
        }
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