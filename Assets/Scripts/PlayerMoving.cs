using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    bool isStartMove = false;
    Vector3 moveDirection;
    float rotateValue;
    private void OnEnable()
    {
        Debug.Log("플레이어가 나타났습니다! 절대 공 아닙니다. 플레이어 입니다!");
    }
    private void Update()
    {
        if (isStartMove == false)
        {
            moveDirection = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            isStartMove = true;
            moveDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            isStartMove = true;
            moveDirection = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            isStartMove = true;
            moveDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            isStartMove = true;
            moveDirection = Vector3.right;
        }
        //else if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, -45.0f, 0));
        //}
        //else if (Input.GetKeyDown(KeyCode.E))
        //{
        //    this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 45.0f, 0));
        //}
        //rotateValue = (rotateValue + 45.0f);
        //this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        this.gameObject.transform.Translate(moveDirection * Time.deltaTime);
    }
}
