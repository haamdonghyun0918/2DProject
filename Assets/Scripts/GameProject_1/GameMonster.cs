using UnityEngine;
using UnityEngine.UI;
public class GameMonster : UiBase
{
    [SerializeField] private Image image_Monster;
    [SerializeField] private Animator animator_Monster;

    public void SetUp(MonsterData data)
    {
        if (data == null) return;
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(data.MonsterAnim);
        if (controller != null)
        {
            animator_Monster.runtimeAnimatorController = controller;
        }
        else
        {
            Debug.LogError("애니메이터를 가져올 수 없습니다! 주소를 확인해보세요");
        }
        
        Sprite[] allSprites = Resources.LoadAll<Sprite>(data.MonsterAnim);
        Sprite targetSprite = System.Array.Find(allSprites, sprite => sprite.name == data.MonsterSpriteName);

        if (targetSprite != null)
        {
            image_Monster.sprite = targetSprite;
        }
        else
        {
            Debug.LogError("스프라이트 이미지를 찾을 수 없습니다. 다시 주소값을 확인하세요");
        }
    }
}