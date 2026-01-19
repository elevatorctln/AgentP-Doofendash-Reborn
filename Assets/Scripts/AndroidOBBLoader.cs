using System.Collections;
using UnityEngine;

public class AndroidOBBLoader : MonoBehaviour
{
	private string expPath;

	private bool alreadyLogged;

	private bool downloadStarted;

	private string m_LoadLevelStatusString;

	private string m_DownloadText = "The game needs to download 50MB of game content.\nIt's recommended to use a WIFI connection.";

	private string m_DownloadButtonText = "Start Download !";

	private bool m_DidStartDownloadingLevel;

	private bool m_IsLoadingLevel;

	private bool m_ShouldTriggerLoadLevelAsync;

	public Texture m_Background_1x;

	public Texture m_Background_2x;

	public Texture m_Background_4x;

	public Texture m_ProgressMeterEmpty;

	public Texture m_ProgressMeterFull;

	public Texture m_ProgressMeterFullEnd;

	public GUIStyle m_GUIStyle;

	private SceneLoader.LoadRequest m_LoadRequest;

	private float m_Progress;

	private Texture background;

	private void Start()
	{
		m_DidStartDownloadingLevel = false;
		m_IsLoadingLevel = false;
		m_ShouldTriggerLoadLevelAsync = false;
		ChooseBackgroundTexture();
	}

	private void ChooseBackgroundTexture()
	{
		background = m_Background_1x;
		if (UI.isHD)
		{
			if (UI.instance.hdExtension.Contains("2x"))
			{
				background = m_Background_2x;
			}
			else if (UI.instance.hdExtension.Contains("4x"))
			{
				background = m_Background_4x;
			}
		}
	}

	private void OnGUITestProgressMeter()
	{
		m_Progress += 0.001f;
		if (m_Progress > 1f)
		{
			m_Progress = 0f;
		}
		GUIProgressMeter();
	}

	private void OnGUI()
	{
		GUI.contentColor = new Color(1f, 1f, 1f);
		if (background != null)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), background);
		}
		if (m_DidStartDownloadingLevel)
		{
			if (!m_IsLoadingLevel)
			{
				return;
			}
			if (m_LoadLevelStatusString != null)
			{
				GUILabelTextCentered(m_LoadLevelStatusString, 0.7f);
				return;
			}
			if (m_LoadRequest != null)
			{
				m_Progress = m_LoadRequest.Progress;
			}
			if (m_Progress > 1f)
			{
				m_Progress = 1f;
			}
			GUIProgressMeter();
			return;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			if (!GooglePlayDownloader.RunningOnAndroid())
			{
				GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "Use GooglePlayDownloader only on Android device!");
				return;
			}
			expPath = GooglePlayDownloader.GetExpansionFilePath();
		}
		else
		{
			expPath = string.Empty;
		}
		if (expPath == null)
		{
			GUI.Label(new Rect(10f, 10f, Screen.width - 10, 20f), "External storage is not available!");
			return;
		}
		string text = null;
		if (Application.platform == RuntimePlatform.Android)
		{
			text = GooglePlayDownloader.GetMainOBBPath(expPath);
			if (!alreadyLogged)
			{
				alreadyLogged = true;
				if (text != null)
				{
					StartCoroutine(loadLevel());
				}
			}
		}
		if (text == null)
		{
			GUILabelMultiLineTextCentered(m_DownloadText, 0.7f);
			float num = (float)Screen.width * 0.42f;
			float num2 = (float)Screen.height * 0.05f;
			float left = (float)Screen.width / 2f - num / 2f;
			float top = (float)Screen.height * 0.8f - num2 / 2f;
			if (GUI.Button(new Rect(left, top, num, num2), m_DownloadButtonText))
			{
				GooglePlayDownloader.FetchOBB();
				StartCoroutine(loadLevel());
			}
		}
	}

	private void GUIProgressMeter()
	{
		Vector2 vector = m_GUIStyle.CalcSize(new GUIContent(m_ProgressMeterEmpty));
		Vector2 vector2 = m_GUIStyle.CalcSize(new GUIContent(m_ProgressMeterFull));
		Vector2 vector3 = m_GUIStyle.CalcSize(new GUIContent(m_ProgressMeterFullEnd));
		float num = vector.x * 0.92f;
		float num2 = num * m_Progress;
		float num3 = num - vector3.x * 2f;
		float num4 = (float)Screen.width / 2f - vector.x / 2f;
		float num5 = (float)Screen.height * 0.7f;
		float num6 = num4 + vector.x * 0.04f;
		float top = num5 + vector.y * 0.15f;
		GUI.DrawTexture(new Rect(num4, num5, vector.x, vector.y), m_ProgressMeterEmpty);
		float num7 = Mathf.Min(num2, vector3.x);
		float num8 = Mathf.Min(num3, num2 - num7);
		float num9 = num2 - num8 - num7;
		GUI.DrawTexture(new Rect(num6, top, num7, vector3.y), m_ProgressMeterFullEnd);
		GUI.DrawTexture(new Rect(vector3.x + num6, top, num8, vector2.y), m_ProgressMeterFull);
		GUI.DrawTexture(new Rect(num6 + vector3.x + num3 + num9, top, 0f - num9, vector3.y), m_ProgressMeterFullEnd);
	}

	private void GUILabelMultiLineTextCentered(string text, float heightPercentage)
	{
		string[] array = text.Split('\n');
		float num = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			GUILabelTextCentered(array[i], heightPercentage, num);
			num += m_GUIStyle.CalcSize(new GUIContent(array[i])).y * 1.025f;
		}
	}

	private void GUILabelTextCentered(string text, float heightPercentage, float heightOffset = 0f)
	{
		Vector2 vector = m_GUIStyle.CalcSize(new GUIContent(text));
		float x = vector.x;
		float y = vector.y;
		float left = (float)Screen.width / 2f - x / 2f;
		float top = (float)Screen.height * 0.7f + heightOffset;
		GUI.Label(new Rect(left, top, x, y), text, m_GUIStyle);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape) && Input.anyKeyDown)
		{
			Application.Quit();
		}
		if (m_ShouldTriggerLoadLevelAsync && !m_IsLoadingLevel)
		{
			m_IsLoadingLevel = true;
			m_LoadRequest = SceneLoader.LoadScene(1, this, 0.1f);
		}
	}

	// www is depricated but it works for now so whatever. This script is never even used now.
	protected IEnumerator loadLevel()
	{
		m_DidStartDownloadingLevel = true;
		m_Progress = 0f;
		string mainPath;
		do
		{
			yield return new WaitForSeconds(0.5f);
			mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
		}
		while (mainPath == null);
		if (!downloadStarted)
		{
			downloadStarted = true;
			string uri = "file://" + mainPath;
			WWW www = WWW.LoadFromCacheOrDownload(uri, 0);
			yield return www;
			if (www.error != null)
			{
				m_LoadLevelStatusString = www.error;
			}
			else
			{
				m_ShouldTriggerLoadLevelAsync = true;
			}
		}
	}
}
