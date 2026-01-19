using System;
using UnityEngine;

public class RunnerModelChooser : MonoBehaviour
{
	public GameObject[] m_RunnerModels;

	private static RunnerModelChooser m_The;

	private void Awake()
	{
		m_The = this;
	}

	public static RunnerModelChooser The()
	{
		return m_The;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public GameObject SelectRunnerModelForGamePlay(string name)
	{
		GameObject result = null;
		GameObject[] runnerModels = m_RunnerModels;
		foreach (GameObject gameObject in runnerModels)
		{
			if (name == gameObject.name.ToLower())
			{
				result = gameObject;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.rotation = Quaternion.identity;
				TransformOps.SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		return result;
	}

	public GameObject SelectRunnerModelForMenu(string name)
	{
		GameObject gameObject = null;
		string text = name.ToLower();
		GameObject[] runnerModels = m_RunnerModels;
		foreach (GameObject gameObject2 in runnerModels)
		{
			if (text == gameObject2.name.ToLower())
			{
				gameObject = gameObject2;
				gameObject.SetActive(true);
				TransformOps.SetLayerRecursively(gameObject, LayerMask.NameToLayer("MenuLayer"));
				break;
			}
		}
		if (Runner.The() != null)
		{
			Runner.The().DisableAllAttachments();
		}
		return gameObject;
	}

	public void DeselectRunnerModel(GameObject go)
	{
		if (go != null)
		{
			go.transform.parent = base.transform;
			go.SetActive(false);
		}
	}

	private void SetupRunnerModelForMenu(GameObject runnerModel, string name)
	{
		try
		{
			runnerModel.transform.position = Vector3.zero;
			runnerModel.transform.localPosition = Vector3.zero;
			runnerModel.transform.rotation = Quaternion.identity;
			GameObject gameObject = runnerModel.transform.Find(name + "Guns/waterGun_Attach").gameObject;
			GameObject gameObject2 = runnerModel.transform.Find(name + "Guns/fireGun_Attach").gameObject;
			GameObject gameObject3 = runnerModel.transform.Find(name + "Guns/zappyGun_Attach").gameObject;
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			gameObject3.SetActive(false);
			GameObject gameObject4 = runnerModel.transform.Find(name + "HangGlider").gameObject;
			GameObject gameObject5 = runnerModel.transform.Find(name + "HangGliderTRANSITION").gameObject;
			GameObject gameObject6 = runnerModel.transform.Find(name + "Helicopter").gameObject;
			gameObject4.SetActive(false);
			gameObject5.SetActive(false);
			gameObject6.SetActive(false);
			GameObject gameObject7 = runnerModel.transform.Find("ForceFieldEffects").gameObject;
			GameObject gameObject8 = runnerModel.transform.Find("MagnetEffects").gameObject;
			GameObject gameObject9 = runnerModel.transform.Find("MultiplierEffects").gameObject;
			gameObject7.SetActive(false);
			gameObject8.SetActive(false);
			gameObject9.SetActive(false);
			GameObject gameObject10 = runnerModel.transform.Find("Eagle_Anims").gameObject;
			gameObject10.SetActive(false);
			TransformOps.SetLayerRecursively(runnerModel, LayerMask.NameToLayer("MenuLayer"));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}
}
