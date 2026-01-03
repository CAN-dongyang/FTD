using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRegistry", menuName = "DataSystem/AssetRegistry")]
public class AssetRegistry : ScriptableObject
{
    public AssetType registryType;
    
    [Header("Registered Assets")]
    // 이 리스트의 순서나 포함 여부가 곧 ai(Asset Index)를 결정합니다.
    public List<BaseAssetSO> assets = new List<BaseAssetSO>();

    // 인스펙터에서 값이 바뀌거나 버튼을 눌렀을 때 실행
    public void SyncIndices()
    {
        if (assets == null) return;

        for (int i = 0; i < assets.Count; i++)
        {
            if (assets[i] != null)
            {
                // 각 에셋에 aa와 ai를 주입
                // 리스트의 인덱스 i가 곧 ai가 됩니다.
                assets[i].SetAddress(registryType, (byte)i);
            }
        }
        Debug.Log($"[AssetRegistry] {registryType} 동기화 완료. ({assets.Count} 개)");
    }

    private void OnValidate()
    {
        // 에디터에서 리스트 구성이 바뀌면 자동으로 ID를 재계산합니다.
        SyncIndices();
    }
}