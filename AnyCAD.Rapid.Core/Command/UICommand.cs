using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    public class UICommand
    {
        public String Name = "";
        public UICommand()
        {

        }

        public virtual bool Execute(UICommandContext ctx)
        {
            return false;
        }
    }
}
