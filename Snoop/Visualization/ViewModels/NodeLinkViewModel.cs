using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snoop.Visualization.ViewModels
{
    public class NodeLinkViewModel : ViewModelBase
    {
        private DiagramNodeViewModel _Origin;
        public DiagramNodeViewModel Origin
        {
            get { return _Origin; }
            set
            {
                _Origin = value;
                NotifyChange(nameof(Origin));
            }
        }

        private DiagramNodeViewModel _Destination;
        public DiagramNodeViewModel Destination
        {
            get { return _Destination; }
            set
            {
                _Destination = value;
                NotifyChange(nameof(Destination));
            }
        }

        public override string ToString()
        {
            return Origin.ControlName + " -> " + Destination.ControlName;
        }
    }
}
