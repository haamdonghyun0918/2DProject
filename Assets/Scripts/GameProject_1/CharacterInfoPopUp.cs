using UnityEngine;
using UnityEngine.UI;
public class CharacterInfoPopUp : UiBase
{
    [SerializeField] UiButton button_Close;

    [SerializeField] private Text text_CharacterName;
    [SerializeField] private Image image_CharacterMovement;
    [SerializeField] private Animator animator_Character;

    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject slotCardPrefab;
    private void OnEnable()
    {
        button_Close.BindOnClickButtonEvent(OnCloseInfoPopup);
        SetUpInfo();
    }
    private void SetUpInfo()
    {
        string charId = UiManager.Instance.ViewCharacterId;
        if (string.IsNullOrEmpty(charId)) return;

        CharacterData data = GameDataManager.Instance.GetCharacterData(charId);
        if (data == null) return;

        if (text_CharacterName != null)
        {
            text_CharacterName.text = data.Name;
        }

        // 애니메이션 파일을 게임 실행 중 실시간으로 갈아끼우는 기능 => JSON 데이터에 있는 주소에서 가져온 뒤, UI에 있는 Animator 컴포넌트의 컨트롤러를 변경한다.
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(data.CharacterAnimAddress);
        if (controller != null && animator_Character != null)
        {
            animator_Character.runtimeAnimatorController = controller;
        }
        // LoadAll을 통하여 스프라이트 시트를 스프라이트 배열 형태로 (Sprite[])로 가져온 뒤 그 안에서 필요한 것을 찾아내는 메서드
        Sprite[] allSprites = Resources.LoadAll<Sprite>(data.CharacterAnimAddress);
        Sprite targetSprite = null;
        
        if (allSprites != null)
        {
            foreach (Sprite sprite in allSprites)
            {
                if (sprite.name == data.CharacterImageSpriteName)
                {
                    targetSprite = sprite;
                    break;
                }
            }
        }
        
        if (targetSprite != null && image_CharacterMovement != null)
        {
            image_CharacterMovement.sprite = targetSprite;
        }

        if (cardContainer != null)
        {
            foreach (Transform child in cardContainer)
            {
                // 이전에 열었던 캐릭터의 정보 UI를 전부 지운다.
                Destroy(child.gameObject);
            }

            if (data.Card != null)
            {
                foreach (string cardId in data.Card)
                {
                    CardData cData = GameDataManager.Instance.GetCardData(cardId);
                    
                    if (cData != null)
                    {
                        // 동적 프리팹 생성 및 데이터 세팅: 캐릭터가 보유한 카드 개수만큼 화면에 카드 UI프리팹을 표기한는 메서드
                        GameObject instantiatedCard = Instantiate(slotCardPrefab, cardContainer);
                        SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();

                        if (slotCardUi != null)
                        {
                            slotCardUi.SetUp(cData);
                        }

                        CardInteractionHandler interactionHandler = instantiatedCard.GetComponent<CardInteractionHandler>();
                        if (interactionHandler != null)
                        {
                            // 카드의 드래그와 DOTween으로 사용되는 카드 효과들을 비활성화하여 카드만 보이게 하는 메서드
                            interactionHandler.enabled = false;
                        }
                    }
                }
            }
        }
    }
    private void OnCloseInfoPopup()
    {
        UiManager.Instance.CloseCharacterInfoPopUp();
    }
}