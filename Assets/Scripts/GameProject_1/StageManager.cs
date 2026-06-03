using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    // Manager역할을 하기 때문에 싱글톤 패턴을 사용
    public static StageManager Instance { get; private set; }

    public int currentStageNum = 1; // 현재 유저가 도전 중인 스테이지
    public int highestClearedStage = 0; // 여태까지 깬 스테이지
    public int playerSavedHp = -1; // 유저의 스테이지 후의 남은 체력 => '-1'은 최대 체력을 의미
    public int[] stageResults = new int[7]; // 1에서 6 까지의 스테이지 결과를 배열로 지정 => (0: 잠금, 1: 성공, 2: 실패)

    public int totalAccumulatedTurns = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetUpStage(GameStage stageView)
    {
        // currentStageNum이 1이면 "Map_01"이라는 문자열을 만들어낸다 => (:D2가 두자리 숫자로 고정하라는 의미)
        string mapId = $"Map_{currentStageNum:D2}";
        MapData mapData = GameDataManager.Instance.GetMapData(mapId);

        string charId = UiManager.Instance.SelectedCharacterId;
        CharacterData charData = GameDataManager.Instance.GetCharacterData(charId);

        if (mapData == null || charData == null) return;

        // 데이터 드리븐으로 가져온 Map.json과 GameCharacter와 GameMonster 프리팹의 데이터들을 가져와서 시각화하는 과정
        stageView.SetMapImage(mapData.MapImageAddress);
        GameCharacter player = stageView.SpawnPlayer(charData, playerSavedHp);
        List<GameMonster> monsters = stageView.SpawnMonsters(mapData.Monster);
        // 카드를 랜덤적으로 가져와서 시각화하는 과정
        stageView.SpawnRandomCards();

        // 실제 전투 시작으로 GameManager에게 넘겨줍니다.
        if (GameManager.Instance != null && player != null)
        {
            GameManager.Instance.StartBattle(player, monsters, stageView);
        }
    }
    // 전투 결과를 이겼는지 졌는지를 통하여 GameMainScene과 연동하여 스테이지를 관리하는 메서드
    public void SaveBattleResult(bool isWin, int remainingHp)
    {
        playerSavedHp = isWin ? remainingHp : -1;

        stageResults[currentStageNum] = isWin ? 1 : 2;

        if (isWin && currentStageNum > highestClearedStage)
        {
            highestClearedStage = currentStageNum;
        }
    }
    // 다시하기 버튼을 눌렀을 때, 같은 캐릭터로 하는 경우 전에 했던 데이터가 남을 수 있으므로 초기화해주는 메서드
    public void ResetStageData()
    {
        currentStageNum = 1;
        highestClearedStage = 0;
        playerSavedHp = -1;
        totalAccumulatedTurns = 0;

        for (int i =0; i < stageResults.Length; i++)
        {
            stageResults[i] = 0;
        }
        Debug.Log("다시하기를 눌러 모든 것이 초기화 되었습니다!");
    }

    public void AddStageTurns(int turns)
    {
        totalAccumulatedTurns += turns;
    }
}