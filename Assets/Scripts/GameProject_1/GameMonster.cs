using UnityEngine;
using UnityEngine.UI;
public class GameMonster : UiBase
{
    [SerializeField] private Image image_Monster;
    [SerializeField] private Animator animator_Monster;
    [SerializeField] private Slider slider_Hp;
    [SerializeField] private Text text_Hp;
    private int currentHp;
    private int attackPower;

    public void SetUp(MonsterData data)
    {
        if (data == null) return;

        currentHp = data.MonsterHp;
        attackPower = data.MonsterAtk;

        if (slider_Hp != null)
        {
            slider_Hp.maxValue = data.MonsterHp;
            slider_Hp.value = data.MonsterHp;
        }
        if (text_Hp != null)
        {
            text_Hp.text = data.MonsterHp.ToString();
        }
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(data.MonsterAnim);
        if (controller != null)
        {
            animator_Monster.runtimeAnimatorController = controller;
        }
        else
        {
            Debug.LogError("애니메이터를 가져올 수 없습니다! 주소를 확인해보세요");
        }
        
        Sprite[] allSprites = Resources.LoadAll<Sprite>(data.MonsterAddress);
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
    public int GetAttackPower() => attackPower;
    public void PlayAttackAnim()
    {
        if (animator_Monster != null)
        {
            animator_Monster.SetTrigger("isAttack");
        }
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        slider_Hp.value = currentHp;
        text_Hp.text = currentHp.ToString();

        if (animator_Monster != null)
        {
            animator_Monster.SetTrigger("isDamaged");
        }
    }
    public bool IsDead() => currentHp <= 0;
}