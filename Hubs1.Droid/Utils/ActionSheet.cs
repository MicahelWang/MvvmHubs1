using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using Orientation = Android.Widget.Orientation;

namespace Hubs1.Droid.Utils
{
    public class ActionSheetArgs
    {
        public string Title { get; set; }

        public Action OnClick { get; set; }
        public ActionSheetArgs(string title)
        {
            Title = title;
        }
        public ActionSheetArgs(string title, Action onClick)
        {
            Title = title;
            OnClick = onClick;
        }
    }
    public sealed class ActionSheet : Dialog
    {

        #region  控件的id 
        private const int CancelButtonId = 100;
        private const int BgViewId = 10;
        private const int TranslateDuration = 300;
        private const int AlphaDuration = 300;
        #endregion   控件的id

        #region 属性

        private readonly Context _context;
        private Attributes _attributes;
        private View _mView;
        private LinearLayout _mPanel;
        private View _bgView;
        private string _cancelTitle = "Cancel";
        private bool _cancelableOnTouchOutside;
        private bool _dismissed = true;

        private List<ActionSheetArgs> _items;

        /// <summary>
        /// 是否设置取消按钮
        /// </summary>
        public bool DisplayCancel { get; set; } = true;

        /// <summary>
        /// 是否边缘取消
        /// </summary>
        public bool CancelableOnTouchOutside
        {
            get
            {
                return _cancelableOnTouchOutside;
            }

            set
            {
                if (!value)
                {
                    DisplayCancel = true;
                }
                _cancelableOnTouchOutside = value;
            }
        }


        /// <summary>
        /// ActionSheet项集合
        /// </summary>
        public List<ActionSheetArgs> Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
                Render();
            }
        }


        #endregion
        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context">当前上下文</param>
        public ActionSheet(Context context) : base(context, Android.Resource.Style.ThemeLightNoTitleBar)// 全屏
        {

            _context = context;
            InitViews();
            if (Window == null) return;
            Window.SetGravity(GravityFlags.Bottom);
            Drawable drawable = new ColorDrawable();
            drawable.Alpha = 0;// 去除黑色背景
            Window.SetBackgroundDrawable(drawable);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="argsItems">ActionSheet项集合</param>
        public ActionSheet(Context context, List<ActionSheetArgs> argsItems) : base(context, Android.Resource.Style.ThemeLightNoTitleBar)// 全屏
        {

            _context = context;
            InitViews();
            Items = argsItems;
            if (Window == null) return;
            Window.SetGravity(GravityFlags.Bottom);
            Drawable drawable = new ColorDrawable();
            drawable.Alpha = 0;// 去除黑色背景
            Window.SetBackgroundDrawable(drawable);
        }


        #endregion 构造方法
        private void InitViews()
        {
            /* 隐藏软键盘 */
            var imm = (InputMethodManager)_context.GetSystemService(Context.InputMethodService);
            if (imm.IsActive)
            {
                View focusView = ((Activity)_context).CurrentFocus;
                if (focusView != null)
                    imm.HideSoftInputFromWindow(focusView.WindowToken, 0);
            }
            _attributes = ReadAttribute();// 获取主题属性
            _mView = CreateView();
            _bgView.StartAnimation(CreateAlphaInAnimation());
            _mPanel.StartAnimation(CreateTranslationInAnimation());
        }

        private Animation CreateTranslationInAnimation()
        {
            const Dimension type = Dimension.RelativeToSelf; //TranslateAnimation.RELATIVE_TO_SELF;
            var an = new TranslateAnimation(type, 0, type, 0, type, 1, type, 0)
            {
                Duration = TranslateDuration
            };
            return an;
        }

        private Animation CreateAlphaInAnimation()
        {
            var an = new AlphaAnimation(0, 1)
            {
                Duration = AlphaDuration
            };
            return an;
        }

        private void Render()
        {
            if (Items != null && Items.Count > 0)
            {
                CreateItems();
            }
        }

        private Animation CreateTranslationOutAnimation()
        {
            const Dimension type = Dimension.RelativeToSelf;
            var an = new TranslateAnimation(type, 0, type, 0, type, 0, type, 1)
            {
                Duration = TranslateDuration,
                FillAfter = true
            };
            return an;
        }

        private Animation CreateAlphaOutAnimation()
        {
            var an = new AlphaAnimation(1, 0)
            {
                Duration = AlphaDuration,
                FillAfter = true
            };
            return an;
        }

        /// <summary>
        /// 创建基本的背景视图
        /// </summary>
        /// <returns></returns>
        private View CreateView()
        {
            var parent = new FrameLayout(_context);
            var parentParams =
                new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            parentParams.Gravity = GravityFlags.Bottom;
            parent.LayoutParameters = parentParams;
            _bgView = new View(_context)
            {
                LayoutParameters =
                    new ActionBar.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };
            _bgView.SetBackgroundColor(Color.Argb(136, 0, 0, 0));
            _bgView.Id = BgViewId;
            _bgView.Click += (sender, args) =>
            {
                if (CancelableOnTouchOutside)
                {
                    //Toast.MakeText(_context, " BgViewId click", 0).Show();
                    DismissMenu();
                }

            };

            _mPanel = new LinearLayout(_context);
            FrameLayout.LayoutParams mPanelParams = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                Gravity = GravityFlags.Bottom
            };
            _mPanel.LayoutParameters = mPanelParams;
            _mPanel.Orientation = Orientation.Vertical;
            parent.AddView(_bgView);
            parent.AddView(_mPanel);
            return parent;
        }

        /// <summary>
        /// 创建MenuItem
        /// </summary>
        private void CreateItems()
        {
            #region 基本项

            if (Items != null && Items.Count > 0)

                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    var itemButton = new Button(_context)
                    {
                        Id = CancelButtonId + i + 1
                    };
                    itemButton.Click += (sender, args) => item.OnClick();
                    itemButton.Tag = "";
                    itemButton.Background = GetOtherButtonBg(Items.Select(m => m.Title).ToArray(), i);
                    itemButton.Text = item.Title;
                    itemButton.SetTextColor(new Color(_attributes.OtherButtonTextColor));
                    itemButton.SetTextSize(ComplexUnitType.Dip, _attributes.ActionSheetTextSize);
                    if (i > 0)
                    {
                        var layoutParams = CreateButtonLayoutParams();
                        layoutParams.TopMargin = _attributes.OtherButtonSpacing;
                        _mPanel.AddView(itemButton, layoutParams);
                    }
                    else
                        _mPanel.AddView(itemButton);
                }

            #endregion 基本项

            #region Cancel按钮

            if (!DisplayCancel) return;
            Button cancelButton = new Button(_context);
            cancelButton.Paint.FakeBoldText = true;
            cancelButton.SetTextSize(ComplexUnitType.Dip, _attributes.ActionSheetTextSize);
            cancelButton.Id = CancelButtonId;
            cancelButton.Background = _attributes.CancelButtonBackground;
            cancelButton.Text = _cancelTitle;
            cancelButton.SetTextColor(new Color(_attributes.CancelButtonTextColor));
            cancelButton.Click += (sender, args) =>
            {
                DismissMenu();
            };
            var param = CreateButtonLayoutParams();
            param.TopMargin = _attributes.CancelButtonMarginTop;
            _mPanel.AddView(cancelButton, param);

            #endregion

            _mPanel.Background = _attributes.Background;
            _mPanel.SetPadding(_attributes.Padding, _attributes.Padding, _attributes.Padding, _attributes.Padding);
        }

        private LinearLayout.LayoutParams CreateButtonLayoutParams()
        {
            var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);
            return layoutParams;
        }

        /// <summary>
        /// item按钮的颜色
        /// </summary>
        /// <param name="titles"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private Drawable GetOtherButtonBg(string[] titles, int i)
        {
            if (titles.Length == 1)
                return _attributes.GetOtherButtonMiddleBackground();
            else if (titles.Length == 2)
                switch (i)
                {
                    case 0:
                        return _attributes.GetOtherButtonMiddleBackground();
                    case 1:
                        return _attributes.GetOtherButtonMiddleBackground();
                }
            else if (titles.Length > 2)
            {
                if (i == 0)
                    return _attributes.GetOtherButtonMiddleBackground();
                else if (i == (titles.Length - 1))
                    return _attributes.GetOtherButtonMiddleBackground();
                return _attributes.GetOtherButtonMiddleBackground();
            }
            return null;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void ShowMenu()
        {
            if (!_dismissed)
                return;
            Show();
            Window.SetContentView(_mView);
            _dismissed = false;
        }

        /// <summary>
        /// dissmiss Menu菜单
        /// </summary>
        public void DismissMenu()
        {
            if (_dismissed)
                return;
            Dismiss();
            OnDismiss();
            _dismissed = true;
        }

        /// <summary>
        /// dismiss时的处理
        /// </summary>
        private void OnDismiss()
        {
            _mPanel.StartAnimation(CreateTranslationOutAnimation());
            _bgView.StartAnimation(CreateAlphaOutAnimation());
        }

        /// <summary>
        /// 设置取消按钮的标题文字
        /// </summary>
        /// <param name="title">标题名称</param>
        /// <returns></returns>
        public ActionSheet SetCancelButtonTitle(string title)
        {
            _cancelTitle = title;
            return this;
        }

        /// <summary>
        /// 设置取消按钮的标题文字
        /// </summary>
        /// <param name="strId">标题资源编号</param>
        /// <returns></returns>
        public ActionSheet SetCancelButtonTitle(int strId)
        {
            return SetCancelButtonTitle(_context.GetString(strId));
        }


        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public ActionSheet AddItems(List<ActionSheetArgs> items)
        {
            if (items == null || items.Count == 0)
                return this;
            Items = items;
            CreateItems();
            return this;
        }

        private Attributes ReadAttribute()
        {
            Attributes attrs = new Attributes(_context);
            TypedArray a = _context.Theme.ObtainStyledAttributes(null, Resource.Styleable.ActionSheet,
                Resource.Attribute.actionSheetStyle, 0);
            Drawable background = a.GetDrawable(Resource.Styleable.ActionSheet_actionSheetBackground);
            if (background != null)
                attrs.Background = background;
            Drawable cancelButtonBackground = a.GetDrawable(Resource.Styleable.ActionSheet_cancelButtonBackground);
            if (cancelButtonBackground != null)
                attrs.CancelButtonBackground = cancelButtonBackground;
            Drawable otherButtonTopBackground = a.GetDrawable(Resource.Styleable.ActionSheet_otherButtonTopBackground);
            if (otherButtonTopBackground != null)
                attrs.OtherButtonTopBackground = otherButtonTopBackground;
            Drawable otherButtonMiddleBackground = a
                .GetDrawable(Resource.Styleable.ActionSheet_otherButtonMiddleBackground);
            if (otherButtonMiddleBackground != null)
                attrs.OtherButtonMiddleBackground = otherButtonMiddleBackground;
            Drawable otherButtonBottomBackground = a
                .GetDrawable(Resource.Styleable.ActionSheet_otherButtonBottomBackground);
            if (otherButtonBottomBackground != null)
                attrs.OtherButtonBottomBackground = otherButtonBottomBackground;
            Drawable otherButtonSingleBackground = a
                .GetDrawable(Resource.Styleable.ActionSheet_otherButtonSingleBackground);
            if (otherButtonSingleBackground != null)
                attrs.OtherButtonSingleBackground = otherButtonSingleBackground;
            attrs.CancelButtonTextColor = a.GetColor(Resource.Styleable.ActionSheet_cancelButtonTextColor,
                attrs.CancelButtonTextColor);
            attrs.OtherButtonTextColor = a.GetColor(Resource.Styleable.ActionSheet_otherButtonTextColor,
                attrs.OtherButtonTextColor);
            attrs.Padding = (int)a.GetDimension(Resource.Styleable.ActionSheet_actionSheetPadding, attrs.Padding);
            attrs.OtherButtonSpacing = (int)a.GetDimension(Resource.Styleable.ActionSheet_otherButtonSpacing,
                attrs.OtherButtonSpacing);
            attrs.CancelButtonMarginTop = (int)a.GetDimension(Resource.Styleable.ActionSheet_cancelButtonMarginTop,
                attrs.CancelButtonMarginTop);
            attrs.ActionSheetTextSize = a.GetDimensionPixelSize(Resource.Styleable.ActionSheet_actionSheetTextSize,
                (int)attrs.ActionSheetTextSize);

            a.Recycle();
            return attrs;
        }

        /// <summary>
        /// 自定义属性的控件主题
        /// </summary>
        private class Attributes
        {
            private readonly Context _mContext;

            public Drawable Background { get; set; }
            public Drawable CancelButtonBackground { get; set; }
            public Drawable OtherButtonTopBackground { get; set; }
            public Drawable OtherButtonMiddleBackground { get; set; }
            public Drawable OtherButtonBottomBackground { get; set; }
            public Drawable OtherButtonSingleBackground { set { } }
            public int CancelButtonTextColor { get; set; }
            public int OtherButtonTextColor { get; set; }
            public int Padding { get; set; }
            public int OtherButtonSpacing { get; set; }
            public int CancelButtonMarginTop { get; set; }
            public float ActionSheetTextSize { get; set; }

            public Attributes(Context context)
            {
                _mContext = context;
                Background = new ColorDrawable(Color.Transparent);
                CancelButtonBackground = new ColorDrawable(Color.Black);
                ColorDrawable gray = new ColorDrawable(Color.Gray);
                OtherButtonTopBackground = gray;
                OtherButtonMiddleBackground = gray;
                OtherButtonBottomBackground = gray;
                OtherButtonSingleBackground = gray;
                CancelButtonTextColor = Color.White;
                OtherButtonTextColor = Color.Black;
                Padding = Dp2Px(20);
                OtherButtonSpacing = Dp2Px(2);
                CancelButtonMarginTop = Dp2Px(10);
                ActionSheetTextSize = Dp2Px(16);
            }

            private int Dp2Px(int dp)
            {
                return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, _mContext.Resources.DisplayMetrics);
            }

            public Drawable GetOtherButtonMiddleBackground()
            {
                if (OtherButtonMiddleBackground is StateListDrawable)
                {
                    TypedArray a = _mContext.Theme.ObtainStyledAttributes(null, Resource.Styleable.ActionSheet,
                        Resource.Attribute.actionSheetStyle, 0);
                    OtherButtonMiddleBackground = a.GetDrawable(Resource.Styleable.ActionSheet_otherButtonMiddleBackground);
                    a.Recycle();
                }
                return OtherButtonMiddleBackground;
            }
        }
    }
}