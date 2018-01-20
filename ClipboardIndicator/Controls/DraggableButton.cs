using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClipboardIndicator
{
    ///<summary>親Windowごとドラッグ移動するボタン</summary>
    public class DraggableButton : Button
    {
        ///<summary>移動するかどうか</summary>
        public bool CanMove { get => (bool)GetValue(CanMoveProperty); set => SetValue(CanMoveProperty, value); }
        public static readonly DependencyProperty CanMoveProperty
            = DependencyProperty.Register(nameof(CanMove), typeof(bool), 
                typeof(DraggableButton), new PropertyMetadata(true));

        private Point? startPos;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            startPos = e.GetPosition(this);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            startPos = null;
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if(e.LeftButton != MouseButtonState.Pressed) return;
            if(!CanMove || startPos == null || !IsMouseCaptured) return;
            if(!IsDragStartable(e.GetPosition(this) - (Point)startPos)) return;

            ReleaseMouseCapture();
            Window.GetWindow(this).DragMove();
            startPos = null;

            bool IsDragStartable(Vector delta)
                => (SystemParameters.MinimumHorizontalDragDistance < Math.Abs(delta.X))
                || (SystemParameters.MinimumVerticalDragDistance < Math.Abs(delta.Y));
        }
    }
}
