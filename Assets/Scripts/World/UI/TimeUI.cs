using UnityEngine;
using TMPro;

public class TimeUI : UIGroup
{
	[SerializeField] private TextMeshProUGUI timeText;

	void LateUpdate()
	{
		int hour = WorldTime.Hour;
		string timePeriod;
		if (hour >= 24) { timePeriod = "새벽"; }       // 24:00 (자정) ~ 02:00
		else if (hour >= 17) { timePeriod = "저녁"; }  // 17:00 ~ 23:59
		else if (hour >= 12) { timePeriod = "오후"; }  // 12:00 ~ 16:59
		else if (hour >= 6) { timePeriod = "오전"; }   // 06:00 ~ 11:59
		else { timePeriod = "새벽"; }

		int hours12 = hour % 12;
		if (hours12 == 0) hours12 = 12; // 0시는 12시로 표시

		string timeString = $"{WorldTime.Year}년 {WorldTime.Quater}분기 {WorldTime.Day}일차\n";
		timeString += $"{timePeriod} {hours12:00}시 {WorldTime.Minutes:00}분 ({WorldTime.DayOfWeek})";

		// UI 텍스트를 업데이트합니다.
		timeText.text = timeString;
	}
}