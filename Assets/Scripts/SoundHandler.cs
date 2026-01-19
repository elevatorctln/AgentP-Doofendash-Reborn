using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundHandler : MonoBehaviour
{
	public enum SoundState
	{
		fadingIn = 0,
		fadingOut = 1,
		standard = 2
	}

	private AudioSource source;

	public SoundState state = SoundState.standard;

	public bool b_BGMusic;

	private string m_soundclippath = string.Empty;

	private string m_musictrack = string.Empty;

	private float starttime;

	public string clip
	{
		get
		{
			return m_soundclippath;
		}
	}

	public string MusicTrackClip
	{
		get
		{
			if (GameManager.The.g_MusicEnabled)
			{
				return m_musictrack;
			}
			return string.Empty;
		}
	}

	private void Start()
	{
		source = base.transform.gameObject.GetComponent<AudioSource>();
	}

	public float PlayClip(string clipname, bool loop)
	{
		m_soundclippath = clipname;
		source = base.transform.gameObject.GetComponent<AudioSource>();
		source.Stop();
		clipname.Trim();
		clipname.ToLower();
		source.clip = ResourcesMonitor.Load("Audio/" + clipname, typeof(AudioClip)) as AudioClip;
		if (source.clip == null)
		{
			Debug.LogWarning("the clip " + clipname + " was not found in the Resource folder");
			GameManager.The.StopClip(this);
			return 0f;
		}
		source.loop = loop;
		source.Play();
		if (!loop)
		{
			Invoke("StopClip", source.clip.length);
		}
		return source.clip.length;
	}

	public float PlayMusic(string trackname)
	{
		source = base.transform.gameObject.GetComponent<AudioSource>();
		trackname.Trim();
		trackname.ToLower();
		m_musictrack = trackname;
		source.clip = ResourcesMonitor.Load("Audio/Music/" + trackname, typeof(AudioClip)) as AudioClip;
		if (source.clip == null)
		{
			Debug.LogWarning("the Music track " + trackname + " was not found in the Resource folder");
			return 0f;
		}
		source.loop = true;
		source.volume = 0f;
		FadeIn();
		source.Play();
		return source.clip.length;
	}

	public void ResetTrack()
	{
		source.Play();
	}

	public void StopClip()
	{
		GameManager.The.StopClip(this);
	}

	public void FadeOut()
	{
		starttime = Time.time;
		state = SoundState.fadingOut;
	}

	public void FadeIn()
	{
		starttime = Time.time;
		state = SoundState.fadingIn;
	}

	private void Update()
	{
		switch (state)
		{
		case SoundState.fadingIn:
			if (source.volume < 1f)
			{
				source.volume += (Time.time - starttime) * 0.05f;
			}
			else
			{
				state = SoundState.standard;
			}
			break;
		case SoundState.fadingOut:
			if (source.volume > 0f)
			{
				source.volume -= (Time.time - starttime) * 0.05f;
			}
			else if (b_BGMusic)
			{
				source.clip = null;
			}
			else
			{
				GameManager.The.StopClip(this);
			}
			break;
		}
	}
}
