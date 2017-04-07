using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DevilDesireDevLib.Implementation.Controls
{
    [TemplateVisualState(Name = "ContentExpanded", GroupName = "ContentStates")]
    [TemplateVisualState(Name = "ContentCollapsed", GroupName = "ContentStates")]
    [TemplatePart(Name = "PART_ExpanderToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_HeaderButton", Type = typeof(ButtonBase))]
    [ContentProperty(Name = "Content")]
    public sealed class Expander : Control
    {
        internal static readonly DependencyProperty ExpanderToggleButtonStyleProperty =
            DependencyProperty.Register("ExpanderToggleButtonStyle", typeof(Style), typeof(Expander),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty HeaderButtonStyleProperty =
            DependencyProperty.Register("HeaderButtonStyle", typeof(Style), typeof(Expander),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Expander),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty HeaderCommandProperty =
            DependencyProperty.Register("HeaderCommand", typeof(ICommand), typeof(Expander),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Expander), new PropertyMetadata(null));


        internal static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(Expander), new PropertyMetadata(false, OnIsExpandedPropertyChanged));

        internal static readonly DependencyProperty TextOnlyProperty =
            DependencyProperty.Register("TextOnly", typeof (bool), typeof (Expander), new PropertyMetadata(false));

        internal static readonly DependencyProperty ExpanderBackgroundProperty =
            DependencyProperty.Register("ExpanderBackground", typeof(SolidColorBrush), typeof(Expander), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        internal static readonly DependencyProperty LeftLineProperty =
            DependencyProperty.Register("LeftLine", typeof(SolidColorBrush), typeof(Expander), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expander = d as Expander;
            if (expander?.m_ExpanderButton == null)
                return;
            var isExpanded = (bool)e.NewValue;
            expander.m_ExpanderButton.IsChecked = isExpanded;
            if (isExpanded)
                expander.ExpandControl();
            else
                expander.CollapseControl();
        }


        private ToggleButton m_ExpanderButton;
        private ButtonBase m_HeaderButton;
        private Border m_HeaderBorder;
        private Rectangle m_LeftLine;

        private RowDefinition m_MainContentRow;

        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ICommand HeaderCommand
        {
            get { return (ICommand)GetValue(HeaderCommandProperty); }
            set { SetValue(HeaderCommandProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public bool TextOnly
        {
            get { return (bool) GetValue(TextOnlyProperty); }
            set { SetValue(TextOnlyProperty, value); }
        }

        public SolidColorBrush ExpanderBackround
        {
            get { return (SolidColorBrush) GetValue(ExpanderBackgroundProperty); }
            set { SetValue(ExpanderBackgroundProperty, value); }
        }

        public SolidColorBrush LeftLine
        {
            get { return (SolidColorBrush) GetValue(LeftLineProperty); }
            set { SetValue(LeftLineProperty, value); }
        }

        public Style HeaderButtonStyle
        {
            get { return (Style)GetValue(HeaderButtonStyleProperty); }
            set { SetValue(HeaderButtonStyleProperty, value); }
        }

        public Style ExpanderToggleButtonStyle
        {
            get { return (Style)GetValue(ExpanderToggleButtonStyleProperty); }
            set { SetValue(ExpanderToggleButtonStyleProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            m_ExpanderButton = GetTemplateChild("PART_ExpanderToggleButton") as ToggleButton;
            m_HeaderButton = GetTemplateChild("PART_HeaderButton") as ButtonBase;
            m_HeaderBorder = GetTemplateChild("PART_HeaderBorder") as Border;
            m_MainContentRow = GetTemplateChild("PART_MainContentRow") as RowDefinition;
            m_LeftLine = GetTemplateChild("LeftLine") as Rectangle;

            if (m_ExpanderButton != null)
            {
                m_ExpanderButton.Checked += OnExpanderButtonChecked;
                m_ExpanderButton.Unchecked += OnExpanderButtonUnChecked;
                m_ExpanderButton.IsChecked = IsExpanded;
                m_ExpanderButton.Visibility = TextOnly ? Visibility.Collapsed : Visibility.Visible;
                m_HeaderBorder.Background = ExpanderBackround;
                m_LeftLine.Stroke = LeftLine;

                if (IsExpanded)
                    ExpandControl();
                else
                    CollapseControl();
            }

            if (m_HeaderBorder != null)
            {
                m_HeaderBorder.Tapped += OnHeaderButtonClick;
            }
        }

        private void OnHeaderButtonClick(object sender, RoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
            if (IsExpanded)
            {
                ExpandControl();
            }
            else
            {
                CollapseControl();
            }
        }

        private void ExpandControl()
        {
            if (m_MainContentRow == null || !m_MainContentRow.Height.Value.Equals(0d))
                return;
            VisualStateManager.GoToState(this, "ContentExpanded", true);
            m_MainContentRow.Height = new GridLength(1, GridUnitType.Auto);
        }

        private void CollapseControl()
        {
            if (m_MainContentRow == null || m_MainContentRow.Height.Value.Equals(0d))
                return;
            VisualStateManager.GoToState(this, "ContentCollapsed", true);
            m_MainContentRow.Height = new GridLength(0d);
        }

        private void OnExpanderButtonUnChecked(object sender, RoutedEventArgs e)
        {
            IsExpanded = false;
            CollapseControl();
        }

        private void OnExpanderButtonChecked(object sender, RoutedEventArgs e)
        {
            IsExpanded = true;
            ExpandControl();
        }
    }
}
