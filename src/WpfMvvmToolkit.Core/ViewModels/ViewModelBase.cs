using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfMvvmToolkit.Core.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        // Not using ObservableProperty attribute
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
