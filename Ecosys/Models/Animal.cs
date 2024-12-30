using Avalonia;
using System;

namespace EcoSys
{
    public abstract class Animal : EcosysObjet
    {
        public enum SexeType { Male, Femelle }
        public SexeType Sexe { get; }
        public double CanvasWidth { get; }
        public double CanvasHeight { get; }
        public double Energy { get; protected set; }
        public int Hearts { get; protected set; }
        public bool IsDead { get; protected set; }
        
        // Utiliser 'new' si vous voulez masquer 'SizeRadius' de 'EcosysObjet'
        public override double SizeRadius { get; protected set; }

        protected const double MAX_ENERGY = 100.0;

        // Constructeur qui initialise Location et appelle le constructeur de la classe de base
        protected Animal(double x, double y, double canvasWidth, double canvasHeight)
            : base(new Point(x, y)) // Appel au constructeur de la classe de base EcosysObjet
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            Sexe = (SexeType)new Random().Next(0, 2); // Assignation aléatoire du sexe
            Energy = MAX_ENERGY;
            Hearts = 3; // Nombre initial de cœurs
            IsDead = false;
        }

        public virtual void LoseHeart()
        {
            if (Hearts > 0)
            {
                Hearts--;
                Energy = MAX_ENERGY; // Réinitialiser l'énergie
                if (Hearts <= 0) IsDead = true; // Mort si plus de cœurs
            }
        }

        // Méthode abstraite que chaque sous-classe doit implémenter
        public abstract void Move(double deltaX, double deltaY);

        public void SetEnergy(double value)
    {
        Energy = value;
        if (Energy <= 0)
        {
            IsDead = true;
        }
    }

        // Méthode pour mettre à jour l'énergie à chaque "tick"
        public virtual void UpdateEnergy(double lossPerTick)
        {
            Energy = Math.Max(0, Energy - lossPerTick);
            if (Energy <= 0) LoseHeart();
        }
    }
}
