using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GameMonster : UiBase
{
    [SerializeField] private Image image_Monster;
    [SerializeField] private Animator animator_Monster;
    [SerializeField] private Slider slider_Hp;
    [SerializeField] private Text text_Hp;
    [SerializeField] private GameObject image_Damaged;
    [SerializeField] private Text text_Damaged;
    [SerializeField] private GameObject image_State;
    [SerializeField] private Text text_Bleed;
    private int currentHp;
    private int attackPower;
    private int currentBleed = 0;

    public void SetUp(MonsterData data)
    {
        if (data == null) return;

        currentHp = data.MonsterHp;
        attackPower = data.MonsterAtk;
        currentBleed = 0;

        if (slider_Hp != null)
        {
            slider_Hp.maxValue = data.MonsterHp;
            slider_Hp.value = data.MonsterHp;
        }
        if (text_Hp != null)
        {
            text_Hp.text = data.MonsterHp.ToString();
        }
        if (image_Damaged != null) image_Damaged.SetActive(false);
        UpdateBleedUI();

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
        Sprite targetSprite = null;
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name == data.MonsterSpriteName)
            {
                targetSprite = sprite;
                break;
            }
        }

        if (targetSprite != null)
        {
            image_Monster.sprite = targetSprite;
        }
        else
        {
            Debug.LogError("스프라이트 이미지를 찾을 수 없습니다. 다시 주소값을 확인하세요");
        }
    }
    public int GetAttackPower()
    {
        return attackPower;
    }
    public void PlayAttackAnim()
    {
        if (animator_Monster != null)
        {
            animator_Monster.SetTrigger("isAttack");
        }
    }
    public void TakeDamage(int damage, bool isBleedDamage = false)
    {
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        slider_Hp.value = currentHp;
        text_Hp.text = currentHp.ToString();

        if (animator_Monster != null)
        {
            animator_Monster.SetTrigger("isDamaged");
        }
        
        if (!isBleedDamage)
        {
            ShowDamageUI(damage);
        }
    }
    public void ShowDamageUI(int damage)
    {
        if (image_Damaged == null || text_Damaged == null) return;

        text_Damaged.text = damage.ToString();
        image_Damaged.SetActive(true);

        StopCoroutine("HideDamageUIRoutine");
        StartCoroutine("HideDamageUIRoutine");
    }
    public void PlayBossDieAnim()
    {
        if (animator_Monster != null)
        {
            animator_Monster.SetTrigger("isDie");
        }
    }
    private IEnumerator HideDamageUIRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        if (image_Damaged != null)
        {
            image_Damaged.SetActive(false);
        }
    }
    public void AddBleed(int amount)
    {
        currentBleed += amount;
        UpdateBleedUI();
    }
    public int GetCurrentBleed()
    {
        return currentBleed;
    }
    public void UpdateBleedUI()
    {
        if (image_State == null || text_Bleed == null) return;

        if (currentBleed > 0)
        {
            image_State.SetActive(true);
            text_Bleed.text = currentBleed.ToString();
        }
        else
        {
            image_State.SetActive(false);
        }
    }
    public void ApplyBleedDamage()
    {
        if (currentBleed > 0)
        {
            Debug.Log($"출혈로 몬스터가 {currentBleed}의 데미지를 받습니다");
            TakeDamage(currentBleed, true);
        }
    }
    public bool IsDead()
    {
        return currentHp <= 0;
    }

    private Outline targetOutline;

    public void SetTargetOutline(bool isTargeted)
    {
        if (targetOutline == null)
        {
            targetOutline = image_Monster.gameObject.GetComponent<Outline>();
            if (targetOutline == null)
            {
                targetOutline = image_Monster.gameObject.AddComponent<Outline>();
            }

            targetOutline.effectColor = Color.red;
            targetOutline.effectDistance = new Vector2(10f, -10f);
        }

        targetOutline.enabled = isTargeted;
    }

    public float GetCurrentAnimLength()
    {
        if (animator_Monster != null)
        {
            return animator_Monster.GetCurrentAnimatorStateInfo(0).length;
        }
        return 0f;
    }

    public void FlipBoss()
    {
        transform.localScale = new Vector3(-1f, 1f, 1f);

        if (slider_Hp != null)
        {
            slider_Hp.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (image_Damaged != null)
        {
            image_Damaged.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (image_State != null)
        {
            image_State.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}