using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DevilDesireDevLib.Implementation.Controls
{
    public class PullToRefreshListView : ListView
    {
        #region Constructor
        public PullToRefreshListView()
        {
            DefaultStyleKey = typeof(PullToRefreshListView);
            Loaded += PullToRefreshScrollViewer_Loaded;
        }
        #endregion

        #region Event
        public event EventHandler RefreshContent;
        public event EventHandler MoreContent;
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty PullPartTemplateProperty =
            DependencyProperty.Register("PullPartTemplate", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ReleasePartTemplateProperty =
            DependencyProperty.Register("ReleasePartTemplate", typeof(string), typeof(PullToRefreshListView), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty RefreshHeaderHeightProperty =
            DependencyProperty.Register("RefreshHeaderHeight", typeof(double), typeof(PullToRefreshListView), new PropertyMetadata(30D));

        #endregion

        #region Property
        public string PullPartTemplate
        {
            get
            {
                return (string)GetValue(PullPartTemplateProperty);
            }
            set
            {
                SetValue(PullPartTemplateProperty, value);
            }
        }

        public string ReleasePartTemplate
        {
            get
            {
                return (string)GetValue(ReleasePartTemplateProperty);
            }
            set
            {
                SetValue(ReleasePartTemplateProperty, value);
            }
        }

        public double RefreshHeaderHeight
        {
            get
            {
                return (double)GetValue(RefreshHeaderHeightProperty);
            }
            set
            {
                SetValue(RefreshHeaderHeightProperty, value);
            }
        }

        public Brush ArrowColor
        {
            get
            {
                return (Brush)GetValue(ArrowColorProperty);
            }
            set
            {
                SetValue(ArrowColorProperty, value);
            }
        }

        public static DependencyProperty ArrowColorProperty { get; } = DependencyProperty.Register("ArrowColor", typeof(Brush), typeof(PullToRefreshListView), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        #endregion

        #region Field

        private const double OFFSET_TRESHHOLD = 40;
        private DispatcherTimer m_CompressionTimer;
        private ScrollViewer m_RootScrollViewer;
        private DispatcherTimer m_Timer;
        private Grid m_ContainerGrid;
        private Border m_PullToRefreshIndicator;
        private bool m_IsCompressionTimerRunning;
        private bool m_IsReadyToRefresh;
        private bool m_IsCompressedEnough;
        #endregion

        #region Override Method
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_RootScrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            if (m_RootScrollViewer != null)
            {
                m_RootScrollViewer.ViewChanging += ScrollViewer_ViewChanging;
                m_RootScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
                m_RootScrollViewer.Margin = new Thickness(0, 0, 0, -RefreshHeaderHeight);
                m_RootScrollViewer.RenderTransform = new CompositeTransform() { TranslateY = -RefreshHeaderHeight };
            }

            m_ContainerGrid = GetTemplateChild("ContainerGrid") as Grid;

            m_PullToRefreshIndicator = GetTemplateChild("PullToRefreshIndicator") as Border;
            SizeChanged += OnSizeChanged;
        }
        #endregion

        #region Routed Event
        private void PullToRefreshScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            m_Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            m_Timer.Tick += Timer_Tick;

            m_CompressionTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            m_CompressionTimer.Tick += CompressionTimer_Tick;

            m_Timer.Start();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
        }

        private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (Math.Abs(e.NextView.VerticalOffset) < 0.1)
            {
                m_Timer.Start();
            }
            else
            {
                m_Timer?.Stop();

                m_CompressionTimer?.Stop();

                m_IsCompressionTimerRunning = false;
                m_IsCompressedEnough = false;
                m_IsReadyToRefresh = false;

                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            if (m_ContainerGrid != null)
            {
                Rect elementBounds = m_PullToRefreshIndicator.TransformToVisual(m_ContainerGrid).TransformBounds(new Rect(0.0, 0.0, m_PullToRefreshIndicator.Height, RefreshHeaderHeight));
                double compressionOffset = elementBounds.Bottom;

                if (compressionOffset > OFFSET_TRESHHOLD)
                {
                    if (m_IsCompressionTimerRunning == false)
                    {
                        m_IsCompressionTimerRunning = true;
                        m_CompressionTimer.Start();
                    }

                    m_IsCompressedEnough = true;
                }
                else if (Math.Abs(compressionOffset) < 0.1 && m_IsReadyToRefresh)
                {
                    InvokeRefresh();
                }
                else
                {
                    m_IsCompressedEnough = false;
                    m_IsCompressionTimerRunning = false;
                }
            }
        }

        private void CompressionTimer_Tick(object sender, object e)
        {
            if (m_IsCompressedEnough)
            {
                VisualStateManager.GoToState(this, "ReadyToRefresh", true);
                m_IsReadyToRefresh = true;
            }
            else
            {
                m_IsCompressedEnough = false;
                m_CompressionTimer.Stop();
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (Math.Abs(m_RootScrollViewer.VerticalOffset - m_RootScrollViewer.ScrollableHeight) < 0.1)
            {
                InvokeMore();
            }
        }
        #endregion

        #region Misc
        private void InvokeRefresh()
        {
            m_IsReadyToRefresh = false;
            VisualStateManager.GoToState(this, "Normal", true);

            RefreshContent?.Invoke(this, EventArgs.Empty);
        }

        private void InvokeMore()
        {
            MoreContent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}