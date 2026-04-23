using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    float moveStep = 0.5f;
    float rotateValue = 0.0f;
    private void OnEnable()
    {
        Debug.Log("플레이어입니다. 절대 박스가 아닙니다. 플레이어입니다!");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("위로 움직입니다");
            transform.Translate(Vector3.forward * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("아래로 움직입니다");
            transform.Translate(Vector3.back * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("뒤로 움직입니다");
            transform.Translate(Vector3.left * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("앞으로 움직입니다");
            transform.Translate(Vector3.right * moveStep);
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("왼쪽으로 회전");
            rotateValue = rotateValue - 45.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("오른쪽으로 회전");
            rotateValue = rotateValue + 45.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            var rigidBody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                Debug.Log("점프하고 있습니다.");
                rigidBody.AddForce(Vector3.up * 200.0f);
            }
        }
    }
}