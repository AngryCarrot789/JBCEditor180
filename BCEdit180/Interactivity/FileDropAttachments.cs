using System;
using System.Windows;
using BCEdit180.Core.Drop;
using BCEdit180.Core.Utils;

namespace BCEdit180.Interactivity {
    public static class FileDropAttachments {
        public static readonly DependencyProperty DropHandlerProperty = DependencyProperty.RegisterAttached("DropHandler", typeof(IDropHandler), typeof(FileDropAttachments), new FrameworkPropertyMetadata(null, OnFileDropNotifierPropertyChanged));
        public static readonly DependencyProperty UsePreviewEventProperty = DependencyProperty.RegisterAttached("UsePreviewEvent", typeof(bool), typeof(FileDropAttachments), new FrameworkPropertyMetadata(BoolBox.False, OnUsePreviewEventPropertyChanged));
        private static readonly DependencyProperty IsProcessingDragDropProcessProperty = DependencyProperty.RegisterAttached("IsProcessingDragDropProcess", typeof(bool), typeof(FileDropAttachments), new PropertyMetadata(BoolBox.False));
        private static readonly DependencyProperty IsProcessingDragDropEnterProperty = DependencyProperty.RegisterAttached("IsProcessingDragDropEnter", typeof(bool), typeof(FileDropAttachments), new PropertyMetadata(BoolBox.False));
        private static readonly DependencyProperty IsProcessingDragDropLeaveProperty = DependencyProperty.RegisterAttached("IsProcessingDragDropLeave", typeof(bool), typeof(FileDropAttachments), new PropertyMetadata(BoolBox.False));
        public static readonly DependencyProperty IsProcessingQueryDragProperty = DependencyProperty.RegisterAttached("IsProcessingQueryDrag", typeof(bool), typeof(FileDropAttachments), new PropertyMetadata(BoolBox.False));
        private static readonly DependencyProperty LastEntryDropEffectsProperty = DependencyProperty.RegisterAttached("LastEntryDropEffects", typeof(DragDropEffects?), typeof(FileDropAttachments), new PropertyMetadata(null));


        public static void SetDropHandler(UIElement element, IDropHandler value) => element.SetValue(DropHandlerProperty, value);
        public static IDropHandler GetDropHandler(UIElement element) => (IDropHandler) element.GetValue(DropHandlerProperty);

        public static void SetUsePreviewEvent(DependencyObject element, bool value) => element.SetValue(UsePreviewEventProperty, value.Box());
        public static bool GetUsePreviewEvent(DependencyObject element) => (bool) element.GetValue(UsePreviewEventProperty);

        private static bool IsProcessingDrop(DependencyObject element) => (bool) element.GetValue(IsProcessingDragDropProcessProperty);
        private static bool IsProcessingDragEnter(DependencyObject element) => (bool) element.GetValue(IsProcessingDragDropEnterProperty);
        private static void SetIsProcessingDragDropLeave(DependencyObject element, bool value) => element.SetValue(IsProcessingDragDropLeaveProperty, value.Box());
        private static bool IsProcessingDragLeave(DependencyObject element) => (bool) element.GetValue(IsProcessingDragDropLeaveProperty);
        private static void SetIsProcessingQueryDrag(DependencyObject element, bool value) => element.SetValue(IsProcessingQueryDragProperty, value.Box());
        private static bool GetIsProcessingQueryDrag(DependencyObject element) => (bool) element.GetValue(IsProcessingQueryDragProperty);
        private static void SetLastEntryDropEffects(DependencyObject element, DragDropEffects? value) => element.SetValue(LastEntryDropEffectsProperty, value);
        private static DragDropEffects? GetLastEntryDropEffects(DependencyObject element) => (DragDropEffects?) element.GetValue(LastEntryDropEffectsProperty);

        private static readonly DragEventHandler PreviewDragEnterHandler = (o, e) => OnElementDragEnter((UIElement) o, e, true);
        private static readonly DragEventHandler PreviewDragLeaveHandler = (o, e) => OnElementDragLeave((UIElement) o, e, true);
        private static readonly DragEventHandler PreviewDropHandler = (o, e) => OnElementDrop((UIElement) o, e, true);
        private static readonly DragEventHandler PreviewDragOverHandler = (o, e) => OnDragOver((UIElement) o, e, true);
        private static readonly QueryContinueDragEventHandler PreviewQueryContinueDragHandler = (o, e) => QueryContinueDrag((UIElement) o, e, true);
        private static readonly DragEventHandler DragEnterHandler = (o, e) => OnElementDragEnter((UIElement) o, e, false);
        private static readonly DragEventHandler DragLeaveHandler = (o, e) => OnElementDragLeave((UIElement) o, e, false);
        private static readonly DragEventHandler DropHandler = (o, e) => OnElementDrop((UIElement) o, e, false);
        private static readonly DragEventHandler DragOverHandler = (o, e) => OnDragOver((UIElement) o, e, false);
        private static readonly QueryContinueDragEventHandler QueryContinueDragHandler = (o, e) => QueryContinueDrag((UIElement) o, e, false);

        private static void UnregisterEvents(UIElement e) {
            e.PreviewDragEnter -= PreviewDragEnterHandler;
            e.PreviewDragLeave -= PreviewDragLeaveHandler;
            e.PreviewDrop -= PreviewDropHandler;
            e.PreviewDragOver -= PreviewDragOverHandler;
            e.PreviewQueryContinueDrag -= PreviewQueryContinueDragHandler;
            e.DragEnter -= DragEnterHandler;
            e.DragLeave -= DragLeaveHandler;
            e.Drop -= DropHandler;
            e.DragOver -= DragOverHandler;
            e.QueryContinueDrag -= QueryContinueDragHandler;
        }

        private static void RegisterEvents(UIElement e, bool preview) {
            if (preview) {
                e.PreviewDragEnter += PreviewDragEnterHandler;
                e.PreviewDragLeave += PreviewDragLeaveHandler;
                e.PreviewDrop += PreviewDropHandler;
                e.PreviewDragOver += PreviewDragOverHandler;
                e.PreviewQueryContinueDrag += PreviewQueryContinueDragHandler;
            }
            else {
                e.DragEnter += DragEnterHandler;
                e.DragLeave += DragLeaveHandler;
                e.Drop += DropHandler;
                e.DragOver += DragOverHandler;
                e.QueryContinueDrag += QueryContinueDragHandler;
            }
        }

        private static void OnFileDropNotifierPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is UIElement element) {
                UnregisterEvents(element);
                if (e.NewValue != null) {
                    element.AllowDrop = true;
                    RegisterEvents(element, GetUsePreviewEvent(element));
                }
            }
        }

        private static void OnUsePreviewEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is UIElement element) {
                UnregisterEvents(element);
                if ((bool) e.NewValue) {
                    element.AllowDrop = true;
                    RegisterEvents(element, (bool) e.NewValue);
                }
            }
        }

        private static void OnElementDragEnter(UIElement element, DragEventArgs e, bool isPreview) {
            if (GetUsePreviewEvent(element) != isPreview)
                return;
            if (IsProcessingDragEnter(element))
                return;
            if (IsProcessingDrop(element))
                return;

            IDropHandler handler = GetDropHandler(element);
            if (handler == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0) {
                element.SetValue(IsProcessingDragDropEnterProperty, true.Box());
                e.Effects = (DragDropEffects) handler.OnDropEnter(files);
                element.ClearValue(IsProcessingDragDropEnterProperty);
                element.SetValue(LastEntryDropEffectsProperty, e.Effects);
                e.Handled = true;
            }
        }

        private static void OnElementDragLeave(UIElement element, DragEventArgs e, bool isPreview) {
            if (GetUsePreviewEvent(element) != isPreview)
                return;
            if (IsProcessingDragLeave(element))
                return;
            if (IsProcessingDrop(element))
                return;
            if (GetIsProcessingQueryDrag(element))
                return;

            IDropHandler handler = GetDropHandler(element);
            if (handler == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                element.SetValue(IsProcessingDragDropLeaveProperty, BoolBox.True);
                handler.OnDropLeave(e.AllowedEffects == DragDropEffects.None);
                element.ClearValue(IsProcessingDragDropLeaveProperty);
                element.ClearValue(LastEntryDropEffectsProperty);
                e.Handled = true;
            }
        }

        private static async void OnElementDrop(UIElement element, DragEventArgs e, bool isPreview) {
            if (GetUsePreviewEvent(element) != isPreview)
                return;
            if (IsProcessingDrop(element))
                return;
            if (GetIsProcessingQueryDrag(element))
                return;

            IDropHandler handler = GetDropHandler(element);
            if (handler == null)
                return;

            if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0) {
                element.SetValue(IsProcessingDragDropProcessProperty, BoolBox.True);
                await handler.OnFilesDropped(files);
                element.ClearValue(IsProcessingDragDropProcessProperty);
                element.ClearValue(LastEntryDropEffectsProperty);
            }
        }

        private static void OnDragOver(DependencyObject element, DragEventArgs e, bool isPreview) {
            if (GetUsePreviewEvent(element) != isPreview)
                return;

            DragDropEffects? effects = GetLastEntryDropEffects(element);
            if (effects.HasValue) {
                e.Effects = effects.Value;
                e.Handled = true;
            }
        }

        private static void QueryContinueDrag(UIElement element, QueryContinueDragEventArgs e, bool isPreview) {
            if (!e.EscapePressed)
                return;

            if (GetUsePreviewEvent(element) != isPreview)
                return;
            if (IsProcessingDragLeave(element))
                return;

            IDropHandler handler = GetDropHandler(element);
            if (handler == null)
                return;

            handler.OnDropLeave(true);
            element.ClearValue(LastEntryDropEffectsProperty);
            e.Handled = true;
        }
    }
}