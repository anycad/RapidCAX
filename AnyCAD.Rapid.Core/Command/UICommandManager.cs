using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnyCAD.Rapid.Core
{
    public class UICommandManager
    {

        private static Dictionary<String, UICommand> mCommands = new Dictionary<string, UICommand>();

        private UICommandManager()
        {

        }

        public static void LoadCommands(Assembly assembly)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(UICommand)))
                {
                    var command = Activator.CreateInstance(type) as UICommand;
                    if (command.Name.Length == 0)
                        continue;
                    mCommands[command.Name] = command;
                }
            }

        }

        public static bool ExecuteCommand(string name, UICommandContext ctx)
        {
            UICommand command;
            if (mCommands.TryGetValue(name, out command))
            {
                return command.Execute(ctx);
            }

            return false;
        }
    }
}
