// Herbivore.cs
using Avalonia;
using System;

namespace EcoSys
{
    public class Herbivore : Animal
{
    private const double ENERGY_LOSS_PER_TICK = 0.05;
    private const double MOVEMENT_ENERGY_COST = 0.08;

    public Herbivore(double left, double top, double canvasWidth, double canvasHeight)
        : base(left, top, canvasWidth, canvasHeight)
    {
        SizeRadius = 20.0; // Taille spécifique
    }

    public override void Move(double deltaX, double deltaY)
    {
        if (IsDead || Energy < MOVEMENT_ENERGY_COST) return;

        // Calculer nouvelle position
        double newLeft = Location.X + deltaX;
        double newTop = Location.Y + deltaY;

        // Gérer les rebonds sur les bords
        if (newLeft < 0 || newLeft > CanvasWidth) deltaX *= -1;
        if (newTop < 0 || newTop > CanvasHeight) deltaY *= -1;

        Location = new Point(
            Math.Clamp(newLeft, 0, CanvasWidth),
            Math.Clamp(newTop, 0, CanvasHeight)
        );

        Energy -= MOVEMENT_ENERGY_COST;
    }

    public void EatPlante() => Energy = Math.Min(Energy + 20, MAX_ENERGY);

    public override void UpdateEnergy(double lossPerTick = ENERGY_LOSS_PER_TICK)
    {
        base.UpdateEnergy(lossPerTick);
    }
}


}