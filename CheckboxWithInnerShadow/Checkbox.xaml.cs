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

        private SKColor boxColor = Color.FromHex("#3A87D3").ToSKColor();
        private SKColor shadowColor = Color.FromHex("#1C68AF").ToSKColor();
        private SKColor checkedColor = Color.FromHex("#F2CB3F").ToSKColor();


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


        public static readonly BindableProperty CanvasSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(CanvasSize),
                returnType: typeof(Double),
                declaringType: typeof(Checkbox),
                defaultValue: 100.00,
                propertyChanged: OnCanvasSizeChanged);

        public Double CanvasSize
        {
            get => (Double)GetValue(CanvasSizeProperty);
            set => SetValue(CanvasSizeProperty, value);
        }

        private static void OnCanvasSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Checkbox typedBindable && newValue is Double typedValue)
            {
                typedBindable.CheckboxCanvas.HeightRequest = typedValue;
                typedBindable.CheckboxCanvas.WidthRequest = typedValue;
            }
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
                    Color = checkedColor,
                    StrokeWidth = height
                };
                canvas.DrawRect(0, 0, width, height, paint);
            }
            else
            {
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = shadowColor,
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
                    Color = boxColor,
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
                case nameof(CanvasSize):
                case nameof(Height):
                case nameof(Width):
                    CheckboxCanvas.InvalidateSurface();
                    break;
            }
        }
    }
}
