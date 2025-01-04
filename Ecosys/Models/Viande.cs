using Avalonia;

namespace EcoSys
{
    // Classe représentant de la viande, héritant de EcosysObjet
    public class Viande : EcosysObjet
    {
        // Constructeur de la classe Viande qui initialise la localisation de la viande
        public Viande(Point location) : base(location)
        {
            SizeRadius = 25; // Définir la taille du rayon pour les collisions (taille spécifique de la viande)
        }
    }
}
