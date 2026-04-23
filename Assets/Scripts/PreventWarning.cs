using UnityEngine;

public class PreventWarning : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogWarning("앞에 장애물이 있습니다.");
    }
}
