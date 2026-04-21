using UnityEngine;

public class Car
{
    public string CarName { get; set; }
    public void Clarkson()
    {
        Debug.LogWarning("차량이 클라션을 울린다: 빵빵");
    }
    public void Move()
    {
        Debug.Log("차량이 움직입니다: 부릉부릉");
    }
    public void Cancel()
    {
        Debug.LogWarning("차량 상태가 매우 좋지 않아 이 차량의 계약을 잠시 해지해야 합니다. 그동안 다른 차량을 타야겠네요...");
    }
    public void BreakCar()
    {
        Debug.LogWarning("아마 이 차량은 폐차장으로 갈 것 같습니다.. 잘 있어라 차량아...");
    }
}
