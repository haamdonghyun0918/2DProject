using UnityEngine;

public class CharacterGunInfoPopUp : UiBase
{
    [SerializeField] UiButton button_Gun_Close;
    private void OnEnable()
    {
        button_Gun_Close.BindOnClickButtonEvent(OnCloseGunInfoPopup);
    }
    private void OnCloseGunInfoPopup()
    {
        UiManager.Instance.CloseCharacterGunInfoPopUp();
    }
}