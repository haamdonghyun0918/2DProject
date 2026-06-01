using Unity.VisualScripting;
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

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(data.CharacterAnimAddress);
        if (controller != null && animator_Character != null)
        {
            animator_Character.runtimeAnimatorController = controller;
        }
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
                Destroy(child.gameObject);
            }

            if (data.Card != null)
            {
                foreach (string cardId in data.Card)
                {
                    CardData cData = GameDataManager.Instance.GetCardData(cardId);
                    if (cData != null)
                    {
                        GameObject instantiatedCard = Instantiate(slotCardPrefab, cardContainer);
                        SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();

                        if (slotCardUi != null)
                        {
                            slotCardUi.SetUp(cData);
                        }

                        CardInteractionHandler interactionHandler = instantiatedCard.GetComponent<CardInteractionHandler>();
                        if (interactionHandler != null)
                        {
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