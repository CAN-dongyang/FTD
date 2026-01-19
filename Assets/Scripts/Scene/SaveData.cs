using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
	public int Chapter;
	public int Year;

    public List<int> HiredProfessorIds; // 고용된 교수의 ID 리스트


	public void Save()
	{
	}

    public SaveData()
    {
        HiredProfessorIds = new List<int>();
    }
}