using Unity.Entities;

// 학생임을 표시하는 컴포넌트
public struct StudentData : IComponentData
{
    public int StudentID; // 리스트에서 받은 ID 값
}

// 교수임을 표시하는 컴포넌트
public struct ProfessorData : IComponentData
{
    public int ProfessorID; // 리스트에서 받은 ID 값
}

// (선택사항) 엔티티의 이름을 저장해 디버깅을 돕는 컴포넌트
public struct EntityNameData : IComponentData
{
    public Unity.Collections.FixedString64Bytes Value;
}

// 데이터를 저장할 컴포넌트 (ECS 전용)
public struct EntityConfig : IComponentData
{
    public Entity StudentPrefab;
    public Entity ProfessorPrefab;
}