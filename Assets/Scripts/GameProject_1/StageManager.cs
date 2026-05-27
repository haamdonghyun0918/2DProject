using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int currentStageNum = 1;
    public int highestClearedStage = 0;

    public int playerSavedHp = -1;

    public int[] stageResults = new int[7];

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
}