using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GraphLayout;

namespace Snoop.Visualization.ViewModels
{
    public class DiagramNodeViewModel : ViewModelBase, GraphLayout.ITreeNode
    {
        IDiagramViewModel _Host;

        public DiagramNodeViewModel(VisualTreeItem model,IDiagramViewModel host)
        {
            _Model = model;
            _Host = host;
        }

        private VisualTreeItem _Model;
        public VisualTreeItem Model
        {
            get { return _Model; }
        }

        Point _Location;
        public Point Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                NotifyChange(nameof(Location));
            }
        }

        //
        public string ControlNameName
        {
            get
            {
                FrameworkElement fe = Model.Target as FrameworkElement;
                return fe.GetType().Name;
            }
        }


        public string ViewModelName
        {
            get
            {
                FrameworkElement fe = Model.Target as FrameworkElement;
                if (fe.DataContext != null)
                {
                    return fe.DataContext.GetType().Name;
                }

                return "No DataContext";
            }
        }


        #region Graphlayout

        public object PrivateNodeInfo
        {
            get; set;
        }

        public TreeNodeGroup TreeChildren { get; set; } = new TreeNodeGroup();

        public double TreeWidth
        {
            get
            {
                return 100;
            }
        }

        public double TreeHeight
        {
            get
            {
                return 100;
            }
        }

        public bool Collapsed
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
