using UnityEngine;

public class BeforeGameStartUi : UiBase
{
    [SerializeField] private UiButton button_GameStart;

    private void OnEnable()
    {
        button_GameStart.BindOnClickButtonEvent(GetStartGame);
    }
    public void GetStartGame()
    {
        // 선택된 캐릭터 Id를 UiManager에 있는 SlectedCharacterId를 가져와서 받아옴
        string slectedCharId = UiManager.Instance.SelectedCharacterId;
        
        // 캐릭터 데이터를 가져옴 (이름, 직업, Hp, Card 까지)
        CharacterData characterData = GameDataManager.Instance.GetCharacterData(slectedCharId);
        if (characterData != null)
        {
            Debug.Log($"선택한 캐릭터 Id: {characterData.Id}, 선택한 캐릭터 이름: {characterData.Name}");
            if (characterData.Card != null && characterData.Card.Length > 0)
            {
                // 카드를 모두 가져오는 반복문
                foreach (string cardId in characterData.Card)
                {
                    CardData cardData = GameDataManager.Instance.GetCardData(cardId);

                    if (cardData != null)
                    {
                        Debug.Log($"카드 정보 ID: {cardData.Id}, 카드 이름: {cardData.Name}, 카드 설명: {cardData.Description}, 카드 데미지: {cardData.Damage}");

                    }
                    else
                    {
                        Debug.LogWarning("위험하다!!! 카드 못찾음!!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("이 캐릭터는 카드를 갖고 있지 않습니다.");
            }
        }
        else
        {
            Debug.LogError($"{slectedCharId}의 데이터를 찾을 수 없습니다.");
        }
        UiManager.Instance.OpenGameMainScene();
        UiManager.Instance.CloseGameStartUi();
        UiManager.Instance.CloseBeforeGameStartUi();
    }
}