// Herbivore.cs
using Avalonia;
using System;

namespace EcoSys
{
    public class Herbivore : EcosysObjet
    {
        private double _imageLeft;
        private double _imageTop;
        private readonly double _canvasWidth;
        private readonly double _canvasHeight;
        private double _energy;
        private int _hearts; // Représente les cœurs
        private const double MAX_ENERGY = 100.0;
        private const double ENERGY_LOSS_PER_TICK = 0.05; // Perd moins d'énergie que les carnivores
        private const double MOVEMENT_ENERGY_COST = 0.08;

        public double Energy
        {
            get => _energy;
            set
            {
                if (SetProperty(ref _energy, Math.Max(0, Math.Min(value, MAX_ENERGY))))
                {
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
                    IsDead = _hearts <= 0; // Mort si plus de cœurs
                }
            }
        }
        public bool IsDead { get; private set; }

        public double ImageLeft
        {
            get => _imageLeft;
            set
            {
                if (SetProperty(ref _imageLeft, value))
                {
                    Location = new Point(value, ImageTop);
                }
            }
        }

        public double ImageTop
        {
            get => _imageTop;
            set
            {
                if (SetProperty(ref _imageTop, value))
                {
                    Location = new Point(ImageLeft, value);
                }
            }
        }

        public Herbivore(double left, double top, double canvasWidth, double canvasHeight)
            : base(new Point(left, top))
        {
            _imageLeft = left;
            _imageTop = top;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
            _energy = MAX_ENERGY;
            _hearts = 3; // Les carnivores commencent avec 3 cœurs
            IsDead = false;
        }
        private void LoseHeart()
        {
            if (Hearts > 0)
            {
                Hearts--; // Perdre un cœur
                _energy = MAX_ENERGY; // Réinitialiser l'énergie
            }
        }

        public void Move(double deltaX, double deltaY)
        {
            if (IsDead || Energy < MOVEMENT_ENERGY_COST) return;

            double newLeft = ImageLeft + deltaX;
            double newTop = ImageTop + deltaY;

            // Gérer les rebonds sur les bords
            if (newLeft < 0)
            {
                newLeft = 0;
                deltaX *= -1;
            }
            else if (newLeft > _canvasWidth - 50)
            {
                newLeft = _canvasWidth - 50;
                deltaX *= -1;
            }

            if (newTop < 0)
            {
                newTop = 0;
                deltaY *= -1;
            }
            else if (newTop > _canvasHeight - 50)
            {
                newTop = _canvasHeight - 50;
                deltaY *= -1;
            }

            ImageLeft = newLeft;
            ImageTop = newTop;
            Energy -= MOVEMENT_ENERGY_COST;
        }

        public void UpdateEnergy()
        {
            Energy -= ENERGY_LOSS_PER_TICK;
        }

        public void EatPlante()
        {
            Energy += 20; // Gain d'énergie en mangeant une plante
        }
    }
}