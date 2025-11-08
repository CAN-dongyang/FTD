
using UnityEngine;
using TMPro;
using Unity.Entities;

public class TimeDateUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI seasonText;

    private EntityManager entityManager;
    private Entity gameTimeEntity;

    void Start()
    {
        // 기본 월드에서 EntityManager를 가져옴.
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        // GameTime 싱글톤 엔티티를 찾기 위한 쿼리 생성.
        EntityQuery gameTimeQuery = entityManager.CreateEntityQuery(typeof(GameTime));
        
        // 싱글톤 엔티티를 가져옴.
        if (!gameTimeQuery.IsEmpty)
        {
            gameTimeEntity = gameTimeQuery.GetSingletonEntity();
        }
    }

    void Update()
    {
        // GameTime 엔티티가 유효한지 확인
        if (gameTimeEntity == Entity.Null || !entityManager.Exists(gameTimeEntity))
        {
            // 싱글톤 미 생성시 다시시도
            EntityQuery gameTimeQuery = entityManager.CreateEntityQuery(typeof(GameTime));
            if (!gameTimeQuery.IsEmpty)
            {
                gameTimeEntity = gameTimeQuery.GetSingletonEntity();
            }
            else
            {
                return;
            }
        }

        // GameTime 컴포넌트 데이터
        GameTime gameTime = entityManager.GetComponentData<GameTime>(gameTimeEntity);

        // 시간 업데이트
        int hours = (int)(gameTime.TimeOfDay / 60);
        int minutes = (int)(gameTime.TimeOfDay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        // 날짜 업데이트
        dateText.text = string.Format("Year {0}, Day {1}", gameTime.Year, gameTime.Day);

        // 계절(분기) 업데이트
        string season;
        switch (gameTime.Quater)
        {
            case 1:
                season = "Spring";
                break;
            case 2:
                season = "Summer";
                break;
            case 3:
                season = "Fall";
                break;
            case 4:
                season = "Winter";
                break;
            default:
                season = "Unknown";
                break;
        }
        seasonText.text = season;
    }
}
