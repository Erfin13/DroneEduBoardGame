using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent;  // 人物的 NavMesh Agent
    public Camera mainCamera;   // 主攝影機

    void Start()
    {
        // 確保有NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 監聽滑鼠右鍵點擊，讓人物移動到點擊位置
        if (Input.GetMouseButtonDown(1))  // 右鍵
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);  // 設置行走目標
            }
        }
    }
}
