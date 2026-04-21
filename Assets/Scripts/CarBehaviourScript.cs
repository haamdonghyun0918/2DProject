using UnityEngine;

public class CarBehaviourScript : MonoBehaviour
{
    private Car carCode = new Car();
    bool _isFirstFixedUpdate = false;
    bool _isFirstUpdate = false;
    bool _isFirstLateUpdate = false;
    private void Awake()
    {
        Debug.Log("이래 보여도 자동차 입니다.");
    }
    private void OnEnable()
    {
        carCode.Clarkson();
    }
    private void Start()
    {
        carCode.Move();
    }

    private void FixedUpdate()
    {
        if(_isFirstFixedUpdate == true)
        {
            return;
        }

        _isFirstFixedUpdate = true;
        Debug.Log("차량에서 연기가 납니다.. 내려서 조치를 취해야겠습니다.");
    }
    private void Update()
    {
        if(_isFirstUpdate == true)
        {
            return;
        }

        _isFirstUpdate = true;
        Debug.Log("큰 문제는 아니고 과열이 심하게 된 것 같습니다.. 연료도 부족하고요..");
    }
    private void LateUpdate()
    {
        if(_isFirstLateUpdate == true)
        {
            return;
        }

        _isFirstLateUpdate = true;
        Debug.Log("보험사를 불러 빠르게 해결하려 했는데, 뭔가 이상합니다.");
    }
    private void OnDisable()
    {
        carCode.Cancel();
    }
    private void OnDestroy()
    {
        carCode.BreakCar();
    }
}
