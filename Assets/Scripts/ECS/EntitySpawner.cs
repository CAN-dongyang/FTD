using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EntitySpawner : MonoBehaviour
{
    [Header("테스트 데이터")]
    // 인스펙터에서 테스트할 ID 리스트를 입력하세요.
    public List<int> inputDataList = new List<int> { 10, 55, 101, 200, 5 };
    
    // 생성 범위 (화면 크기)
    public float2 spawnRange = new float2(8, 4);

    void Start()
    {
        // 1. EntityManager 가져오기
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        // 2. 설정 데이터(AssignmentConfig) 찾기
        // (AssignmentComponents.cs에 정의된 struct를 찾습니다)
        EntityQuery query = em.CreateEntityQuery(typeof(EntityConfig));
        
        if (query.TryGetSingleton<EntityConfig>(out EntityConfig config))
        {
            Debug.Log("🚀 [EntitySpawner] Config 데이터 로드 성공! 생성을 시작합니다.");
            CreateEntities(inputDataList, config);
        }
        else
        {
            // 이 오류가 뜨면 'Config' 오브젝트가 SubScene에 없거나 베이킹이 안 된 것입니다.
            Debug.LogError("❌ [오류] EntityConfig 데이터를 찾을 수 없습니다! \n" +
                           "1. 'Config' 오브젝트에 'EntityConfigAuthoring' 스크립트가 있나요?\n" +
                           "2. 'Config' 오브젝트가 'SubScene' 안에 들어있나요?");
        }
    }

    public void CreateEntities(List<int> intList, EntityConfig config)
    {
        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;

        foreach (int value in intList)
        {
            Entity entity;

            // ---------------------------------------------------
            // [핵심 로직] 리스트 값에 따라 프리팹 복제 및 데이터 부착
            // ---------------------------------------------------
            if (value < 100)
            {
                // [학생]
                // 1. 프리팹 복제 (Instantiate)
                entity = em.Instantiate(config.StudentPrefab);
                
                // 2. 학생 데이터 컴포넌트 부착
                em.AddComponentData(entity, new StudentData { StudentID = value });
                
                // 3. (선택) 디버깅용 이름 부착
                em.AddComponentData(entity, new EntityNameData { Value = $"학생_{value}" });
                
                // 4. 위치 및 크기 설정 (학생은 약간 작게 0.5배)
                em.SetComponentData(entity, LocalTransform.FromPositionRotationScale(GetRandomPosition(), quaternion.identity, 0.5f));
                
                Debug.Log($"✅ 생성 완료: ID {value} (학생)");
            }
            else
            {
                // [교수]
                // 1. 프리팹 복제
                entity = em.Instantiate(config.ProfessorPrefab);
                
                // 2. 교수 데이터 컴포넌트 부착
                em.AddComponentData(entity, new ProfessorData { ProfessorID = value });
                
                // 3. (선택) 디버깅용 이름 부착
                em.AddComponentData(entity, new EntityNameData { Value = $"교수_{value}" });
                
                // 4. 위치 및 크기 설정 (교수는 기본 크기 0.8배)
                em.SetComponentData(entity, LocalTransform.FromPositionRotationScale(GetRandomPosition(), quaternion.identity, 0.8f));
                
                Debug.Log($"🎓 생성 완료: ID {value} (교수)");
            }
        }
    }

    // 랜덤 위치 계산 함수
    private float3 GetRandomPosition()
    {
        float x = UnityEngine.Random.Range(-spawnRange.x, spawnRange.x);
        float y = UnityEngine.Random.Range(-spawnRange.y, spawnRange.y);
        return new float3(x, y, 0);
    }
}