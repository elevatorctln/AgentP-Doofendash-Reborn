using UnityEngine;

public class CollectibleOps
{
	public static T Spawn<T>(T spawnObj, Transform parent, float laneOffset, int lane) where T : MonoBehaviour
	{
		Object gameObject = spawnObj.gameObject;
		GameObject gameObject2 = CacheManager.The().Spawn(gameObject);
		Vector3 position = gameObject2.transform.position;
		gameObject2.transform.position = parent.transform.position;
		gameObject2.transform.rotation = parent.transform.rotation;
		gameObject2.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(gameObject2.transform, Obstacle.CalcLanePosition(lane, laneOffset));
		TransformOps.AddPositionY(gameObject2.transform, position.y * parent.transform.localScale.y);
		TransformOps.AddPositionZ(gameObject2.transform, position.z * parent.transform.localScale.z);
		return gameObject2.GetComponent<T>();
	}

	public static T Spawn<T>(T spawnObj, Transform parent, float laneOffset, int lane, float offsetY, float offsetZ) where T : MonoBehaviour
	{
		Object gameObject = spawnObj.gameObject;
		GameObject gameObject2 = CacheManager.The().Spawn(gameObject);
		Vector3 position = gameObject2.transform.position;
		gameObject2.transform.position = parent.transform.position;
		TransformOps.AddPositionY(gameObject2.transform, offsetY);
		TransformOps.AddPositionZ(gameObject2.transform, offsetZ);
		gameObject2.transform.rotation = parent.transform.rotation;
		gameObject2.transform.parent = parent.transform;
		TransformOps.SetLocalPositionX(gameObject2.transform, Obstacle.CalcLanePosition(lane, laneOffset));
		TransformOps.AddPositionY(gameObject2.transform, position.y * parent.transform.localScale.y);
		TransformOps.AddPositionZ(gameObject2.transform, position.z * parent.transform.localScale.z);
		return gameObject2.GetComponent<T>();
	}
}
