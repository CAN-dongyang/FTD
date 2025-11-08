using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

// 길찾기에 필요한 맵 정보를 담는 Blob Asset 구조체입니다.
public struct PathfindingGridData
{
    // Blob Asset은 NativeArray를 직접 가질 수 없으므로, BlobArray를 사용해야 합니다.
    public BlobArray<bool> Grid;
}

// 길찾기 시스템이 사용할 맵의 크기와 데이터에 대한 참조를 담는 싱글톤 컴포넌트입니다.
public struct PathfindingParams : IComponentData
{
    public int2 MapSize;
    public int2 GridOrigin; 
    public BlobAssetReference<PathfindingGridData> GridBlob;
}

// 맵 변환 시스템이 딱 한 번만 실행되도록 보장하는 태그입니다.
public struct MapConvertedTag : IComponentData { }

