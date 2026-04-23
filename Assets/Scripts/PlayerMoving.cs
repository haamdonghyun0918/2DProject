using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    bool isStartMove = false;
    Vector3 moveDirection;
    private void OnEnable()
    {
        Debug.Log("플레이어입니다. 절대 박스가 아닙니다. 플레이어입니다!");
    }
    private void Update()
    {
        if (isStartMove == false)
        {
            moveDirection = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("위로 움직입니다");
            isStartMove = true;
            moveDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("아래로 움직입니다");
            isStartMove = true;
            moveDirection = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("뒤로 움직입니다");
            isStartMove = true;
            moveDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("앞으로 움직입니다");
            isStartMove = true;
            moveDirection = Vector3.right;
        }
        this.gameObject.transform.Translate(moveDirection * Time.deltaTime);
    }
}
