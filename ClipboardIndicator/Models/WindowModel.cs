using System.Runtime.Serialization;

namespace ClipboardIndicator
{
    [DataContract(Namespace = "")]
    public class WindowModel : BindableBase
    {
        [DataMember(Name = "Top", Order = 0)]
        private double _Top;
        public double Top { get => _Top; set => Set(ref _Top, value); }

        [DataMember(Name = "Left", Order = 1)]
        private double _Left;
        public double Left { get => _Left; set => Set(ref _Left, value); }

        [DataMember(Name = "Width", Order = 2)]
        private double _Width;
        public double Width { get => _Width; set => Set(ref _Width, value); }

        [DataMember(Name = "CanMove", Order = 3)]
        private bool _CanMove;
        public bool CanMove { get => _CanMove; set => Set(ref _CanMove, value); }

        protected override void Init()
        {
            _Top = 0;
            _Left = 500;
            _Width = 100;
            _CanMove = true;
        }
    }
}
