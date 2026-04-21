using UnityEngine;

public class CarBehaviourScript : MonoBehaviour
{
    private Car carCode = new Car();
    private void Awake()
    {
        carCode.Clarkson();
        Debug.Log("이래 보여도 자동차 입니다.");
    }
}
