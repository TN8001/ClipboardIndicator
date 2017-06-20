using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ClipboardIndicator
{
    ///<summary>Window表示時に場所をアピールするPopupを表示 アニメーション後自動非表示</summary>
    ///<remarks>コンテンツのサイズをStartupPopupのWidth Heightに設定すること</remarks>
    public class StartupPopup : Popup
    {
        public StartupPopup() : base()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //xaml指定があった場合 上書き
            AllowsTransparency = true;
            PopupAnimation = PopupAnimation.Fade;
            Placement = PlacementMode.Custom;
            CustomPopupPlacementCallback = CustomCallback;
            RenderTransform = new RotateTransform();

            var window = this.GetLogicalAncestor<Window>();
            window.ContentRendered += Window_ContentRendered;
        }
        //PointToScreenを取れるかつRenderTransformが動かせる（唯一の？）タイミング
        //この後でAngle等いじっても反映されない PopupAnimation関連でのPopupの仕様??
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var screen = PointToScreen(new Point(0, 0));
            if(screen.Y + Height > SystemParameters.WorkArea.Height)
                ((RotateTransform)RenderTransform).Angle = 180;

            //アピールするアニメーション 指でツンツンを5回 計1.5秒
            //基本下側 表示しきれない場合上側に表示
            var frame = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3)),
                Value = Height + 30,
                EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn },
            };

            var animation = new DoubleAnimationUsingKeyFrames();
            animation.KeyFrames.Add(frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Height"));

            var storyboard = new Storyboard
            {
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(5),
            };
            storyboard.Children.Add(animation);
            storyboard.Completed += (s, _) => IsOpen = false;

            IsOpen = true;
            BeginStoryboard(storyboard);
        }
        private CustomPopupPlacement[] CustomCallback(Size popupSize, Size targetSize, Point offset)
        {
            //上下反転すると（よくわからないが）ずれたので自前実装
            //第１希望:真下 第２希望:真上
            var x = (targetSize.Width - popupSize.Width) / 2;

            return new CustomPopupPlacement[]
            {
                new CustomPopupPlacement(new Point(x , targetSize.Height), PopupPrimaryAxis.None),
                new CustomPopupPlacement(new Point(x, -popupSize.Height), PopupPrimaryAxis.None),
            };
        }
    }

    static class DependencyObjectExtensions
    {
        public static T GetLogicalAncestor<T>(this DependencyObject depObj) where T : class
        {
            var target = depObj;
            do
            {
                target = LogicalTreeHelper.GetParent(target);
                if(target == null) break;

            } while(!(target is T));

            return target as T;
        }
    }
}
