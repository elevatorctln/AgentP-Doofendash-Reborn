using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectGUIManager : MonoBehaviour
{
	private delegate void CostumeStates();

	private static UIHorizontalLayout m_selectButtonsLayout;

	private static UIHorizontalLayout m_selectButtonsLayoutBottom;

	private static UIButton m_rightCharArrow;

	private static UIButton m_leftCharArrow;

	private static UISprite[] m_selectButtons;

	private static UISprite[] m_selectButtonsOn;

	private static UIAnimation m_leftArrowAnimation;

	private static UIAnimation m_rightArrowAnimation;

	private static UIButton m_buyCharacterButton;

	private static UISprite m_buyCharacterPlate;

	private static UITextInstance m_characterTokenCost;

	private static UITextInstance m_characterNameLabel;

	private static UIVerticalLayout m_costumesLayout;

	private static List<UIButton> m_costumes;

	private static List<UISprite> m_costumesLock;

	private static List<int> m_mainCharactersOrder;

	private Vector3 m_newCamPos;

	private float m_cameraChangeSpeed = 7f;

	private int m_currentCharIndex;

	private int m_lastCharIndex;

	private bool m_autoPlay;

	private bool m_touchDown;

	private bool m_touchMoved;

	private float m_swipeSpeedX;

	private float m_swipeXThreshold = 1f;

	private GlobalGUIManager.CharacterSelectData[] m_currentCharData;

	private int m_charSelectDepthStart = 4;

	private float m_currentScaleFactor = 1f;

	private bool m_isCameraMoving;

	private bool m_ShouldDisableSelectControls;

	private static CharSelectGUIManager m_the;

	public bool m_isMainMenu;

	public bool m_isInGame;

	private Camera charCam;

	private bool m_IsWaitingOnEndCapsule;

	private bool m_isMouseDown;

	private bool m_isMouseMoving;

	private float mouseStartX;

	private float touchStartX;

	private float m_CameraYOffsetMainMenu = 2f;

	private CostumeStates m_CostumeState;

	public static CharSelectGUIManager The
	{
		get
		{
			if (m_the == null)
			{
				GameObject gameObject = new GameObject("CharSelectGUIManager");
				((CharSelectGUIManager)gameObject.AddComponent<CharSelectGUIManager>()).Init();
			}
			return m_the;
		}
	}

	public bool m_canPlay
	{
		get
		{
			PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
			PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
			if (characterItemByUniqueID.m_owned)
			{
				return true;
			}
			return false;
		}
	}

	public void EnableSelectControls()
	{
		m_ShouldDisableSelectControls = false;
	}

	public void DisableSelectControls()
	{
		m_ShouldDisableSelectControls = true;
	}

	private void Awake()
	{
		MainMenuEventManager.FinishedCapsuleEndAnimsEvents += FinishedCapsuleEndAnimsListener;
	}

	private void OnDestroy()
	{
		MainMenuEventManager.FinishedCapsuleEndAnimsEvents -= FinishedCapsuleEndAnimsListener;
	}

	private void Start()
	{
		charCam = GameObject.Find("GlobalMenuPrefab/Main Camera").GetComponent<Camera>();
	}

	private void PlayAnimEndCapsuleMainMenu()
	{
		m_IsWaitingOnEndCapsule = true;
		m_lastCharIndex = m_currentCharIndex;
		PlayableCharacterChooser component = GlobalGUIManager.The.m_mainMenuCharacters[m_currentCharIndex].GetComponent<PlayableCharacterChooser>();
		component.PlayEndAnimations();
		ShopPlayGUIManager.The.HideButtons();
	}

	private void PlayAnimEndCapsuleInGame()
	{
		m_IsWaitingOnEndCapsule = true;
		m_lastCharIndex = m_currentCharIndex;
		PlayableCharacterChooser component = GlobalGUIManager.The.m_inGameMenuCharacters[m_currentCharIndex].GetComponent<PlayableCharacterChooser>();
		component.PlayEndAnimations();
		ShopPlayGUIManager.The.HideButtons();
	}

	private void FinishedCapsuleEndAnimsListener()
	{
		if (GlobalGUIManager.The.IsInMainMenu())
		{
			GamePlay.ms_ShouldSkipTubeIntro = false;
		}
		else
		{
			GamePlay.ms_ShouldSkipTubeIntro = true;
		}
		MainMenuEventManager.TriggerPlayWithCurrentCharacter(m_currentCharIndex);
		m_IsWaitingOnEndCapsule = false;
	}

	private void Update()
	{
		if (!m_IsWaitingOnEndCapsule && !m_ShouldDisableSelectControls)
		{
			if (m_CostumeState != null)
			{
				m_CostumeState();
			}
			else
			{
				UpdateCharSelectiPhoneControls();
			}
		}
	}

	private void UpdateUI()
	{
		if (m_currentCharData != null && GlobalGUIManager.The.IsInCharSelectScreen())
		{
			if (GlobalGUIManager.The.IsInMainMenu())
			{
				HideAll();
				ShowMainMenuCharSelect(ref m_currentCharData);
			}
			else if (GlobalGUIManager.The.IsInInGameMenu())
			{
				HideAll();
				ShowInGameMenuCharSelect(ref m_currentCharData);
			}
		}
		else
		{
			HideAll();
		}
	}

	public void Init()
	{
		if (!(m_the != null))
		{
			m_the = this;
			Object.DontDestroyOnLoad(base.gameObject);
			InitPhoneControls();
			InitMainMenuCharacterSelectButtons();
			InitCostumes();
			HideAll();
		}
	}

	private void InitPhoneControls()
	{
		if (Screen.dpi > 0f)
		{
			float num = Screen.dpi / 450f;
			num *= 60f;
			num = Mathf.Clamp(num, 30f, 60f);
			m_swipeSpeedX = num / (float)Screen.width;
		}
		else
		{
			m_swipeSpeedX = 30f / (float)Screen.width;
		}
	}

	private void InitMainMenuCharacterSelectButtons()
	{
		m_selectButtonsLayout = new UIHorizontalLayout(0);
		m_selectButtonsLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_selectButtonsLayoutBottom = new UIHorizontalLayout(0);
		m_selectButtonsLayoutBottom.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_leftCharArrow = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SwipeArrow.png", "SwipeArrow.png", 0, 0, m_charSelectDepthStart + 2);
		m_leftCharArrow.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_leftCharArrow.pixelsFromLeft(0);
		m_leftCharArrow.positionFromCenter(-0.2f, -0.23f);
		m_leftCharArrow.onTouchUpInside += onTouchUpInsidePrevCharButton;
		m_rightCharArrow = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "SwipeArrow.png", "SwipeArrow.png", 0, 0, m_charSelectDepthStart + 2);
		m_rightCharArrow.centerize();
		m_rightCharArrow.eulerAngles = new Vector3(m_rightCharArrow.eulerAngles.x, m_rightCharArrow.eulerAngles.y, 180f);
		m_rightCharArrow.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_rightCharArrow.pixelsFromLeft(0);
		m_rightCharArrow.positionFromCenter(-0.2f, 0.23f);
		m_rightCharArrow.onTouchUpInside += onTouchUpInsideNextCharButton;
		m_buyCharacterButton = UIButton.create(GlobalGUIManager.The.m_hudToolkit, "TokenSaleButton.png", "TokenSaleButtonOver.png", 0, 0, m_charSelectDepthStart + 1);
		m_buyCharacterButton.highlightedTouchOffsets = new UIEdgeOffsets(30);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_buyCharacterButton);
		m_buyCharacterButton.onTouchUpInside += onTouchUpInsideBuyCharacterButton;
		m_buyCharacterPlate = GlobalGUIManager.The.m_hudToolkit.addSprite("ScorePopUp.png", 0, 0, m_charSelectDepthStart + 6);
		m_buyCharacterPlate.hidden = true;
		m_characterTokenCost = GlobalGUIManager.The.defaultText.addTextInstance("999,999,999", 0f, 0f, 1f, m_charSelectDepthStart);
		m_characterTokenCost.verticalAlignMode = UITextVerticalAlignMode.Middle;
		m_characterTokenCost.alignMode = UITextAlignMode.Center;
		int totalNumberOfCharInSelectScreen = GlobalGUIManager.The.GetTotalNumberOfCharInSelectScreen();
		m_selectButtons = new UISprite[totalNumberOfCharInSelectScreen];
		m_selectButtonsOn = new UISprite[totalNumberOfCharInSelectScreen];
		m_mainCharactersOrder = new List<int>();
		for (int i = 0; i < m_selectButtons.Length; i++)
		{
			m_selectButtons[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("SwipeCounterOff.png", 0, 0, m_charSelectDepthStart);
			if ((float)i < (float)m_selectButtons.Length / 2f)
			{
				m_selectButtonsLayout.addChild(m_selectButtons[i]);
			}
			else
			{
				m_selectButtonsLayoutBottom.addChild(m_selectButtons[i]);
			}
			m_selectButtonsOn[i] = GlobalGUIManager.The.m_menuToolkit.addSprite("SwipeCounterOn.png", 0, 0, m_charSelectDepthStart);
			m_selectButtonsOn[i].parentUIObject = m_selectButtons[i];
			m_selectButtonsOn[i].positionFromCenter(0f, 0f);
			m_mainCharactersOrder.Add(i);
		}
		m_selectButtonsLayout.matchSizeToContentSize();
		m_selectButtonsLayoutBottom.matchSizeToContentSize();
		m_characterNameLabel = GlobalGUIManager.The.defaultTextAlt.addTextInstance("Perry", 0f, 0f, 1f, m_charSelectDepthStart);
		m_characterNameLabel.setColorForAllLetters(Color.white);
		m_characterNameLabel.alignMode = UITextAlignMode.Left;
		SwapCharacterOrder();
	}

	private void StartArrowAnimation()
	{
		if (m_leftArrowAnimation != null)
		{
			m_leftArrowAnimation.stop();
		}
		if (m_rightArrowAnimation != null)
		{
			m_rightArrowAnimation.stop();
		}
		Vector3 position = m_leftCharArrow.position;
		position.x -= 10f;
		animateArrow(m_leftCharArrow, position, 0.5f, m_leftArrowAnimation);
		Vector3 position2 = m_rightCharArrow.position;
		position2.x += 10f;
		animateArrow(m_rightCharArrow, position2, 0.5f, m_rightArrowAnimation);
	}

	private void animateArrow(UISprite arrow, Vector3 to, float duration, UIAnimation arrowAnimation)
	{
		arrowAnimation = arrow.positionTo(duration, to, Easing.Linear.easeInOut);
		arrowAnimation.autoreverse = true;
		arrowAnimation.onComplete = delegate
		{
			animateArrow(arrow, to, duration, arrowAnimation);
		};
	}

	private void InitCostumes()
	{
		int num = 3;
		m_costumesLayout = new UIVerticalLayout(0);
		m_costumesLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;
		m_costumesLayout.verticalAlignMode = UIAbstractContainer.UIContainerVerticalAlignMode.Top;
		m_costumes = new List<UIButton>();
		m_costumesLock = new List<UISprite>();
		for (int i = 0; i < num; i++)
		{
			UIButton sprite = UIButton.create(GlobalGUIManager.The.m_menuToolkit, "PerryButon.png", "PerryButonOver.png", 0, 0, m_charSelectDepthStart + 1);
			sprite.centerize();
			sprite.onTouchUpInside += onTouchUpInsideCostume;
			GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref sprite);
			m_costumes.Add(sprite);
			UISprite uISprite = GlobalGUIManager.The.m_menuToolkit.addSprite("Lock.png", 0, 0, m_charSelectDepthStart);
			m_costumesLock.Add(uISprite);
			uISprite.parentUIObject = sprite;
			uISprite.positionFromCenter(0.1f, 0.4f);
			m_costumesLayout.addChild(sprite);
		}
		if (m_costumes.Count > 0)
		{
			m_costumesLayout.positionFromTopLeft(0.55f, 0.12f);
		}
	}

	private void SetActivePlayableCharacterChooser(int index)
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(index);
		playableCharacterChooser.gameObject.SetActive(true);
		playableCharacterChooser.PlayIdleAnimations();
	}

	private void SetInactivePlayableCharacterChooser(int index)
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(index);
		playableCharacterChooser.gameObject.SetActive(false);
	}

	private void HideOffScreenCharacters()
	{
		if (!GameManager.The.IsLowEndDevice())
		{
			return;
		}
		for (int i = 0; i < GlobalGUIManager.The.m_mainMenuCharacters.Length; i++)
		{
			if (m_currentCharIndex != i)
			{
				SetInactivePlayableCharacterChooser(i);
			}
		}
		SetActivePlayableCharacterChooser(m_currentCharIndex);
	}

	public void HideAll()
	{
		HideMainMenuCharSelect();
	}

	public void HideMainMenuCharSelect()
	{
		m_isMainMenu = false;
		m_isInGame = false;
		m_leftCharArrow.hidden = true;
		m_rightCharArrow.hidden = true;
		HideAllCharSelectDots();
		HideCostumes(true);
		m_characterNameLabel.hidden = true;
		m_buyCharacterButton.hidden = true;
		m_characterTokenCost.hidden = true;
		m_selectButtonsLayout.hidden = true;
		m_selectButtonsLayoutBottom.hidden = true;
		m_costumesLayout.hidden = true;
	}

	public void HideCostumes(bool hide)
	{
		for (int i = 0; i < m_costumes.Count; i++)
		{
			m_costumes[i].hidden = hide;
			m_costumesLock[i].hidden = hide;
		}
	}

	public void HideAllCharSelectDots()
	{
		for (int i = 0; i < m_selectButtonsOn.Length; i++)
		{
			m_selectButtonsOn[i].hidden = true;
		}
	}

	private void EnableAllButtons()
	{
		foreach (UIButton costume in m_costumes)
		{
			costume.disabled = false;
		}
		m_rightCharArrow.disabled = false;
		m_leftCharArrow.disabled = false;
		m_buyCharacterButton.disabled = false;
	}

	private void DisableAllButtons()
	{
		foreach (UIButton costume in m_costumes)
		{
			costume.disabled = true;
		}
		m_rightCharArrow.disabled = true;
		m_leftCharArrow.disabled = true;
		m_buyCharacterButton.disabled = true;
	}

	public void ShowMainMenuCharSelect(ref GlobalGUIManager.CharacterSelectData[] charSelectDataObjs)
	{
		m_isMainMenu = true;
		m_currentScaleFactor = 1f;
		m_currentCharData = charSelectDataObjs;
		PositionUIElements(ref charSelectDataObjs);
		UpdateAllCharSelectElements();
		Invoke("ResetCam", 0.1f);
	}

	private void ResetCam()
	{
	}

	public void ShowInGameMenuCharSelect(ref GlobalGUIManager.CharacterSelectData[] charSelectDataObjs)
	{
		m_isInGame = true;
		m_currentScaleFactor = 0.55f;
		m_currentCharData = charSelectDataObjs;
		PositionUIElements(ref charSelectDataObjs);
		UpdateAllCharSelectElements();
	}

	public void ShowPrevAndNextButtons()
	{
		if (!m_isCameraMoving)
		{
			ShowPrevCharButton();
			ShowNextCharButton();
		}
	}

	public void ShowCharSelect()
	{
		m_leftCharArrow.hidden = false;
		m_rightCharArrow.hidden = false;
		ShowCharSelectedGUI(m_currentCharIndex);
		m_characterNameLabel.hidden = false;
		m_buyCharacterButton.hidden = false;
		m_characterTokenCost.hidden = false;
		m_selectButtonsLayout.hidden = true;
		m_selectButtonsLayoutBottom.hidden = true;
		m_costumesLayout.hidden = false;
	}

	private void PositionUIElements(ref GlobalGUIManager.CharacterSelectData[] charSelectDataObjs)
	{
		int currentCharIndex = m_currentCharIndex;
		Vector2 vector = GlobalGUIManager.The.ConvertFromWorldToScreenCoords(charSelectDataObjs[currentCharIndex].scrollLPos.transform.position);
		Vector2 vector2 = GlobalGUIManager.The.ConvertFromWorldToScreenCoords(charSelectDataObjs[currentCharIndex].scrollRPos.transform.position);
		Vector2 vector3 = GlobalGUIManager.The.ConvertFromWorldToScreenCoords(charSelectDataObjs[currentCharIndex].dotsPos.transform.position);
		Vector2 vector4 = GlobalGUIManager.The.ConvertFromWorldToScreenCoords(charSelectDataObjs[currentCharIndex].buyBtnPos.transform.position);
		int num = (int)vector.x;
		int num2 = (int)vector2.x;
		m_leftCharArrow.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_leftCharArrow.positionFromLeft(0.055f);
		m_leftCharArrow.position = new Vector3(m_leftCharArrow.position.x + (float)num - m_leftCharArrow.width, 0f - vector.y - m_leftCharArrow.height / -1f, m_leftCharArrow.position.z);
		m_rightCharArrow.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_rightCharArrow.positionFromLeft(-0.055f);
		m_rightCharArrow.position = new Vector3(m_rightCharArrow.position.x + (float)num2, 0f - vector2.y - m_rightCharArrow.height / -2f, m_rightCharArrow.position.z);
		m_selectButtonsLayout.parentUIObject = null;
		m_selectButtonsLayout.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_selectButtonsLayout.matchSizeToContentSize();
		m_selectButtonsLayout.alignMode = UIAbstractContainer.UIContainerAlignMode.Left;
		m_selectButtonsLayout.positionFromTopLeft(-0.03f, 0f);
		m_selectButtonsLayout.position = new Vector3(m_selectButtonsLayout.position.x + vector3.x - m_selectButtonsLayout.width / 2f, m_selectButtonsLayout.position.y - vector3.y, m_selectButtonsLayout.position.z);
		m_selectButtonsLayoutBottom.parentUIObject = null;
		m_selectButtonsLayoutBottom.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		m_selectButtonsLayoutBottom.matchSizeToContentSize();
		m_selectButtonsLayoutBottom.alignMode = UIAbstractContainer.UIContainerAlignMode.Left;
		m_selectButtonsLayoutBottom.positionFromTopLeft(-0.02f, 0f);
		m_selectButtonsLayoutBottom.position = new Vector3(m_selectButtonsLayoutBottom.position.x + vector3.x - m_selectButtonsLayoutBottom.width / 2f, m_selectButtonsLayoutBottom.position.y - vector3.y - m_selectButtonsLayoutBottom.height, m_selectButtonsLayoutBottom.position.z);
		GlobalGUIManager.The.ResizeUIElementToProperAspectRatio(ref m_buyCharacterButton);
		m_buyCharacterPlate.scale = new Vector3(m_currentScaleFactor, m_currentScaleFactor, 1f);
		int num3 = (int)(vector4.x - m_buyCharacterPlate.width / 2f);
		int num4 = (int)vector4.y;
		m_buyCharacterPlate.positionFromTopLeft(0.45f, 0f);
		m_buyCharacterPlate.position = new Vector3(m_buyCharacterPlate.position.x + (float)num3, m_buyCharacterPlate.position.y - (float)num4, m_buyCharacterPlate.position.z);
		m_buyCharacterButton.parentUIObject = m_buyCharacterPlate;
		m_buyCharacterButton.positionFromBottom(0.15f);
		float num5 = ((GameManager.The.aspectRatio.x != 3f || GameManager.The.aspectRatio.y != 4f) ? 0.22f : 0.35f);
		num5 = ((UI.scaleFactor != 1) ? num5 : (num5 / 2f));
		m_characterTokenCost.textScale = num5 * UIHelper.CalcFontScale();
		m_characterTokenCost.parentUIObject = m_buyCharacterButton;
		m_characterTokenCost.alignMode = UITextAlignMode.Center;
		m_characterTokenCost.positionFromCenter(0f, 0f);
		m_characterNameLabel.alignMode = UITextAlignMode.Center;
		if (GlobalGUIManager.The.IsInMainMenu())
		{
			m_characterNameLabel.positionFromTop(0.22f);
			m_characterNameLabel.textScale = 0.65f * m_currentScaleFactor * 0.98f;
			if (LocalTextManager.CurrentLanguageType == LocalTextManager.PerryLanguages.Japanese)
			{
				m_characterNameLabel.textScale = 0.5f * m_currentScaleFactor * 0.98f;
			}
			return;
		}
		if (GameManager.The.aspectRatio.x != 3f && GameManager.The.aspectRatio.y != 4f)
		{
			m_characterNameLabel.positionFromTop(-2.15f, 0.225f);
		}
		else
		{
			m_characterNameLabel.positionFromTop(0.15f, 0.2f);
		}
		if (GameManager.The.aspectRatio.x == 9f && GameManager.The.aspectRatio.y == 16f)
		{
			m_characterNameLabel.positionFromTop(0.15f, 0.235f);
		}
		m_characterNameLabel.textScale = 0.65f * m_currentScaleFactor * 0.98f;
		m_characterNameLabel.positionFromTop(-5.15f, 0.225f);
	}

	private void UpdateAllCharSelectElements()
	{
		if (!m_isCameraMoving)
		{
			m_selectButtonsLayout.hidden = true;
			m_selectButtonsLayoutBottom.hidden = true;
			if (GlobalGUIManager.The.IsInMainMenu())
			{
				UpdateCharSelectButtons();
			}
			PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
			PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
			UpdateCharSelectName(characterItemByUniqueID);
			ShowCostumes();
		}
	}

	public void UpdateCharSelectButtons()
	{
		int currentCharIndex = m_currentCharIndex;
		ShowPrevAndNextButtons();
		m_selectButtonsLayout.hidden = true;
		m_selectButtonsLayoutBottom.hidden = true;
		for (int i = 0; i < m_selectButtonsOn.Length; i++)
		{
			m_selectButtonsOn[i].hidden = true;
		}
		ShowCharSelectedGUI(currentCharIndex);
	}

	private void ShowPrevCharButton()
	{
		m_leftCharArrow.hidden = false;
	}

	private void ShowNextCharButton()
	{
		m_rightCharArrow.hidden = false;
	}

	private void ShowCharSelectedGUI(int index)
	{
		if (index > -1 && index < m_selectButtonsOn.Length)
		{
			m_selectButtonsOn[index].hidden = true;
		}
	}

	public void UpdateCharSelectName(PurchasableItem chooser)
	{
		if (chooser == null)
		{
			Debug.Log("chooser is null");
			return;
		}
		chooser = AllItemData.GetCharacterItem(chooser);
		m_characterNameLabel.text = chooser.m_name;
		m_characterNameLabel.hidden = false;
		if (chooser.m_owned)
		{
			m_characterTokenCost.hidden = true;
			m_buyCharacterButton.hidden = true;
			return;
		}
		m_characterTokenCost.text = ((chooser.m_fedoraCost <= 0) ? chooser.m_tokenCost.ToString("N0") : chooser.m_fedoraCost.ToString("N0"));
		m_characterTokenCost.hidden = false;
		m_buyCharacterButton.hidden = false;
		string spriteImage = ((chooser.m_fedoraCost <= 0) ? "TokenSaleButton.png" : "FedoraSaleButton.png");
		string highlightedImage = ((chooser.m_fedoraCost <= 0) ? "TokenSaleButtonOver.png" : "FedoraSaleButtonOver.png");
		m_buyCharacterButton.setSpriteImage(spriteImage);
		m_buyCharacterButton.SetHighlightedImage(highlightedImage);
	}

	private void ShowCostumes()
	{
		if (m_isInGame)
		{
			HideCostumes(true);
			return;
		}
		for (int i = 0; i < m_costumes.Count; i++)
		{
			m_costumes[i].hidden = true;
			m_costumesLock[i].hidden = true;
		}
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		int num = 0;
		for (int j = 0; j < playableCharacterChooser.m_CostumeThumbnail.Length; j++)
		{
			if (j == playableCharacterChooser.m_CurrentCostumeIndex)
			{
				continue;
			}
			string text = playableCharacterChooser.m_CostumeThumbnail[j];
			string[] array = text.Split('.');
			string highlightedImage = array[0] + "Over." + array[1];
			m_costumes[num].beginUpdates();
			m_costumes[num].setSpriteImage(text);
			m_costumes[num].SetHighlightedImage(highlightedImage);
			m_costumes[num].endUpdates();
			PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID(j));
			if (characterItemByUniqueID != null)
			{
				m_costumes[num].hidden = false;
				if (characterItemByUniqueID.m_owned)
				{
					m_costumesLock[num].hidden = true;
				}
				else
				{
					m_costumesLock[num].hidden = false;
				}
				m_costumes[num].userData = j;
				num++;
			}
		}
	}

	private void SwapCharacterOrder()
	{
		if (m_currentCharIndex == m_mainCharactersOrder[0])
		{
			Transform characterTransformByIndex = GlobalGUIManager.The.getCharacterTransformByIndex(m_mainCharactersOrder[0]);
			Transform characterTransformByIndex2 = GlobalGUIManager.The.getCharacterTransformByIndex(m_mainCharactersOrder[m_mainCharactersOrder.Count - 1]);
			characterTransformByIndex2.position = new Vector3(characterTransformByIndex.position.x + 25f, characterTransformByIndex2.position.y, characterTransformByIndex2.position.z);
			Transform inGameCharacterTransformByIndex = GlobalGUIManager.The.getInGameCharacterTransformByIndex(m_mainCharactersOrder[0]);
			Transform inGameCharacterTransformByIndex2 = GlobalGUIManager.The.getInGameCharacterTransformByIndex(m_mainCharactersOrder[m_mainCharactersOrder.Count - 1]);
			inGameCharacterTransformByIndex2.position = new Vector3(inGameCharacterTransformByIndex.position.x + 25f, inGameCharacterTransformByIndex2.position.y, inGameCharacterTransformByIndex2.position.z);
			int item = m_mainCharactersOrder[m_mainCharactersOrder.Count - 1];
			m_mainCharactersOrder.RemoveAt(m_mainCharactersOrder.Count - 1);
			m_mainCharactersOrder.Insert(0, item);
		}
		if (m_currentCharIndex == m_mainCharactersOrder[m_mainCharactersOrder.Count - 1])
		{
			Transform characterTransformByIndex3 = GlobalGUIManager.The.getCharacterTransformByIndex(m_mainCharactersOrder[0]);
			Transform characterTransformByIndex4 = GlobalGUIManager.The.getCharacterTransformByIndex(m_mainCharactersOrder[m_mainCharactersOrder.Count - 1]);
			characterTransformByIndex3.position = new Vector3(characterTransformByIndex4.position.x - 25f, characterTransformByIndex3.position.y, characterTransformByIndex3.position.z);
			Transform inGameCharacterTransformByIndex3 = GlobalGUIManager.The.getInGameCharacterTransformByIndex(m_mainCharactersOrder[0]);
			Transform inGameCharacterTransformByIndex4 = GlobalGUIManager.The.getInGameCharacterTransformByIndex(m_mainCharactersOrder[m_mainCharactersOrder.Count - 1]);
			inGameCharacterTransformByIndex3.position = new Vector3(inGameCharacterTransformByIndex4.position.x - 25f, inGameCharacterTransformByIndex3.position.y, inGameCharacterTransformByIndex3.position.z);
			int item2 = m_mainCharactersOrder[0];
			m_mainCharactersOrder.RemoveAt(0);
			m_mainCharactersOrder.Add(item2);
		}
	}

	private bool IsInCharSelectBounds(Vector2 screenPoint)
	{
		bool flag = GlobalGUIManager.The.m_hudToolkit.IsButtonTouched(new Vector2(screenPoint.x, (float)Screen.height - screenPoint.y));
		bool flag2 = GlobalGUIManager.The.m_menuToolkit.IsButtonTouched(new Vector2(screenPoint.x, (float)Screen.height - screenPoint.y));
		if (flag || flag2)
		{
			return false;
		}
		Ray ray = GlobalGUIManager.The.GetCurrentCamera().ScreenPointToRay(screenPoint);
		LayerMask layerMask = 1 << LayerMask.NameToLayer("CharSelectRect");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100f, layerMask) && !flag && hitInfo.collider != null)
		{
			return true;
		}
		return false;
	}

	private void UpdateCharSelectMouseControls()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!IsInCharSelectBounds(Input.mousePosition))
			{
				m_isMouseDown = false;
				return;
			}
			HideMainMenuCharSelect();
			ExternalButtonsGUIManager.The.HideMainMenuButtons();
			MainMenuGUIManager.The.DisableAllButtons(true);
			m_isMouseDown = true;
			m_isMouseMoving = false;
			mouseStartX = Input.mousePosition.x;
		}
		if (Input.GetMouseButtonUp(0) && m_isMouseDown)
		{
			m_isMouseDown = false;
			if (!m_isMouseMoving)
			{
				PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
				PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
				if (characterItemByUniqueID.m_owned)
				{
					ShowMainMenuCharSelect(ref m_currentCharData);
					ExternalButtonsGUIManager.The.ShowMainMenuButtons();
				}
				else
				{
					ShowMainMenuCharSelect(ref m_currentCharData);
					ExternalButtonsGUIManager.The.ShowMainMenuButtons();
					MainMenuGUIManager.The.DisableAllButtons(false);
				}
				return;
			}
			MainMenuGUIManager.The.DisableAllButtons(false);
			MoveToClosestCharToCamera();
		}
		if (m_isMouseDown && Mathf.Abs(mouseStartX - Input.mousePosition.x) > 1f)
		{
			m_isMouseMoving = true;
			float xChange = (Input.mousePosition.x - mouseStartX) / (float)Screen.width;
			MoveCamera(xChange);
		}
	}

	private void UpdateCharSelectiPhoneControls()
	{
		float num = 0f;
		if (Input.touches.Length <= 0)
		{
			return;
		}
		Touch touch = Input.touches[0];
		if (touch.phase == TouchPhase.Began)
		{
			if (!IsInCharSelectBounds(touch.position))
			{
				m_touchDown = false;
				return;
			}
			touchStartX = touch.position.x;
			HideMainMenuCharSelect();
			ExternalButtonsGUIManager.The.HideMainMenuButtons();
			MainMenuGUIManager.The.DisableAllButtons(true);
			m_touchDown = true;
			m_touchMoved = false;
		}
		else if (touch.phase == TouchPhase.Moved && m_touchDown)
		{
			num += Input.touches[0].deltaPosition.x * m_swipeSpeedX;
			if (Mathf.Abs(touchStartX - touch.position.x) > 1f)
			{
				MoveCamera(num);
				m_touchMoved = true;
			}
		}
		else
		{
			if (touch.phase != TouchPhase.Ended || !m_touchDown)
			{
				return;
			}
			m_touchDown = false;
			MainMenuGUIManager.The.DisableAllButtons(false);
			if (m_touchMoved)
			{
				num += Input.touches[0].deltaPosition.x * m_swipeSpeedX;
				if (num >= m_swipeXThreshold)
				{
					GoToPrevCharacter();
				}
				else if (num <= 0f - m_swipeXThreshold)
				{
					GoToNextCharacter();
				}
				else
				{
					MoveToClosestCharToCamera();
				}
				m_touchMoved = false;
				return;
			}
			PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
			PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
			if (!characterItemByUniqueID.m_owned)
			{
				ShowMainMenuCharSelect(ref m_currentCharData);
				ExternalButtonsGUIManager.The.ShowMainMenuButtons();
			}
			else
			{
				ShowMainMenuCharSelect(ref m_currentCharData);
				ExternalButtonsGUIManager.The.ShowMainMenuButtons();
				MainMenuGUIManager.The.DisableAllButtons(false);
			}
		}
	}

	private void MoveToClosestCharToCamera()
	{
		float num = 7f;
		float x = GlobalGUIManager.The.GetCurrentCamera().transform.position.x;
		float num2 = x - GetNextCharPos().x;
		float num3 = GetPrevCharPos().x - x;
		float num4 = Mathf.Abs(x - GetCurrentCharPos().x);
		if (num2 == num4)
		{
			num2 = 1000f;
		}
		else if (num3 == num4)
		{
			num3 = 1000f;
		}
		if (num2 < num3 && num2 - num < num4)
		{
			GoToNextCharacter();
		}
		else if (num3 < num2 && num3 - num < num4)
		{
			GoToPrevCharacter();
		}
		else
		{
			ResetCameraToCurrentCharacter();
		}
	}

	private Vector3 GetCurrentCharPos()
	{
		return GlobalGUIManager.The.getCharacterDataByIndex(m_currentCharIndex).camPos.transform.position;
	}

	private Vector3 GetNextCharPos()
	{
		int num = m_currentCharIndex + 1;
		if (num == m_mainCharactersOrder.Count)
		{
			num = 0;
		}
		return GlobalGUIManager.The.getCharacterDataByIndex(num).camPos.transform.position;
	}

	private Vector3 GetPrevCharPos()
	{
		int num = m_currentCharIndex - 1;
		if (num < 0)
		{
			num = m_mainCharactersOrder.Count - 1;
		}
		return GlobalGUIManager.The.getCharacterDataByIndex(num).camPos.transform.position;
	}

	private bool IsPrevCharAvailable()
	{
		return m_currentCharIndex > 0;
	}

	private bool IsNextCharAvailable()
	{
		return m_currentCharIndex + 1 < GlobalGUIManager.The.GetTotalNumberOfCharInSelectScreen();
	}

	private void MoveCamera(float xChange)
	{
		StopAllCoroutines();
		float num = 0f;
		float num2 = 0f;
		Vector3 position = GlobalGUIManager.The.GetCurrentCamera().transform.position;
		num2 = GetPrevCharPos().x - position.x;
		if (!IsPrevCharAvailable())
		{
		}
		num = GetNextCharPos().x - position.x;
		if (!IsNextCharAvailable())
		{
		}
		if (xChange > num2)
		{
			xChange = num2;
		}
		else if (xChange < num)
		{
			xChange = num;
		}
		GlobalGUIManager.The.GetCurrentCamera().transform.Translate(new Vector3(0f - xChange, 0f, 0f));
	}

	private void GoToNextCharacter()
	{
		if (IsNextCharAvailable())
		{
			m_currentCharIndex++;
			PlayerData.CurrentCharacterIndex = m_currentCharIndex;
		}
		else
		{
			m_currentCharIndex = 0;
			PlayerData.CurrentCharacterIndex = m_currentCharIndex;
		}
		ResetCameraToCurrentCharacter();
	}

	private void GoToPrevCharacter()
	{
		if (IsPrevCharAvailable())
		{
			m_currentCharIndex--;
			PlayerData.CurrentCharacterIndex = m_currentCharIndex;
		}
		else
		{
			m_currentCharIndex = m_mainCharactersOrder.Count - 1;
			PlayerData.CurrentCharacterIndex = m_currentCharIndex;
		}
		ResetCameraToCurrentCharacter();
	}

	public void GoToLastCharacterAndPlay()
	{
		m_autoPlay = true;
		m_currentCharIndex = m_lastCharIndex;
		PlayerData.CurrentCharacterIndex = m_currentCharIndex;
		ResetCameraToCurrentCharacter();
	}

	private void AutoPlay()
	{
		m_autoPlay = false;
		PlayNow();
	}

	public void SetCameraPositionForStory()
	{
		m_newCamPos = GlobalGUIManager.The.getCharacterDataByIndex(m_currentCharIndex).camPos.transform.position;
		m_newCamPos += new Vector3(5f, 2f, 10f);
		GlobalGUIManager.The.SetCurrentCamPosition(m_newCamPos);
	}

	public void ResetCameraNow(bool isIngameMenu)
	{
		m_newCamPos = GlobalGUIManager.The.getCharacterDataByIndex(m_currentCharIndex).camPos.transform.position;
		if (isIngameMenu)
		{
			m_newCamPos += new Vector3(15f, 4.5f, 0f);
		}
		else
		{
			m_newCamPos += new Vector3(0f, m_CameraYOffsetMainMenu, 0f);
		}
		GlobalGUIManager.The.SetCurrentCamPosition(m_newCamPos);
	}

	private void ResetCameraToCurrentCharacter()
	{
		m_newCamPos = GlobalGUIManager.The.getCharacterDataByIndex(m_currentCharIndex).camPos.transform.position;
		if (m_isInGame)
		{
			m_newCamPos += new Vector3(15f, 4.5f, 0f);
		}
		else
		{
			m_newCamPos += new Vector3(0f, m_CameraYOffsetMainMenu, 0f);
		}
		StopCoroutine("GoToNewCameraPos");
		StartCoroutine(GoToNewCameraPos(m_cameraChangeSpeed));
	}

	private IEnumerator GoToNewCameraPos(float speed)
	{
		MainMenuEventManager.TriggerStartAnimation();
		m_isCameraMoving = true;
		Vector3 currentCamPos = GlobalGUIManager.The.GetCurrentCamera().transform.position;
		float startTime = Time.time;
		float totalDist = Vector3.Distance(currentCamPos, m_newCamPos);
		float fracDist = 0f;
		while (fracDist < 1f && totalDist != 0f)
		{
			float distCovered = (Time.time - startTime) * 60f;
			fracDist = distCovered / totalDist;
			Vector3 newPos = Vector3.Lerp(currentCamPos, m_newCamPos, fracDist);
			GlobalGUIManager.The.SetCurrentCamPosition(newPos);
			UpdateUI();
			yield return 0;
		}
		m_isCameraMoving = false;
		UpdateUI();
		SwapCharacterOrder();
		MainMenuEventManager.TriggerEndAnimation();
		if (m_autoPlay)
		{
			if (!IsCurrentCostumeCharacterOwned())
			{
				TriggerCloseDoor();
			}
			else
			{
				AutoPlay();
			}
		}
	}

	public void DisableCostumeButtons()
	{
		m_buyCharacterButton.disabled = true;
		foreach (UIButton costume in m_costumes)
		{
			costume.disabled = true;
		}
	}

	public void EnableCostumeButtons()
	{
		m_buyCharacterButton.disabled = false;
		foreach (UIButton costume in m_costumes)
		{
			costume.disabled = false;
		}
	}

	private void BuySelectedCharacter()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("ItemName", characterItemByUniqueID.m_name);
		dictionary.Add("ItemType", "Character");
		FlurryFacade.Instance.LogEvent("SoftPurchase", dictionary, false);
		MainMenuEventManager.TriggerBuyCharacter(characterItemByUniqueID);
	}

	public void onTouchUpInsideNextCharButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		GoToNextCharacter();
	}

	public void onTouchUpInsidePrevCharButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		GoToPrevCharacter();
	}

	public void onTouchUpInsideBuyCharacterButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UIPURCHASE);
		if (!PopUpGUIManager.The.isAPopupActive)
		{
			BuySelectedCharacter();
		}
	}

	public void onTouchUpInsidePlayButton(UIButton sender)
	{
		GameManager.The.PlayClip(AudioClipFiles.UICLICK);
		PlayAnimEndCapsuleInGame();
	}

	public void PlayNow()
	{
		if (m_isMainMenu && m_canPlay)
		{
			PlayAnimEndCapsuleMainMenu();
			ExternalButtonsGUIManager.The.HideAll();
			ShopPlayGUIManager.The.HideButtons();
			MainMenuGUIManager.The.HideAll();
		}
		if (m_isInGame && m_canPlay)
		{
			PlayAnimEndCapsuleInGame();
			GameManager.The.PlayMusic(AudioClipFiles.ROOFTOPTHEME);
			ExternalButtonsGUIManager.The.HideAll();
			ShopPlayGUIManager.The.HideButtons();
			InGameMenuGUIManager.The.HideAll();
			MonogramGUIManager.The.HideAll();
		}
	}

	public void onTouchUpInsideCostume(UIButton sender)
	{
		if (!IsInChangingCostumeState())
		{
			ShopPlayGUIManager.The.DisableButtons(true);
			GameManager.The.PlayClip(AudioClipFiles.UICLICK);
			int currentCostumeIndex = (int)sender.userData;
			PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
			PlayableCharacterChooser playableCharacterChooser2 = GlobalGUIManager.The.FindInGamePlayableCharacterChooser(m_currentCharIndex);
			playableCharacterChooser.m_CurrentCostumeIndex = currentCostumeIndex;
			playableCharacterChooser2.m_CurrentCostumeIndex = currentCostumeIndex;
			TriggerCloseDoor();
		}
	}

	private bool IsCurrentCostumeCharacterOwned()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID());
		return characterItemByUniqueID.m_owned;
	}

	private bool IsInChangingCostumeState()
	{
		return m_CostumeState != null;
	}

	private void ChooseFirstOwnedCostumeCharacter()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		PlayableCharacterChooser playableCharacterChooser2 = GlobalGUIManager.The.FindInGamePlayableCharacterChooser(m_currentCharIndex);
		for (int i = 0; i < playableCharacterChooser.m_UniqueIDs.Length; i++)
		{
			PurchasableItem characterItemByUniqueID = AllItemData.GetCharacterItemByUniqueID(playableCharacterChooser.FindUniqueID(i));
			if (characterItemByUniqueID.m_owned)
			{
				playableCharacterChooser.m_CurrentCostumeIndex = i;
				playableCharacterChooser2.m_CurrentCostumeIndex = i;
				break;
			}
		}
	}

	private void TriggerCloseDoor()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		playableCharacterChooser.PlayCloseDoorAnimation();
		ShopPlayGUIManager.The.DisableButtons(true);
		StoreGUIManagerPersistentElements.The.DisableAllButtons(true);
		ExternalButtonsGUIManager.The.DisableButtons();
		DisableAllButtons();
		m_CostumeState = CloseDoorCostumeState;
	}

	private void CloseDoorCostumeState()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		if (!playableCharacterChooser.IsCloseDoorAnimationPlaying())
		{
			if (m_autoPlay)
			{
				ChooseFirstOwnedCostumeCharacter();
				UpdateAllCharSelectElements();
			}
			playableCharacterChooser.ChooseModelFromRunnerModelChooser();
			playableCharacterChooser.PlayOpenDoorAnimation();
			m_CostumeState = OpenDoorCostumeState;
		}
	}

	private void OpenDoorCostumeState()
	{
		PlayableCharacterChooser playableCharacterChooser = GlobalGUIManager.The.FindPlayableCharacterChooser(m_currentCharIndex);
		if (!playableCharacterChooser.IsOpenDoorAnimationPlaying())
		{
			m_CostumeState = null;
			ShopPlayGUIManager.The.DisableButtons(false);
			StoreGUIManagerPersistentElements.The.DisableAllButtons(false);
			ExternalButtonsGUIManager.The.EnableButtons();
			EnableAllButtons();
			if (m_autoPlay)
			{
				AutoPlay();
			}
			else
			{
				UpdateAllCharSelectElements();
			}
		}
	}

	public void ReloadStaticText()
	{
	}
}
