using System;

namespace EcoSys
{
    // Classe générique représentant la gestation d'un objet EcosysObjet (par exemple, un animal)
    public class Gestation<T> where T : EcosysObjet
    {
        // La mère qui est en gestation, peut être un carnivore ou un herbivore
        public T Mere { get; }

        // La date de fin de la gestation
        public DateTime FinGestation { get; }

        // Constructeur de la classe Gestation qui initialise la mère et la date de fin de gestation
        public Gestation(T mere, TimeSpan dureeGestation)
        {
            Mere = mere; // Affecte la mère à la propriété Mere
            FinGestation = DateTime.Now + dureeGestation; // Calcule la fin de la gestation en ajoutant la durée à la date actuelle
        }
    }
}
