using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILanguageManager
{
	public delegate void OnLanguageChanged();

	private delegate string OnLoadLanguage(string language);

	private static Dictionary<string, UILanguageManager> _languageManagers;

	private static Dictionary<string, string> _languages;

	private static string _currentLanguage = string.Empty;

	private Dictionary<string, string> _texts;

	private string _filename;

	private string _localLanguage;

	public static string Language
	{
		get
		{
			return _currentLanguage;
		}
		set
		{
			if (value == null || _currentLanguage.Equals(value))
			{
				return;
			}
			if (!_languages.ContainsKey(value))
			{
				Debug.LogError("Language " + value + " was not found!");
				return;
			}
			bool flag = true;
			if (UILanguageManager.onLoadLanguage != null)
			{
				Delegate[] invocationList = UILanguageManager.onLoadLanguage.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					OnLoadLanguage onLoadLanguage = (OnLoadLanguage)invocationList[i];
					flag &= value == onLoadLanguage(value);
				}
			}
			if (flag)
			{
				_currentLanguage = value;
				if (UILanguageManager.onLanguageChanged != null)
				{
					UILanguageManager.onLanguageChanged();
				}
			}
			else if (UILanguageManager.onLoadLanguage != null)
			{
				UILanguageManager.onLoadLanguage(_currentLanguage);
			}
		}
	}

	public string this[string token]
	{
		get
		{
			if (_texts.ContainsKey(token))
			{
				return _texts[token];
			}
			throw new ArgumentException("Token (" + token + ") was not found!", "token");
		}
	}

	public static event OnLanguageChanged onLanguageChanged;

	private static event OnLoadLanguage onLoadLanguage;

	private UILanguageManager(string filename)
	{
		_filename = filename;
		_localLanguage = loadLanguageTextsFromJSON(_currentLanguage);
	}

	public static UILanguageManager GetManager(string filename)
	{
		if (string.IsNullOrEmpty(filename))
		{
			filename = "guiText";
		}
		if (_languageManagers == null)
		{
			_languageManagers = new Dictionary<string, UILanguageManager>(1);
			CheckLanguages();
		}
		UILanguageManager uILanguageManager;
		if (_languageManagers.ContainsKey(filename))
		{
			uILanguageManager = _languageManagers[filename];
		}
		else
		{
			uILanguageManager = new UILanguageManager(filename);
			UILanguageManager.onLoadLanguage = (OnLoadLanguage)Delegate.Combine(UILanguageManager.onLoadLanguage, new OnLoadLanguage(uILanguageManager.LoadLanguage));
			_languageManagers.Add(filename, uILanguageManager);
		}
		return uILanguageManager;
	}

	public static void RemoveManager(string filename)
	{
		if (_languageManagers.ContainsKey(filename))
		{
			UILanguageManager uILanguageManager = _languageManagers[filename];
			UILanguageManager.onLoadLanguage = (OnLoadLanguage)Delegate.Remove(UILanguageManager.onLoadLanguage, new OnLoadLanguage(uILanguageManager.LoadLanguage));
			_languageManagers.Remove(filename);
		}
	}

	public static void AddLanguage(string language, string extension)
	{
		CheckLanguages();
		if (_languages.ContainsKey(language))
		{
			_languages[language] = extension;
			Debug.LogWarning("Language extension for " + language + " have been overriden!");
		}
		else
		{
			_languages.Add(language, extension);
		}
	}

	public static bool HasLanguage(string language)
	{
		CheckLanguages();
		return _languages.ContainsKey(language);
	}

	public static string[] GetLanguages()
	{
		CheckLanguages();
		string[] array = new string[_languages.Keys.Count];
		_languages.Keys.CopyTo(array, 0);
		return array;
	}

	private static void CheckLanguages()
	{
		if (_languages == null)
		{
			_languages = new Dictionary<string, string>(2);
			_languages.Add("English", "EN");
			_currentLanguage = "English";
		}
	}

	private string LoadLanguage(string language)
	{
		if (language != _localLanguage)
		{
			_localLanguage = loadLanguageTextsFromJSON(language);
		}
		return language;
	}

	private string loadLanguageTextsFromJSON(string language)
	{
		string path = _filename + "_" + _languages[language];
		TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Language file for " + language + " could not be found!");
			if (!language.Equals("English"))
			{
				Debug.LogWarning("Defaulting to English if possible.");
				language = "English";
				path = _filename + "_" + _languages[language];
				textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
				if (textAsset == null)
				{
					Debug.LogError("English language was not found. Aborting.");
					return null;
				}
			}
		}
		string text = textAsset.text;
		IDictionary dictionary = text.hashtableFromJson();
		IDictionary dictionary2 = dictionary["TranslatedStrings"] as IDictionary;
		if (dictionary2 == null)
		{
			Debug.LogError("Translation file for " + language + " is invalid!");
			return null;
		}
		if (_texts == null)
		{
			_texts = new Dictionary<string, string>();
		}
		else
		{
			_texts.Clear();
		}
		foreach (DictionaryEntry item in dictionary2)
		{
			string value = (string)((IDictionary)item.Value)["StringToRead"];
			_texts.Add(item.Key.ToString(), value);
		}
		textAsset = null;
		Resources.UnloadUnusedAssets();
		return language;
	}
}
