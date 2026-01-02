using System;
using System.Collections.Generic;

[Serializable]
public class ProfessorData
{
    public int Id; // 교수의 고유 ID (int로 변경)
    public string Name; // 교수의 이름
    public string Major; // 전공 분야
    public int Salary; // 고용 비용
    public string Description; // 교수에 대한 설명
    public List<int> DispositionIds; // 교수의 성향(Disposition) ID 리스트

    // JSON 로드를 위한 기본 생성자
    public ProfessorData() { }

    public ProfessorData(int id, string name, string major, int salary, string description, List<int> dispositionIds)
    {
        Id = id;
        Name = name;
        Major = major;
        Salary = salary;
        Description = description;
        DispositionIds = dispositionIds;
    }
}

[Serializable]
public class ProfessorDataList
{
    public List<ProfessorData> Professors;
}