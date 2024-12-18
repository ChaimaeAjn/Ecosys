using System.Collections.Generic;
using System.ComponentModel;

namespace EcoSys.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        public List<Plante> Plantes { get; set; }

        private readonly Animal _animal;

        public double ImageLeft => _animal.ImageLeft;
        public double ImageTop => _animal.ImageTop;

        public MainWindowViewModel()
        {
            CanvasWidth = 800;
            CanvasHeight = 450;

            _animal = new Animal(100, 150, CanvasWidth, CanvasHeight);
            _animal.PropertyChanged += Animal_PropertyChanged;

            Plantes = new List<Plante>();
            for (int i = 0; i < 5; i++)
            {
                Plantes.Add(new Plante(CanvasWidth, CanvasHeight));
            }
        }

        // Notifier les changements à l'interface utilisateur
        private void Animal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Animal.ImageLeft))
                OnPropertyChanged(nameof(ImageLeft));
            if (e.PropertyName == nameof(Animal.ImageTop))
                OnPropertyChanged(nameof(ImageTop));
        }
    }
}
