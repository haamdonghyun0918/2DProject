using UnityEngine;
using UnityEngine.UI;
public class GameCharacter : UiBase
{
    [SerializeField] private Image image_Character;
    [SerializeField] private Animator animator_Character;

    public void SetUp(CharacterData data)
    {
        if (data == null) return;

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(data.CharacterAnimAddress);
        if (controller != null)
        {
            animator_Character.runtimeAnimatorController = controller;
        }
        else
        {
            Debug.LogError("애니메이터를 가져올 수 없습니다! 주소를 확인해보세요");
        }
        Sprite[] allSprites = Resources.LoadAll<Sprite>(data.CharacterImageAddress);
        foreach (var s in allSprites) Debug.Log($"로드된 조각: {s.name}");
        Sprite targetSprite = System.Array.Find(allSprites, sprite => sprite.name == data.CharacterImageSpriteName);

        if (targetSprite != null)
        {
            image_Character.sprite = targetSprite;
        }
        else
        {
            Debug.LogError($"[에러] '{data.CharacterImageAddress}' 경로에서 " +
                       $"'{data.CharacterImageSpriteName}' 이름의 조각을 못 찾음!");
        }
    }
}