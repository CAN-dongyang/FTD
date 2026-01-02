/// <summary>
/// 데이터등록번호(int)에 대한 타입 감별자.
/// 
/// <para>aa nn ii nn<br/>
/// <c>DataID</c>와 직접 비교가 가능하다.
/// </para>
/// </summary>
public enum DataType : uint
{
	None = 0, // Invalid
	Max = 0xFFFFFFFF, // UInt32 overflow (4 byte)
}