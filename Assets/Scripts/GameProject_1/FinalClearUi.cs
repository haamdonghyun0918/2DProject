using UnityEngine;
using UnityEngine.UI;

public class FinalClearUi : MonoBehaviour
{
    [SerializeField] private UiButton button_FinalClear;
    [SerializeField] private Text text_TotalTurnsNum;
    [SerializeField] private Text text_CharacterName;

    private void OnEnable()
    {
        button_FinalClear.BindOnClickButtonEvent(OnClickClear);
        SetFinalStats();
    }

    public void OnClickClear()
    {
        UiManager.Instance.OpenMainUi();
        
        if (StageManager.Instance != null)
        {
            StageManager.Instance.ResetStageData();
        }
        GameDataManager.Instance.ResetCharacterData();
        
        UiManager.Instance.CloseFinalClearUi();
        UiManager.Instance.CloseGameMainScene();
    }

    private void SetFinalStats()
    {
        if (StageManager.Instance != null && text_TotalTurnsNum != null)
        {
            text_TotalTurnsNum.text = StageManager.Instance.totalAccumulatedTurns.ToString();
        }

        if (UiManager.Instance != null && text_CharacterName != null)
        {
            string charId = UiManager.Instance.SelectedCharacterId;
            CharacterData data = GameDataManager.Instance.GetCharacterData(charId);

            if (data != null)
            {
                text_CharacterName.text = data.Name;
            }
        }
    }
}