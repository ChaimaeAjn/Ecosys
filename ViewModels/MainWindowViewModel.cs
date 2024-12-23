using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia;
using Avalonia.Threading;
using System.Runtime.CompilerServices;

namespace EcoSys.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public double CanvasWidth { get; }
        public double CanvasHeight { get; }

        private ObservableCollection<EcosysObjet> _ecosysObjects;
        public ObservableCollection<EcosysObjet> EcosysObjects
        {
            get => _ecosysObjects;
            private set
            {
                _ecosysObjects = value;
                OnPropertyChanged(nameof(EcosysObjects));
            }
        }

        private readonly Animal _animal;
        private readonly Random _random;
        private readonly DispatcherTimer _timer;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            CanvasWidth = 800;
            CanvasHeight = 450;
            _random = new Random();

            _ecosysObjects = new ObservableCollection<EcosysObjet>();
            
            // Initialisation de l'animal
            _animal = new Animal(
                _random.Next(0, (int)CanvasWidth),
                _random.Next(0, (int)CanvasHeight),
                CanvasWidth,
                CanvasHeight
            );

            InitializeEcosystemObjects();

            // Configuration du timer pour le mouvement automatique
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void InitializeEcosystemObjects()
        {
            // Ajout des plantes
            for (int i = 0; i < 5; i++)
            {
                EcosysObjects.Add(new Plante(new Point(
                    _random.Next(0, (int)CanvasWidth),
                    _random.Next(0, (int)CanvasHeight)
                )));
            }

            // Ajout de l'animal
            EcosysObjects.Add(_animal);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Mouvement aléatoire de l'animal
            double deltaX = _random.NextDouble() * 10 - 5; // Mouvement entre -5 et 5 pixels
            double deltaY = _random.NextDouble() * 10 - 5;
            
            _animal.Move(deltaX, deltaY);
        }
    }
}