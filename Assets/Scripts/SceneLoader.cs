using System.Collections;
using UnityEngine;

public static class SceneLoader
{
	public sealed class LoadRequest
	{
		public readonly string SceneName;
		public readonly int? SceneIndex;
		public float Progress { get; set; }
		public bool IsDone { get; set; }
		public AsyncOperation UnityAsyncOperation { get; internal set; }

		public LoadRequest(string sceneName)
		{
			SceneName = sceneName;
			SceneIndex = null;
		}

		public LoadRequest(int sceneIndex)
		{
			SceneIndex = sceneIndex;
			SceneName = null;
		}
	}

	public static LoadRequest LoadScene(string sceneName, MonoBehaviour runner, float minDelaySeconds = 0f)
	{
		LoadRequest req = new LoadRequest(sceneName);
		runner.StartCoroutine(LoadSceneCoroutine(req, minDelaySeconds));
		return req;
	}

	public static LoadRequest LoadScene(int sceneIndex, MonoBehaviour runner, float minDelaySeconds = 0f)
	{
		LoadRequest req = new LoadRequest(sceneIndex);
		runner.StartCoroutine(LoadSceneCoroutine(req, minDelaySeconds));
		return req;
	}

	private static IEnumerator LoadSceneCoroutine(LoadRequest req, float minDelaySeconds)
	{
		req.IsDone = false;
		req.Progress = 0f;
		req.UnityAsyncOperation = null;

		yield return null;

		float elapsed = 0f;
		while (elapsed < minDelaySeconds)
		{
			elapsed += Time.deltaTime;
			req.Progress = Mathf.Clamp01((minDelaySeconds <= 0f) ? 0.9f : (elapsed / minDelaySeconds) * 0.9f);
			yield return null;
		}

		AsyncOperation op = null;
		bool canUseAsync = false;
		#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_2017 || UNITY_2018 || UNITY_2019 || UNITY_2020 || UNITY_2021 || UNITY_2022 || UNITY_2023 || UNITY_2024
		canUseAsync = true;
		try
		{
			if (req.SceneIndex.HasValue)
			{
				op = Application.LoadLevelAsync(req.SceneIndex.Value);
			}
			else
			{
				op = Application.LoadLevelAsync(req.SceneName);
			}
		}
		catch
		{
			canUseAsync = false;
			op = null;
		}
		#endif

		req.UnityAsyncOperation = op;
		if (canUseAsync && op != null)
		{
			while (!op.isDone)
			{
				req.Progress = Mathf.Clamp01(op.progress / 0.9f);
				yield return null;
			}
			req.Progress = 1f;
			req.IsDone = true;
			yield break;
		}

		req.Progress = 0.95f;
		yield return null;
		if (req.SceneIndex.HasValue)
		{
			Application.LoadLevel(req.SceneIndex.Value);
		}
		else
		{
			Application.LoadLevel(req.SceneName);
		}
		req.Progress = 1f;
		req.IsDone = true;
	}
}
