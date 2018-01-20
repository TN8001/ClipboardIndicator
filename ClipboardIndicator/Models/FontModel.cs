using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ClipboardIndicator
{
    [DataContract(Namespace = "")]
    public class FontModel : BindableBase
    {
        // 冗長だが...まあ妥協点か？
        [DataMember(Name = "Family", Order = 0)]
        private string FamilyString
        {
            get => Family.ConvertToString();
            set { try { Family = Family.ConvertFromString(value); } catch { } }
        }
        private FontFamily _Family;
        public FontFamily Family { get => _Family; set => Set(ref _Family, value); }

        [DataMember(Name = "Style", Order = 1)]
        public string StyleString
        {
            get => Style.ConvertToString();
            set { try { Style = Style.ConvertFromString(value); } catch { } }
        }
        private FontStyle _Style;
        public FontStyle Style { get => _Style; set => Set(ref _Style, value); }

        [DataMember(Name = "Weight", Order = 2)]
        public string WeightString
        {
            get => Weight.ConvertToString();
            set { try { Weight = Weight.ConvertFromString(value); } catch { } }
        }
        private FontWeight _Weight;
        public FontWeight Weight { get => _Weight; set => Set(ref _Weight, value); }

        [DataMember(Name = "Stretch", Order = 3)]
        public string StretchString
        {
            get => Stretch.ConvertToString();
            set { try { Stretch = Stretch.ConvertFromString(value); } catch { } }
        }
        private FontStretch _Stretch;
        public FontStretch Stretch { get => _Stretch; set => Set(ref _Stretch, value); }

        [DataMember(Name = "Size", Order = 4)]
        private double _Size;
        public double Size { get => _Size; set => Set(ref _Size, value); }

        protected override void Init()
        {
            _Family = new FontFamily("Arial");
            _Style = FontStyles.Normal;
            _Weight = FontWeights.Black;
            _Stretch = FontStretches.Normal;
            _Size = 11;
        }
    }
}
