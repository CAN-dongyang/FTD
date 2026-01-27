using System;
using System.Collections;
using UnityEngine;

public class School : MonoBehaviour
{
	[SerializeField] private SchoolData _data;
	[SerializeField] private Grid _gridExternal;
	[SerializeField] private Grid _gridInternal;
	[SerializeField] private Home _home;

	public static SchoolData Data => Instance._data;
	public static Grid GridExternal => Instance._gridExternal;
	public static Grid GridInternal => Instance._gridInternal;

	public Home Home => Instance._home;

	public void MoveIn()
	{
		if(Player.Instance) Player.Instance.Spawn(GridInternal, Vector3Int.zero);
	}
	public void MoveOut()
	{
		if(Player.Instance) Player.Instance.Spawn(GridExternal, Vector3Int.zero);
	}

	#region Routines
	IEnumerable DayStart()
	{
		// Home 에서 시작
		MoveIn();
		
		FadeInOut fade = FindAnyObjectByType<FadeInOut>();
		fade.OpenUI();

		World.Instance.Pause();
		fade.FadeIn();

		while(fade.IsShow)
			yield return null;
		
		World.Instance.Resume();
	}
	#endregion

	private void OnEnable()
	{
		if(World.Instance) World.Instance.OnDayStart.Add(DayStart);
	}
	private void OnDisable()
	{
		if(World.Instance) World.Instance.OnDayStart.Remove(DayStart);
	}

	#region Singleton
	private static School _instance = null;
	public static School Instance
	{
		get
		{
			if(!_instance) _instance = FindAnyObjectByType<School>();
			return _instance;
		}
	}
	#endregion
}