using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConstructionInfo
{

	public struct RoomData
	{
		public RoomAsset asset;
		public Vector3Int[] cells;

		public RoomData(RoomAsset asset, IEnumerable<Vector3Int> cells)
		{
			this.asset = asset;

			int count = cells.Count();
			this.cells = new Vector3Int[count];
			for(int i=0; i<count; i++)
				this.cells[i] = cells.ElementAt(i);
		}
	}
}