using Unity.Entities;
using UnityEngine;

public class TalkTestAuthoring : MonoBehaviour
{
    public bool IsPlayer = false;
    public bool IsNPC = false;
    public int TalkID = 0; // NPC일 경우 사용할 대화 ID

    class Baker : Baker<TalkTestAuthoring>
    {
        public override void Bake(TalkTestAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            if (authoring.IsPlayer)
            {
                AddComponent<PlayerTag>(entity);
                AddComponent(entity, new MoveSpeedComponent { Value = 5f }); // 이동 속도
            }

            if (authoring.IsNPC)
            {
                AddComponent<InteractableTag>(entity);
                AddComponent(entity, new TalkIDData { Value = authoring.TalkID });
            }
        }
    }
}