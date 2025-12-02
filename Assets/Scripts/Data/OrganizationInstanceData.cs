using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class OrganizationInstanceData : InstanceData
{
	[Header("Organization")]
	public List<DataID> owner_ids;
	public List<DataID> member_ids;
	public List<DataID> activity_ids;

	#region Room
	/// <summary>
	/// 
	/// </summary>
	public List<Room> rooms;
	[Serializable]
	public struct Room
	{
		public Tile tile;
		public BoundsInt bounds;
	}
	#endregion

	#region Schedule
	/// <summary>
	/// <MemberID, ActivityID> Members Schedules
	/// </summary>
	public List<MemberSchedule> schedules;

	[Serializable]
	public struct MemberSchedule
	{
		public DataID member_id;
		public List<DataID> activitys;
	}
	#endregion

	public OrganizationInstanceData(DataAsset asset, DataType type) : base(asset, type) {}
}