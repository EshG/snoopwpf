using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Snoop.Visualization.CustomControls
{
    /// <summary>
    /// This extension for the canvas helps with notifying the scrollviewer for size changes
    /// </summary>
    public class CustomCanvas : Canvas
    {
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            InvalidateMeasure();
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            base.InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);
            var desiredSize = new Size();
            foreach (UIElement child in Children)
            {
                desiredSize = new Size(
                    Math.Max(desiredSize.Width, GetLeft(child) + child.DesiredSize.Width),
                    Math.Max(desiredSize.Height, GetTop(child) + child.DesiredSize.Height));
            }
            return desiredSize;
        }
    }
}
