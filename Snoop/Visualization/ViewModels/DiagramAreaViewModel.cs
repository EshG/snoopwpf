using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Snoop.Visualization.ViewModels
{
    public class DiagramAreaViewModel : ViewModelBase
    {
        private DiagramNodeViewModel _Parent;
        public DiagramNodeViewModel Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private ObservableCollection<NodeLinkViewModel> _Nodes = new ObservableCollection<NodeLinkViewModel>();
        public ObservableCollection<NodeLinkViewModel> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; }
        }

    }
}
