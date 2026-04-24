using UnityEngine;

public class PlayerAnimal : MonoBehaviour
{
    float moveStep = 0.2f;
    float rotateValue = 0.0f;
    private float _rayCastMaxDist = 5.0f;
    void StartRaycast()
    {
        RaycastHit hit;

        bool isRaycasted = Physics.Raycast(this.transform.position, this.transform.forward, out hit, _rayCastMaxDist);
        if(isRaycasted)
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.CompareTag("Monster") == false)
                {
                    return;
                }
                Debug.Log($"감지된 물체 : {hit.collider.gameObject.name}");
                Destroy(hit.collider.gameObject);
                Debug.LogWarning($"{hit.collider.gameObject.name}이 사라졌습니다!!");
            }
            else
            {
                Debug.Log("올바르지 않은 상대입니다...");
            }
        }
        else
        {
            Debug.Log("감지되지 않음...");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, this.transform.forward * _rayCastMaxDist);
    }

    private void OnEnable()
    {
        Debug.Log("말은 플레이어입니다.");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartRaycast();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("앞으로 움직입니다");
            transform.Translate(Vector3.forward * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("뒤로 움직입니다");
            transform.Translate(Vector3.back * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("왼쪽으로 움직입니다");
            transform.Translate(Vector3.left * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("오른쪽으로 움직입니다");
            transform.Translate(Vector3.right * moveStep);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("왼쪽으로 회전");
            rotateValue = rotateValue - 15.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("오른쪽으로 회전");
            rotateValue = rotateValue + 15.0f;
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            var rigidBody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                Debug.Log("점프하고 있습니다.");
                rigidBody.AddForce(Vector3.up * 100.0f);
            }
        }
    }
}