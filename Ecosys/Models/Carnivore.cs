using Avalonia;
using System;

namespace EcoSys
{
    // Classe représentant un carnivore qui hérite de la classe Animal
    public class Carnivore : Animal
    {
        // Constantes pour la perte d'énergie et le coût du mouvement
        private const double ENERGY_LOSS_PER_TICK = 0.1; // Perte d'énergie par tick (par défaut)
        private const double MOVEMENT_ENERGY_COST = 0.1; // Coût en énergie pour se déplacer

        // Constructeur de la classe Carnivore qui initialise la position et la taille
        public Carnivore(double left, double top, double canvasWidth, double canvasHeight)
            : base(left, top, canvasWidth, canvasHeight)
        {
            SizeRadius = 15.0; // Taille du carnivore
        }

        // Méthode pour déplacer le carnivore
        public override void Move(double deltaX, double deltaY)
        {
            // Si l'animal est mort ou n'a pas assez d'énergie, on ne le déplace pas
            if (IsDead || Energy < MOVEMENT_ENERGY_COST) return;

            // Calculer la nouvelle position
            double newLeft = Location.X + deltaX;
            double newTop = Location.Y + deltaY;

            // Vérifier les rebonds sur les bords du canvas et inverser la direction si nécessaire
            if (newLeft < 0 || newLeft > CanvasWidth) deltaX *= -1;
            if (newTop < 0 || newTop > CanvasHeight) deltaY *= -1;

            // Mettre à jour la position du carnivore en s'assurant qu'il reste à l'intérieur du canvas
            Location = new Point(
                Math.Clamp(newLeft, 0, CanvasWidth), // Limiter à la largeur du canvas
                Math.Clamp(newTop, 0, CanvasHeight) // Limiter à la hauteur du canvas
            );

            // Réduire l'énergie du carnivore en fonction du coût du mouvement
            Energy -= MOVEMENT_ENERGY_COST;
        }

        // Méthode pour que le carnivore mange un herbivore (récupère de l'énergie)
        public void EatHerbivore() => Energy = Math.Min(Energy + 30, MAX_ENERGY); // Ajoute 30 à l'énergie, mais ne dépasse pas l'énergie maximale

        // Méthode pour que le carnivore mange de la viande (récupère de l'énergie)
        public void EatMeat() => Energy = Math.Min(Energy + 15, MAX_ENERGY); // Ajoute 15 à l'énergie, mais ne dépasse pas l'énergie maximale

    }
}
