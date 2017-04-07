using System;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DevilDesireDevLib.Implementation.Extensions;
using Microsoft.Xaml.Interactivity;

namespace DevilDesireDevLib.Implementation.Behavior
{
    public class ScrollToBottomBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(ScrollToBottomBehavior), new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(object sender,
        DependencyPropertyChangedEventArgs e)
        {
            ScrollToBottomBehavior behavior = sender as ScrollToBottomBehavior;
            if (behavior?.AssociatedObject == null || e.NewValue == null) return;

            INotifyCollectionChanged collection = behavior.ItemsSource as INotifyCollectionChanged;
            if (collection != null)
            {
                ScrollViewer scrollViewer = behavior.AssociatedObject as ScrollViewer;
                ItemsControl itemsControl = behavior.AssociatedObject.GetFirstDescendantOfType<ItemsControl>();
                itemsControl.SizeChanged += (s, args) =>
                {
                    scrollViewer?.ChangeView(null, itemsControl.ActualHeight, null);
                };
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            ScrollViewer control = associatedObject as ScrollViewer;
            if (control == null)
            {
                throw new ArgumentException("ScrollToBottomBehavior can be attached only to ListView.");
            }

            AssociatedObject = associatedObject;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }
    }
}