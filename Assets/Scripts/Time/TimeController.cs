using UnityEngine;
using Unity.Entities;
using System;

public class TimeController : MonoBehaviour
{
    private EntityManager entityManager;
    private EntityQuery timeQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		timeQuery = entityManager.CreateEntityQuery(typeof(GameTime));

		try
		{
			if (timeQuery.IsEmpty)
				throw new NullReferenceException("GameTime is Empty");
		}
		catch (NullReferenceException e)
		{
			Debug.LogError(e.Message);
			Destroy(this); // 자신을 지움
		}
	}

    public void TogglePause()
    {
        //if (timeQuery.IsEmpty) return;
        var timeData = timeQuery.GetSingletonRW<GameTime>();
        timeData.ValueRW.IsPaused = !timeData.ValueRW.IsPaused;
    }

    public void EndDay()
    {
        //if (timeQuery.IsEmpty) return;
		var timeData = timeQuery.GetSingletonRW<GameTime>();
        
		if(timeData.ValueRO.TimeOfDay > 360f)	// 6:00 AM 전인지
			timeData.ValueRW.Day++;
        timeData.ValueRW.TimeOfDay = 360f; //  6:00 AM으로 설정
        timeData.ValueRW.IsPaused = false;
    }
}