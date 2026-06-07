using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStage : UiBase
{
    //캐릭터 로드
    [SerializeField] private Transform spawn_Character;
    [SerializeField] private GameObject characterPrefab;
    //맵 이미지 로드
    [SerializeField] private Image image_Map;
    //몬스터 로드
    [SerializeField] private Transform[] spawn_Monsters;
    [SerializeField] private GameObject monsterPrefab;
    //카드 로드
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private GameObject slotCardPrefab;

    //덱 연출
    [SerializeField] private Image image_Deck;
    [SerializeField] private UiButton button_Deck;
    [SerializeField] private Transform deckTransform;
    [SerializeField] private GameObject ui_CardInventory;

    [SerializeField] private Text text_TurnNum;

    //카드를 다시 뽑을 때를 대비하여 현재 캐릭터 데이터를 담는 변수
    private CharacterData currentCharacterData;

    private void OnEnable()
    {
        if (StageManager.Instance != null)
        {
            //스테이지 매니저에서 준 데이터를 바탕으로 몬스터랑 맵 데이터를 가져옴
            StageManager.Instance.SetUpStage(this);
        }
        button_Deck.BindOnClickButtonEvent(OnClickDeck);
    }

    public void OnClickDeck()
    {
        UiManager.Instance.OpenInventory();
    }

    public void SetMapImage(string mapImagePath)
    {
        image_Map.sprite = Resources.Load<Sprite>(mapImagePath);
    }
    public GameCharacter SpawnPlayer(CharacterData charData, int savedHp)
    {
        foreach (Transform child in spawn_Character) Destroy(child.gameObject);
        if (charData == null) return null;

        currentCharacterData = charData;

        GameObject charObj = Instantiate(characterPrefab, spawn_Character);
        GameCharacter characterComp = charObj.GetComponent<GameCharacter>();

        if (characterComp != null)
        {
            characterComp.SetUp(charData);
            if (savedHp != -1) characterComp.SetCurrentHp(savedHp);
        }
        return characterComp;
    }

    // 몬스터 스폰 위치 지정해주는 메서드 => 일단 스테이지 들어가면 모든 데이터 지우고 다시 호출
    public List<GameMonster> SpawnMonsters(string[] monsterIds)
    {
        List<GameMonster> spawnedMonsters = new List<GameMonster>();
        foreach(var spawn in spawn_Monsters)
        {
            foreach (Transform child in spawn) Destroy(child.gameObject);
        }
        if (monsterIds == null || monsterIds.Length == 0) return spawnedMonsters;

        int count = monsterIds.Length;
        
        switch (count)
        {
            case 1:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[1]));
                break;
            case 2:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[1], spawn_Monsters[2]));
                break;
            case 3:
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[0], spawn_Monsters[0]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[1], spawn_Monsters[1]));
                spawnedMonsters.Add(SpawnSingleMonster(monsterIds[2], spawn_Monsters[2]));
                break;
        }

        for (int i = spawnedMonsters.Count -1; i >= 0; i--)
        {
            if (spawnedMonsters[i] == null)
            {
                spawnedMonsters.RemoveAt(i);
            }
        }
        return spawnedMonsters;
    }

    // 몬스터 각 한마리의 배치 메서드
    private GameMonster SpawnSingleMonster(string monsterIds, Transform spawnPoint)
    {
        MonsterData mData = GameDataManager.Instance.GetMonsterData(monsterIds);
        if (mData == null) return null;

        GameObject mObj = Instantiate(monsterPrefab, spawnPoint);
        GameMonster mComp = mObj.GetComponent<GameMonster>();
        
        if (mComp != null) mComp.SetUp(mData);

        return mComp;
    }

    // 랜덤으로 카드 3장 스폰하는 함수
    public void SpawnRandomCards()
    {
        if (cardSpawnPoint == null || currentCharacterData == null || currentCharacterData.Card == null) return;

        foreach (Transform child in cardSpawnPoint) Destroy(child.gameObject);

        SetUpDeckImage();

        for (int i = 0; i < 3; i++)
        {
            SpawnInitialCard();
        }
    }

    public GameMonster SpawnBossMonster(string bossId)
    {
        foreach (Transform child in spawn_Monsters[1]) Destroy(child.gameObject);
        GameMonster boss = SpawnSingleMonster(bossId, spawn_Monsters[1]);

        if (boss != null)
        {
            boss.FlipBoss();
        }
        return boss;
    }

    // 카드 드로우했을 때의 새로 들어오는 카드의 스폰되는 함수
    public void SpawnInitialCard()
    {
        int randomIndex = Random.Range(0, currentCharacterData.Card.Length);
        string randomCardId = currentCharacterData.Card[randomIndex];
        CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);

        if (cardData != null)
        {
            GameObject instantiatedCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi slotCardUi = instantiatedCard.GetComponent<SlotCardUi>();
            if (slotCardUi != null) slotCardUi.SetUp(cardData);

            CanvasGroup cg = instantiatedCard.GetComponent<CanvasGroup>();
            if (cg == null) cg = instantiatedCard.AddComponent<CanvasGroup>();
            cg.alpha = 1f;

            instantiatedCard.transform.localScale = Vector3.one;
        }
    }

    // 카드 덱 이미지 함수로 카드 이미지를 가져옴
    public void SetUpDeckImage()
    {
        if (image_Deck == null || currentCharacterData.Card.Length == 0) return;

        string firstCardId = currentCharacterData.Card[0];
        CardData firstCardData = GameDataManager.Instance.GetCardData(firstCardId);

        if (firstCardData != null)
        {
            Sprite deckSprite = Resources.Load<Sprite>(firstCardData.ImageCardAddress);
            if (deckSprite != null)
            {
                image_Deck.sprite = deckSprite;
            }
        }
    }

    // 카드를 사용하고 난 뒤에 다시 카드를 랜덤적으로 한 장 가져오는 함수
    public void RefillUsedCard()
    {
        if (cardSpawnPoint == null || currentCharacterData == null || currentCharacterData.Card == null) return;

        int randomIndex = Random.Range(0, currentCharacterData.Card.Length);
        string randomCardId = currentCharacterData.Card[randomIndex];
        CardData cardData = GameDataManager.Instance.GetCardData(randomCardId);

        if (cardData != null)
        {
            // 진짜 카드를 카드 위치에 만들지만 Vector3.zero를 통하여 보이지 않게 숨긴다.
            GameObject realCard = Instantiate(slotCardPrefab, cardSpawnPoint);
            SlotCardUi realSlotUi = realCard.GetComponent<SlotCardUi>();
            if (realSlotUi != null) realSlotUi.SetUp(cardData);
            realCard.transform.DOKill();
            realCard.transform.localScale = Vector3.zero;

            if (deckTransform != null)
            {
                // 카드를 새로 뽑는 3번째 카드위치를 계산하게끔 만드는 코드 그 후, 받아온 위치를 저장한다.
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(cardSpawnPoint.GetComponent<RectTransform>());
                Vector3 targetPos = realCard.transform.position;

                //부모를 Content가 아닌 현재 UI창(this.transform)으로 빼서 Layout Group의 간섭을 완벽히 차단합니다!
                GameObject fakeCard = Instantiate(slotCardPrefab, this.transform);
                SlotCardUi fakeSlotUi = fakeCard.GetComponent<SlotCardUi>();
                if (fakeSlotUi != null) fakeSlotUi.SetUp(cardData);

                // 덱에서 카드를 뽑는 이미지를 가짜 카드로 하여 날아다니는 모션만 하게 하는데, 그래서 드래그 및 상호작용은 꺼놓는다.
                fakeCard.transform.DOKill();
                CardInteractionHandler fakeInteraction = fakeCard.GetComponent<CardInteractionHandler>();
                if (fakeInteraction != null) fakeInteraction.enabled = false;

                // DOTween을 통하여 날아가는 느낌을 주는 메서드
                fakeCard.transform.position = deckTransform.position;
                fakeCard.transform.localScale = Vector3.one * 0.2f; // 덱에서 작게 시작

                fakeCard.transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutCubic);
                fakeCard.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);
                StartCoroutine(CardDrawCompleteRoutine(fakeCard, realCard));
            }
            else
            {
                // 덱 이미지가 없을 경우를 대비한 안전 장치
                realCard.transform.localScale = Vector3.one;
            }
        }
    }

    // 그 후 가짜 카드를 삭제하는 코루틴을 만들어서 가짜 카드가 사라지는 순간 진짜 카드가 나타나게 대체해주는 메서드
    private IEnumerator CardDrawCompleteRoutine(GameObject fakeCard, GameObject realCard)
    {
        yield return new WaitForSeconds(0.5f);

        if (fakeCard != null)
        {
            Destroy(fakeCard);
        }

        if (realCard != null)
        {
            realCard.transform.localScale = Vector3.one;
            realCard.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0.0f), 0.2f);
        }
    }

    public void UpdateTurnText(int turnNum)
    {
        if (text_TurnNum != null)
        {
            // 인스펙터에 있는 Text_Turn(NUM)에 숫자를 덮어씌운다
            text_TurnNum.text = turnNum.ToString();
        }
    }
}