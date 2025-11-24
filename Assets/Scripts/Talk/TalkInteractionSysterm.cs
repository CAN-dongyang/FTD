using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem; 

// 상호작용 가능한 엔티티에 붙일 컴포넌트들 (기존 유지)
public struct InteractableTag : IComponentData { }
public struct TalkIDData : IComponentData { public int Value; }


public partial class TalkInteractionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // F키 입력 확인
        var keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.fKey.wasPressedThisFrame) return;

        // 1. 플레이어 위치 찾기 (SystemAPI.Query 사용)
        float3 playerPos = float3.zero;
        bool playerFound = false;

        // RefRO<T>: 읽기 전용 접근 (Read Only)
        // WithAll<PlayerTag>()로 필터링
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            playerPos = transform.ValueRO.Position;
            playerFound = true;
            break; // 플레이어가 한 명이라고 가정하고 찾으면 루프 종료
        }

        if (!playerFound) return;

        // 2. 가장 가까운 NPC 찾기
        Entity closestEntity = Entity.Null;
        float closestDist = 2.0f; // 상호작용 가능 거리
        int targetTalkID = -1;

        // WithEntityAccess()를 통해 엔티티 정보(Entity ID)도 함께 가져옴
        foreach (var (transform, talkID, entity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<TalkIDData>>()
                                                        .WithAll<InteractableTag>()
                                                        .WithEntityAccess())
        {
            float dist = math.distance(playerPos, transform.ValueRO.Position);
            
            if (dist <= closestDist)
            {
                closestDist = dist;
                closestEntity = entity;
                targetTalkID = talkID.ValueRO.Value;
            }
        }

        // 3. 대화 시작
        if (closestEntity != Entity.Null)
        {
            Debug.Log($"[Interaction] NPC와 대화 시작 (ID: {targetTalkID})");
            
            // 메인 스레드에서 UI 호출
            if (TalkUIController.Instance != null)
            {
                // NPC 이름은 데이터를 따로 관리하거나 컴포넌트에서 가져와야 하지만, 일단 "NPC"로 넘깁니다.
                TalkUIController.Instance.ShowTalk(targetTalkID, "NPC");
            }
        }
    }
}