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
	[Tooltip("prefab's active always false")]
	[SerializeField] private IGenericInventoryItem _prefab;
	[SerializeField] private List<IGenericInventoryItem> _items;
	private int _nowEnd = 0;

	public virtual void OnChangedDatas(List<object> data) {}

	public void CreateItem()
	{
		IGenericInventoryItem newItem;

		// 아이템이 부족하다면
		if(_items[_items.Count-1].gameObject.activeSelf)
		{
			GameObject g = Instantiate(_prefab.gameObject);
			g.transform.SetParent(_contents);
			g.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			g.SetActive(true);

			newItem = g.GetComponent<IGenericInventoryItem>();
			_items.Add(newItem);

			_nowEnd = _items.Count;
		}
		else (newItem = _items[_nowEnd]).gameObject.SetActive(true);

		// newItem.SetData(
	}
	
	public void SetContents(List<object> newDatas)
	{
		_nowEnd = newDatas.Count;

		if(_items.Count < newDatas.Count) // inventory가 더 작다면
		{
			GameObject g;
			for(int i=_items.Count; i<newDatas.Count; i++)
			{
				g = Instantiate(_prefab.gameObject);
				g.transform.SetParent(_contents);
				g.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				g.SetActive(true);

				_items.Add(g.GetComponent<IGenericInventoryItem>());
			}
		}
		else if(_items.Count > _nowEnd) // inventory가 남는다면
			for(int i=_nowEnd; i<_items.Count; i++)
				_items[i].gameObject.SetActive(false); // 남는 gameObject 비활성화

		for(int i=0; i<newDatas.Count; i++)
		{
			_items[i].SetData(newDatas[i]);
		}

		OnChangedDatas(newDatas);
	}

	private void Awake() => _prefab.gameObject.SetActive(false); // always false
}