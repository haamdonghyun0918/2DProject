using UnityEngine;

public class CharacterUi : UiBase
{
    [SerializeField] private UiButton button_Punch;
    [SerializeField] private UiButton button_Gun;
    [SerializeField] private UiButton button_Knife;
    [SerializeField] private UiButton button_Mainscene;
    private void OnEnable()
    {
        button_Punch.BindOnClickButtonEvent(OnOpenPunchInfoPopup);
        button_Gun.BindOnClickButtonEvent(OnOpenGunInfoPopup);
        button_Knife.BindOnClickButtonEvent(OnOpenKnifeInfoPopup);
        button_Mainscene.BindOnClickButtonEvent(OnBackMainUi);
    }

    public void OnOpenPunchInfoPopup()
    {
        UiManager.Instance.OpenCharacterPunchInfoPopUp();
        UiManager.Instance.CloseCharacterKnifeInfoPopUp();
        UiManager.Instance.CloseCharacterGunInfoPopUp();
    }
    public void OnOpenGunInfoPopup()
    {
        UiManager.Instance.OpenCharacterGunInfoPopUp();
        UiManager.Instance.CloseCharacterPunchInfoPopUp();
        UiManager.Instance.CloseCharacterKnifeInfoPopUp();
    }
    public void OnOpenKnifeInfoPopup()
    {
        UiManager.Instance.OpenCharacterKnifeInfoPopUp();
        UiManager.Instance.CloseCharacterPunchInfoPopUp();
        UiManager.Instance.CloseCharacterGunInfoPopUp();
    }
    public void OnBackMainUi()
    {
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseCharacterUi();
        UiManager.Instance.CloseCharacterPunchInfoPopUp();
        UiManager.Instance.CloseCharacterGunInfoPopUp();
        UiManager.Instance.CloseCharacterKnifeInfoPopUp();
    }
}