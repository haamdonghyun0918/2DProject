using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject Prefab_Ball;

    private void OnEnable()
    {
        // 1) 프리팹의 게임오브젝트 동적 생성 => new / Heap메모리
        Instantiate(Prefab_Ball);
    }
}
