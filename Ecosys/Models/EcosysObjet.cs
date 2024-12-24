using Avalonia;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoSys
{
    public abstract class EcosysObjet : INotifyPropertyChanged
    {
        private Point _location;
        private double _sizeRadius;  // Ajout du champ SizeRadius

        public Point Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        public double SizeRadius  // Propriété SizeRadius
        {
            get => _sizeRadius;
            protected set
            {
                if (_sizeRadius != value)
                {
                    _sizeRadius = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected EcosysObjet(Point location)
        {
            _location = location;
        }
    }
}
