using System.Collections.Generic;
using UnityEngine;

public class UISprite : UIObject, IPositionable
{
	public UIToolkit manager;

	public bool ___hidden;

	public static readonly Rect _rectZero = new Rect(0f, 0f, 0f, 0f);

	private bool _suspendUpdates;

	private float _clippedWidth;

	private float _clippedHeight;

	public bool gameObjectOriginInCenter;

	public Color _color;

	public int index;

	public UIVertexIndices vertexIndices;

	public Vector3 v1 = default(Vector3);

	public Vector3 v2 = default(Vector3);

	public Vector3 v3 = default(Vector3);

	public Vector3 v4 = default(Vector3);

	protected UIUVRect _uvFrame;

	protected UIUVRect _uvFrameClipped;

	protected bool _clipped;

	private float _clippedTopYOffset;

	private float _clippedLeftXOffset;

	protected Vector3[] meshVerts;

	protected Vector2[] uvs;

	protected Dictionary<string, UISpriteAnimation> spriteAnimations;

	public bool ___hiddenOutsideOfLayoutContainer;

	public new float width
	{
		get
		{
			return _width * scale.x;
		}
	}

	public new float height
	{
		get
		{
			return _height * scale.y;
		}
	}

	public virtual UIUVRect uvFrame
	{
		get
		{
			return (!_clipped) ? _uvFrame : _uvFrameClipped;
		}
		set
		{
			if (_uvFrame != value)
			{
				_uvFrame = value;
				if (_clipped)
				{
					float num = _uvFrameClipped.getWidth(manager.textureSize);
					float num2 = _uvFrameClipped.getHeight(manager.textureSize);
					_uvFrameClipped = value.rectClippedToBounds(num, num2, _uvFrameClipped.clippingPlane, manager.textureSize);
				}
				manager.updateUV(this);
			}
		}
	}

	public virtual UIUVRect uvFrameClipped
	{
		get
		{
			return _uvFrameClipped;
		}
		set
		{
			_uvFrameClipped = value;
			_clipped = value != UIUVRect.zero;
			manager.updateUV(this);
		}
	}

	public virtual bool hiddenOutsideOfLayoutContainer
	{
		get
		{
			return ___hiddenOutsideOfLayoutContainer;
		}
		set
		{
			___hiddenOutsideOfLayoutContainer = value;
		}
	}

	public virtual bool hidden
	{
		get
		{
			return ___hidden;
		}
		set
		{
			if (value != ___hidden)
			{
				if (value)
				{
					hiddenOutsideOfLayoutContainer = true;
					manager.hideSprite(this);
				}
				else
				{
					manager.showSprite(this);
				}
			}
		}
	}

	public bool clipped
	{
		get
		{
			return _clipped;
		}
		set
		{
			if (value != _clipped)
			{
				_clipped = value;
				_clippedTopYOffset = (_clippedLeftXOffset = 0f);
				updateVertPositions();
				manager.updateUV(this);
			}
		}
	}

	public override Vector3 position
	{
		get
		{
			return clientTransform.position;
		}
		set
		{
			base.position = value;
			updateTransform();
		}
	}

	public override Vector3 localPosition
	{
		get
		{
			return clientTransform.localPosition;
		}
		set
		{
			base.localPosition = value;
			updateTransform();
		}
	}

	public override Vector3 eulerAngles
	{
		get
		{
			return clientTransform.eulerAngles;
		}
		set
		{
			base.eulerAngles = value;
			updateTransform();
		}
	}

	public override Vector3 scale
	{
		get
		{
			return base.scale;
		}
		set
		{
			base.scale = value;
			updateTransform();
		}
	}

	public override Vector3 localScale
	{
		get
		{
			return clientTransform.localScale;
		}
		set
		{
			base.localScale = value;
			updateTransform();
		}
	}

	public override Color color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			manager.updateColors(this);
		}
	}

	public UISprite()
	{
	}

	public UISprite(Rect frame, int depth, UIUVRect uvFrame)
		: this(frame, depth, uvFrame, false)
	{
	}

	public UISprite(Rect frame, int depth, UIUVRect uvFrame, bool gameObjectOriginInCenter)
	{
		this.gameObjectOriginInCenter = gameObjectOriginInCenter;
		if (gameObjectOriginInCenter)
		{
			_anchorInfo.OriginUIxAnchor = UIxAnchor.Center;
			_anchorInfo.OriginUIyAnchor = UIyAnchor.Center;
		}
		base.client.transform.position = new Vector3(frame.x, 0f - frame.y, depth);
		_width = frame.width;
		_height = frame.height;
		_uvFrame = uvFrame;
	}

	public virtual void setSpriteImage(string filename)
	{
		uvFrame = manager.uvRectForFilename(filename);
	}

	public void beginUpdates()
	{
		_suspendUpdates = true;
	}

	public void endUpdates()
	{
		_suspendUpdates = false;
		updateVertPositions();
		updateTransform();
	}

	public override void transformChanged()
	{
		base.transformChanged();
		updateTransform();
	}

	public void initializeSize()
	{
		setSize(width, height);
		manager.updateUV(this);
	}

	public virtual void setSize(float width, float height)
	{
		_width = width;
		_height = height;
		updateVertPositions();
		updateTransform();
	}

	public void updateVertPositions()
	{
		float num = ((!_clipped) ? _width : _clippedWidth);
		float num2 = ((!_clipped) ? _height : _clippedHeight);
		if (gameObjectOriginInCenter)
		{
			v1 = new Vector3((0f - num) / 2f + _clippedLeftXOffset, num2 / 2f - _clippedTopYOffset, 0f);
			v2 = new Vector3((0f - num) / 2f + _clippedLeftXOffset, (0f - num2) / 2f - _clippedTopYOffset, 0f);
			v3 = new Vector3(num / 2f + _clippedLeftXOffset, (0f - num2) / 2f - _clippedTopYOffset, 0f);
			v4 = new Vector3(num / 2f + _clippedLeftXOffset, num2 / 2f - _clippedTopYOffset, 0f);
		}
		else
		{
			v1 = new Vector3(_clippedLeftXOffset, 0f - _clippedTopYOffset, 0f);
			v2 = new Vector3(_clippedLeftXOffset, 0f - num2 - _clippedTopYOffset, 0f);
			v3 = new Vector3(num + _clippedLeftXOffset, 0f - num2 - _clippedTopYOffset, 0f);
			v4 = new Vector3(num + _clippedLeftXOffset, 0f - _clippedTopYOffset, 0f);
		}
	}

	public void setClippedSize(float width, float height, UIClippingPlane clippingPlane)
	{
		_clippedWidth = width;
		_clippedHeight = height;
		switch (clippingPlane)
		{
		case UIClippingPlane.Left:
			_clippedLeftXOffset = _width - _clippedWidth;
			break;
		case UIClippingPlane.Right:
			_clippedLeftXOffset = 0f;
			break;
		case UIClippingPlane.Top:
			_clippedTopYOffset = _height - _clippedHeight;
			break;
		case UIClippingPlane.Bottom:
			_clippedTopYOffset = 0f;
			break;
		}
		updateVertPositions();
		updateTransform();
	}

	public void setBuffers(Vector3[] v, Vector2[] uv)
	{
		meshVerts = v;
		uvs = uv;
	}

	public virtual void updateTransform()
	{
		if (!hidden && !_suspendUpdates)
		{
			meshVerts[vertexIndices.mv.one] = clientTransform.TransformPoint(v1);
			meshVerts[vertexIndices.mv.two] = clientTransform.TransformPoint(v2);
			meshVerts[vertexIndices.mv.three] = clientTransform.TransformPoint(v3);
			meshVerts[vertexIndices.mv.four] = clientTransform.TransformPoint(v4);
			manager.updatePositions();
		}
	}

	public virtual void centerize()
	{
		if (!gameObjectOriginInCenter)
		{
			Vector3 vector = clientTransform.position;
			vector.x += _width / 2f;
			vector.y -= _height / 2f;
			clientTransform.position = vector;
			gameObjectOriginInCenter = true;
			_anchorInfo.OriginUIxAnchor = UIxAnchor.Center;
			_anchorInfo.OriginUIyAnchor = UIyAnchor.Center;
			setSize(_width, _height);
		}
	}

	public virtual void destroy()
	{
		manager.removeElement(this);
	}

	public UISpriteAnimation addSpriteAnimation(string name, float frameTime, params string[] filenames)
	{
		if (spriteAnimations == null)
		{
			spriteAnimations = new Dictionary<string, UISpriteAnimation>();
		}
		List<UIUVRect> list = new List<UIUVRect>(filenames.Length);
		foreach (string filename in filenames)
		{
			list.Add(manager.uvRectForFilename(filename));
		}
		UISpriteAnimation uISpriteAnimation = new UISpriteAnimation(frameTime, list);
		spriteAnimations[name] = uISpriteAnimation;
		return uISpriteAnimation;
	}

	public void playSpriteAnimation(string name, int loopCount)
	{
		playSpriteAnimation(spriteAnimations[name], loopCount);
	}

	public void playSpriteAnimation(UISpriteAnimation anim, int loopCount)
	{
		UI.instance.StartCoroutine(anim.play(this, loopCount));
	}
}
