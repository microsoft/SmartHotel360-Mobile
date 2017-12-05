using SmartHotel.Clients.Live.ViewModels.Base;
using System;
using System.Windows.Input;

namespace SmartHotel.Clients.Live.Models
{
    public class MenuItem : ExtendedBindableObject
    {
        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private Type _viewModelType;

        public Type ViewModelType
        {
            get
            {
                return _viewModelType;
            }

            set
            {
                _viewModelType = value;
                RaisePropertyChanged(() => ViewModelType);
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        private ICommand _command;

        public ICommand Command
        {
            get
            {
                return _command;
            }

            set
            {
                _command = value;
                RaisePropertyChanged(() => Command);
            }
        }
    }
}