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
        // 단순히 캐릭터의 정보와 카드를 보여주기 위하여, UiManager에 있는 ViewCharacterId를 가져옴
        UiManager.Instance.ViewCharacterId = "character_punch_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnClickGun()
    {
        // 단순히 캐릭터의 정보와 카드를 보여주기 위하여, UiManager에 있는 ViewCharacterId를 가져옴
        UiManager.Instance.ViewCharacterId = "character_gun_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnClickKnife()
    {
        // 단순히 캐릭터의 정보와 카드를 보여주기 위하여, UiManager에 있는 ViewCharacterId를 가져옴
        UiManager.Instance.ViewCharacterId = "character_knife_01";
        UiManager.Instance.OpenCharacterInfoPopUp();
    }
    public void OnBackMainUi()
    {
        // 단순히 캐릭터의 정보와 카드를 보여주기 위하여, UiManager에 있는 ViewCharacterId를 가져옴
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseCharacterUi();
        UiManager.Instance.CloseCharacterInfoPopUp();
    }
}