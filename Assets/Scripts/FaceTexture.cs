using System;
using System.Collections;
using UnityEngine;

public class FaceTexture : MonoBehaviour
{
	public UITouchableSprite parent;

	public UITouchableSprite HUDparent;

	public string fbId;

	public float upClip;

	public float downClip;

	private bool m_isTextureLoaded;

	private string m_url;

	public void SetClipping()
	{
		Debug.Log("SetClipping " + downClip + " " + upClip);
		base.GetComponent<Renderer>().material.SetFloat("_ClipMinY", downClip);
		base.GetComponent<Renderer>().material.SetFloat("_ClipMaxY", upClip);
	}

	public void LoadDebuggingTexture()
	{
		Material material = new Material(base.GetComponent<Renderer>().material.shader);
		base.GetComponent<Renderer>().material = material;
	}

	public void LoadTexture(string url)
	{
		m_url = url;
		Material material = new Material(base.GetComponent<Renderer>().material.shader);
		base.GetComponent<Renderer>().material = material;
		StartCoroutine(LoadTextureAsync(url));
	}

	private IEnumerator LoadTextureAsync(string url)
	{
		url = Uri.EscapeUriString(url);
		Debug.Log("LoadTextureAsync " + url);
		WWW www = new WWW(url);
		float elapsedTime = 0f;
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 20f)
			{
				Debug.Log("Timeout , texture not loaded");
				break;
			}
			yield return null;
		}
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			Debug.LogError(string.Format("Fail Whale!\n{0}", www.error));
			yield break;
		}
		Debug.Log("Texture loaded " + url);
		m_isTextureLoaded = true;
		base.GetComponent<Renderer>().material.mainTexture = www.texture;
	}

	public void ReloadTexture()
	{
		if (!m_isTextureLoaded)
		{
			Debug.Log("Reload Texture");
			LoadTexture(m_url);
		}
	}

	private void LateUpdate()
	{
		if (parent != null)
		{
			base.transform.position = parent.client.transform.position + new Vector3(base.transform.localScale.x * 16f, (0f - base.transform.localScale.y) * 7f, -3f);
		}
		else if (HUDparent != null)
		{
			base.transform.position = HUDparent.client.transform.position + new Vector3(base.transform.localScale.x * 6f, (0f - base.transform.localScale.y) * 6f, -3f);
		}
	}
}
