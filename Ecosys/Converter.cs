using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace EcoSys.Converters
{
    // Convertisseur pour diviser une valeur par un double
    public class DivideByDoubleConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double doubleValue && parameter is string parameterString && double.TryParse(parameterString, out double doubleParameter))
            {
                return doubleValue / doubleParameter;
            }
            return 0;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Convertisseur pour afficher des cœurs (en fonction de la valeur d'énergie)
    public class HeartsToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Vérification si la valeur est un entier représentant le nombre de cœurs
            if (value is int hearts && parameter is string heartIndex)
            {
                // Convertir le paramètre en entier (index du cœur)
                int index = int.Parse(heartIndex);
                return hearts >= index ? true : false;  // Retourner true si le nombre de cœurs est supérieur ou égal à l'index
            }
            return false;  // Retourner false si la valeur n'est pas valide
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
