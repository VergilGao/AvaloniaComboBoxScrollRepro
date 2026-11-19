# Avalonia ComboBox Scroll Repro

最小复现工程：

```powershell
dotnet run --project AvaloniaComboBoxScrollRepro.csproj
```

问题页使用 `TabControl -> ScrollViewer -> ComboBox`，并启用 `Popup.ShouldUseOverlayLayer=True`。

真实 Desktop 平台验证结果：

```text
问题页  Offset.Y: 290 -> 143
修复页  Offset.Y: 290 -> 290
```

Fluent 默认把 Popup 放在独立 PopupRoot 中；Overlay 模式则通过 OverlayPopupHost 挂到
父窗口的视觉树。ComboBox 打开后聚焦选中项发出的 `BringIntoView` 请求因此能够到达
页面 `ScrollContentPresenter`，在 Popup 已经定位后再次滚动页面。

修复页保持相同布局，只在 `ScrollViewer.Conten ` 的根节点拦截目标不属于该内容的
`RequestBringIntoView` 事件，页面偏移保持不变。
