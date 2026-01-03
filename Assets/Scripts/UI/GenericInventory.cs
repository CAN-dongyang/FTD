using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory의 기능을 서술한 클래스.
/// View가 될 클래스를 정의하는 것만으로 사용이 가능하다.
///
/// View는 IGenericInventoryItem를 상속해야 한다.
/// </summary>
public class GenericInventory : MonoBehaviour
{
	[SerializeField] private Transform _contents;
	[SerializeField, Tooltip("prefab's active always false")]
	private GameObject _prefab;

	public readonly List<IGenericInventoryItem> Items;
	public virtual void OnChangedDatas(List<object> data) {}

	public void CreateItem(object data)
	{
		IGenericInventoryItem newItem;

		// 아이템이 부족하다면
		if(Items[Items.Count-1].gameObject.activeSelf)
		{
			GameObject g = Instantiate(_prefab.gameObject);
			g.transform.SetParent(_contents);
			g.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			g.SetActive(true);

			newItem = g.GetComponent<IGenericInventoryItem>();
			Items.Add(newItem);

			_nowEnd = Items.Count;
		}
		else (newItem = Items[_nowEnd]).gameObject.SetActive(true);

		newItem.SetData(data);
	}
	public void SetContents(List<object> newDatas)
	{
		_nowEnd = newDatas.Count;

		if(Items.Count < newDatas.Count) // inventory가 더 작다면
		{
			GameObject g;
			for(int i=Items.Count; i<newDatas.Count; i++)
			{
				g = Instantiate(_prefab.gameObject);
				g.transform.SetParent(_contents);
				g.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				g.SetActive(true);

				Items.Add(g.GetComponent<IGenericInventoryItem>());
			}
		}
		else if(Items.Count > _nowEnd) // inventory가 남는다면
			for(int i=_nowEnd; i<Items.Count; i++)
				Items[i].gameObject.SetActive(false); // 남는 gameObject 비활성화

		for(int i=0; i<newDatas.Count; i++)
			Items[i].SetData(newDatas[i]);

		OnChangedDatas(newDatas);
	}

	private void Awake()
	{
		if(_prefab) _prefab.gameObject.SetActive(false); // always false
	}
	private int _nowEnd = 0;
}