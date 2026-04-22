using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject Prefab_Ball;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpwanPrefab();
        }

    }

    private void SpwanPrefab()
    {
        // 1) 프리팹의 게임오브젝트 동적 생성 => new / Heap메모리
        Instantiate(Prefab_Ball);
    }
}
