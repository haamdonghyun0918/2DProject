using System;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject Prefab_Ball;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpwanPrefab();
            Debug.Log("공이 생성되었습니다.. 중력에 의해 떨어집니다.");
        }

    }

    private void SpwanPrefab()
    {
        // 1) 프리팹의 게임오브젝트 동적 생성 => new / Heap메모리
        var balls = Instantiate(Prefab_Ball);
        balls.name = "Ball";
        //balls.transform.SetParent(Root_ball);
    }
}
