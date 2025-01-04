// Herbivore.cs
using Avalonia;
using System;

namespace EcoSys
{
    // Classe représentant un herbivore qui hérite de la classe Animal
    public class Herbivore : Animal
    {
        // Constantes pour la perte d'énergie et le coût du mouvement
        private const double ENERGY_LOSS_PER_TICK = 0.05; // Perte d'énergie par tick (par défaut)
        private const double MOVEMENT_ENERGY_COST = 0.08; // Coût en énergie pour se déplacer

        // Constructeur de la classe Herbivore qui initialise la position et la taille
        public Herbivore(double left, double top, double canvasWidth, double canvasHeight)
            : base(left, top, canvasWidth, canvasHeight)
        {
            SizeRadius = 20.0; // Taille spécifique de l'herbivore
        }

        // Méthode pour déplacer l'herbivore
        public override void Move(double deltaX, double deltaY)
        {
            // Si l'herbivore est mort ou n'a pas assez d'énergie, on ne le déplace pas
            if (IsDead || Energy < MOVEMENT_ENERGY_COST) return;

            // Calculer la nouvelle position
            double newLeft = Location.X + deltaX;
            double newTop = Location.Y + deltaY;

            // Vérifier les rebonds sur les bords du canvas et inverser la direction si nécessaire
            if (newLeft < 0 || newLeft > CanvasWidth) deltaX *= -1;
            if (newTop < 0 || newTop > CanvasHeight) deltaY *= -1;

            // Mettre à jour la position de l'herbivore en s'assurant qu'il reste à l'intérieur du canvas
            Location = new Point(
                Math.Clamp(newLeft, 0, CanvasWidth), // Limiter la position sur l'axe X dans les limites du canvas
                Math.Clamp(newTop, 0, CanvasHeight) // Limiter la position sur l'axe Y dans les limites du canvas
            );

            // Réduire l'énergie de l'herbivore en fonction du coût du mouvement
            Energy -= MOVEMENT_ENERGY_COST;
        }

        // Méthode pour que l'herbivore mange une plante et récupère de l'énergie
        public void EatPlante() => Energy = Math.Min(Energy + 20, MAX_ENERGY); // Récupère 20 d'énergie, mais ne dépasse pas l'énergie maximale

    }
}
