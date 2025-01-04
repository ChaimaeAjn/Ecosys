using Avalonia;
using System;

namespace EcoSys
{
    public abstract class Animal : EcosysObjet
    {
        // Enumération définissant les types de sexes pour les animaux
        public enum SexeType { Male, Femelle }

        // Sexe de l'animal (aléatoirement assigné lors de la création)
        public SexeType Sexe { get; }

        // Dimensions du canvas (pour limiter les déplacements de l'animal)
        public double CanvasWidth { get; }
        public double CanvasHeight { get; }

        private double _energy;
        private int _hearts; // Représente les cœurs

        // Constantes pour la gestion de l'énergie
        protected const double MAX_ENERGY = 100.0;
        private const double ENERGY_LOSS_PER_TICK = 0.1;
        private const double MOVEMENT_ENERGY_COST = 0.1;

        // Propriétés de l'animal
        public double Energy
        {
            get => _energy;
            set
            {
                if (SetProperty(ref _energy, Math.Max(0, Math.Min(value, MAX_ENERGY))))
                {
                    // Si l'énergie tombe à zéro, on fait perdre un cœur
                    if (_energy <= 0)
                    {
                        LoseHeart(); // Perdre un cœur si l'énergie tombe à zéro
                    }
                }
            }
        }

        public int Hearts
        {
            get => _hearts;
            private set
            {
                if (SetProperty(ref _hearts, Math.Max(0, value)))
                {
                    IsDead = _hearts <= 0; // L'animal est mort si il n'a plus de cœurs
                }
            }
        }

        // Propriété indiquant si l'animal est mort
        public bool IsDead { get; protected set; }

        // Rayon de la taille de l'animal, surchargé depuis la classe de base 'EcosysObjet'
        public override double SizeRadius { get; protected set; }

        // Constructeur qui initialise les propriétés de l'animal
        protected Animal(double x, double y, double canvasWidth, double canvasHeight)
            : base(new Point(x, y)) 
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            Sexe = (SexeType)new Random().Next(0, 2); // Sexe assigné aléatoirement
            _energy = MAX_ENERGY; // L'animal commence avec l'énergie maximale
            Hearts = 3; // L'animal commence avec 3 cœurs
            IsDead = false; // Initialement, l'animal est vivant
        }

        // Méthode pour faire perdre un cœur à l'animal
        public virtual void LoseHeart()
        {
            if (Hearts > 0)
            {
                Hearts--; // Réduire le nombre de cœurs
                _energy = MAX_ENERGY; // Réinitialiser l'énergie après la perte du cœur
                if (Hearts <= 0) 
                {
                    IsDead = true; // L'animal meurt si il n'a plus de cœurs
                }
            }
        }

        // Méthode abstraite que les sous-classes doivent implémenter pour définir le déplacement
        public abstract void Move(double deltaX, double deltaY);

        // Méthode pour mettre à jour l'énergie à chaque "tick" de la simulation
        public void UpdateEnergy()
        {
            // Réduire l'énergie à chaque "tick"
            Energy -= ENERGY_LOSS_PER_TICK;
        }
    }
}
