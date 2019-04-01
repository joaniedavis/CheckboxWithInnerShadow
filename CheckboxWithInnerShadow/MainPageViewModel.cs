using System;
using System.ComponentModel;
namespace CheckboxWithInnerShadow
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _value = 20;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}
