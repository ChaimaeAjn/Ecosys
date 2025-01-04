using Avalonia;

namespace EcoSys
{
    // Classe représentant une plante, héritant de EcosysObjet
    public class Plante : EcosysObjet
    {
        // Constructeur de la classe Plante qui initialise la localisation de la plante
        public Plante(Point location) : base(location)
        {
            SizeRadius = 10;  // Taille spécifique pour une plante, ici 10
        }
    }
}
