using UnityEngine;
using UnityEngine.UI;
public class GameMainScene : UiBase
{
    [SerializeField] private UiButton button_GameOver;
    [SerializeField] private Image image_Character;
    [SerializeField] private Animator animator_CharacterMovement;
    public void OnEnable()
    {
        button_GameOver.BindOnClickButtonEvent(OnClickGameOver);
        SetupSelectedCharacter();
    }
    public void OnClickGameOver()
    {
        Application.Quit();
    }
    private void SetupSelectedCharacter()
    {
        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData characterdata = GameDataManager.Instance.GetCharacterData(charId);

        if (characterdata == null) return;
        Debug.Log($"게임이 시작되었습니다!! 선택된 캐릭터는 {characterdata.Name}입니다!");

        string textureName = string.Empty;
        string animatorName = string.Empty;
        string subSpriteName = string.Empty;
        switch (charId)
        {
            case "character_punch_01":
                textureName = "punch_idle";
                animatorName = "punch_idle_0";
                subSpriteName = "punch_idle_0";
                break;
            case "character_gun_01":
                textureName = "gun_idle";
                animatorName = "gun_idle_0";
                subSpriteName = "gun_idle_0";
                break;
            case "character_knife_01":
                textureName = "knife_idle";
                animatorName = "knife_idle_0";
                subSpriteName = "knife_idle_0";
                break;
            default:
                Debug.LogError("가져올 수 없는 캐릭터입니다. 존재하지 않거나 오타가 난 것 같습니다.");
                return;
        }
        Sprite[] allSprites = Resources.LoadAll<Sprite>($"Image/{textureName}");
        Sprite targetSprite = System.Array.Find(allSprites, sprite => sprite.name == subSpriteName);
        if (targetSprite != null)
        {
            image_Character.sprite = targetSprite;
            Debug.Log("이미지 가져오기 성공!!");
        }
        else
        {
            Debug.LogError("이미지를 가져올 수 없습니다... 이름이나 경로를 다시 확인해보세요!");
        }

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"Animator/{animatorName}");
        if (controller != null)
        {
            animator_CharacterMovement.runtimeAnimatorController = controller;
            Debug.Log("애니메이터 가져오기 성공!!");
        }
        else
        {
            Debug.LogError("애니메이터를 가져올 수 없습니다... 이름이나 경로를 다시 확인해보세요!");
        }
    }
}