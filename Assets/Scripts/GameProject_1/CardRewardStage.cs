using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardRewardStage : UiBase
{
    [SerializeField] private GameObject CardSpawn1;
    [SerializeField] private GameObject CardSpawn2;
    [SerializeField] private GameObject CardSpawn3;
    [SerializeField] private GameObject slotCardPrefab;
    [SerializeField] private UiButton button_Continue;
    // 처음에 선택한 카드가 없기 때문에 null처리
    private CardData selectedCardData = null;
    // 새로운 카드를 리스트에 넣어서 게임 도중 계속 추가되어 사용할 수 있도록 함
    private List<GameObject> spawnedCardObjects = new List<GameObject>();
    private void OnEnable()
    {
        GenerateRewardCards();
        button_Continue.BindOnClickButtonEvent(OnClickContinue);
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
                if (!ownedCardIds.Contains(card.Id)) // 플레이어가 가지지 않은 카드인지 확인 아니라면 추가
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

        // 스폰되는 카드는 무조건 3장이 나오고 그 위치를 정해놓았기 때문에 생성
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
        //스폰된 오브젝트 리스트 비우기
        spawnedCardObjects.Clear();
        selectedCardData = null;
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
        spawnedCardObjects.Add(instantiatedCard);

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
       

        // 생성된 카드 프리팹에 EventTrigger를 넣어서 클릭했을 때 반응이 되도록 사용
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
        entry.callback.AddListener((data) => { OnCardSelected(cardData, instantiatedCard); });
        trigger.triggers.Add(entry);
    }

    // 선택한 카드 확인하게 하는 확대시키는 이펙트 메서드
    public void OnCardSelected(CardData choseCardData, GameObject cardObj)
    {
        selectedCardData = choseCardData;
        foreach (var obj in spawnedCardObjects)
        {
            if (obj == null) continue;
            obj.transform.DOScale(Vector3.one, 0.2f);
        }

        // 선택한 카드만 부드럽게 커지고 위로 올라가는 연출
        cardObj.transform.DOScale(Vector3.one * 1.15f, 0.2f);
    }
    // 계속하기 버튼을 누르면 그 카드가 추가되어 들어가도록 하는 메서드
    public void OnClickContinue()
    {
        if (selectedCardData == null)
        {
            Debug.LogWarning("카드를 선택하지 않았습니다");
            return;
        }
        string selectedCharId = UiManager.Instance.SelectedCharacterId;
        if (string.IsNullOrEmpty(selectedCharId)) return;

        CharacterData characterData = GameDataManager.Instance.GetCharacterData(selectedCharId);
        if (characterData == null) return;

        List<string> updatedCardList = new List<string>(characterData.Card ?? new string[0]);
        updatedCardList.Add(selectedCardData.Id);
        characterData.Card = updatedCardList.ToArray();
        UiManager.Instance.OpenClearPopUp();
        UiManager.Instance.CloseCardRewardStage();
    }
}