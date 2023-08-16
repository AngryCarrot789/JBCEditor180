using System.Collections.Generic;
using BCEdit180.Core.Actions.Contexts;
using BCEdit180.Core.AdvancedContextService;
using BCEdit180.Core.Editor.Classes;

namespace BCEdit180.Core.Editor.Context {
    public class InterfaceContextGenerator : IContextGenerator {
        public static InterfaceContextGenerator Instance { get; } = new InterfaceContextGenerator();

        private InterfaceContextGenerator() {

        }

        public void Generate(List<IContextEntry> list, IDataContext context) {
            if (context.TryGetContext(out ClassInfoViewModel info)) {
                if (context.TryGetContext(out ClassNameViewModel className)) {
                    list.Add(new CommandContextEntry("Rename", null, "Rename this interface to something else", info.RenameInterfaceCommand, className));
                    list.Add(SeparatorEntry.Instance);
                    list.Add(new CommandContextEntry("Remove", null, "Remove this interface", info.RemoveInterfaceCommand, className));
                }

                if (list.Count > 0) {
                    list.Add(SeparatorEntry.Instance);
                }

                list.Add(new CommandContextEntry("Add Interface", null, "Add a new interface", info.AddNewInterfaceCommand));
            }
        }
    }
}