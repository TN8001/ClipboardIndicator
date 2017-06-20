using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ClipboardIndicator
{
    [DataContract(Namespace = "")]
    public class ColorModel : BindableBase
    {
        #region Foreground
        [DataMember(Name = "Foreground", Order = 0)]
        public string ForegroundString
        {
            get => Foreground.ConvertToString();
            set { try { Foreground = Foreground.ConvertFromString(value); } catch { } }
        }
        private Color _Foreground;
        public Color Foreground { get => _Foreground; set => SetProperty(ref _Foreground, value); }
        #endregion
        #region Background
        [DataMember(Name = "Background", Order = 1)]
        public string BackgroundString
        {
            get => Background.ConvertToString();
            set { try { Background = Background.ConvertFromString(value); } catch { } }
        }
        private Color _Background;
        public Color Background { get => _Background; set => SetProperty(ref _Background, value); }
        #endregion

        protected override void Init()
        {
            Foreground = Colors.Black;
            Background = Background.ConvertFromString("#BCCBD9");
        }
    }

    [DataContract(Namespace = "")]
    public class WindowModel : BindableBase
    {
        [DataMember(Name = "Top", Order = 0)]
        private double _Top;
        public double Top { get => _Top; set => SetProperty(ref _Top, value); }

        [DataMember(Name = "Left", Order = 1)]
        private double _Left;
        public double Left { get => _Left; set => SetProperty(ref _Left, value); }

        [DataMember(Name = "Width", Order = 2)]
        private double _Width;
        public double Width { get => _Width; set => SetProperty(ref _Width, value); }

        //[DataMember(Order = 3, Name = "Height")]
        //private double _Height;
        //public double Height { get => _Height; set => SetProperty(ref _Height, value); }

        [DataMember(Name = "CanMove", Order = 4)]
        private bool _CanMove;
        public bool CanMove { get { return _CanMove; } set { SetProperty(ref _CanMove, value); } }

        protected override void Init()
        {
            _Top = 0;
            _Left = 500;
            _Width = 100;
            //_Height = 600;
            _CanMove = true;
        }
    }

    [DataContract(Namespace = "")]
    public class FontModel : BindableBase
    {
        #region Family
        [DataMember(Name = "Family", Order = 0)]
        private string FamilyString
        {
            get => Family.ConvertToString();
            set { try { Family = Family.ConvertFromString(value); } catch { } }
        }
        private FontFamily _Family;
        public FontFamily Family { get => _Family; set => SetProperty(ref _Family, value); }
        #endregion
        #region Style
        [DataMember(Name = "Style", Order = 1)]
        public string StyleString
        {
            get => Style.ConvertToString();
            set { try { Style = Style.ConvertFromString(value); } catch { } }
        }
        private FontStyle _Style;
        public FontStyle Style { get => _Style; set => SetProperty(ref _Style, value); }
        #endregion
        #region Weight
        [DataMember(Name = "Weight", Order = 2)]
        public string WeightString
        {
            get => Weight.ConvertToString();
            set { try { Weight = Weight.ConvertFromString(value); } catch { } }
        }
        private FontWeight _Weight;
        public FontWeight Weight { get => _Weight; set => SetProperty(ref _Weight, value); }
        #endregion
        #region Stretch
        [DataMember(Name = "Stretch", Order = 3)]
        public string StretchString
        {
            get => Stretch.ConvertToString();
            set { try { Stretch = Stretch.ConvertFromString(value); } catch { } }
        }
        private FontStretch _Stretch;
        public FontStretch Stretch { get => _Stretch; set => SetProperty(ref _Stretch, value); }
        #endregion
        [DataMember(Name = "Size", Order = 4)]
        private double _Size;
        public double Size { get => _Size; set => SetProperty(ref _Size, value); }

        protected override void Init()
        {
            _Family = new FontFamily("Arial");
            _Style = FontStyles.Normal;
            _Weight = FontWeights.Black;
            _Stretch = FontStretches.Normal;
            _Size = 11;
        }
    }

    [DataContract(Namespace = "", Name = "Setting")]
    public class SettingsModel : BindableBase
    {
        [DataMember(Order = 0)]
        public WindowModel Window { get; private set; }

        [DataMember(Order = 1)]
        public FontModel Font { get; private set; }

        [DataMember(Order = 2)]
        public ColorModel Color { get; private set; }

        protected override void Init()
        {
            Window = new WindowModel();
            Font = new FontModel();
            Color = new ColorModel();
        }
    }

    internal static class ConvertExtensions
    {
        public static T ConvertFromString<T>(this T target, string value)
            => (T)TypeDescriptor.GetConverter(target.GetType()).ConvertFrom(value);
        public static string ConvertToString<T>(this T value)
            => (string)TypeDescriptor.GetConverter(value.GetType()).ConvertTo(value, typeof(string));
    }
}
