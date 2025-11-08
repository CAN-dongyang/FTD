using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarUI : MonoBehaviour
{
	[SerializeField] private CanvasGroup _calendarView;
	[SerializeField] private CanvasGroup _scheduleView;
	[SerializeField] private Button _homeButton;
	private TextMeshProUGUI _homeButtonText;
	private bool _isCalendarView;

	public void ShowCalendar()
	{
		SetView(true);
	}
	public void ShowDaySchedule(int day)
	{
		SetView(false);
	}

	private void SetView(bool calendar)
	{
		_isCalendarView = calendar;

		_calendarView.alpha = _isCalendarView ? 1f : 0f;
		_calendarView.interactable = _calendarView.blocksRaycasts = _isCalendarView;

		_scheduleView.alpha = _isCalendarView ? 0f : 1f;
		_scheduleView.interactable = _scheduleView.blocksRaycasts = !_isCalendarView;

		_homeButtonText.text = _isCalendarView ? "Home" : "Calendar";
	}

	private void Awake()
	{
		_homeButton.onClick.AddListener(ShowCalendar);
		_homeButtonText = _homeButton.GetComponentInChildren<TextMeshProUGUI>();

		var buttons = _calendarView.GetComponentsInChildren<Button>();

		// Button Setting 더 있다고 치고
		for(int i=0; i<buttons.Length; i++)
		{
			buttons[i].onClick.RemoveAllListeners();
			buttons[i].onClick.AddListener(() => ShowDaySchedule(i));
		}

		ShowCalendar();
	}
}