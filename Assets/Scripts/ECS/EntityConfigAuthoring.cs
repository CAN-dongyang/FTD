using Unity.Entities;
using UnityEngine;

// 인스펙터에서 프리팹을 연결할 Authoring 클래스 (MonoBehaviour)
public class EntityConfigAuthoring : MonoBehaviour
{
    public GameObject StudentPrefab;
    public GameObject ProfessorPrefab;

    // 3. 베이킹(Baking): GameObject 프리팹을 ECS Entity로 변환
    class Baker : Baker<EntityConfigAuthoring>
    {
        public override void Bake(EntityConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            AddComponent(entity, new EntityConfig
            {
                StudentPrefab = GetEntity(authoring.StudentPrefab, TransformUsageFlags.Dynamic),
                ProfessorPrefab = GetEntity(authoring.ProfessorPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}