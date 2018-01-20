using System.Runtime.Serialization;
using System.Windows.Media;

namespace ClipboardIndicator
{
    [DataContract(Namespace = "")]
    public class ColorModel : BindableBase
    {
        // 冗長だが...まあ妥協点か？
        [DataMember(Name = "Foreground", Order = 0)]
        public string ForegroundString
        {
            get => Foreground.ConvertToString();
            set { try { Foreground = Foreground.ConvertFromString(value); } catch { } }
        }
        private Color _Foreground;
        public Color Foreground { get => _Foreground; set => Set(ref _Foreground, value); }

        [DataMember(Name = "Background", Order = 1)]
        public string BackgroundString
        {
            get => Background.ConvertToString();
            set { try { Background = Background.ConvertFromString(value); } catch { } }
        }
        private Color _Background;
        public Color Background { get => _Background; set => Set(ref _Background, value); }

        protected override void Init()
        {
            Foreground = Colors.Black;
            Background = Background.ConvertFromString("#BCCBD9");
        }
    }
}
