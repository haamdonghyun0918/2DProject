using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardRewardStage : UiBase
{
    [SerializeField] private GameObject CardSpawn1;
    [SerializeField] private GameObject CardSpawn2;
    [SerializeField] private GameObject CardSpawn3;
    [SerializeField] private GameObject slotCardPrefab;

    private void OnEnable()
    {
        GenerateRewardCards();
    }

    private void GenerateRewardCards()
    {
        ClearSpawnPoints();
        if (GameDataManager.Instance.CardDataList == null || GameDataManager.Instance.CardDataList.Count == 0)
        {
            Debug.LogError("카드 데이터 리스트가 비어있습니다");
            return;
        }
        //선택된 캐릭터의 카드 목록 가져오기
        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        //캐릭터가 이미 가지고 있는 카드 ID리스트 생성
        if (characterData == null) return;
        List<string> ownedCardIds = new List<string>(characterData.Card ?? new string[0]);
        //전체 공통 카드중에서 안 가진 카드만 있는 리스트 생성
        List<CardData> commonCards = new List<CardData>();
        foreach (CardData card in GameDataManager.Instance.CardDataList.Values)
        {
            //Id가 card_common으로 시작하는 카드만 추가합니다.
            if (card != null && card.Id.StartsWith("card_common"))
            {
                if (!ownedCardIds.Contains(card.Id))
                {
                    commonCards.Add(card);
                }
            }
        }

        if (commonCards.Count < 3)
        {
            Debug.LogError("공용카드가 3개 미만입니다!");
            return;
        }

        List<CardData> selectedRewardCards = GetRandomCards(commonCards, 3);

        SetUpRewardCard(CardSpawn1.transform, selectedRewardCards[0]);
        SetUpRewardCard(CardSpawn2.transform, selectedRewardCards[1]);
        SetUpRewardCard(CardSpawn3.transform, selectedRewardCards[2]);
    }

    private void ClearSpawnPoints()
    {
        foreach (Transform child in CardSpawn1.transform) Destroy(child.gameObject);
        foreach (Transform child in CardSpawn2.transform) Destroy(child.gameObject);
        foreach (Transform child in CardSpawn3.transform) Destroy(child.gameObject);
    }
    //중복 없는 랜덤 카드 추출
    private List<CardData> GetRandomCards(List<CardData> sourceList, int count)
    {
        List<CardData> shuffled = new List<CardData>(sourceList);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            CardData temp = shuffled[randomIndex];
            shuffled[randomIndex] = shuffled[i];
            shuffled[i] = temp;
        }
        return shuffled.GetRange(0, count); //앞에서부터 3개만 짤라서 가져옴
    }
    // 공통 카드 생성 + 카드 선택 기능 추가
    private void SetUpRewardCard(Transform spawnPoint, CardData cardData)
    {
        GameObject instantiatedCard = Instantiate(slotCardPrefab, spawnPoint);

        SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
        if (slotCardUi != null)
        {
            slotCardUi.SetUp(cardData);
        }

        CardInteractionHandler interactionHandler = instantiatedCard.GetComponent<CardInteractionHandler>();
        if (interactionHandler != null)
        {
            interactionHandler.enabled = false;
        }
       
        EventTrigger trigger = instantiatedCard.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = instantiatedCard.AddComponent<EventTrigger>();
        }

        if (trigger.triggers == null)
        {
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;

        if (entry.callback == null)
        {
            entry.callback = new EventTrigger.TriggerEvent();
        }
        entry.callback.AddListener((data) => { OnCardSelected(cardData); });
        trigger.triggers.Add(entry);
    }
    public void OnCardSelected(CardData choseCardData)
    {
        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        if (string.IsNullOrEmpty(selectedCharId))
        {
            Debug.LogError("선택된 캐릭터가 없습니다.");
            return;
        }

        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        if (characterData == null)
        {
            Debug.LogError("캐릭터 데이터를 찾을 수 없습니다");
            return;
        }
        List<string> updatedCardList = new List<string>(characterData.Card ??  new string[0]);
        updatedCardList.Add(choseCardData.Id);
        characterData.Card = updatedCardList.ToArray();

        UiManager.Instance.CloseCardRewardStage();
    }
}