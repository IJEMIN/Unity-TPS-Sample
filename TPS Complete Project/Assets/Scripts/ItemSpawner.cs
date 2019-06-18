using UnityEngine;
using UnityEngine.AI;

// 내비메쉬 관련 코드

// 주기적으로 아이템을 플레이어 근처에 생성하는 스크립트
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // 생성할 아이템들
    public Transform playerTransform; // 플레이어의 트랜스폼
    
    private float lastSpawnTime; // 마지막 생성 시점
    public float maxDistance = 5f; // 플레이어 위치로부터 아이템이 배치될 최대 반경
    
    private float timeBetSpawn; // 생성 간격

    public float timeBetSpawnMax = 7f; // 최대 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 시간 간격

    private void Start()
    {
        // 생성 간격과 마지막 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }


    // 주기적으로 아이템 생성 처리 실행
    private void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            lastSpawnTime = Time.time; // 마지막 생성 시간 갱신
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax); // 생성 주기를 랜덤으로 변경
            Spawn(); // 실제 아이템 생성
        }
    }

    // 실제 아이템 생성 처리
    private void Spawn()
    {
        // 플레이어 근처의 네브 메쉬위의 랜덤 위치를 가져옵니다.
        var spawnPosition = Utility.GetRandomPointOnNavMesh(playerTransform.position, maxDistance, NavMesh.AllAreas);
        spawnPosition += Vector3.up * 0.5f; // 바닥에서 0.5만큼 위로 올립니다.

        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 생성합니다.
        var item = Instantiate(items[Random.Range(0, items.Length)], spawnPosition, Quaternion.identity);
        // 생성된 아이템을 5초 뒤에 파괴
        Destroy(item, 5f);
    }
}