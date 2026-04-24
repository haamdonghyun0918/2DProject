using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject Prefab_Animal;
    private void SpawnAnimal()
    {
        var animal = Instantiate(Prefab_Animal);
        animal.name = "말";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAnimal();
            Debug.Log("플레이어가 생성되었습니다!!");
        }
    }
}
