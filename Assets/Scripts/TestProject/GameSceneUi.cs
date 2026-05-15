using UnityEngine;

public class GameSceneUi : UiBase
{
    private void Start()
    {
        SpawnExplorer explorerSpawner = GetComponentInChildren<SpawnExplorer>();
        
        if (explorerSpawner != null )
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
    }
}