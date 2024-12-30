using Avalonia;

namespace EcoSys
{
    public class Viande : EcosysObjet
    {
        public Viande(Point location) : base(location)
        {
            SizeRadius = 25; // Définir la taille du rayon pour les collisions
        }
    }
}

