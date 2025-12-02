using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityInstanceData : InstanceData
{
	[Header("Activity")]
	public int startTime = 0;
	public int endTime = 0;
	public DataID room;

	public ActivityInstanceData(DataAsset asset, DataType type) : base(asset, type) {}
}

[Serializable]
public class LessonInstanceData : ActivityInstanceData
{
	[Header("Lesson")]
	public DataID professor;
	public List<DataID> students;

	public void OnCompleted()
	{
		students.ForEach(stu =>
		{
			// 1. 성향 보너스 % 계산 (데이터 값을 배율이 아닌 보너스 비율로 직접 사용)
			float synergyBonusRate = 0.0f;
			


			// 먼저 현재 스탯 객체와 값을 가져옵니다.
			Stat targetStat = new();

		float currentStatValue = targetStat.Value;
		
		// 2. 적성 보너스 계산 (별도 메서드에서 처리)
		float aptitudeBonusRate = 0.0f; // statType, student.Aptitudes

		// 3. 모든 보너스를 합산
		float totalBonusRate = synergyBonusRate + aptitudeBonusRate;

		// 4. 최종 증가량 계산 (현재 스탯 값 * 총 보너스 비율)
		float finalIncreaseFloat = currentStatValue * totalBonusRate;

		// 5. 계산된 값을 스탯에 추가
		targetStat.Value += finalIncreaseFloat;

		// 로그 기록
		// log.AppendLine($"{statType} 스탯 증가: {finalIncreaseFloat:F2} (성향: {synergyBonusRate * 100:F0}%apts | 현재 값: {Mathf.FloorToInt(targetStat.Value)}");
		});
	}

	public LessonInstanceData(DataAsset asset, DataType type) : base(asset, type) {}
}