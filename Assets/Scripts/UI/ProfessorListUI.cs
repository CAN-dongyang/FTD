using UnityEngine;
using TMPro;
using System.Linq;

    public class ProfessorListUI : MonoBehaviour
    {
        public static ProfessorListUI Instance { get; private set; }

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

        var availableProfessors = ProfessorDataManager.Instance.AllProfessors
                                                .Where(p => !ProfessorDataManager.Instance.HiredProfessors.Any(hp => hp.Id == p.Id))
                                                .ToList();

        if (professorListItemPrefab == null || contentParent == null)
        {
            return;
        }

        foreach (var professorData in availableProfessors)
        {
            GameObject newItem = Instantiate(professorListItemPrefab, contentParent);
            if (newItem == null) continue;
            
            ProfessorSlotUI slotUI = newItem.GetComponent<ProfessorSlotUI>();
            if (slotUI != null)
            {
                slotUI.Setup(professorData, OnSlotClicked, false);
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
