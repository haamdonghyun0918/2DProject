using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int currentStageNum = 1;
    public int highestClearedStage = 0;
    //최대 체력
    public int playerSavedHp = -1;

    public int[] stageResults = new int[7];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SetUpStage(GameStage stageView)
    {
        string mapId = $"Map_{currentStageNum:D2}";
        MapData mapData = GameDataManager.Instance.GetMapData(mapId);

        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData charData = GameDataManager.Instance.GetCharacterData(charId);

        if (mapData == null || charData == null) return;

        stageView.SetMapImage(mapData.MapImageAddress);
        GameCharacter player = stageView.SpawnPlayer(charData, playerSavedHp);
        List<GameMonster> monsters = stageView.SpawnMonsters(mapData.Monster);
        stageView.SpawnRandomCards();

        if (GameManager.Instance != null && player != null)
        {
            GameManager.Instance.StartBattle(player, monsters, stageView);
        }
    }
    public void SaveBattleResult(bool isWin, int remainingHp)
    {
        playerSavedHp = isWin ? remainingHp : -1;

        stageResults[currentStageNum] = isWin ? 1 : 2;

        if (isWin && currentStageNum > highestClearedStage)
        {
            highestClearedStage = currentStageNum;
        }
    }
    public void ResetStageData()
    {
        currentStageNum = 1;
        highestClearedStage = 0;
        playerSavedHp = -1;

        for (int i =0; i < stageResults.Length; i++)
        {
            stageResults[i] = 0;
        }
        Debug.Log("다시하기를 눌러 모든 것이 초기화 되었습니다!");
    }
}