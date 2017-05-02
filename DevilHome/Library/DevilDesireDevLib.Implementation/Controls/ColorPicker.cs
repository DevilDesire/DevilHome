using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace DevilDesireDevLib.Implementation.Controls
{
    public class ColorPicker : RelativePanel
    {
        #region private Values

        private Rectangle m_ActColorElement;
        private Polygon m_Fleche;
        private Rectangle m_RainbowPanel;
        private Color m_ActSpectre;
        private SolidColorBrush m_ActColor;
        private RelativePanel m_ChoiceGrid;
        private Canvas m_PickerCanvas;
        private Grid m_GridEllipse;
        private double m_X;
        private double m_Y;
        private bool m_ActualColorChanged;

        #endregion

        #region DependencyPropertys

        public static readonly DependencyProperty SpectrePointerColorProperty = DependencyProperty.Register(
            "SpectrePointerColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(default(Color), SpectrePointerColorChangedCallback));

        public static readonly DependencyProperty ActualColorProperty = DependencyProperty.Register(
            "ActualColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(default(Color), ActualColorChangedCallback));

        private static void SpectrePointerColorChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ColorPicker classe = dependencyObject as ColorPicker;
            classe?.UpdateSpectrePointerColor();
        }

        private static void ActualColorChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ColorPicker classe = dependencyObject as ColorPicker;
            classe?.UpdateActualColor();
        }

        public delegate void ActualColorEvent(Color newColor);

        #endregion

        #region Events

        public event ActualColorEvent ActualColorChanged;


        #endregion

        #region private Methods

        private void UpdateActualColor()
        {
            if (!m_ActualColorChanged)
            {
                double[] hsl = RgbToHsl(ActualColor);
                Color v = HslToRgb(hsl[0], 1, 0.5);
                m_ChoiceGrid.Background = new SolidColorBrush(v);
                m_ActSpectre = v;
            }
            m_ActualColorChanged = false;
            m_ActColor = new SolidColorBrush(ActualColor);
            m_ActColorElement.Fill = m_ActColor;
        }

        private void UpdateSpectrePointerColor()
        {
            m_Fleche.Fill = new SolidColorBrush(SpectrePointerColor);
        }

        private void DefSpectre()
        {
            m_RainbowPanel = new Rectangle
            {
                Margin = new Thickness(1, 0, 1, 0),
                Width = 50,
                Fill = new LinearGradientBrush
                {
                    GradientStops = {
                        new GradientStop
                        {
                            Offset = 0,
                            Color = Color.FromArgb(255, 255, 0, 0)
                        },
                        new GradientStop
                        {
                            Offset = 0.2,
                            Color = Color.FromArgb(255, 255, 255, 0)
                        },
                        new GradientStop
                        {
                            Offset = 0.4,
                            Color = Color.FromArgb(255, 0, 255, 0)
                        },
                        new GradientStop
                        {
                            Offset = 0.6,
                            Color = Color.FromArgb(255, 0, 0, 255)
                        },
                        new GradientStop
                        {
                            Offset = 0.8,
                            Color = Color.FromArgb(255, 255, 0, 255)
                        },
                        new GradientStop
                        {
                            Offset = 1,
                            Color = Color.FromArgb(255, 255, 0, 0)
                        }
                    },
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1)
                }
            };
            m_RainbowPanel.PointerPressed += (sender, args) =>
            {
                SpectreChoiceOnPointerPressed(sender, args);
                PointerMoved += SpectreChoiceOnPointerPressed;
                PointerReleased += SpectreChoicePointerReleased;
            };

            SetAlignRightWithPanel(m_RainbowPanel, true);
            SetAlignBottomWithPanel(m_RainbowPanel, true);
            SetBelow(m_RainbowPanel, m_ActColorElement);
            Children.Add(m_RainbowPanel);
        }

        private void SpectreChoicePointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            PointerMoved -= SpectreChoiceOnPointerPressed;
            //EndColor = ActualColor;
            PointerReleased -= SpectreChoicePointerReleased;
        }
        private void SpectreChoiceOnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            double t = m_RainbowPanel.ActualHeight;
            double x = args.GetCurrentPoint(m_RainbowPanel).Position.Y;
            x = x <= 0 ? 0 : x;
            Canvas.SetTop(m_Fleche, x);
            
            m_ActSpectre = HslToRgb(x / t * 360, 1, 0.5);
            m_ActualColorChanged = true;
            ActualColor = RecalculerCouleur();
            ActualColorChanged?.Invoke(ActualColor);
            m_ActColorElement.Fill = new SolidColorBrush(ActualColor);
            m_ChoiceGrid.Background = new SolidColorBrush(m_ActSpectre);
        }

        #endregion

        #region Colors

        public Color SpectrePointerColor
        {
            get { return (Color)GetValue(SpectrePointerColorProperty); }
            set { SetValue(SpectrePointerColorProperty, value); }
        }

        public Color ActualColor
        {
            get { return (Color)GetValue(ActualColorProperty); }
            set { SetValue(ActualColorProperty, value); }
        }

        private Color RecalculerCouleur()
        {
            double eheight = m_ChoiceGrid.ActualHeight;
            double ewidth = m_ChoiceGrid.ActualWidth;
            double width = Math.Max(m_X, 0);
            double height = Math.Max(m_Y, 0);
            height = height < eheight ? height : eheight;
            width = width < ewidth ? width : ewidth;
            double ratiox = 1 - width / ewidth;
            double ratioy = 1 - height / eheight;
            byte newr = (byte)((m_ActSpectre.R + (255 - m_ActSpectre.R) * ratiox) * ratioy);
            byte newb = (byte)((m_ActSpectre.B + (255 - m_ActSpectre.B) * ratiox) * ratioy);
            byte newg = (byte)((m_ActSpectre.G + (255 - m_ActSpectre.G) * ratiox) * ratioy);
            return Color.FromArgb(255, newr, newg, newb);
        }

        private void AddColorMainSelection()
        {
            m_ActColor = new SolidColorBrush(m_ActSpectre);
            m_ChoiceGrid = new RelativePanel()
            {
                Background = m_ActColor,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            m_ChoiceGrid.PointerPressed += (sender, args) =>
            {
                UpdatingColor(sender, args);
                PointerMoved += UpdatingColor;
                PointerReleased += GridChoicePointerReleased;
            };

            m_ChoiceGrid.SizeChanged += ColorPicker_SizeChanged;
            m_ChoiceGrid.Children.Add(m_PickerCanvas);

            Rectangle rect1 = new Rectangle
            {
                Fill = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                    GradientStops =
                    {
                        new GradientStop
                        {
                            Offset = 0,
                            Color = Colors.White
                        },
                        new GradientStop
                        {
                            Offset = 1,
                            Color = Color.FromArgb(0, 255, 255, 255)
                        }
                    }
                }
            };

            SetAlignLeftWithPanel(rect1, true);
            SetAlignRightWithPanel(rect1, true);
            SetAlignTopWithPanel(rect1, true);
            SetAlignBottomWithPanel(rect1, true);
            m_ChoiceGrid.Children.Add(rect1);

            Rectangle rect2 = new Rectangle
            {
                Fill = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(0, 1),
                    GradientStops =
                    {
                        new GradientStop
                        {
                            Offset = 0,
                            Color = Color.FromArgb(0, 0, 0, 0)
                        },
                        new GradientStop
                        {
                            Offset = 1,
                            Color = Colors.Black
                        }
                    }
                }
            };

            SetAlignLeftWithPanel(rect2, true);
            SetAlignRightWithPanel(rect2, true);
            SetAlignTopWithPanel(rect2, true);
            SetAlignBottomWithPanel(rect2, true);


            m_ChoiceGrid.Children.Add(rect2);

            SetAlignLeftWithPanel(m_ChoiceGrid, true);
            SetAlignBottomWithPanel(m_ChoiceGrid, true);
            SetBelow(m_ChoiceGrid, m_ActColorElement);
            SetLeftOf(m_ChoiceGrid, m_RainbowPanel);
            m_ChoiceGrid.Margin = new Thickness(0, 0, 10, 0);
            Children.Add(m_ChoiceGrid);

            Canvas flecheCanvas = new Canvas();
            flecheCanvas.SizeChanged += FlecheCanvasSizeChanged;
            m_Fleche = new Polygon
            {
                Points = { new Point(8, -3), new Point(0, 0), new Point(8, 3) },
                Fill = new SolidColorBrush(Colors.White),
            };
            flecheCanvas.Children.Add(m_Fleche);

            SetAlignRightWithPanel(flecheCanvas, true);
            SetAlignBottomWithPanel(flecheCanvas, true);
            SetBelow(flecheCanvas, m_ActColorElement);
            Children.Add(flecheCanvas);

            m_ChoiceGrid.Loaded += _choiceGrid_Loaded;
            ActualColor = Colors.Red;
        }

        private void AddPreviewPanel()
        {
            m_ActColorElement = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                Height = 50
            };

            SetAlignRightWithPanel(m_ActColorElement, true);
            SetAlignTopWithPanel(m_ActColorElement, true);
            SetAlignLeftWithPanel(m_ActColorElement, true);
            m_ActColorElement.Margin = new Thickness(0, 0, 0, 10);
            Children.Add(m_ActColorElement);
        }

        private void UpdatingColor(object sender, PointerRoutedEventArgs args)
        {
            double eheight = m_ChoiceGrid.ActualHeight;
            double ewidth = m_ChoiceGrid.ActualWidth;
            Point i = args.GetCurrentPoint(m_ChoiceGrid).Position;
            m_X = i.X;
            m_Y = i.Y;
            double width = Math.Max(m_X, 0);
            double height = Math.Max(m_Y, 0);
            height = height < eheight ? height : eheight;
            width = width < ewidth ? width : ewidth;
            UpdatePosition(height, width);
            double ratiox = 1 - width / ewidth;
            double ratioy = 1 - height / eheight;
            byte newr = (byte)((m_ActSpectre.R + (255 - m_ActSpectre.R) * ratiox) * ratioy);
            byte newb = (byte)((m_ActSpectre.B + (255 - m_ActSpectre.B) * ratiox) * ratioy);
            byte newg = (byte)((m_ActSpectre.G + (255 - m_ActSpectre.G) * ratiox) * ratioy);
            Color actColor = Color.FromArgb(255, newr, newg, newb);
            m_ActColor = new SolidColorBrush(actColor);
            m_ActColorElement.Fill = m_ActColor;
            m_ActualColorChanged = true;
            ActualColor = actColor;
            ActualColorChanged?.Invoke(actColor);
        }

        private void FlecheCanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double i = Canvas.GetTop(m_Fleche);
            double j = i / e.PreviousSize.Height;
            if (double.IsNaN(j))
                j = 0;
            Canvas.SetTop(m_Fleche, j * e.NewSize.Height);
        }

        private void UpdatePosition(double h, double w)
        {
            m_X = w;
            m_Y = h;
            Canvas.SetTop(m_GridEllipse, h);
            Canvas.SetLeft(m_GridEllipse, w);
        }

        private void GridChoicePointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            PointerMoved -= UpdatingColor;
            PointerReleased -= GridChoicePointerReleased;
        }

        private void ColorPicker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double i = Canvas.GetTop(m_GridEllipse);
            double j = Canvas.GetLeft(m_GridEllipse);
            double k = i / e.PreviousSize.Height;
            double l = j / e.PreviousSize.Width;
            if (double.IsNaN(k))
                k = 0;
            if (double.IsNaN(l))
                l = 0;
            Canvas.SetTop(m_GridEllipse, k * e.NewSize.Height);
            Canvas.SetLeft(m_GridEllipse, l * e.NewSize.Width);
        }

        private void DefPickerCanvas()
        {
            m_PickerCanvas = new Canvas
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };
            m_GridEllipse = new Grid
            {
                Margin = new Thickness(-7, -7, 0, 0)
            };
            Canvas.SetTop(m_GridEllipse, m_Y);
            Canvas.SetLeft(m_GridEllipse, m_Y);
            m_GridEllipse.Children.Add(new Ellipse
            {
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 14,
                Height = 14,
                UseLayoutRounding = false
            });
            m_GridEllipse.Children.Add(new Ellipse
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                Width = 12,
                Height = 12,
                UseLayoutRounding = false
            });
            m_PickerCanvas.Children.Add(m_GridEllipse);
        }

        private void _choiceGrid_Loaded(object sender, RoutedEventArgs e)
        {
            double[] hsl = RgbToHsl(ActualColor);
            double h = m_ChoiceGrid.ActualHeight * (1 - 2 * hsl[2]);
            h = h >= 0 ? h : 0;
            h = h <= m_ChoiceGrid.ActualHeight ? h : m_ChoiceGrid.ActualHeight;
            double w = m_ChoiceGrid.ActualWidth * hsl[1];
            w = w >= 0 ? w : 0;
            w = w <= m_ChoiceGrid.ActualWidth ? w : m_ChoiceGrid.ActualWidth;
            UpdatePosition(h, w);
        }

        #endregion

        #region public Methods

        public static double[] RgbToHsl(Color value)
        {
            return RgbToHsl(value.R, value.G, value.B);
        }

        public static double[] RgbToHsl(double r, double g, double b)
        {
            double m1 = Math.Max(Math.Max(r, g), b);
            double m2 = Math.Min(Math.Min(r, g), b);
            double c = m1 - m2;
            double h2;
            if (c == 0)
            {
                h2 = 0;
            }
            else if (m1 == r)
            {
                h2 = ((g - b) / c + 6) % 6;
            }
            else if (m1 == g)
            {
                h2 = (b - r) / c + 2;
            }
            else
            {
                h2 = (r - g) / c + 4;
            }
            double h = 60f * h2;
            double l = 0.5f * (m1 + m2);
            double s = l == 1 ? 0 : c / m1;
            return new[] { h, s, l / 255f };

        }

        public static Color HslToRgb(double h, double s, double l)
        {
            double hBase = 60;
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs(h / hBase % 2 - 1));
            double m = l - c / 2;
            double r, g, b;
            if (h < hBase)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (h < hBase * 2)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (h < hBase * 3)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (h < hBase * 4)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (h < hBase * 5)
            {
                r = x;
                g = 0;
                b = c;
            }
            else
            {
                r = c;
                g = 0;
                b = x;
            }
            return Color.FromArgb(255, (byte)((r + m) * 255), (byte)((g + m) * 255), (byte)((b + m) * 255));
        }

        public ColorPicker()
        {
            AddPreviewPanel();
            DefPickerCanvas();
            DefSpectre();
            AddColorMainSelection();
        }

        #endregion
    }
}