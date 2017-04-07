using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace DevilDesireDevLib.Implementation.Extensions
{
    public static class VisualTreeHelperExtensions
    {
        public static UIElement GetRealWindowRoot(Window window = null)
        {
            if (window == null)
            {
                window = Window.Current;
            }

            if (window == null)
            {
                return null;
            }

            FrameworkElement root = window.Content as FrameworkElement;

            List<DependencyObject> ancestors = root?.GetAncestors().ToList();

            if (ancestors?.Count > 0)
            {
                root = (FrameworkElement)ancestors[ancestors.Count - 1];
            }

            return root;
        }

        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendantsOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendants().OfType<T>();
        }

        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
        {
            if (start == null)
            {
                yield break;
            }

            Queue<DependencyObject> queue = new Queue<DependencyObject>();

            Popup popup = start as Popup;

            if (popup != null)
            {
                if (popup.Child != null)
                {
                    queue.Enqueue(popup.Child);
                    yield return popup.Child;
                }
            }
            else
            {
                int count = VisualTreeHelper.GetChildrenCount(start);

                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(start, i);
                    queue.Enqueue(child);
                    yield return child;
                }
            }

            while (queue.Count > 0)
            {
                DependencyObject parent = queue.Dequeue();

                popup = parent as Popup;

                if (popup != null)
                {
                    if (popup.Child != null)
                    {
                        queue.Enqueue(popup.Child);
                        yield return popup.Child;
                    }
                }
                else
                {
                    int count = VisualTreeHelper.GetChildrenCount(parent);

                    for (int i = 0; i < count; i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                        yield return child;
                        queue.Enqueue(child);
                    }
                }
            }
        }

        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent)
        {
            Popup popup = parent as Popup;

            if (popup?.Child != null)
            {
                yield return popup.Child;
                yield break;
            }

            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
            }
        }

        public static IEnumerable<DependencyObject> GetChildrenByZIndex(
            this DependencyObject parent)
        {
            int i = 0;
            var indexedChildren =
                parent.GetChildren().Cast<FrameworkElement>().Select(
                child => new { Index = i++, ZIndex = Canvas.GetZIndex(child), Child = child });

            return
                from indexedChild in indexedChildren
                orderby indexedChild.ZIndex, indexedChild.Index
                select indexedChild.Child;
        }

        public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestorsOfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestors().OfType<T>();
        }

        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(start);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        public static IEnumerable<DependencyObject> GetSiblings(this DependencyObject start)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(start);

            if (parent == null)
            {
                yield return start;
            }
            else
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                }
            }
        }

        public static bool IsInVisualTree(this DependencyObject dob)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return false;
            }

            if (Window.Current == null)
            {
                return false;
            }

            UIElement root = GetRealWindowRoot();

            return
                root != null && dob.GetAncestors().Contains(root) ||
                VisualTreeHelper.GetOpenPopups(Window.Current)
                    .Any(popup => popup.Child != null && dob.GetAncestors().Contains(popup.Child));
        }

        public static Point GetPosition(this UIElement dob, Point origin = new Point(), UIElement relativeTo = null)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return new Point();
            }

            if (relativeTo == null)
            {
                relativeTo = Window.Current.Content;
            }

            if (relativeTo == null)
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            FrameworkElement fe = relativeTo as FrameworkElement;
            double aw = fe?.ActualWidth ?? 0;
            double ah = fe?.ActualHeight ?? 0;

            Point absoluteOrigin = new Point(aw * origin.X, ah * origin.X);

            if (dob == relativeTo)
            {
                return absoluteOrigin;
            }

            DependencyObject[] ancestors = dob.GetAncestors().ToArray();

            if (!ancestors.Contains(relativeTo))
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            return
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(absoluteOrigin);
        }

        public static Rect GetBoundingRect(this UIElement dob, UIElement relativeTo = null)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return Rect.Empty;
            }

            if (relativeTo == null)
            {
                relativeTo = Window.Current.Content as FrameworkElement;
            }

            if (relativeTo == null)
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            if (dob == relativeTo)
            {
                FrameworkElement fe = relativeTo as FrameworkElement;
                double aw = fe?.ActualWidth ?? 0;
                double ah = fe?.ActualHeight ?? 0;

                return new Rect(0, 0, aw, ah);
            }

            FrameworkElement fe2 = dob as FrameworkElement;
            double aw2 = fe2?.ActualWidth ?? 0;
            double ah2 = fe2?.ActualHeight ?? 0;

            Point topLeft =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(new Point());
            Point topRight =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            aw2,
                            0));
            Point bottomLeft =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            0,
                            ah2));
            Point bottomRight =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            aw2,
                            ah2));

            double minX = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
            double maxX = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
            double minY = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
            double maxY = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    }

}