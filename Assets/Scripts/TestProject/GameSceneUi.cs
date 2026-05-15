using UnityEngine;

public class GameSceneUi : UiBase
{
    private void OnEnable()
    {
        ResetAndSpawn();
    }
    public void ResetAndSpawn()
    {
        GameObject oldPlayer = GameObject.FindWithTag("Player");
        if (oldPlayer != null) Destroy(oldPlayer);

        GameObject[] oldMonsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject m in oldMonsters) Destroy(m);
        SpawnExplorer explorerSpawner = GetComponentInChildren<SpawnExplorer>();

        if (explorerSpawner != null)
        {
            explorerSpawner.Spawn();
        }
        else
        {
            Debug.LogError("GameSceneUi에서 SpawnExplorer를 찾을 수 없습니다.. 다시 확인해보세요");
        }

        SpawnMonster[] monsterSpawners = GetComponentsInChildren<SpawnMonster>();

        foreach (SpawnMonster spawner in monsterSpawners)
        {
            spawner.Spawn();
        }
        if (monsterSpawners == null)
        {
            Debug.LogError("GameSceneUi에서 SpawnMonster를 찾을 수 없습니다.. 다시 확인해보세요");
        }
        Debug.LogWarning("E버튼으로 앞으로 가며, R버튼을 누르면 방향을 바꾸고, A버튼을 누르면 공격하고, Space버튼을 누르면 점프합니다!");
        Debug.LogWarning("받은 데미지가 50이상이 되면 게임이 종료됩니다. 가시:1, 몬스터: 5입니다!!");
        Debug.LogWarning("장애물과 몬스터를 피해서 탈출문에 도달하여 이곳을 빠져나가세요!!");
    }
}