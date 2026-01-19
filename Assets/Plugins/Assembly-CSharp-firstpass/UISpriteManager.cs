using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UISpriteManager : MonoBehaviour
{
	public enum UISpriteWindowOrder
	{
		CCW = 0,
		CW = 1
	}

	public string texturePackerConfigName;

	public Material material;

	public int layer;

	protected bool meshIsDirty;

	protected bool vertsChanged;

	protected bool uvsChanged;

	protected bool colorsChanged;

	protected bool vertCountChanged;

	protected bool updateBounds;

	private UISprite[] _sprites = new UISprite[0];

	private UISpriteWindowOrder winding;

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private Mesh[] _mesh = new Mesh[2];

	private int meshIndex;

	[HideInInspector]
	public Vector2 textureSize = Vector2.zero;

	protected Vector3[] vertices = new Vector3[0];

	protected int[] triIndices = new int[0];

	protected Vector2[] UVs = new Vector2[0];

	protected Color[] colors = new Color[0];

	[HideInInspector]
	public Dictionary<string, UITextureInfo> textureDetails;

	protected virtual void Awake()
	{
		_meshFilter = base.gameObject.AddComponent<MeshFilter>();
		_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		Material material = new Material(this.material.shader);
		material.CopyPropertiesFromMaterial(this.material);
		this.material = material;
		_meshRenderer.GetComponent<Renderer>().material = this.material;
		_mesh[0] = _meshFilter.mesh;
		_mesh[1] = new Mesh();
		base.transform.position = new Vector3(0f, 0f, layer);
		base.transform.rotation = Quaternion.identity;
	}

	public void loadTextureAndPrepareForUse()
	{
		Debug.Log("Loading textures and preparing for use...");
		if (UI.isHD)
		{
			texturePackerConfigName += UI.instance.hdExtension;
		}
		Texture texture = (Texture)Resources.Load(texturePackerConfigName, typeof(Texture));
		Debug.Log("Texture is set to " + texture);
		if (texture == null)
		{
			Debug.LogError("UI texture is being autoloaded and it doesn't exist.  Cannot find texturePackerConfigName: " + texturePackerConfigName);
		}
		material.mainTexture = texture;
		Texture mainTexture = material.mainTexture;
		textureSize = new Vector2(mainTexture.width, mainTexture.height);
		textureDetails = loadTexturesFromTexturePackerJSON(texturePackerConfigName, textureSize);
	}

	public static Dictionary<string, UITextureInfo> loadTexturesFromTexturePackerJSON(string filename, Vector2 textureSize)
	{
		Dictionary<string, UITextureInfo> dictionary = new Dictionary<string, UITextureInfo>();
		TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Could not find Texture Packer json config file in Resources folder: " + filename);
		}
		string text = textAsset.text;
		Hashtable hashtable = text.hashtableFromJson();
		IDictionary dictionary2 = (IDictionary)hashtable["frames"];
		foreach (DictionaryEntry item in dictionary2)
		{
			IDictionary dictionary3 = (IDictionary)((IDictionary)item.Value)["frame"];
			int num = int.Parse(dictionary3["x"].ToString());
			int num2 = int.Parse(dictionary3["y"].ToString());
			int num3 = int.Parse(dictionary3["w"].ToString());
			int num4 = int.Parse(dictionary3["h"].ToString());
			IDictionary dictionary4 = (IDictionary)((IDictionary)item.Value)["spriteSourceSize"];
			int num5 = int.Parse(dictionary4["x"].ToString());
			int num6 = int.Parse(dictionary4["y"].ToString());
			int num7 = int.Parse(dictionary4["w"].ToString());
			int num8 = int.Parse(dictionary4["h"].ToString());
			IDictionary dictionary5 = (IDictionary)((IDictionary)item.Value)["sourceSize"];
			int num9 = int.Parse(dictionary5["w"].ToString());
			int num10 = int.Parse(dictionary5["h"].ToString());
			bool trimmed = (bool)((IDictionary)item.Value)["trimmed"];
			bool rotated = (bool)((IDictionary)item.Value)["rotated"];
			dictionary.Add(value: new UITextureInfo
			{
				frame = new Rect(num, num2, num3, num4),
				uvRect = new UIUVRect(num, num2, num3, num4, textureSize),
				spriteSourceSize = new Rect(num5, num6, num7, num8),
				sourceSize = new Vector2(num9, num10),
				trimmed = trimmed,
				rotated = rotated
			}, key: item.Key.ToString());
		}
		textAsset = null;
		Resources.UnloadUnusedAssets();
		return dictionary;
	}

	public UITextureInfo textureInfoForFilename(string filename)
	{
		return textureDetails[filename];
	}

	public UIUVRect uvRectForFilename(string filename)
	{
		return textureDetails[filename].uvRect;
	}

	public Rect frameForFilename(string filename)
	{
		return textureDetails[filename].frame;
	}

	protected int expandMaxSpriteLimit(int count)
	{
		int num = _sprites.Length;
		UISprite[] sprites = _sprites;
		_sprites = new UISprite[_sprites.Length + count];
		sprites.CopyTo(_sprites, 0);
		Vector3[] array = vertices;
		vertices = new Vector3[vertices.Length + count * 4];
		array.CopyTo(vertices, 0);
		Vector2[] uVs = UVs;
		UVs = new Vector2[UVs.Length + count * 4];
		uVs.CopyTo(UVs, 0);
		Color[] array2 = colors;
		colors = new Color[colors.Length + count * 4];
		array2.CopyTo(colors, 0);
		int[] array3 = triIndices;
		triIndices = new int[triIndices.Length + count * 6];
		array3.CopyTo(triIndices, 0);
		for (int i = 0; i < num; i++)
		{
			_sprites[i].setBuffers(vertices, UVs);
		}
		for (int j = num; j < _sprites.Length; j++)
		{
			if (winding == UISpriteWindowOrder.CCW)
			{
				triIndices[j * 6] = j * 4;
				triIndices[j * 6 + 1] = j * 4 + 1;
				triIndices[j * 6 + 2] = j * 4 + 3;
				triIndices[j * 6 + 3] = j * 4 + 3;
				triIndices[j * 6 + 4] = j * 4 + 1;
				triIndices[j * 6 + 5] = j * 4 + 2;
			}
			else
			{
				triIndices[j * 6] = j * 4;
				triIndices[j * 6 + 1] = j * 4 + 3;
				triIndices[j * 6 + 2] = j * 4 + 1;
				triIndices[j * 6 + 3] = j * 4 + 3;
				triIndices[j * 6 + 4] = j * 4 + 2;
				triIndices[j * 6 + 5] = j * 4 + 1;
			}
		}
		vertsChanged = true;
		uvsChanged = true;
		colorsChanged = true;
		vertCountChanged = true;
		meshIsDirty = true;
		return num;
	}

	protected void updateMeshProperties()
	{
		meshIndex = (meshIndex + 1) % 2;
		Mesh mesh = _mesh[meshIndex];
		_meshFilter.mesh = mesh;
		vertCountChanged = false;
		colorsChanged = false;
		vertsChanged = false;
		uvsChanged = false;
		updateBounds = false;
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = UVs;
		mesh.colors = colors;
		mesh.triangles = triIndices;
	}

	public UISprite addSprite(string name, int xPos, int yPos)
	{
		return addSprite(name, xPos, yPos, 1, false);
	}

	public UISprite addSprite(string name, int xPos, int yPos, int depth)
	{
		return addSprite(name, xPos, yPos, depth, false);
	}

	public UISprite addSprite(string name, int xPos, int yPos, int depth, bool gameObjectOriginInCenter)
	{
		UITextureInfo uITextureInfo = textureDetails[name];
		Rect frame = new Rect(xPos, yPos, uITextureInfo.frame.width, uITextureInfo.frame.height);
		return addSprite(frame, uITextureInfo.uvRect, depth, gameObjectOriginInCenter);
	}

	private UISprite addSprite(Rect frame, UIUVRect uvFrame, int depth, bool gameObjectOriginInCenter)
	{
		UISprite uISprite = new UISprite(frame, depth, uvFrame, gameObjectOriginInCenter);
		addSprite(uISprite);
		return uISprite;
	}

	public void addSprite(UISprite sprite)
	{
		int i;
		for (i = 0; i < _sprites.Length && _sprites[i] != null; i++)
		{
		}
		if (i == _sprites.Length)
		{
			i = expandMaxSpriteLimit(5);
		}
		_sprites[i] = sprite;
		sprite.index = i;
		sprite.manager = this as UIToolkit;
		sprite.parent = base.transform;
		sprite.setBuffers(vertices, UVs);
		sprite.vertexIndices.initializeVertsWithIndex(i);
		sprite.initializeSize();
		sprite.color = Color.white;
		vertsChanged = true;
		uvsChanged = true;
		meshIsDirty = true;
	}

	protected void removeSprite(UISprite sprite)
	{
		vertices[sprite.vertexIndices.mv.one] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.two] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.three] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.four] = Vector3.zero;
		_sprites[sprite.index] = null;
		sprite.parentUIObject = null;
		Object.Destroy(sprite.client);
		vertsChanged = true;
		meshIsDirty = true;
	}

	public void hideSprite(UISprite sprite)
	{
		sprite.___hidden = true;
		vertices[sprite.vertexIndices.mv.one] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.two] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.three] = Vector3.zero;
		vertices[sprite.vertexIndices.mv.four] = Vector3.zero;
		vertsChanged = true;
		meshIsDirty = true;
	}

	public void showSprite(UISprite sprite)
	{
		if (sprite.___hidden)
		{
			sprite.___hiddenOutsideOfLayoutContainer = false;
			sprite.___hidden = false;
			sprite.updateTransform();
		}
	}

	public void updateUV(UISprite sprite)
	{
		UVs[sprite.vertexIndices.uv.one] = sprite.uvFrame.lowerLeftUV + Vector2.up * sprite.uvFrame.uvDimensions.y;
		UVs[sprite.vertexIndices.uv.two] = sprite.uvFrame.lowerLeftUV;
		UVs[sprite.vertexIndices.uv.three] = sprite.uvFrame.lowerLeftUV + Vector2.right * sprite.uvFrame.uvDimensions.x;
		UVs[sprite.vertexIndices.uv.four] = sprite.uvFrame.lowerLeftUV + sprite.uvFrame.uvDimensions;
		uvsChanged = true;
		meshIsDirty = true;
	}

	public void updateColors(UISprite sprite)
	{
		colors[sprite.vertexIndices.cv.one] = sprite.color;
		colors[sprite.vertexIndices.cv.two] = sprite.color;
		colors[sprite.vertexIndices.cv.three] = sprite.color;
		colors[sprite.vertexIndices.cv.four] = sprite.color;
		colorsChanged = true;
		meshIsDirty = true;
	}

	public void updatePositions()
	{
		vertsChanged = true;
		meshIsDirty = true;
	}
}
