using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace EcoSys.Converters
{
    // Convertisseur qui permet de diviser une valeur par un double spécifié dans le paramètre
    public class DivideByDoubleConverter : IValueConverter
    {
        // Méthode pour effectuer la conversion (diviser la valeur par un double)
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Vérifie si la valeur est un double et si le paramètre est un string pouvant être converti en double
            if (value is double doubleValue && parameter is string parameterString && double.TryParse(parameterString, out double doubleParameter))
            {
                return doubleValue / doubleParameter; // Divise la valeur par le paramètre
            }
            return 0; // Si les conditions ne sont pas remplies, retourner 0
        }

        // Méthode de conversion inverse non implémentée
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // L'inversion de la conversion n'est pas nécessaire
        }
    }

    // Convertisseur qui affiche des cœurs (utilisé pour afficher une interface avec des cœurs en fonction de l'énergie)
    public class HeartsToVisibilityConverter : IValueConverter
    {
        // Méthode pour convertir la valeur d'énergie en visibilité des cœurs
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Vérifie si la valeur est un entier représentant le nombre de cœurs
            if (value is int hearts && parameter is string heartIndex)
            {
                // Convertir l'index du cœur en entier
                int index = int.Parse(heartIndex);
                // Retourne true si le nombre de cœurs est supérieur ou égal à l'index du cœur, sinon false
                return hearts >= index ? true : false;
            }
            return false; // Retourne false si la valeur n'est pas valide
        }

        // Méthode de conversion inverse non implémentée
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // L'inversion de la conversion n'est pas nécessaire
        }
    }
}
