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
        // Dimensions de la zone de simulation (canvas graphique)
        public double CanvasWidth { get; }  // Largeur de la zone de simulation
        public double CanvasHeight { get; } // Hauteur de la zone de simulation

        // Collection observable des objets de l'écosystème pour mettre à jour l'interface utilisateur
        private ObservableCollection<EcosysObjet> _ecosysObjects;
        public ObservableCollection<EcosysObjet> EcosysObjects
        {
            get => _ecosysObjects; // Retourne les objets actuellement dans l'écosystème
            private set
            {
                _ecosysObjects = value;
                OnPropertyChanged(nameof(EcosysObjects)); // Notifie l'interface utilisateur d'un changement
            }
        }

        // Listes séparées pour stocker différents types d'objets dans l'écosystème
        private readonly List<Carnivore> _carnivores; // Liste des carnivores
        private readonly List<Herbivore> _herbivores; // Liste des herbivores
        private readonly List<Plante> _plantes;       // Liste des plantes
        private readonly List<Viande> _viandes;      // Liste des morceaux de viande
        private readonly List<Gestation<EcosysObjet>> _gestations; // Suivi des objets en gestation (reproduction)

        // Générateur de nombres aléatoires pour positionnement et comportements
        private readonly Random _random;

        // Timer pour actualiser la simulation à intervalles réguliers
        private readonly DispatcherTimer _timer;

        // Constante définissant la distance à laquelle une collision est détectée
        private const double COLLISION_DISTANCE = 10.0;

        // Événement de notification des changements de propriété (implémentation de INotifyPropertyChanged)
        public event PropertyChangedEventHandler? PropertyChanged;

        // Méthode pour notifier les changements de propriété (utile pour l'interface utilisateur)
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructeur de la classe principale
        public MainWindowViewModel()
        {
            // Initialisation des dimensions de la zone de simulation
            CanvasWidth = 800; // Largeur fixe
            CanvasHeight = 450; // Hauteur fixe

            // Initialisation des listes et du générateur aléatoire
            _random = new Random();
            _carnivores = new List<Carnivore>();
            _herbivores = new List<Herbivore>();
            _plantes = new List<Plante>();
            _viandes = new List<Viande>();
            _gestations = new List<Gestation<EcosysObjet>>();
            _ecosysObjects = new ObservableCollection<EcosysObjet>();

            // Création des objets initiaux dans l'écosystème
            InitializeEcosystemObjects();

            // Configuration du timer pour actualiser la simulation toutes les 100 millisecondes
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100) // Durée entre chaque "tick" du timer
            };
            _timer.Tick += Timer_Tick;
            _timer.Start(); // Lance le timer
        }

        // Méthode pour initialiser les objets dans l'écosystème au démarrage
        private void InitializeEcosystemObjects()
        {
            // Ajouter 10 plantes aléatoirement positionnées
            for (int i = 0; i < 10; i++)
            {
                var plante = new Plante(new Point(
                    _random.Next(0, (int)CanvasWidth),  // Position X aléatoire
                    _random.Next(0, (int)CanvasHeight) // Position Y aléatoire
                ));
                _plantes.Add(plante); // Ajoute la plante à la liste des plantes
                EcosysObjects.Add(plante); // Ajoute la plante à la collection observable
            }

            // Ajouter 10 herbivores avec des positions aléatoires
            for (int i = 0; i < 10; i++)
            {
                var herbivore = new Herbivore(
                    _random.Next(0, (int)CanvasWidth),  // Position X aléatoire
                    _random.Next(0, (int)CanvasHeight), // Position Y aléatoire
                    CanvasWidth,                        // Largeur pour limiter les déplacements
                    CanvasHeight                       // Hauteur pour limiter les déplacements
                );
                _herbivores.Add(herbivore); // Ajoute l'herbivore à la liste
                EcosysObjects.Add(herbivore); // Ajoute l'herbivore à la collection observable
            }

            // Ajouter 10 carnivores avec des positions aléatoires
            for (int i = 0; i < 10; i++)
            {
                var carnivore = new Carnivore(
                    _random.Next(0, (int)CanvasWidth),  // Position X aléatoire
                    _random.Next(0, (int)CanvasHeight), // Position Y aléatoire
                    CanvasWidth,                        // Largeur pour limiter les déplacements
                    CanvasHeight                       // Hauteur pour limiter les déplacements
                );
                _carnivores.Add(carnivore); // Ajoute le carnivore à la liste
                EcosysObjects.Add(carnivore); // Ajoute le carnivore à la collection observable
            }
        }

        // Méthode pour vérifier si deux objets se superposent (collision)
        private bool IsObjectOnTopOfAnother(EcosysObjet obj1, EcosysObjet obj2)
        {
            // Calcul des différences de position
            double dx = obj1.Location.X - obj2.Location.X;
            double dy = obj1.Location.Y - obj2.Location.Y;

            // Calcul de la distance entre les deux objets
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Vérifie si la distance est inférieure à la somme des rayons des deux objets
            return distance < (obj1.SizeRadius + obj2.SizeRadius);
        }

        // Méthode pour calculer la distance entre deux objets
        private double DistanceBetween(EcosysObjet obj1, EcosysObjet obj2)
        {
            double dx = obj1.Location.X - obj2.Location.X;
            double dy = obj1.Location.Y - obj2.Location.Y;
            return Math.Sqrt(dx * dx + dy * dy); // Théorème de Pythagore
        }

        // Vérifier les collisions et gérer les interactions entre objets
        private void CheckCollisions()
        {
            // Gestion des collisions entre herbivores et plantes
            foreach (var herbivore in _herbivores.ToList())
            {
                if (herbivore.IsDead) continue; // Ignore les herbivores morts

                foreach (var plante in _plantes.ToList())
                {
                    if (IsObjectOnTopOfAnother(herbivore, plante)) // Vérifie si un herbivore est sur une plante
                    {
                        herbivore.EatPlante(); // L'herbivore consomme la plante
                        _plantes.Remove(plante); // Retire la plante de la liste
                        EcosysObjects.Remove(plante); // Retire la plante de la collection observable

                        // Créer une nouvelle plante à une position aléatoire
                        var nouvellePlante = new Plante(new Point(
                            _random.Next(0, (int)CanvasWidth),
                            _random.Next(0, (int)CanvasHeight)
                        ));
                        _plantes.Add(nouvellePlante); // Ajoute la nouvelle plante
                        EcosysObjects.Add(nouvellePlante); // Ajoute à la collection observable
                    }
                }
            }

            // Gestion des collisions entre carnivores et herbivores
            foreach (var carnivore in _carnivores.ToList())
            {
                if (carnivore.IsDead) continue; // Ignore les carnivores morts

                foreach (var herbivore in _herbivores.ToList())
                {
                    if (herbivore.IsDead) continue; // Ignore les herbivores morts

                    if (IsObjectOnTopOfAnother(carnivore, herbivore)) // Vérifie si un carnivore est sur un herbivore
                    {
                        carnivore.EatHerbivore(); // Le carnivore consomme l'herbivore
                        herbivore.Energy = 50;
                        _herbivores.Remove(herbivore); // Retire l'herbivore de la liste
                        EcosysObjects.Remove(herbivore); // Met à jour la collection observable
                    }
                }
            }
        }

        

        private bool CanReproduce(Animal animal1, Animal animal2)
        {
            // Vérifie si les deux animaux sont différents (pas le même objet en mémoire)
            return animal1 != animal2 &&
                // Vérifie que les deux animaux sont de sexes opposés
                animal1.Sexe != animal2.Sexe &&
                // Vérifie qu'aucun des deux animaux n'est déjà en gestation
                !_gestations.Any(g => g.Mere == animal1 || g.Mere == animal2) &&
                // Vérifie que les deux animaux sont suffisamment proches pour se reproduire
                DistanceBetween(animal1, animal2) < COLLISION_DISTANCE;
        }

        private void CheckReproduction()
        {
            // Parcours tous les couples possibles d'herbivores
            foreach (var herbivore1 in _herbivores.ToList())
            {
                foreach (var herbivore2 in _herbivores.ToList())
                {
                    // Si les deux herbivores peuvent se reproduire
                    if (CanReproduce(herbivore1, herbivore2))
                    {
                        // Identifie la mère parmi les deux (celle qui est femelle)
                        var mere = herbivore1.Sexe == Animal.SexeType.Femelle ? herbivore1 : herbivore2;

                        // Ajoute une nouvelle gestation avec une durée définie (10 secondes pour les herbivores)
                        _gestations.Add(new Gestation<EcosysObjet>(
                            mere,
                            TimeSpan.FromSeconds(10)
                        ));
                    }
                }
            }

            // Parcours tous les couples possibles de carnivores
            foreach (var carnivore1 in _carnivores.ToList())
            {
                foreach (var carnivore2 in _carnivores.ToList())
                {
                    // Si les deux carnivores peuvent se reproduire
                    if (CanReproduce(carnivore1, carnivore2))
                    {
                        // Identifie la mère parmi les deux (celle qui est femelle)
                        var mere = carnivore1.Sexe == Animal.SexeType.Femelle ? carnivore1 : carnivore2;

                        // Ajoute une nouvelle gestation avec une durée définie (15 secondes pour les carnivores)
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
            // Parcours toutes les gestations en cours
            foreach (var gestation in _gestations.ToList())
            {
                // Vérifie si la durée de la gestation est terminée
                if (DateTime.Now >= gestation.FinGestation)
                {
                    // Génère une légère variation de position pour le nouveau-né
                    double offsetX = _random.Next(-20, 20);
                    double offsetY = _random.Next(-20, 20);

                    // Si la mère est un herbivore, crée un nouvel herbivore
                    if (gestation.Mere is Herbivore)
                    {
                        var newHerbivore = new Herbivore(
                            gestation.Mere.Location.X + offsetX,
                            gestation.Mere.Location.Y + offsetY,
                            CanvasWidth,
                            CanvasHeight
                        );
                        _herbivores.Add(newHerbivore); // Ajoute à la liste des herbivores
                        EcosysObjects.Add(newHerbivore); // Ajoute à la collection observable
                    }
                    // Si la mère est un carnivore, crée un nouveau carnivore
                    else if (gestation.Mere is Carnivore)
                    {
                        var newCarnivore = new Carnivore(
                            gestation.Mere.Location.X + offsetX,
                            gestation.Mere.Location.Y + offsetY,
                            CanvasWidth,
                            CanvasHeight
                        );
                        _carnivores.Add(newCarnivore); // Ajoute à la liste des carnivores
                        EcosysObjects.Add(newCarnivore); // Ajoute à la collection observable
                    }

                    // Retire la gestation terminée de la liste
                    _gestations.Remove(gestation);
                }
            }
        }

        private void HandlePredation()
        {
            // Parcours tous les carnivores
            foreach (var carnivore in _carnivores)
            {
                if (!carnivore.IsDead) // Si le carnivore est vivant
                {
                    // Parcours tous les herbivores
                    foreach (var herbivore in _herbivores.ToList())
                    {
                        if (!herbivore.IsDead && IsObjectOnTopOfAnother(carnivore, herbivore))
                        {
                            // Si le carnivore est sur l'herbivore, il le mange
                            carnivore.EatHerbivore(); // Le carnivore gagne de l'énergie
                            herbivore.LoseHeart();    // L'herbivore perd un cœur (meurt s'il n'a plus de cœurs)
                        }
                    }
                }
            }
        }

        private void HandlePlantConsumption()
        {
            // Parcours tous les herbivores
            foreach (var herbivore in _herbivores)
            {
                if (!herbivore.IsDead) // Si l'herbivore est vivant
                {
                    // Parcours toutes les plantes
                    foreach (var plante in _plantes.ToList())
                    {
                        if (IsObjectOnTopOfAnother(herbivore, plante))
                        {
                            // Si l'herbivore est sur la plante, il la mange
                            herbivore.EatPlante(); // L'herbivore gagne de l'énergie
                            _plantes.Remove(plante); // Retire la plante de la liste
                            EcosysObjects.Remove(plante); // Met à jour la collection observable
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
                herbivore.UpdateEnergy(); // Met à jour l'énergie de l'herbivore
                if (!herbivore.IsDead) // Si l'herbivore est vivant
                {
                    double speed = _random.NextDouble() * 5; // Vitesse des herbivores
                    double angle = _random.NextDouble() * 2 * Math.PI;
                    double deltaX = Math.Cos(angle) * speed;
                    double deltaY = Math.Sin(angle) * speed;
                    herbivore.Move(deltaX, deltaY); // Déplace l'herbivore
                }
            }

            // Déplacer les carnivores
            foreach (var carnivore in _carnivores)
            {
                carnivore.UpdateEnergy(); // Met à jour l'énergie du carnivore
                if (!carnivore.IsDead) // Si le carnivore est vivant
                {
                    double angle = _random.NextDouble() * 2 * Math.PI;
                    double speed = _random.NextDouble() * 10; // Les carnivores sont plus rapides
                    double deltaX = Math.Cos(angle) * speed;
                    double deltaY = Math.Sin(angle) * speed;
                    carnivore.Move(deltaX, deltaY); // Déplace le carnivore
                }
            }

            // Vérifications et mises à jour de la simulation
            CheckCollisions();  // Gère les collisions
            CheckReproduction(); // Gère les reproductions
            HandleBirths();      // Gère les naissances
            HandlePlantConsumption();  // Appel pour gérer la consommation des plantes
        }
    }
}

