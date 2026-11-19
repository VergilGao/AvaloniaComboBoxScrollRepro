using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace AvaloniaComboBoxScrollRepro;

public sealed partial class ReproPage : UserControl
{
    private bool applyFix;

    public ReproPage()
    {
        InitializeComponent();

        /*
         * BringIntoView 从 ComboBoxItem 冒泡到页面滚动器时，TabControl 的
         * ContentPresenter 会让页面滚动器接受这个不属于其内容的请求。
         * 必须在 ScrollViewer.Content 内部先拦截，因为挂到 ScrollViewer
         * 本身的处理器执行时，页面偏移已经改变。
         */
        ContentRoot.AddHandler(
            RequestBringIntoViewEvent,
            OnRequestBringIntoView,
            RoutingStrategies.Bubble
        );
    }

    public void Configure(string title, bool fixEnabled)
    {
        PageTitle.Text = title;
        applyFix = fixEnabled;
    }

    private void OnPageScrollChanged(object? sender, ScrollChangedEventArgs e) =>
        OffsetText.Text = $"Offset.Y = {PageScroll.Offset.Y:0}";

    private void OnRequestBringIntoView(object? sender, RequestBringIntoViewEventArgs e)
    {
        if (
            !applyFix
            || e.TargetObject is not Visual target
            || target == ContentRoot
            || target.GetVisualAncestors().Contains(ContentRoot)
        )
        {
            return;
        }

        e.Handled = true;
    }
}
