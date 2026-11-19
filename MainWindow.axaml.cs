using Avalonia.Controls;

namespace AvaloniaComboBoxScrollRepro;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ProblemPage.Configure("问题页：Popup.ShouldUseOverlayLayer=True", false);
        FixedPage.Configure("修复页：拦截弹层 BringIntoView", true);
    }
}
