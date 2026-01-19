using System.Collections;
using UnityEngine;

/// <summary>
/// Unity-4-friendly scene loading wrapper.
/// 
/// In Unity 4.x, Application.LoadLevelAsync was historically a Pro-only feature.
/// This wrapper preserves the old "async" call pattern (yielding + progress)
/// while still allowing the project to run under Unity 4 Personal by
/// falling back to a staged synchronous load.
/// 
/// Key property: call sites can yield and display UI for at least 1 frame
/// (and optionally a minimum duration) before the blocking load occurs.
/// </summary>
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

		// Ensure at least one frame passes so any loading UI can render.
		yield return null;

		float elapsed = 0f;
		while (elapsed < minDelaySeconds)
		{
			elapsed += Time.deltaTime;
			// Fake progress up to 0.9 while we're waiting.
			req.Progress = Mathf.Clamp01((minDelaySeconds <= 0f) ? 0.9f : (elapsed / minDelaySeconds) * 0.9f);
			yield return null;
		}

		// Try real async if available. In Unity 4 Personal this may not exist / not work,
		// so we keep a safe synchronous fallback.
		AsyncOperation op = null;
		bool canUseAsync = false;
		// Unity 4 Personal throws a runtime exception if you call LoadLevelAsync.
		// We simply never call it under Unity 4.x, and always use our staged
		// synchronous fallback there.
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
				// Unity reports 0..0.9 during async load.
				req.Progress = Mathf.Clamp01(op.progress / 0.9f);
				yield return null;
			}
			req.Progress = 1f;
			req.IsDone = true;
			yield break;
		}

		// Fallback: synchronous load.
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
