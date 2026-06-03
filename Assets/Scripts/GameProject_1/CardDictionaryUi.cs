using UnityEngine;

public class CardDictionaryUi : UiBase
{
    [SerializeField] private UiButton button_Exit;
    [SerializeField] private GameObject slotCardPrefab;
    [SerializeField] private Transform cardContainer;
    
    private void OnEnable()
    {
        button_Exit.BindOnClickButtonEvent(OnClickMain);
        RefreshCardDictionary();
    }

    public void RefreshCardDictionary()
    {
        // 위치를 지정하지 않으면 얼리 리턴
        if (cardContainer == null) return;
        
        // 기존에 있었던 카드들이 중복으로 나타날 수 있기 때문에, 실행할 때마다 원래 있던 카드들을 없앤다.
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
        // 카드 데이터가 없거나 0개이면 얼리 리턴
        if (GameDataManager.Instance.CardDataList == null || GameDataManager.Instance.CardDataList.Count == 0) return;

        // 카드 데이터들을 실체화하는 과정
        foreach (CardData cardData in GameDataManager.Instance.CardDataList.Values)
        {
            if (cardData == null) continue;
            // Content의 자식으로 Slot_Card 프리팹을 동적 생성
            GameObject instantiatedCard = Instantiate(slotCardPrefab, cardContainer);

            // Slot_Card프리팹에 카드 데이터 삽입
            SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
            if (slotCardUi != null)
            {
                slotCardUi.SetUp(cardData);
            }
            // 드래그, 올려놓는 것 등등 게임상황이 아니므로 비활성화
            CardInteractionHandler interactionHandler = instantiatedCard.GetComponent<CardInteractionHandler>();
            if (interactionHandler != null)
            {
                interactionHandler.enabled = false;
            }
        }
        Debug.Log($"도감을 불러왔습니다. 총 {GameDataManager.Instance.CardDataList.Count}개의 카드를 불러왔습니다.");
    }
    public void OnClickMain()
    {
        UiManager.Instance.OpenMainUi();
        UiManager.Instance.CloseCardDictionaryUi();
    }
}