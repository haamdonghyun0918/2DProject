using UnityEngine;

public class CharacterKnifeInfoPopUp : UiBase
{
    [SerializeField] UiButton button_Knife_Close;
    private void OnEnable()
    {
        button_Knife_Close.BindOnClickButtonEvent(OnCloseKnifeInfoPopup);
    }
    private void OnCloseKnifeInfoPopup()
    {
        UiManager.Instance.CloseCharacterKnifeInfoPopUp();
    }
}