using UnityEngine;

/// <summary>
/// SetData(object)를 통해 View 내용을 채울 수 있다.
/// </summary>
public abstract class IGenericInventoryItem : MonoBehaviour
{
	abstract public void SetData(object data);
}