using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class HiredProfessorListUI : MonoBehaviour
{
    public static HiredProfessorListUI Instance { get; private set; }

    [Header("UI Elements")]
    public Transform contentParent; 
    public GameObject professorListItemPrefab; 
    public ProfessorHirePopupUI hirePopupUI; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (ProfessorDataManager.Instance == null)
        {
            return;
        }

        var hiredProfessors = ProfessorDataManager.Instance.HiredProfessors;

        if (professorListItemPrefab == null || contentParent == null)
        {
            return;
        }

        foreach (var professorData in hiredProfessors)
        {
            GameObject newItem = Instantiate(professorListItemPrefab, contentParent);
            if (newItem == null) continue;
            
            ProfessorSlotUI slotUI = newItem.GetComponent<ProfessorSlotUI>();
            if (slotUI != null)
            {
                slotUI.Setup(professorData, OnSlotClicked, true); 
            }
        }
    }

    private void OnSlotClicked(ProfessorData data)
    {
        if (hirePopupUI != null)
        {
            hirePopupUI.Open(data);
        }
    }
}
