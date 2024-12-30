using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Threading;

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
        private readonly List<Viande> _viandes;
        private readonly List<Gestation<EcosysObjet>> _gestations;
        private readonly Random _random;
        private readonly DispatcherTimer _timer;

        private const double COLLISION_DISTANCE = 30.0;

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
            _viandes = new List<Viande>();
            _gestations = new List<Gestation<EcosysObjet>>();
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

        private bool IsObjectOnTopOfAnother(EcosysObjet obj1, EcosysObjet obj2)
        {
            double dx = obj1.Location.X - obj2.Location.X;
            double dy = obj1.Location.Y - obj2.Location.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance < (obj1.SizeRadius + obj2.SizeRadius);
        }

        private double DistanceBetween(EcosysObjet obj1, EcosysObjet obj2)
        {
            double dx = obj1.Location.X - obj2.Location.X;
            double dy = obj1.Location.Y - obj2.Location.Y;
            return Math.Sqrt(dx * dx + dy * dy);
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
                        herbivore.SetEnergy(0);
                        _herbivores.Remove(herbivore);
                        EcosysObjects.Remove(herbivore);

                    }
                }
            }
        }

        private bool CanReproduce(Animal animal1, Animal animal2)
        {
            return animal1 != animal2 &&
                   animal1.Sexe != animal2.Sexe &&
                   !_gestations.Any(g => g.Mere == animal1 || g.Mere == animal2) &&
                   DistanceBetween(animal1, animal2) < COLLISION_DISTANCE;
        }

        private void CheckReproduction()
        {
            foreach (var herbivore1 in _herbivores.ToList())
            {
                foreach (var herbivore2 in _herbivores.ToList())
                {
                    if (CanReproduce(herbivore1, herbivore2))
                    {
                        var mere = herbivore1.Sexe == Animal.SexeType.Femelle ? herbivore1 : herbivore2;
                        _gestations.Add(new Gestation<EcosysObjet>(
                            mere,
                            TimeSpan.FromSeconds(10)
                        ));
                    }
                }
            }

            foreach (var carnivore1 in _carnivores.ToList())
            {
                foreach (var carnivore2 in _carnivores.ToList())
                {
                    if (CanReproduce(carnivore1, carnivore2))
                    {
                        var mere = carnivore1.Sexe == Animal.SexeType.Femelle ? carnivore1 : carnivore2;
                        _gestations.Add(new Gestation<EcosysObjet>(
                            mere,
                            TimeSpan.FromSeconds(15)
                        ));
                    }
                }
            }
        }

        private void HandleBirths()
        {
            foreach (var gestation in _gestations.ToList())
            {
                if (DateTime.Now >= gestation.FinGestation)
                {
                    double offsetX = _random.Next(-20, 20);
                    double offsetY = _random.Next(-20, 20);

                    if (gestation.Mere is Herbivore)
                    {
                        var newHerbivore = new Herbivore(
                            gestation.Mere.Location.X + offsetX,
                            gestation.Mere.Location.Y + offsetY,
                            CanvasWidth,
                            CanvasHeight
                        );
                        _herbivores.Add(newHerbivore);
                        EcosysObjects.Add(newHerbivore);
                    }
                    else if (gestation.Mere is Carnivore)
                    {
                        var newCarnivore = new Carnivore(
                            gestation.Mere.Location.X + offsetX,
                            gestation.Mere.Location.Y + offsetY,
                            CanvasWidth,
                            CanvasHeight
                        );
                        _carnivores.Add(newCarnivore);
                        EcosysObjects.Add(newCarnivore);
                    }

                    _gestations.Remove(gestation);
                }
            }
        }

        private void HandlePredation()
        {
            foreach (var carnivore in _carnivores)
            {
                if (!carnivore.IsDead)
                {
                    foreach (var herbivore in _herbivores.ToList())
                    {
                        if (!herbivore.IsDead && IsObjectOnTopOfAnother(carnivore, herbivore))
                        {
                            carnivore.EatHerbivore(); // Carnivore mange l'herbivore
                            herbivore.LoseHeart();    // Herbivore perd un cœur (meurt)
                        }
                    }
                }
            }
        }

        private void HandlePlantConsumption()
        {
            foreach (var herbivore in _herbivores)
            {
                if (!herbivore.IsDead)
                {
                    foreach (var plante in _plantes.ToList())
                    {
                        if (IsObjectOnTopOfAnother(herbivore, plante))
                        {
                            herbivore.EatPlante(); // Herbivore mange la plante
                            _plantes.Remove(plante); // Retirer la plante de la simulation
                            EcosysObjects.Remove(plante); // Retirer la plante de la collection affichée
                        }
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
                    double speed = _random.NextDouble() * 5; // Vitesse des herbivores
                    double angle = _random.NextDouble() * 2 * Math.PI;
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
            CheckCollisions();
            CheckReproduction();
            HandleBirths();
        }
    }
}

