using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Snoop.Visualization.ViewModels
{
    public class DiagramViewModel : ViewModelBase, IDiagramViewModel
    {
        const int VERTICAL_SEPERATION = 150;
        const int HORIZONTAL_SEPERATION = 150;



        public DiagramViewModel(VisualTreeItem root)
        {
            Nodes = new ObservableCollection<DiagramNodeViewModel>(LoadViewModels(root));
            ReArange();
        }

        private DiagramNodeViewModel _Root;
        public DiagramNodeViewModel Root
        {
            get { return _Root; }
            private set { _Root = value; }
        }


        /// <summary>
        /// Sets the nodes location on the diagram.
        /// The first iteration sets the vertical position, and the secound sets the horizontal
        /// </summary>
        public void ReArange()
        {
            Dictionary<DiagramNodeViewModel, int> levelByNode = new Dictionary<DiagramNodeViewModel, int>();

            Dictionary<int, int> nodesInLevel = new Dictionary<int, int>();

            GraphLayout.LayeredTreeDraw _ltd = new GraphLayout.LayeredTreeDraw(Root, 100, 0, 0, GraphLayout.VerticalJustification.top);
            _ltd.LayoutTree();

            foreach (DiagramNodeViewModel node in Nodes)
            {
                node.Location = new Point(_ltd.X(node), _ltd.Y(node));

            }

        }


        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }


        private void ArrangeVertical(Dictionary<DiagramNodeViewModel, int> levelByNode, Dictionary<int, int> nodesInLevel)
        {
            foreach (DiagramNodeViewModel node in Nodes.OrderBy(n => n.Model.Depth))
            {
                var link = Links.Where(l => l.Destination == node && l.Origin != node).FirstOrDefault();

                DiagramNodeViewModel parent = null;
                if (link != null)
                    parent = link.Origin;

                if (parent != null)
                {
                    parent.TreeChildren.Add(node);
                }
            }
        }

        private void ArrangeHorizontal(Dictionary<DiagramNodeViewModel, int> levelByNode, Dictionary<int, int> nodesInLevel)
        {
            int maxInLevel = nodesInLevel.Values.Max();

            int width = (maxInLevel * HORIZONTAL_SEPERATION) + HORIZONTAL_SEPERATION;

            int middle = width / 2;

            Dictionary<int, int> stepsByLevel = new Dictionary<int, int>();

            foreach (DiagramNodeViewModel node in Nodes)
            {
                int currentLevel = levelByNode[node];

                if (!stepsByLevel.ContainsKey(currentLevel))
                    stepsByLevel[currentLevel] = 0;



                node.Location = new Point(0, node.Location.Y);

                stepsByLevel[currentLevel]++;
            }
        }

        Dictionary<VisualTreeItem, DiagramNodeViewModel> viewModelsByTreeItem = new Dictionary<VisualTreeItem, DiagramNodeViewModel>();
        List<DiagramNodeViewModel> listToReturn = new List<DiagramNodeViewModel>();


        List<DiagramNodeViewModel> LoadViewModels(VisualTreeItem value)
        {
            foreach (VisualTreeItem item in value.Children)
            {
                //Only if the item is a framework element it has a datacontext
                if (item.Target is FrameworkElement)
                {
                    FrameworkElement fe = item.Target as FrameworkElement;


                    DiagramNodeViewModel node = new DiagramNodeViewModel(item, this);

                    if (Root == null)
                        Root = node;
                    listToReturn.Add(node);

                    viewModelsByTreeItem[item] = node;

                    DiagramNodeViewModel parent = GetParent(item.Parent);

                    //We don't want to show regular elements unless their view model has changed
                    //if (item.Target.GetType().BaseType != typeof(UserControl) && (parent != null && parent.ViewModelName == node.ViewModelName))
                    //{
                    //    continue;
                    //}

                    //The root may not be a framework element (App)
                    if (value.Target is FrameworkElement && parent != null && node != null)
                    {
                        NodeLinkViewModel link = new NodeLinkViewModel()
                        {
                            Origin = parent,
                            Destination = node
                        };
                        Links.Add(link);
                        parent.TreeChildren.Add(node);
                    }


                    //Since this is a recursive method, we try to minimize the number of calls as much as possible
                    if (item.Children.Count > 0)
                    {
                        LoadViewModels(item);
                    }
                }
            }

            return listToReturn;
        }


        /// <summary>
        /// Pass here the parent treeItem, not the item itself
        /// </summary>
        /// <param name="treeItem"></param>
        /// <returns></returns>
        private DiagramNodeViewModel GetParent(VisualTreeItem treeItem)
        {
            if (viewModelsByTreeItem.ContainsKey(treeItem))
                return viewModelsByTreeItem[treeItem];

            if (treeItem.Parent == null)
                return null;

            return GetParent(treeItem.Parent);
        }


        private ObservableCollection<DiagramNodeViewModel> _Nodes = new ObservableCollection<DiagramNodeViewModel>();
        public ObservableCollection<DiagramNodeViewModel> Nodes
        {
            get { return _Nodes; }
            set
            {
                _Nodes = value;
                NotifyChange(nameof(Nodes));
            }
        }


        private ObservableCollection<NodeLinkViewModel> _Links = new ObservableCollection<NodeLinkViewModel>();
        public ObservableCollection<NodeLinkViewModel> Links
        {
            get { return _Links; }
            set
            {
                _Links = value;
                NotifyChange(nameof(Links));
            }
        }


    }
}
