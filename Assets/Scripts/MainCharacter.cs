using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    bool isStartMove = false;
    Vector3 moveDirection;
    float rotateValue = 0.0f;
    private float _rayCastMaxDist = 10.0f;
    void StartRaycast()
    {
        RaycastHit hit;

        bool isRaycasted = Physics.Raycast(this.transform.position, this.transform.forward, out hit, _rayCastMaxDist);
        if(isRaycasted)
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Enemy") == false)
                {
                    return;
                }
                Debug.Log($"감지된 적 : {hit.collider.gameObject.name}");
                Destroy(hit.collider.gameObject);
                Debug.Log($"{hit.collider.gameObject.name}(이/가) 공격받아 쓰러졌습니다.");
            }
            else
            {
                Debug.Log("적이 아닙니다..");
            }
        }
        else
        {
            Debug.Log("적이 감지되지 않았습니다.");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, this.transform.forward * _rayCastMaxDist);
    }
    private void OnEnable()
    {
        Debug.LogWarning("플레이어(메인 캐릭터)가 나타났습니다!");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartRaycast();
            Debug.Log("플레이어가 총을 쏩니다. 탕탕!!");
        }
        if(isStartMove == false)
        {
            moveDirection = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("앞으로 이동합니다.");
            isStartMove = true;
            moveDirection = Vector3.forward;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("뒤로 이동합니다.");
            isStartMove = true;
            moveDirection = Vector3.back;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("오른쪽으로 이동합니다.");
            isStartMove = true;
            moveDirection = Vector3.right;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("왼쪽으로 이동합니다.");
            isStartMove = true;
            moveDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("왼쪽으로 회전합니다.");
            rotateValue = rotateValue - 15.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("오른쪽으로 회전합니다.");
            rotateValue = rotateValue + 15.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            var rigidBody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                Debug.Log("점프합니다.");
                rigidBody.AddForce(Vector3.up * 200.0f);
            }
        }
        this.gameObject.transform.Translate(moveDirection * Time.deltaTime);
    }
}