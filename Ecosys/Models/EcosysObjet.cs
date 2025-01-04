using Avalonia;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EcoSys
{
    // Classe abstraite représentant un objet du système écologique qui implémente INotifyPropertyChanged pour la gestion des propriétés modifiées
    public abstract class EcosysObjet : INotifyPropertyChanged
    {
        // Propriété privée pour la localisation de l'objet
        private Point _location;

        // Propriété virtuelle qui déclare la taille de l'objet
        public virtual double SizeRadius { get; protected set; }

        // Propriété virtuelle pour savoir si l'objet peut être mangé, par défaut à false
        public virtual bool IsEdible { get; protected set; } = false;

        // Propriété publique pour la localisation de l'objet
        public Point Location
        {
            get => _location;
            set
            {
                // Si la nouvelle valeur de la localisation est différente, mettre à jour et notifier le changement
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(); // Notifie que la propriété a changé
                }
            }
        }

        // L'événement PropertyChanged qui permet d'informer les abonnés des changements de propriété
        public event PropertyChangedEventHandler? PropertyChanged;

        // Méthode protégée pour invoquer l'événement PropertyChanged, indiquant qu'une propriété a changé
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            // Si l'événement PropertyChanged est abonné, le déclencher avec le nom de la propriété modifiée
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Méthode protégée pour définir une propriété et notifier le changement si nécessaire
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            // Si la valeur est identique à la précédente, ne rien faire
            if (Equals(field, value)) return false;
            
            // Sinon, mettre à jour la valeur et notifier le changement
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Constructeur de la classe EcosysObjet qui initialise la localisation
        protected EcosysObjet(Point location)
        {
            _location = location;
        }
    }
}
