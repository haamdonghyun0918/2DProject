using UnityEngine;

public class ClearPopUp : UiBase
{
    [SerializeField] private UiButton button_Exit;
    [SerializeField] private UiButton button_Card;
    [SerializeField] private UiButton button_Rest;
    //스테이지가 끝나고 난 뒤, 보상이나 체력 회복을 선택했는지 확인하는 변수
    private bool hasChoicedReward = false;
    public void OnEnable()
    {
        //처음에는 선택하지 않았기 때문에 false로 시작
        hasChoicedReward = false;
        //보상을 선택하지 않으면 나가기 버튼이 활성화되지 않게 하여서 나가지 못하게 함
        button_Exit.gameObject.SetActive(false);
        //보상 선택은 활성화시켜서 선택할 수 있게 함
        button_Card.gameObject.SetActive(true);
        button_Rest.gameObject.SetActive(true);

        button_Exit.BindOnClickButtonEvent(OnClickPopUpExit);
        button_Card.BindOnClickButtonEvent(OnClickReward);
        button_Rest.BindOnClickButtonEvent(OnClickHeal);
    }
    public void OnClickPopUpExit()
    {
        //혹시 모르니 한번 더 보상을 선택하지 않으면 실행되지 않게 무시
        if (!hasChoicedReward) return;
        UiManager.Instance.OpenGameMainScene();
        UiManager.Instance.CloseStageUi();
        UiManager.Instance.CloseClearPopUp();
    }
    public void OnClickReward()
    {
        //다른 보상을 선택했다면 그냥 무시
        if (hasChoicedReward) return;
        //보상화면을 들어왔으므로 true로 변경시킴
        hasChoicedReward = true;
        UiManager.Instance.OpenCardRewardStage();
        ShowExitButton();
    }
    public void OnClickHeal()
    {
        if (hasChoicedReward) return;
        hasChoicedReward = true;
        if (StageManager.Instance != null)
        {
            StageManager.Instance.playerSavedHp += 20;
            if (StageManager.Instance.playerSavedHp > 100)
            {
                StageManager.Instance.playerSavedHp = 100;
            }
        }
        ShowExitButton();
    }
    public void ShowExitButton()
    {
        //나가기 버튼이 활성화 되었기에 나머지 보상 버튼은 false로 표기
        button_Exit.gameObject.SetActive(true);
        button_Card.gameObject.SetActive(false);
        button_Rest.gameObject.SetActive(false);
    }
}