using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class UIHelper
{
	public delegate void UIHelperCallback();

	private static Texture m_oldBGTex;

	private static RawImage m_replacedTex;

	private static Color m_oldBGColor;

	private static ArrayList m_numberAnimTxts;

	private static UITextInstance m_animateTextInstance;

	public static int LineBreakTextByMaxX(ref UITextInstance txtInstance, float maxXPos)
	{
		float num = txtInstance.position.x + txtInstance.width;
		if (num > maxXPos)
		{
			return LineBreakText(ref txtInstance, maxXPos - txtInstance.position.x);
		}
		return 1;
	}

	public static int LineBreakText(ref UITextInstance txtInstance, float width, int maxLines, bool resizeToFitSameHeight)
	{
		string text = txtInstance.text;
		int num = LineBreakText(ref txtInstance, width);
		if (num > maxLines)
		{
			Debug.Log("Too Many Lines");
			if (resizeToFitSameHeight)
			{
				Debug.Log("txtInstance.text beforeEvenBreak: " + txtInstance.text);
				txtInstance.text = text;
				LineBreakTextEvenly(ref txtInstance, maxLines);
				Debug.Log("txtInstance.text afterEvenBreak: " + txtInstance.text);
				float width2 = txtInstance.width;
				float num2 = 1f;
				if (width2 > width)
				{
					num2 = width / width2;
				}
				txtInstance.textScale *= num2;
			}
			else
			{
				string text2 = txtInstance.text;
				string text3 = string.Empty;
				StringReader stringReader = new StringReader(text2);
				for (int i = 0; i < maxLines; i++)
				{
					text3 = text3 + stringReader.ReadLine() + "\n";
				}
				txtInstance.text = text3;
			}
		}
		return num;
	}

	public static void LineBreakTextEvenly(ref UITextInstance txtInstance, int lineNum)
	{
		float width = txtInstance.width;
		width /= (float)lineNum;
		width += 0.5f;
		string text = txtInstance.text;
		StringReader stringReader = new StringReader(text);
		string empty = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		int num = 1;
		for (int num2 = stringReader.Read(); num2 != -1; num2 = stringReader.Read())
		{
			empty = txtInstance.text;
			string text4 = Convert.ToChar(num2).ToString();
			text3 += text4;
			if (text4 == " " || stringReader.Peek() == -1)
			{
				text2 += text3;
				txtInstance.text = text2;
				if (txtInstance.width >= width)
				{
					text2 = empty;
					if (num < lineNum)
					{
						num++;
						text2 = text2 + "\n" + text3;
					}
					else
					{
						text2 += text3;
					}
					txtInstance.text = text2;
				}
				text3 = string.Empty;
			}
		}
	}

	public static int LineBreakText(ref UITextInstance txtInstance, float width)
	{
		int num = 1;
		float width2 = txtInstance.width;
		if (width2 > width)
		{
			string text = txtInstance.text;
			string empty = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			StringReader stringReader = new StringReader(text);
			int num2 = stringReader.Read();
			float num3 = 0f;
			float num4 = 0f;
			while (num2 != -1)
			{
				empty = txtInstance.text;
				string text4 = Convert.ToChar(num2).ToString();
				text3 += text4;
				if (text4 == " " || stringReader.Peek() == -1)
				{
					text2 += text3;
					txtInstance.text = text2;
					num4 = num3;
					num3 = txtInstance.width;
					if (num3 >= width)
					{
						text2 = empty;
						text2 = text2 + "\n" + text3;
						txtInstance.text = text2;
						num++;
						num3 -= num4;
					}
					text3 = string.Empty;
				}
				num2 = stringReader.Read();
			}
		}
		return num;
	}

	public static string LineBreakString(string str, int maxCharPerLine)
	{
		int item = 0;
		int num = 0;
		string text = string.Empty;
		List<int> list = new List<int>();
		for (int i = 0; i < str.Length; i++)
		{
			string text2 = str.Substring(i, 1);
			text += text2;
			num++;
			if (text2 == " ")
			{
				item = i;
			}
			if (num > maxCharPerLine)
			{
				list.Add(item);
				num = 0;
			}
		}
		foreach (int item2 in list)
		{
			text = text.Remove(item2, 1);
			text = text.Insert(item2, "\n");
		}
		return text;
	}

	public static float ResizeSpriteToWidth(ref UISprite uiObj, float width)
	{
		float num = 1f;
		if (uiObj.width > width)
		{
			num = width / uiObj.width;
			uiObj.scale = new Vector3(num, num, 1f);
		}
		return num;
	}

	public static float ResizeButtonToWidth(ref UIButton uiObj, float width)
	{
		float num = 1f;
		if (uiObj.width > width)
		{
			num = width / uiObj.width;
			uiObj.scale = new Vector3(num, num, 1f);
		}
		return num;
	}

	public static float ResizeToggleButtonToWidth(ref UIToggleButton uiObj, float width)
	{
		float num = 1f;
		if (uiObj.width > width)
		{
			num = width / uiObj.width;
			uiObj.scale = new Vector3(num, num, 1f);
		}
		return num;
	}

	public static void ResizeTextToWidth(ref UITextInstance txtInstance, float width, bool uniformScale = true)
	{
		float width2 = txtInstance.width;
		if (width2 > width)
		{
			float num = width / width2;
			txtInstance.textScale *= num * 0.9f;
		}
	}

	public static void PositionSpriteGroupAroundCenter(ref UISprite[] sprites, int leftMostPos, int rightMostPos, UIToolkit spriteToolkit, int fixedCenterTopPos, int startCenterLeftPos, string spriteName, int spriteInc, float spriteScale, int spriteDepth, UIObject parentObj = null)
	{
		leftMostPos = 0;
		rightMostPos = 0;
		int num = startCenterLeftPos;
		for (int i = 0; i < sprites.Length; i++)
		{
			if (i == 0)
			{
				if (sprites.Length % 2 == 0)
				{
					num -= spriteInc / 2;
				}
				rightMostPos = num;
				leftMostPos = num;
			}
			else if (i % 2 == 0)
			{
				num = rightMostPos - spriteInc;
				rightMostPos = num;
			}
			else
			{
				num = leftMostPos + spriteInc;
				leftMostPos = num;
			}
			sprites[i] = spriteToolkit.addSprite(spriteName, 0, 0, spriteDepth);
			sprites[i].scale = new Vector3(spriteScale, spriteScale, 1f);
			if (parentObj != null)
			{
				sprites[i].parentUIObject = parentObj;
			}
			sprites[i].pixelsFromCenter(fixedCenterTopPos, num);
			Debug.Log("SpriteReorder s: " + sprites[i].position);
		}
	}

	public static void ReorderSpriteArrayByLeftToRight(ref UISprite[] spriteArray)
	{
		ArrayList arrayList = new ArrayList(spriteArray);
		ArrayList arrayList2 = new ArrayList();
		while (arrayList.Count > 0)
		{
			UISprite uISprite = (UISprite)arrayList[0];
			for (int i = 1; i < arrayList.Count; i++)
			{
				UISprite uISprite2 = (UISprite)arrayList[i];
				if (uISprite2.localPosition.x < uISprite.localPosition.x)
				{
					uISprite = uISprite2;
				}
			}
			arrayList2.Add(uISprite);
			arrayList.Remove(uISprite);
		}
		spriteArray = (UISprite[])arrayList2.ToArray(typeof(UISprite));
	}

	public static void TakeScreenshot()
	{
		Camera component = GameObject.Find("UICamera").GetComponent<Camera>();
		LayerMask layerMask = component.cullingMask;
		LayerMask layerMask2 = (1 << LayerMask.NameToLayer("UILayer")) | (1 << LayerMask.NameToLayer("BackgroundImage")) | (1 << LayerMask.NameToLayer("MaskImage"));
		component.cullingMask = layerMask2;
		RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
		renderTexture.Create();
		component.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
		RawImage rawImage = GameObject.Find("bgMask").GetComponent<RawImage>();
		rawImage.texture = renderTexture;
		RenderTexture.active = active;
		component.cullingMask = layerMask;
		component.targetTexture = null;
	}

	public static IEnumerator TakeScreenshot2(RawImage replaceTex, Action<bool> completionHandler)
	{
		yield return new WaitForEndOfFrame();
		Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		tex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
		tex.Apply();
		m_replacedTex = replaceTex;
		m_oldBGTex = m_replacedTex.texture;
		m_replacedTex.texture = tex;
		m_oldBGColor = m_replacedTex.color;
		m_replacedTex.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
		float dim = 0.2f;
		m_replacedTex.color = new Color(m_oldBGColor.r - dim, m_oldBGColor.g - dim, m_oldBGColor.b - dim, m_oldBGColor.a);
		yield return 0;
		completionHandler(true);
	}

	public static void RemoveScreenshot()
	{
		m_replacedTex.texture = m_oldBGTex;
		m_replacedTex.color = m_oldBGColor;
	}

	public static void SetTextForWriteNumberAnim(ref UITextInstance txtInstance)
	{
		if (m_numberAnimTxts == null)
		{
			m_numberAnimTxts = new ArrayList(1);
		}
		m_numberAnimTxts.Add(txtInstance);
	}

	public static IEnumerator TextNumberGrowAnimAction(int numStart, int numEnd, Action<bool> onDone)
	{
		if (m_numberAnimTxts == null)
		{
			onDone(false);
			yield break;
		}
		UITextInstance txtInstance = (UITextInstance)m_numberAnimTxts[m_numberAnimTxts.Count - 1];
		m_numberAnimTxts.RemoveAt(m_numberAnimTxts.Count - 1);
		if (txtInstance == null)
		{
			onDone(false);
			yield break;
		}
		int curNum = numStart;
		float numDif = numEnd - numStart;
		int numInc = (int)(numDif * 0.015f);
		if (numInc <= 0)
		{
			numInc = 1;
		}
		Debug.Log("NumInc: " + numInc);
		while (curNum < numEnd)
		{
			curNum += numInc;
			if (curNum > numEnd)
			{
				curNum = numEnd;
			}
			txtInstance.text = curNum.ToString("N0");
			yield return 0;
		}
		onDone(true);
		yield return 0;
	}

	public static IEnumerator TextNumberGrowAnim(int numStart, int numEnd, UIHelperCallback callback)
	{
		if (m_numberAnimTxts == null)
		{
			callback();
			yield break;
		}
		UITextInstance txtInstance = (UITextInstance)m_numberAnimTxts[m_numberAnimTxts.Count - 1];
		m_numberAnimTxts.RemoveAt(m_numberAnimTxts.Count - 1);
		if (txtInstance == null)
		{
			callback();
			yield break;
		}
		int curNum = numStart;
		float numDif = numEnd - numStart;
		int numInc = (int)(numDif * 0.015f);
		if (numInc <= 0)
		{
			numInc = 1;
		}
		Debug.Log("NumInc: " + numInc);
		while (curNum < numEnd)
		{
			curNum += numInc;
			if (curNum > numEnd)
			{
				curNum = numEnd;
			}
			txtInstance.text = curNum.ToString("N0");
			yield return 0;
		}
		callback();
		yield return 0;
	}

	public static void SetTextForAnimateTextBox(ref UITextInstance txtInstance)
	{
		m_animateTextInstance = txtInstance;
	}

	public static IEnumerator AnimateLetters(float tick, Action<bool> onDone)
	{
		return AnimateLettersWithPaging(tick, 0.5f, 0, onDone);
	}

	public static IEnumerator AnimateLettersWithPaging(float letterTick, float pageTick, int maxLines, Action<bool> onDone)
	{
		if (m_animateTextInstance == null)
		{
			if (onDone != null)
			{
				onDone(false);
			}
			yield break;
		}
		string txtStr = m_animateTextInstance.text;
		if (m_animateTextInstance != null)
		{
			m_animateTextInstance.text = string.Empty;
		}
		StringReader strReader = new StringReader(txtStr);
		int c = strReader.Read();
		string charStr = string.Empty;
		int curLines = 1;
		while (c != -1)
		{
			charStr += Convert.ToChar(c);
			if (charStr.Contains("\n") || charStr.Contains(" "))
			{
				if (m_animateTextInstance != null)
				{
					m_animateTextInstance.text += charStr;
				}
				if (charStr.Contains("\n") && maxLines > 0)
				{
					curLines++;
					if (curLines > maxLines)
					{
						yield return new WaitForSeconds(pageTick);
						if (m_animateTextInstance != null)
						{
							m_animateTextInstance.text = string.Empty;
						}
						curLines = 1;
					}
				}
				charStr = string.Empty;
				c = strReader.Read();
			}
			else
			{
				yield return new WaitForSeconds(letterTick);
				if (m_animateTextInstance != null)
				{
					m_animateTextInstance.text += charStr;
				}
				charStr = string.Empty;
				c = strReader.Read();
			}
		}
		if (onDone != null)
		{
			onDone(true);
		}
		m_animateTextInstance = null;
		yield return 0;
	}

	public static string WordWrap(string text, int width)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (width < 1)
		{
			return text;
		}
		int i = 0;
		while (i < text.Length)
		{
			int num = text.IndexOf(Environment.NewLine, i);
			int num2 = ((num != -1) ? (num + Environment.NewLine.Length) : (num = text.Length));
			if (num > i)
			{
				do
				{
					int num3 = num - i;
					if (num3 > width)
					{
						num3 = BreakLine(text, i, width);
					}
					stringBuilder.Append(text, i, num3);
					stringBuilder.Append(Environment.NewLine);
					for (i += num3; i < num && char.IsWhiteSpace(text[i]); i++)
					{
					}
				}
				while (num > i);
			}
			else
			{
				stringBuilder.Append(Environment.NewLine);
			}
			i = num2;
		}
		return stringBuilder.ToString();
	}

	private static int BreakLine(string text, int pos, int max)
	{
		int num = max;
		while (num >= 0 && !char.IsWhiteSpace(text[pos + num]))
		{
			num--;
		}
		if (num < 0)
		{
			return max;
		}
		while (num >= 0 && char.IsWhiteSpace(text[pos + num]))
		{
			num--;
		}
		return num + 1;
	}

	/// Most of the code below this point is new, for reference.
	public static float CalcFontScale()
	{
		float baseScale = UIScalingHelper.CalcFontScale();
		
		float legacyAdjustment = 4f / Mathf.Max(UI.scaleFactor, 1);
		
		float result = baseScale * legacyAdjustment * 0.5f;
		
		return Mathf.Clamp(result, 0.3f, 2.5f);
	}
	
	public static float GetTouchableElementScale()
	{
		return UIScalingHelper.GetElementScale();
	}

	public static bool IsModernPhone()
	{
		return UIScalingHelper.IsModernPhoneAspect;
	}

	public static bool IsTablet()
	{
		return UIScalingHelper.IsTablet;
	}

	public static float GetTopSafeMargin()
	{
		return UIScalingHelper.GetTopSafeOffset();
	}

	public static float GetBottomSafeMargin()
	{
		return UIScalingHelper.GetBottomSafeOffset();
	}

	public static float ScalePosition(float referencePosition)
	{
		return UIScalingHelper.ScalePixels(referencePosition);
	}

	public static float GetAdaptiveOffset(float baseOffset, float phoneMultiplier = 1.2f)
	{
		if (UIScalingHelper.IsModernPhoneAspect)
		{
			return baseOffset * phoneMultiplier;
		}
		return baseOffset;
	}
}
