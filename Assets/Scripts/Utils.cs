using UnityEngine;
using UnityEngine.AI;

public static class Utils
{
    // 네브 메시 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서 랜덤한 위치를 찾는다.
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        // center를 중심으로 반지름이 maxDinstance인 구 안에서의 랜덤한 위치 하나를 저장
        // Random.insideUnitSphere는 반지름이 1인 구 안에서의 랜덤한 한 점을 반환하는 프로퍼티
        var randomPos = Random.insideUnitSphere * distance + center;

        // 네브 메시 샘플링의 결과 정보를 저장하는 변수
        NavMeshHit hit;

        // randomPos를 기준으로 maxDistance 반경 안에서, randomPos에 가장 가까운 네브 메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);

        // 찾은 점 반환
        return hit.position;
    }
}