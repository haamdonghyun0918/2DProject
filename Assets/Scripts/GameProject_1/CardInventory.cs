using UnityEngine;

public class CardInventory : UiBase
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private GameObject slotCardPrefab;
    private void BringCardBook()
    {
        if (cardContainer == null) return;

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        if (selectedCharId == null)
        {
            Debug.LogError("선택한 캐릭터가 없습니다");
            return;
        }
        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        if (characterData == null || characterData.Card == null)
        {
            Debug.LogError("선택한 캐릭터의 데이터나 카드가 없습니다");
            return;
        }
        
        foreach(string cardId in characterData.Card)
        {
            CardData cardData = GameDataManager.Instance.GetCardData(cardId);
            if (cardData != null)
            {
                GameObject instantiatedCard = Instantiate(slotCardPrefab, cardContainer);
                SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
                if (slotCardUi != null)
                {
                    slotCardUi.SetUp(cardData);
                }
            }
            else
            {
                Debug.LogError("카드 데이터를 찾을 수 없습니다");
            }
        }
        Debug.Log("카드들을 Slot_Card에 이미지와 텍스트를 다 가져왔습니다");
    }
    private void OnEnable()
    {
        BringCardBook();
    }
}