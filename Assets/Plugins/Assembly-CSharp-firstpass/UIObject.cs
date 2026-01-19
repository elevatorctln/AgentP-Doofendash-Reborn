using System;
using UnityEngine;

public class UIObject : IPositionable
{
	public delegate void UIObjectTransformChangedDelegate();

	private GameObject _client;

	protected Transform clientTransform;

	private UIObject _parentUIObject;

	protected UIAnchorInfo _anchorInfo;

	public bool autoRefreshPositionOnScaling = true;

	protected float _width;

	protected float _height;

	public object userData;

	public GameObject client
	{
		get
		{
			return _client;
		}
	}

	public virtual Color color { get; set; }

	public virtual float zIndex
	{
		get
		{
			return clientTransform.position.z;
		}
		set
		{
			Vector3 vector = clientTransform.position;
			vector.z = value;
			clientTransform.position = vector;
		}
	}

	public virtual Vector3 position
	{
		get
		{
			return clientTransform.position;
		}
		set
		{
			clientTransform.position = value;
			if (this.onTransformChanged != null)
			{
				this.onTransformChanged();
			}
		}
	}

	public virtual Vector3 localPosition
	{
		get
		{
			return clientTransform.localPosition;
		}
		set
		{
			clientTransform.localPosition = value;
			if (this.onTransformChanged != null)
			{
				this.onTransformChanged();
			}
		}
	}

	public virtual Vector3 eulerAngles
	{
		get
		{
			return clientTransform.eulerAngles;
		}
		set
		{
			clientTransform.eulerAngles = value;
			if (this.onTransformChanged != null)
			{
				this.onTransformChanged();
			}
		}
	}

	public virtual Vector3 scale
	{
		get
		{
			Vector3 result = clientTransform.localScale;
			if (_parentUIObject != null)
			{
				Vector3 vector = _parentUIObject.scale;
				result.x *= vector.x;
				result.y *= vector.y;
				result.z *= vector.z;
			}
			return result;
		}
		set
		{
			Vector3 vector = value;
			if (_parentUIObject != null)
			{
				Vector3 vector2 = _parentUIObject.scale;
				vector.x /= vector2.x;
				vector.y /= vector2.y;
				vector.z /= vector2.z;
			}
			clientTransform.localScale = vector;
			if (autoRefreshPositionOnScaling)
			{
				this.refreshPosition();
			}
			else if (this.onTransformChanged != null)
			{
				this.onTransformChanged();
			}
		}
	}

	public virtual Vector3 localScale
	{
		get
		{
			return clientTransform.localScale;
		}
		set
		{
			clientTransform.localScale = value;
			if (autoRefreshPositionOnScaling)
			{
				this.refreshPosition();
			}
			else if (this.onTransformChanged != null)
			{
				this.onTransformChanged();
			}
		}
	}

	public virtual Transform parent
	{
		get
		{
			return clientTransform.parent;
		}
		set
		{
			clientTransform.parent = value;
		}
	}

	public UIObject parentUIObject
	{
		get
		{
			return _parentUIObject;
		}
		set
		{
			if (value != _parentUIObject)
			{
				if (_parentUIObject != null)
				{
					UIObject uIObject = _parentUIObject;
					uIObject.onTransformChanged = (UIObjectTransformChangedDelegate)Delegate.Remove(uIObject.onTransformChanged, new UIObjectTransformChangedDelegate(transformChanged));
				}
				_parentUIObject = value;
				if (_parentUIObject != null)
				{
					clientTransform.parent = _parentUIObject.clientTransform;
					UIObject uIObject2 = _parentUIObject;
					uIObject2.onTransformChanged = (UIObjectTransformChangedDelegate)Delegate.Combine(uIObject2.onTransformChanged, new UIObjectTransformChangedDelegate(transformChanged));
				}
				else if (this is UISprite)
				{
					clientTransform.parent = ((UISprite)this).manager.transform;
				}
				else if (this is UITextInstance)
				{
					clientTransform.parent = ((UITextInstance)this).manager.transform;
				}
				else
				{
					clientTransform.parent = null;
				}
				_anchorInfo.ParentUIObject = _parentUIObject;
				this.refreshAnchorInformation();
			}
		}
	}

	public virtual float width
	{
		get
		{
			return _width;
		}
	}

	public virtual float height
	{
		get
		{
			return _height;
		}
	}

	public UIAnchorInfo anchorInfo
	{
		get
		{
			return _anchorInfo;
		}
		set
		{
			_anchorInfo = value;
		}
	}

	public event UIObjectTransformChangedDelegate onTransformChanged;

	public UIObject()
	{
		_client = new GameObject(GetType().Name);
		_client.transform.parent = UI.instance.transform;
		_client.layer = UI.instance.layer;
		UIElement uIElement = _client.AddComponent<UIElement>();
		uIElement.UIObject = this;
		clientTransform = _client.transform;
		_anchorInfo = UIAnchorInfo.DefaultAnchorInfo();
	}

	public virtual void transformChanged()
	{
	}
}
