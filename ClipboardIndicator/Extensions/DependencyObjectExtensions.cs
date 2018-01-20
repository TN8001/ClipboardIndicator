using System.Windows;

namespace ClipboardIndicator
{
    static class DependencyObjectExtensions
    {
        ///<summary>LogicalTreeHelperで祖先を検索</summary>
        public static T GetLogicalAncestor<T>(this DependencyObject obj) where T : class
        {
            do
            {
                obj = LogicalTreeHelper.GetParent(obj);
                if(obj == null) break;

            } while(!(obj is T));

            return obj as T;
        }
    }
}
