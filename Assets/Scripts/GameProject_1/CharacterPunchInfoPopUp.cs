using UnityEngine;

public class CharacterPunchInfoPopUp : UiBase
{
    [SerializeField] UiButton button_Punch_Close;
    private void OnEnable()
    {
        button_Punch_Close.BindOnClickButtonEvent(OnClosePunchInfoPopup);
    }
    private void OnClosePunchInfoPopup()
    {
        UiManager.Instance.CloseCharacterPunchInfoPopUp();
    }
}