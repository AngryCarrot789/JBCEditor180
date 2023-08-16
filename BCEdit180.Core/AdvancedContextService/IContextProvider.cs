using System.Collections.Generic;

namespace BCEdit180.Core.AdvancedContextService {
    public interface IContextProvider {
        void GetContext(List<IContextEntry> list);
    }
}