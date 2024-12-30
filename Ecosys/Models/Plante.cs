using Avalonia;

namespace EcoSys
{
    public class Plante : EcosysObjet
    {
        public Plante(Point location) : base(location)
        {
            SizeRadius = 20;  // Taille spécifique pour une plante
        }
    }
}

