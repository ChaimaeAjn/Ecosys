using Avalonia;
using System.ComponentModel;

namespace EcoSys
{
    public class Animal : EcosysObjet
    {
        private double _imageLeft;
        private double _imageTop;
        private readonly double _canvasWidth;
        private readonly double _canvasHeight;

        public double ImageLeft
        {
            get => _imageLeft;
            set
            {
                if (SetProperty(ref _imageLeft, value))
                {
                    Location = new Point(value, ImageTop);
                }
            }
        }

        public double ImageTop
        {
            get => _imageTop;
            set
            {
                if (SetProperty(ref _imageTop, value))
                {
                    Location = new Point(ImageLeft, value);
                }
            }
        }

        public Animal(double left, double top, double canvasWidth, double canvasHeight)
            : base(new Point(left, top))
        {
            _imageLeft = left;
            _imageTop = top;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
        }

        public void Move(double deltaX, double deltaY)
        {
            double newLeft = ImageLeft + deltaX;
            double newTop = ImageTop + deltaY;

            // Vérifier les limites du canvas
            if (newLeft >= 0 && newLeft <= _canvasWidth)
            {
                ImageLeft = newLeft;
            }

            if (newTop >= 0 && newTop <= _canvasHeight)
            {
                ImageTop = newTop;
            }
        }
    }
}