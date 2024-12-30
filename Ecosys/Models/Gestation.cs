using System;

namespace EcoSys
{
    public class Gestation<T> where T : EcosysObjet
    {
        public T Mere { get; }             // La mère (carnivore ou herbivore)
        public DateTime FinGestation { get; } // Date de fin de la gestation

        public Gestation(T mere, TimeSpan dureeGestation)
        {
            Mere = mere;
            FinGestation = DateTime.Now + dureeGestation;
        }
    }
}
