using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public void Spawn()
    {
        GameObject prefab_Monster = Resources.Load<GameObject>("Prefabs/TestProject/Monster");

        if (prefab_Monster != null)
        {
            GameObject monster = Instantiate(prefab_Monster, transform.position, transform.rotation);
            monster.tag = "Monster";
            Debug.Log("몬스터가 나타났습니다!!");
        }

        else
        {
            Debug.LogError("몬스터를 찾을 수 없습니다. 몬스터가 출현하지 않습니다...");
        }
    }
}