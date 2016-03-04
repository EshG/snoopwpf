using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Snoop.Visualization.Converters
{
    class LinkEndsPositionConverter : IMultiValueConverter
    {
        const double MARGIN = 10;

        enum Direction { Right, Left, Top, Bottom };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">0 = ViewModel,1 = Target ItemsControl.Items,2 Link end value</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">x | y, 1 | 2. for example "x2" for destenation 2</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || parameter == null)
                return null;

            ViewModels.NodeLinkViewModel link = values[0] as ViewModels.NodeLinkViewModel;
            

            ItemContainerGenerator ic = values[1] as ItemContainerGenerator;

            double length = (double)values[2];

            string strParam = (string)parameter;

            ContentPresenter cp = (ContentPresenter)ic.ContainerFromItem(link.Destination);

            if (strParam[1] == '1')
                cp = (ContentPresenter)ic.ContainerFromItem(link.Origin);
            else
                cp = (ContentPresenter)ic.ContainerFromItem(link.Destination);

            Direction direction = GetDirection(link, strParam);

            return length + GetValueToAdd(cp, direction, strParam);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }



        private double GetValueToAdd(ContentPresenter node, Direction dir, string parameter)
        {
            if (dir == Direction.Top)
            {
                if (parameter[0] == 'x')
                {
                    return (node.ActualWidth / 2d);
                }
                else if (parameter[0] == 'y')
                {
                    return -MARGIN;
                }
            }
            else if (dir == Direction.Bottom)
            {
                if (parameter[0] == 'x')
                {
                    return (node.ActualWidth / 2d);
                }
                else if (parameter[0] == 'y')
                {
                    return node.ActualHeight + MARGIN;
                }
            }
            else if (dir == Direction.Left)
            {
                if (parameter[0] == 'x')
                {
                    return -MARGIN;
                }
                else if (parameter[0] == 'y')
                {
                    return node.ActualHeight / 2d;
                }
            }
            else if (dir == Direction.Right)
            {
                if (parameter[0] == 'x')
                {
                    return node.ActualWidth + MARGIN;
                }
                else if (parameter[0] == 'y')
                {
                    return node.ActualHeight / 2d;
                }
            }

            throw new NotImplementedException();
        }

        private Direction GetDirection(ViewModels.NodeLinkViewModel link, string parameter)
        {
            System.Windows.Vector diff = (link.Origin.Location - link.Destination.Location);

            bool sides = false;

            sides = Math.Abs(diff.X) > Math.Abs(diff.Y);

            if (parameter[1] == '2')//Destination
            {
                if (sides)
                {
                    if (diff.X > 0)
                        return Direction.Right;
                    else
                        return Direction.Left;
                }
                else
                {
                    if (diff.Y > 0)
                        return Direction.Bottom;
                    else
                        return Direction.Top;
                }
            }//Origin
            else
            {
                if (sides)
                {
                    if (diff.X > 0)
                        return Direction.Left;
                    else
                        return Direction.Right;
                }
                else
                {
                    if (diff.Y > 0)
                        return Direction.Top;

                    else
                        return Direction.Bottom;
                }
            }
        }






        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
