using System;
using Avalonia.Threading;
using System.ComponentModel;

namespace EcoSys.ViewModels
{
    public class Animal : ViewModelBase
    {
        private readonly Random _random = new Random();
        private double _imageLeft;
        private double _imageTop;
        private double _canvasWidth;
        private double _canvasHeight;

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

        public Animal(double initialX, double initialY, double canvasWidth, double canvasHeight)
        {
            _imageLeft = initialX;
            _imageTop = initialY;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;

            StartMoving();
        }

        public void StartMoving()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
            timer.Tick += (sender, args) =>
            {
                var deltaX = _random.NextDouble() * 6 - 3; // Mouvement aléatoire (-3, 3)
                var deltaY = _random.NextDouble() * 6 - 3;

                ImageLeft = Math.Clamp(ImageLeft + deltaX, 0, _canvasWidth - 50);
                ImageTop = Math.Clamp(ImageTop + deltaY, 0, _canvasHeight - 50);
            };
            timer.Start();
        }
    }
}
