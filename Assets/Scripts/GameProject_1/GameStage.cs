using UnityEngine;
using UnityEngine.UI;

public class GameStage : UiBase
{
    [SerializeField] private GameCharacter gameCharacter;
    [SerializeField] private Image image_Map;
    private void OnEnable()
    {
        SetupSelectedCharacter();
        GetMapImage();
    }
    private void SetupSelectedCharacter()
    {
        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData characterdata = GameDataManager.Instance.GetCharacterData(charId);

        if (characterdata == null) return;
        Debug.Log($"게임이 시작되었습니다!! 선택된 캐릭터는 {characterdata.Name}입니다!");

        if (gameCharacter != null)
        {
            gameCharacter.SetUp(characterdata);
        }
        else
        {
            Debug.LogError("캐릭터를 찾지 못했습니다!");
        }
    }
    private void GetMapImage()
    {

    }
}