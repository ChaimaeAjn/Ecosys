using Avalonia.Threading;
using System;

namespace EcoSys.ViewModels
{
    public class Plante : ViewModelBase
    {
        private readonly Random _random = new Random();
        private double _imageLeft;
        private double _imageTop;
        private double _canvasWidth = 800;  // Taille du Canvas
        private double _canvasHeight = 450; // Taille du Canvas

        public double ImageLeft
        {
            get => _imageLeft;
            set => SetProperty(ref _imageLeft, value);
        }

        public double ImageTop
        {
            get => _imageTop;
            set => SetProperty(ref _imageTop, value);
        }

        public double CanvasWidth
        {
            get => _canvasWidth;
            set => SetProperty(ref _canvasWidth, value);
        }

        public double CanvasHeight
        {
            get => _canvasHeight;
            set => SetProperty(ref _canvasHeight, value);
        }

        public Plante(double canvasWidth, double canvasHeight)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
            RandomizePosition();
        }

        private void RandomizePosition()
        {
            _imageLeft = _random.Next(0, (int)(_canvasWidth - 50));  // Plante à une position aléatoire
            _imageTop = _random.Next(0, (int)(_canvasHeight - 50));
        }
    }
}
