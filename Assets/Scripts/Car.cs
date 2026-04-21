using UnityEngine;

public class Car
{
    public string CarName { get; set; }
    public void Clarkson()
    {
        Debug.LogWarning("차량이 클라션을 울린다: 빵빵");
    }
}
