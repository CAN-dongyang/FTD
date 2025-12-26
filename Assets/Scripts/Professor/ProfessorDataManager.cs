using UnityEngine;
using System.Collections.Generic;
using System.IO; // 추가
using System.Linq; // HiredProfessors 재구성을 위해 추가

public class ProfessorDataManager : MonoBehaviour
{
    private static ProfessorDataManager _instance;

    public static ProfessorDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindObjectOfType<ProfessorDataManager>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject go = new GameObject("ProfessorDataManager");
                    _instance = go.AddComponent<ProfessorDataManager>();
                    Debug.Log("ProfessorDataManager instance was created automatically.");
                }
            }
            return _instance;
        }
    }

    public List<ProfessorData> AllProfessors { get; private set; }
    public List<ProfessorData> HiredProfessors { get; private set; } = new List<ProfessorData>();

    private const string SAVE_FILE_NAME = "hiredProfessors.json"; 
    
    // 개발 환경에서 테스트를 위해 Assets/Data 폴더에 저장/로드해야한다.
    // 빌드 시에는 Application.persistentDataPath를 사용해야한다 (C:\사용자\이름\folder 이런식).
    private string SaveFilePath => Application.dataPath + "/Data/" + SAVE_FILE_NAME;
    private string SaveDirectoryPath => Application.dataPath + "/Data/";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (AllProfessors == null || AllProfessors.Count == 0) LoadProfessorData();
        if (HiredProfessors == null || HiredProfessors.Count == 0) LoadHiredProfessors();
    }

    private void OnApplicationQuit()
    {
        SaveHiredProfessors();
    }

    private void LoadProfessorData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/professors");

        if (jsonFile != null)
        {
            ProfessorDataList professorDataList = JsonUtility.FromJson<ProfessorDataList>(jsonFile.text);
            AllProfessors = professorDataList.Professors;
        }
        else
        {
            Debug.LogError("professors.json not found in Resources/Data folder.");
            AllProfessors = new List<ProfessorData>();
        }
    }

    public ProfessorData GetProfessorById(int id)
    {
        if (AllProfessors == null) return null;
        return AllProfessors.Find(p => p.Id == id);
    }

    private void SaveHiredProfessors()
    {
        SaveData saveData = new SaveData();
        saveData.HiredProfessorIds = HiredProfessors.Select(p => p.Id).ToList();

        string json = JsonUtility.ToJson(saveData, true);
        
        try
        {
            if (!Directory.Exists(SaveDirectoryPath))
            {
                Directory.CreateDirectory(SaveDirectoryPath);
            }
            File.WriteAllText(SaveFilePath, json);

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save hired professors: {e.Message}");
        }
    }

    private void LoadHiredProfessors()
    {
        if (File.Exists(SaveFilePath))
        {
            try
            {
                string json = File.ReadAllText(SaveFilePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                HiredProfessors.Clear();
                foreach (int id in saveData.HiredProfessorIds)
                {
                    ProfessorData professor = GetProfessorById(id);
                    if (professor != null)
                    {
                        HiredProfessors.Add(professor);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load hired professors: {e.Message}");
                HiredProfessors.Clear();
            }
        }
        else
        {
            HiredProfessors.Clear();
        }
    }

    public bool HireProfessor(int id)
    {
        ProfessorData professor = GetProfessorById(id);
        if (professor == null)
        {
            Debug.LogError($"Cannot find professor with ID: {id}");
            return false;
        }

        if (HiredProfessors.Contains(professor))
        {
            Debug.Log($"Professor {professor.Name} is already hired.");
            return false;
        }

        HiredProfessors.Add(professor);
        Debug.Log($"Successfully hired professor: {professor.Name}");
        return true;
    }

    public bool FireProfessor(int id)
    {
        ProfessorData professorToRemove = HiredProfessors.Find(p => p.Id == id);
        if (professorToRemove == null)
        {
            Debug.LogError($"Cannot find hired professor with ID: {id}");
            return false;
        }

        HiredProfessors.Remove(professorToRemove);
        Debug.Log($"Successfully fired professor: {professorToRemove.Name}");
        return true;
    }
}