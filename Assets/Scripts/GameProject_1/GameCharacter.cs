using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class GameCharacter : UiBase
{
    [SerializeField] private Image image_Character;
    [SerializeField] private Animator animator_Character;
    [SerializeField] private Slider slider_Hp;
    [SerializeField] private Text text_Hp;
    [SerializeField] private GameObject image_Damaged;
    [SerializeField] private Text text_Damaged;
    [SerializeField] private GameObject image_Heal;
    [SerializeField] private Text text_Heal;
    private int currentHp;
    private int maxHp;

    public int GetCurrentHp()
    {
        return currentHp;
    }
    public void SetCurrentHp(int hp)
    {
        currentHp = hp;
        if (slider_Hp != null) slider_Hp.value = currentHp;
        if (text_Hp != null) text_Hp.text = currentHp.ToString();
    }
    public void SetUp(CharacterData data)
    {
        if (data == null) return;
        maxHp = data.Hp;
        currentHp = data.Hp;

        if (slider_Hp != null)
        {
            slider_Hp.maxValue = data.Hp;
            slider_Hp.value = data.Hp;
        }
        if (text_Hp != null)
        {
            text_Hp.text = data.Hp.ToString();
        }

        if (image_Damaged != null) image_Damaged.SetActive(false);
        if (image_Heal != null) image_Heal.SetActive(false);

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
        Sprite targetSprite = null;
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name == data.CharacterImageSpriteName)
            {
                targetSprite = sprite;
                break;
            }
        }

        if (targetSprite != null)
        {
            image_Character.sprite = targetSprite;
        }
        else
        {
            Debug.LogError($"[에러] {data.CharacterImageAddress} 경로에서 {data.CharacterImageSpriteName} 이름의 조각을 못 찾음!");
        }
    }
    public void PlayAttackAnim()
    {
        if (animator_Character != null)
        {
            animator_Character.SetTrigger("isAttack");
        }
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        if (slider_Hp != null) slider_Hp.DOValue(currentHp, 0.5f).SetEase(Ease.OutQuad);
        if (text_Hp != null) text_Hp.text = currentHp.ToString();
        //slider_Hp.value = currentHp;
        //text_Hp.text = currentHp.ToString();

        if (animator_Character != null)
        {
            animator_Character.SetTrigger("isDamaged");
        }

        ShowDamageUI(damage);
    }
    public void ShowDamageUI(int damage)
    {
        if (image_Damaged == null || text_Damaged == null) return;

        text_Damaged.text = damage.ToString();
        image_Damaged.SetActive(true);

        StopCoroutine("HideDamageUIRoutine");
        StartCoroutine("HideDamageUIRoutine");
    }
    public IEnumerator HideDamageUIRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        if (image_Damaged != null)
        {
            image_Damaged.SetActive(false);
        }
    }
    public void HealHp(int healAmount)
    {
        if (healAmount <= 0) return;
        currentHp += healAmount;
        
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        if (slider_Hp != null) slider_Hp.DOValue(currentHp, 0.5f).SetEase(Ease.OutQuad);
        if (text_Hp != null) text_Hp.text = currentHp.ToString();
        //if (slider_Hp != null) slider_Hp.value = currentHp;
        //if (text_Hp != null) text_Hp.text = currentHp.ToString();

        ShowHealUI(healAmount);
    }
    public void ShowHealUI(int healAmount)
    {
        if (image_Heal == null || text_Heal == null) return;

        text_Heal.text = healAmount.ToString();
        image_Heal.SetActive(true);

        StopCoroutine("HideHealUIRoutine");
        StartCoroutine("HideHealUIRoutine");
    }
    public IEnumerator HideHealUIRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        if (image_Heal != null)
        {
            image_Heal.SetActive(false);
        }
    }
    public bool IsDead()
    {
        return currentHp <= 0;
    }
}