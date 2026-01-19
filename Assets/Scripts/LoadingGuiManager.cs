using UnityEngine;

public sealed class LoadingGuiManager : MonoBehaviour
{
	public UIToolkit LoadingTextToolkit;

	private static UIText loadText;

	private static UITextInstance m_loadingLabel;

	private int m_loadingState = 3;

	private static LoadingGuiManager m_the;

	public static LoadingGuiManager The
	{
		get
		{
			return m_the;
		}
	}

	private void Awake()
	{
		m_the = this;
	}

	private void Start()
	{
		InitLoadingScreen();
		InvokeRepeating("UpdateLoadText", 0f, 0.5f);
	}

	private void InitLoadingScreen()
	{
		loadText = new UIText(LoadingTextToolkit, "snyder", "snyder.png");
		m_loadingLabel = loadText.addTextInstance("Loading. . .", 0f, 0f);
		m_loadingLabel.positionFromBottomLeft(0.01f, 0.2f);
		m_loadingLabel.alignMode = UITextAlignMode.Center;
		m_loadingLabel.hidden = true;
	}

	private void Update()
	{
	}

	public static void ToggleLoadingText()
	{
		m_loadingLabel.hidden = !m_loadingLabel.hidden;
	}

	private void UpdateLoadText()
	{
		if (!m_loadingLabel.hidden)
		{
			if (m_loadingState > 2)
			{
				m_loadingLabel.text = "Loading . . .";
				m_loadingState = 0;
			}
			else if (m_loadingState > 1)
			{
				m_loadingLabel.text = "Loading . .";
			}
			else if (m_loadingState > 0)
			{
				m_loadingLabel.text = "Loading .";
			}
			m_loadingState++;
		}
	}

	public void HideAll(bool bHide)
	{
		m_loadingLabel.hidden = bHide;
	}
}
