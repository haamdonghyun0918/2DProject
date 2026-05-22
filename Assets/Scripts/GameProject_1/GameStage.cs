using UnityEngine;
using UnityEngine.UI;

public class GameStage : UiBase
{
    [SerializeField] private Transform spawn_Character;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Image image_Map;
    private void OnEnable()
    {
        SpawnSelectedCharacter();
        GetMapImage();
    }
    private void SpawnSelectedCharacter()
    {
        foreach (Transform child in spawn_Character) Destroy(child.gameObject);

        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData charData = GameDataManager.Instance.GetCharacterData(charId);

        if (charData == null) return;
        
        GameObject charObj = Instantiate(characterPrefab, spawn_Character);
        GameCharacter characterComp = charObj.GetComponent<GameCharacter>();

        if (characterComp != null)
        {
            characterComp.SetUp(charData);
        }
        else
        {
            Debug.LogError("캐릭터 프리팹이 확인되지 않았습니다.");
        }
    }
    private void GetMapImage()
    {

    }
}