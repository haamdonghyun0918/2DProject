using UnityEngine;

public class CharacterUi : UiBase
{
    [SerializeField] private UiButton button_Punch;
    [SerializeField] private UiButton button_Gun;
    [SerializeField] private UiButton button_Knife;
    [SerializeField] private UiButton button_Mainscene;
    private void OnEnable()
    {
        button_Punch.BindOnClickButtonEvent(OnClickPunch);
        button_Gun.BindOnClickButtonEvent(OnClickGun);
        button_Knife.BindOnClickButtonEvent(OnClickKnife);
        button_Mainscene.BindOnClickButtonEvent(OnBackMainUi);
    }
    public void OnClickPunch()
    {
        UiManager.Instance.ViewCharacterId = "character_punch_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnClickGun()
    {
        UiManager.Instance.ViewCharacterId = "character_gun_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnClickKnife()
    {
        UiManager.Instance.ViewCharacterId = "character_knife_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnBackMainUi()
    {
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseCharacterUi();
    }
}