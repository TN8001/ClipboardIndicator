using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace ClipboardIndicator
{
    ///<summary>シリアライズ向けBindableBase</summary>
    [DataContract]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BindableBase() => Init();

        protected virtual bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [OnDeserializing]
        private void OnDeserializing(StreamingContext sc) => Init();
        ///<summary>デシリアライズ時の初期値設定</summary>
        protected virtual void Init() { }
    }
}
