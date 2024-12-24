using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Threading;
using System.Runtime.CompilerServices;
using System.Linq;

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

        private readonly List<Carnivore> _carnivores;
        private readonly List<Herbivore> _herbivores;
        private readonly List<Plante> _plantes;
        private readonly Random _random;
        private readonly DispatcherTimer _timer;
        private const double COLLISION_DISTANCE = 30.0; // Distance pour détecter une collision

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

            _carnivores = new List<Carnivore>();
            _herbivores = new List<Herbivore>();
            _plantes = new List<Plante>();
            _ecosysObjects = new ObservableCollection<EcosysObjet>();

            InitializeEcosystemObjects();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void InitializeEcosystemObjects()
        {
            // Ajouter des plantes
            for (int i = 0; i < 10; i++)
            {
                var plante = new Plante(new Point(
                    _random.Next(0, (int)CanvasWidth),
                    _random.Next(0, (int)CanvasHeight)
                ));
                _plantes.Add(plante);
                EcosysObjects.Add(plante);
            }

            // Ajouter des herbivores
            for (int i = 0; i < 10; i++)
            {
                var herbivore = new Herbivore(
                    _random.Next(0, (int)CanvasWidth),
                    _random.Next(0, (int)CanvasHeight),
                    CanvasWidth,
                    CanvasHeight
                );
                _herbivores.Add(herbivore);
                EcosysObjects.Add(herbivore);
            }

            // Ajouter des carnivores
            for (int i = 0; i < 10; i++)
            {
                var carnivore = new Carnivore(
                    _random.Next(0, (int)CanvasWidth),
                    _random.Next(0, (int)CanvasHeight),
                    CanvasWidth,
                    CanvasHeight
                );
                _carnivores.Add(carnivore);
                EcosysObjects.Add(carnivore);
            }
        }

        private double DistanceBetween(EcosysObjet obj1, EcosysObjet obj2)
        {
            double dx = obj1.Location.X - obj2.Location.X;
            double dy = obj1.Location.Y - obj2.Location.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private bool IsObjectOnTopOfAnother(EcosysObjet obj1, EcosysObjet obj2)
        {
            double distance = DistanceBetween(obj1, obj2);
            double combinedRadius = obj1.SizeRadius + obj2.SizeRadius; // Somme des rayons
            return distance < combinedRadius;
        }

        private void CheckCollisions()
        {
            // Vérifier les collisions herbivores-plantes
            foreach (var herbivore in _herbivores.ToList())
            {
                if (herbivore.IsDead) continue;

                foreach (var plante in _plantes.ToList())
                {
                    if (IsObjectOnTopOfAnother(herbivore, plante)) // Utilisation de la nouvelle méthode de collision
                    {
                        herbivore.EatPlante();
                        _plantes.Remove(plante);
                        EcosysObjects.Remove(plante);

                        // Créer une nouvelle plante ailleurs
                        var nouvellePlante = new Plante(new Point(
                            _random.Next(0, (int)CanvasWidth),
                            _random.Next(0, (int)CanvasHeight)
                        ));
                        _plantes.Add(nouvellePlante);
                        EcosysObjects.Add(nouvellePlante);
                    }
                }
            }

            // Vérifier les collisions carnivores-herbivores
            foreach (var carnivore in _carnivores.ToList())
            {
                if (carnivore.IsDead) continue;

                foreach (var herbivore in _herbivores.ToList())
                {
                    if (herbivore.IsDead) continue;

                    if (IsObjectOnTopOfAnother(carnivore, herbivore)) // Utilisation de la nouvelle méthode de collision
                    {
                        carnivore.EatHerbivore();
                        herbivore.Energy = 0; // L'herbivore meurt
                        _herbivores.Remove(herbivore);
                        EcosysObjects.Remove(herbivore);

                    }
                }
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Déplacer les herbivores
            foreach (var herbivore in _herbivores)
            {
                herbivore.UpdateEnergy();
                if (!herbivore.IsDead)
                {
                    double angle = _random.NextDouble() * 2 * Math.PI;
                    double speed = _random.NextDouble() * 5;
                    double deltaX = Math.Cos(angle) * speed;
                    double deltaY = Math.Sin(angle) * speed;
                    herbivore.Move(deltaX, deltaY);
                }
            }

            // Déplacer les carnivores
            foreach (var carnivore in _carnivores)
            {
                carnivore.UpdateEnergy();
                if (!carnivore.IsDead)
                {
                    double angle = _random.NextDouble() * 2 * Math.PI;
                    double speed = _random.NextDouble() * 10; // Les carnivores sont plus rapides
                    double deltaX = Math.Cos(angle) * speed;
                    double deltaY = Math.Sin(angle) * speed;
                    carnivore.Move(deltaX, deltaY);
                }
            }

            // Vérifier les collisions et la nourriture
            CheckCollisions();
        }
    }
}
