using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace CheckboxWithInnerShadow
{
    public partial class Checkbox : ContentView
    {
        private static readonly float ShadowRatio = 0.4f;
        private static readonly float ShadowCoordinateRatio = (1 - ShadowRatio) / 2;

        public Checkbox()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), false);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            IsChecked = !IsChecked;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            float height = (float)info.Height;
            float width = (float)info.Width;

            canvas.Clear();

            if (IsChecked)
            {
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Blue.ToSKColor(),
                    StrokeWidth = height
                };
                canvas.DrawRect(0, 0, width, height, paint);
            }
            else
            {
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Black.ToSKColor(),
                    StrokeWidth = height
                };
                canvas.DrawRect(0, 0, width, height, paint);


                float innerWidth = (float)(width * ShadowRatio);
                float innerHeight = (float)(height * ShadowRatio);

                float innerX = (float)(width * ShadowCoordinateRatio);
                float innerY = (float)(height * ShadowCoordinateRatio);
                SKPaint innerPaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Red.ToSKColor(),
                    StrokeWidth = innerHeight,
                    MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 9.0f)
                };
                canvas.DrawRect(innerY, innerX, innerWidth, innerHeight, innerPaint);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(IsChecked):
                case nameof(Height):
                case nameof(Width):
                    CheckboxCanvas.InvalidateSurface();
                    break;
            }
        }
    }
}
