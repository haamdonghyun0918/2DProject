using UnityEngine;

public class CardInventory : UiBase
{
    // 카드를 표기할 위치 잡는 곳
    [SerializeField] private Transform cardContainer;
    // 프리팹된 슬롯을 가져오는 내용
    [SerializeField] private GameObject slotCardPrefab;

    [SerializeField] private UiButton button_Close;
    private void BringCardBook()
    {
        // 카드를 표기할 곳이 없다면 얼리 리턴함
        if (cardContainer == null) return;

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject); // 카드 인벤토리를 열 때마다 원래 있었던 오브젝트들을 삭제하는 과정
        }
        // 직업에 따라 카드가 달라지기 때문에 게임에 선택한 캐릭터의 데이터를 토대로 카드 데이터를 가져온다 (캐릭터의 외형과 애니메이션까지)
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
        
        // 카드 프리팹 동적 생성을 하고, 가져오는 과정
        foreach(string cardId in characterData.Card)
        {
            CardData cardData = GameDataManager.Instance.GetCardData(cardId);
            if (cardData != null)
            {
                // 원래 프리팹을 복사하여 cardContainer의 자식 오브젝트로 화면에 표현한다.
                GameObject instantiatedCard = Instantiate(slotCardPrefab, cardContainer);
                // 생성된 카드 객체에서 UI 표현을 담당하는 스크립트를 가져옴
                SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();

                if (slotCardUi != null)
                {
                    slotCardUi.SetUp(cardData); // 카드 Ui에 데이터 드리븐으로 데이터들을 가져온다.
                }
                // 여기서도 마찬가지로 카드를 드래그하거나 그러지 않을 것이므로, false로 비활성화 해준다.
                CardInteractionHandler interactionHandler = instantiatedCard.GetComponent<CardInteractionHandler>();
                
                if (interactionHandler != null)
                {
                    interactionHandler.enabled = false;
                }
            }
            else
            {
                Debug.LogError("카드 데이터를 찾을 수 없습니다");
            }
        }
        Debug.Log("카드들을 Slot_Card에 이미지와 텍스트를 다 가져왔습니다");
    }
    public void OnClickClose()
    {
        UiManager.Instance.CloseInventory();
    }

    private void OnEnable()
    {
        // Ui에서 맨 앞으로 보내는 코드
        this.transform.SetAsLastSibling();

        BringCardBook();
        button_Close.BindOnClickButtonEvent(OnClickClose);
    }
}