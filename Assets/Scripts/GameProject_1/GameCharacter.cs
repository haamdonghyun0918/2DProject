using UnityEngine;
using UnityEngine.UI;
public class GameCharacter : UiBase
{
    [SerializeField] private Image image_Character;
    [SerializeField] private Animator animator_Character;
    [SerializeField] private Slider slider_Hp;
    [SerializeField] private Text text_Hp;
    public void SetUp(CharacterData data)
    {
        if (data == null) return;
        
        if (slider_Hp != null)
        {
            slider_Hp.maxValue = data.Hp;
            slider_Hp.value = data.Hp;
        }

        if (text_Hp != null)
        {
            text_Hp.text = data.Hp.ToString();
        }
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
        Sprite targetSprite = System.Array.Find(allSprites, sprite => sprite.name == data.CharacterImageSpriteName);

        if (targetSprite != null)
        {
            image_Character.sprite = targetSprite;
        }
        else
        {
            Debug.LogError($"[에러] {data.CharacterImageAddress} 경로에서 {data.CharacterImageSpriteName} 이름의 조각을 못 찾음!");
        }
    }
}