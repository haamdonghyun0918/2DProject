using UnityEngine;

public class CharacterInfoPopUp : UiBase
{
    [SerializeField] UiButton button_Close;
    private void OnEnable()
    {
        button_Close.BindOnClickButtonEvent(OnCloseInfoPopup);
    }
    private void OnCloseInfoPopup()
    {
        UiManager.Instance.CloseCharacterInfoPopUp();
    }
}