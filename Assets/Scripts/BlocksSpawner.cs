using UnityEngine;

public class BlocksSpawner : MonoBehaviour
{
    public GameObject Prefab_Block;


    //private void OnEnable()
    //{
    //    Instantiate (Prefab_Block);
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpwanPrefab();
            Debug.Log("나무 블록이 떨어진다.");
        }
    }

    private void SpwanPrefab()
    {
        var balls = Instantiate(Prefab_Block);
        balls.name = "Blocks";
    }
}
