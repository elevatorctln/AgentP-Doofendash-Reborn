using System.Collections.Generic;
using UnityEngine;

public abstract class UIAbstractContainer : UIObject, IPositionable
{
	public enum UIContainerAlignMode
	{
		Left = 0,
		Center = 1,
		Right = 2
	}

	public enum UIContainerVerticalAlignMode
	{
		Top = 0,
		Middle = 1,
		Bottom = 2
	}

	public enum UILayoutType
	{
		Horizontal = 0,
		Vertical = 1,
		BackgroundLayout = 2,
		AbsoluteLayout = 3
	}

	private UIContainerAlignMode _alignMode;

	private UIContainerVerticalAlignMode _verticalAlignMode;

	private UILayoutType _layoutType;

	protected int _spacing;

	public UIEdgeInsets _edgeInsets;

	protected float _scrollPosition;

	protected List<UISprite> _children = new List<UISprite>();

	protected bool _suspendUpdates;

	protected float _contentWidth;

	protected float _contentHeight;

	private bool _hidden;

	public UIContainerAlignMode alignMode
	{
		get
		{
			return _alignMode;
		}
		set
		{
			_alignMode = value;
			layoutChildren();
		}
	}

	public UIContainerVerticalAlignMode verticalAlignMode
	{
		get
		{
			return _verticalAlignMode;
		}
		set
		{
			_verticalAlignMode = value;
			layoutChildren();
		}
	}

	public UILayoutType layoutType
	{
		get
		{
			return _layoutType;
		}
		set
		{
			_layoutType = value;
			layoutChildren();
		}
	}

	public int spacing
	{
		get
		{
			return _spacing;
		}
		set
		{
			_spacing = value;
			layoutChildren();
		}
	}

	public UIEdgeInsets edgeInsets
	{
		get
		{
			return _edgeInsets;
		}
		set
		{
			_edgeInsets = value;
			layoutChildren();
		}
	}

	public virtual bool hidden
	{
		get
		{
			return _hidden;
		}
		set
		{
			if (value == _hidden)
			{
				return;
			}
			_hidden = value;
			foreach (UISprite child in _children)
			{
				child.hidden = value;
			}
		}
	}

	public UIAbstractContainer()
		: this(UILayoutType.Vertical)
	{
	}

	public UIAbstractContainer(UILayoutType layoutType)
	{
		_layoutType = layoutType;
		base.onTransformChanged += transformChanged;
	}

	public virtual void addChild(params UISprite[] children)
	{
		foreach (UISprite uISprite in children)
		{
			uISprite.parentUIObject = this;
			_children.Add(uISprite);
		}
		layoutChildren();
	}

	public void removeChild(UISprite child, bool removeFromManager)
	{
		_children.Remove(child);
		layoutChildren();
		if (removeFromManager)
		{
			child.manager.removeElement(child);
		}
	}

	public void beginUpdates()
	{
		_suspendUpdates = true;
	}

	public virtual void endUpdates()
	{
		_suspendUpdates = false;
		layoutChildren();
	}

	protected virtual void layoutChildren()
	{
		if (_suspendUpdates)
		{
			return;
		}
		float num = 1f / (float)UI.scaleFactor;
		if (_layoutType == UILayoutType.Horizontal || _layoutType == UILayoutType.Vertical)
		{
			_contentWidth = _edgeInsets.left;
			_contentHeight = _edgeInsets.top;
			UIAnchorInfo uIAnchorInfo = UIAnchorInfo.DefaultAnchorInfo();
			uIAnchorInfo.ParentUIObject = this;
			if (_layoutType == UILayoutType.Horizontal)
			{
				switch (_verticalAlignMode)
				{
				case UIContainerVerticalAlignMode.Top:
					uIAnchorInfo.UIyAnchor = UIyAnchor.Top;
					uIAnchorInfo.ParentUIyAnchor = UIyAnchor.Top;
					uIAnchorInfo.OffsetY = (float)_edgeInsets.top * num;
					break;
				case UIContainerVerticalAlignMode.Middle:
					uIAnchorInfo.UIyAnchor = UIyAnchor.Center;
					uIAnchorInfo.ParentUIyAnchor = UIyAnchor.Center;
					break;
				case UIContainerVerticalAlignMode.Bottom:
					uIAnchorInfo.UIyAnchor = UIyAnchor.Bottom;
					uIAnchorInfo.ParentUIyAnchor = UIyAnchor.Bottom;
					uIAnchorInfo.OffsetY = (float)_edgeInsets.bottom * num;
					break;
				}
				int num2 = 0;
				int count = _children.Count;
				foreach (UISprite child in _children)
				{
					if (num2 != 0 && num2 != count)
					{
						_contentWidth += _spacing;
					}
					uIAnchorInfo.OffsetX = (_contentWidth + _scrollPosition) * num;
					uIAnchorInfo.OriginUIxAnchor = child.anchorInfo.OriginUIxAnchor;
					uIAnchorInfo.OriginUIyAnchor = child.anchorInfo.OriginUIyAnchor;
					child.anchorInfo = uIAnchorInfo;
					_contentWidth += child.width;
					if (_contentHeight < child.height)
					{
						_contentHeight = child.height;
					}
					num2++;
				}
			}
			else
			{
				switch (_alignMode)
				{
				case UIContainerAlignMode.Left:
					uIAnchorInfo.UIxAnchor = UIxAnchor.Left;
					uIAnchorInfo.ParentUIxAnchor = UIxAnchor.Left;
					uIAnchorInfo.OffsetX = (float)_edgeInsets.left * num;
					break;
				case UIContainerAlignMode.Center:
					uIAnchorInfo.UIxAnchor = UIxAnchor.Center;
					uIAnchorInfo.ParentUIxAnchor = UIxAnchor.Center;
					break;
				case UIContainerAlignMode.Right:
					uIAnchorInfo.UIxAnchor = UIxAnchor.Right;
					uIAnchorInfo.ParentUIxAnchor = UIxAnchor.Right;
					uIAnchorInfo.OffsetX = (float)_edgeInsets.right * num;
					break;
				}
				int num3 = 0;
				int count2 = _children.Count;
				foreach (UISprite child2 in _children)
				{
					if (num3 != 0 && num3 != count2)
					{
						_contentHeight += _spacing;
					}
					uIAnchorInfo.OffsetY = (_contentHeight + _scrollPosition) * num;
					uIAnchorInfo.OriginUIxAnchor = child2.anchorInfo.OriginUIxAnchor;
					uIAnchorInfo.OriginUIyAnchor = child2.anchorInfo.OriginUIyAnchor;
					child2.anchorInfo = uIAnchorInfo;
					_contentHeight += child2.height;
					if (_contentWidth < child2.width)
					{
						_contentWidth = child2.width;
					}
					num3++;
				}
			}
			_contentWidth += _edgeInsets.right;
			_contentHeight += _edgeInsets.bottom;
		}
		else if (_layoutType == UILayoutType.AbsoluteLayout)
		{
			foreach (UISprite child3 in _children)
			{
				if (!child3.hidden)
				{
					child3.localPosition = new Vector3(child3.position.x, child3.position.y, child3.position.z);
					if (_contentWidth < child3.localPosition.x + child3.width)
					{
						_contentWidth = child3.localPosition.x + child3.width;
					}
					if (_contentHeight < 0f - child3.localPosition.y + child3.height)
					{
						_contentHeight = 0f - child3.localPosition.y + child3.height;
					}
				}
			}
		}
		foreach (UISprite child4 in _children)
		{
			child4.refreshPosition();
		}
	}

	public override void transformChanged()
	{
		base.transformChanged();
		layoutChildren();
	}

	public void matchSizeToContentSize()
	{
		_width = _contentWidth;
		_height = _contentHeight;
	}
}
