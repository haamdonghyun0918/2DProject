using UnityEngine;

public class SpawnExplorer : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public void Spawn()
    {
        GameObject prefab_Character = Resources.Load<GameObject>("Prefabs/TestProject/Character");
        if (prefab_Character != null)
        {
            GameObject explorer = Instantiate(prefab_Character, transform.position, transform.rotation);
            explorer.tag = "Explorer_Character";
            Debug.Log("탐험가 등장!");
        }
        else
        {
            Debug.LogError("탐험가를 찾을 수 없습니다. 등장하지 않습니다.");
        }
    }
}