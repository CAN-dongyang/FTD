using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Data Manager", menuName="Data Manager")]
public class DataManager : ScriptableObject
{
	[SerializeReference]
	[SerializeField]
	private List<IData> _datas;
	private Dictionary<DataID, IData> Datas = new();
	private Dictionary<DataID, uint> CacheCount = new();
	
	public IData Get(DataID id)
	{
		return null;
	}


	private IData GetData(DataID id)
	{
		bool isAsset = id.IsAsset;
		
		if(isAsset)
		{
			
		}
		return null;
	}
}