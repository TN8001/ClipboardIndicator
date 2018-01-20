using System.Runtime.Serialization;

namespace ClipboardIndicator
{
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
}
