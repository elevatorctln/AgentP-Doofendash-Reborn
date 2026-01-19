using System;
using System.Collections;
using Reign.Logs;
using UnityEngine;

public class ReignServices : MonoBehaviour
{
	internal delegate void frameDoneCallbackMethod();

	public delegate void ServiceMethod();

	internal bool requestFrameDone;

	internal frameDoneCallbackMethod frameDoneCallback;

	public static ReignServices Singleton { get; private set; }

	private static event ServiceMethod updateService;

	private static event ServiceMethod onguiService;

	private static event ServiceMethod destroyService;

	public static void AddService(ServiceMethod update, ServiceMethod onGUI, ServiceMethod destroy)
	{
		if (update != null)
		{
			ReignServices.updateService = (ServiceMethod)Delegate.Remove(ReignServices.updateService, update);
			ReignServices.updateService = (ServiceMethod)Delegate.Combine(ReignServices.updateService, update);
		}
		if (onGUI != null)
		{
			ReignServices.onguiService = (ServiceMethod)Delegate.Remove(ReignServices.onguiService, onGUI);
			ReignServices.onguiService = (ServiceMethod)Delegate.Combine(ReignServices.onguiService, onGUI);
		}
		if (destroy != null)
		{
			ReignServices.destroyService = (ServiceMethod)Delegate.Remove(ReignServices.destroyService, destroy);
			ReignServices.destroyService = (ServiceMethod)Delegate.Combine(ReignServices.destroyService, destroy);
		}
	}

	public static void RemoveService(ServiceMethod update, ServiceMethod onGUI, ServiceMethod destroy)
	{
		if (update != null)
		{
			ReignServices.updateService = (ServiceMethod)Delegate.Remove(ReignServices.updateService, update);
		}
		if (onGUI != null)
		{
			ReignServices.onguiService = (ServiceMethod)Delegate.Remove(ReignServices.onguiService, onGUI);
		}
		if (destroy != null)
		{
			ReignServices.destroyService = (ServiceMethod)Delegate.Remove(ReignServices.destroyService, destroy);
		}
	}

	private void Awake()
	{
		if (Singleton != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Singleton = this;
		Reign.Logs.Debug.Log = (Reign.Logs.Debug.LogMethod)Delegate.Combine(Reign.Logs.Debug.Log, new Reign.Logs.Debug.LogMethod(UnityEngine.Debug.Log));
		Reign.Logs.Debug.LogError = (Reign.Logs.Debug.LogMethod)Delegate.Combine(Reign.Logs.Debug.LogError, new Reign.Logs.Debug.LogMethod(UnityEngine.Debug.LogError));
		dispose();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		if (ReignServices.destroyService != null)
		{
			ReignServices.destroyService();
		}
		dispose();
	}

	private void dispose()
	{
		ReignServices.updateService = null;
		ReignServices.onguiService = null;
		ReignServices.destroyService = null;
	}

	private void Update()
	{
		if (ReignServices.updateService != null)
		{
			ReignServices.updateService();
		}
		if (requestFrameDone)
		{
			requestFrameDone = false;
			StartCoroutine(frameSync());
		}
	}

	private IEnumerator frameSync()
	{
		yield return new WaitForEndOfFrame();
		if (frameDoneCallback != null)
		{
			frameDoneCallback();
		}
	}

	private void OnGUI()
	{
		if (ReignServices.onguiService != null)
		{
			ReignServices.onguiService();
		}
	}
}
