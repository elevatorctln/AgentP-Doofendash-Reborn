using UnityEngine;

public class TransformOps
{
	public static void SetLocalPosition(Transform t, float x, float y, float z)
	{
		Vector3 localPosition = new Vector3(x, y, z);
		t.localPosition = localPosition;
	}

	public static void SetLocalPositionX(Transform t, float x)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.x = x;
		t.localPosition = localPosition;
	}

	public static void SetLocalPositionY(Transform t, float y)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.y = y;
		t.localPosition = localPosition;
	}

	public static void SetLocalPositionZ(Transform t, float z)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.z = z;
		t.localPosition = localPosition;
	}

	public static void SetPosition(Transform t, float x, float y, float z)
	{
		Vector3 position = new Vector3(x, y, z);
		t.position = position;
	}

	public static void SetPositionX(Transform t, float x)
	{
		Vector3 position = t.position;
		position.x = x;
		t.position = position;
	}

	public static void SetPositionY(Transform t, float y)
	{
		Vector3 position = t.position;
		position.y = y;
		t.position = position;
	}

	public static void SetPositionZ(Transform t, float z)
	{
		Vector3 position = t.position;
		position.z = z;
		t.position = position;
	}

	public static void AddPositionX(Transform t, float x)
	{
		Vector3 position = t.position;
		position.x += x;
		t.position = position;
	}

	public static void AddPositionY(Transform t, float y)
	{
		Vector3 position = t.position;
		position.y += y;
		t.position = position;
	}

	public static void AddPositionZ(Transform t, float z)
	{
		Vector3 position = t.position;
		position.z += z;
		t.position = position;
	}

	public static void AddPositionXZ(Transform t, float x, float z)
	{
		Vector3 position = t.position;
		position.x += x;
		position.z += z;
		t.position = position;
	}

	public static void AddLocalPositionX(Transform t, float x)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.x += x;
		t.localPosition = localPosition;
	}

	public static void AddLocalPositionY(Transform t, float y)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.y += y;
		t.localPosition = localPosition;
	}

	public static void AddLocalPositionZ(Transform t, float z)
	{
		Vector3 localPosition = t.localPosition;
		localPosition.z += z;
		t.localPosition = localPosition;
	}

	public static void SetLocalScale(Transform t, float scale)
	{
		Vector3 localScale = new Vector3(scale, scale, scale);
		t.localScale = localScale;
	}

	public static void SetLocalEulerX(Transform t, float angle)
	{
		Vector3 localEulerAngles = t.localEulerAngles;
		localEulerAngles.x = angle;
		t.localEulerAngles = localEulerAngles;
	}

	public static void SetLocalEulerY(Transform t, float angle)
	{
		Vector3 localEulerAngles = t.localEulerAngles;
		localEulerAngles.x = angle;
		t.localEulerAngles = localEulerAngles;
	}

	public static void SetLocalEulerZ(Transform t, float angle)
	{
		Vector3 localEulerAngles = t.localEulerAngles;
		localEulerAngles.x = angle;
		t.localEulerAngles = localEulerAngles;
	}

	public static void SetEulers(Transform t, float x, float y, float z)
	{
		Vector3 eulerAngles = t.eulerAngles;
		eulerAngles.x = x;
		eulerAngles.y = y;
		eulerAngles.z = z;
		t.eulerAngles = eulerAngles;
	}

	public static void RotateAboutPointXZ(Transform t, Vector3 pivotPoint, float angle)
	{
		Vector3 localPosition = t.localPosition;
		Vector3 vector = localPosition - pivotPoint;
		Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.up);
		vector = quaternion * vector;
		localPosition += vector;
		t.localPosition = localPosition;
		t.localRotation = quaternion;
	}

	public static void RotateAboutPoint(Transform t, Vector3 pivotPoint, ref Quaternion q)
	{
		Vector3 localPosition = t.localPosition;
		Vector3 vector = localPosition - pivotPoint;
		vector = q * vector;
		localPosition = vector + pivotPoint;
		t.localPosition = localPosition;
		t.localRotation = q;
	}

	public static void ConstrainVelocity(Transform transform, float velMaxMagnitude)
	{
		Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;
		if (velocity.sqrMagnitude > velMaxMagnitude * velMaxMagnitude)
		{
			velocity.Normalize();
			velocity *= velMaxMagnitude;
			transform.GetComponent<Rigidbody>().velocity = velocity;
		}
	}

	public static Vector3 ConstrainVelocity(Vector3 velocity, float velMaxMagnitude)
	{
		Vector3 result = velocity;
		if (result.sqrMagnitude > velMaxMagnitude * velMaxMagnitude)
		{
			result.Normalize();
			result *= velMaxMagnitude;
		}
		return result;
	}

	public static void ScaleVelocity(Transform transform, float scale)
	{
		transform.GetComponent<Rigidbody>().velocity *= scale;
	}

	public static void SetRigidBodyVelocity(Transform t, float x, float y, float z)
	{
		Vector3 velocity = new Vector3(x, y, z);
		t.GetComponent<Rigidbody>().velocity = velocity;
	}

	public static void SetRigidBodyVelocityX(Transform t, float x)
	{
		Vector3 velocity = t.GetComponent<Rigidbody>().velocity;
		velocity.x = x;
		t.GetComponent<Rigidbody>().velocity = velocity;
	}

	public static void SetRigidBodyVelocityY(Transform t, float y)
	{
		Vector3 velocity = t.GetComponent<Rigidbody>().velocity;
		velocity.y = y;
		t.GetComponent<Rigidbody>().velocity = velocity;
	}

	public static void SetRigidBodyVelocityZ(Transform t, float z)
	{
		Vector3 velocity = t.GetComponent<Rigidbody>().velocity;
		velocity.z = z;
		t.GetComponent<Rigidbody>().velocity = velocity;
	}

	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
		foreach (Transform transform in componentsInChildren)
		{
			transform.gameObject.layer = layerNumber;
		}
	}
}
