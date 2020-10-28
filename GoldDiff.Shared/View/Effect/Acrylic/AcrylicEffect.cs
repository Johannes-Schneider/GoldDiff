using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace GoldDiff.Shared.View.Effect.Acrylic
{
    public class AcrylicEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), MethodBase.GetCurrentMethod().DeclaringType, 0);

        public static readonly DependencyProperty RandomInputProperty = RegisterPixelShaderSamplerProperty(nameof(RandomInput), MethodBase.GetCurrentMethod().DeclaringType, 1);

        public static readonly DependencyProperty RatioProperty = DependencyProperty.Register(nameof(Ratio), typeof(double), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                              new UIPropertyMetadata(0.15d, PixelShaderConstantCallback(0)));

        public Brush? Input
        {
            get => GetValue(InputProperty) as Brush;
            set
            {
                SetValue(InputProperty, value ?? throw new ArgumentNullException(nameof(Input)));
                UpdateShaderValue(InputProperty);
            }
        }

        public Brush RandomInput
        {
            get => (GetValue(RandomInputProperty) as Brush)!;
            set
            {
                SetValue(RandomInputProperty, value ?? throw new ArgumentNullException(nameof(RandomInput)));
                UpdateShaderValue(RandomInputProperty);
            }
        }

        public double Ratio
        {
            get => (double) GetValue(RatioProperty);
            set
            {
                SetValue(RatioProperty, value);
                UpdateShaderValue(RatioProperty);
            }
        }

        public AcrylicEffect()
        {
            var pixelShader = new PixelShader {UriSource = new Uri("pack://application:,,,/GoldDiff.Shared;component/View/Effect/Acrylic/Shader.ps")};
            PixelShader = pixelShader;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/GoldDiff.Shared;component/View/Effect/Acrylic/Noise.png");
            bitmap.EndInit();
            RandomInput =
                new ImageBrush(bitmap)
                {
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0, 0, 800, 600),
                    ViewportUnits = BrushMappingMode.Absolute,
                };

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RandomInputProperty);
            UpdateShaderValue(RatioProperty);
        }
    }
}