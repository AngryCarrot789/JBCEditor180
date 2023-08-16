using System.Collections.Generic;
using System.Linq;
using BCEdit180.Core.PropertyEditing;
using BCEdit180.Core.Utils;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.PropertyEditors.InnerClasses {
    public class InnerClassPropertyEditorViewModel : BasePropertyEditorViewModel {
        public IEnumerable<InnerClassViewModel> InnerClasses => this.Handlers.Cast<InnerClassViewModel>();

        private string innerClassName;
        public string InnerClassName {
            get => this.innerClassName;
            set {
                this.RaisePropertyChanged(ref this.innerClassName, value);
                this.InnerClasses.ForEach(x => x.InnerClassName = value);
            }
        }

        private string outerClassName;
        public string OuterClassName {
            get => this.outerClassName;
            set {
                this.RaisePropertyChanged(ref this.outerClassName, value);
                this.InnerClasses.ForEach(x => x.OuterClassName = value);
            }
        }

        private string innerName;
        public string InnerName {
            get => this.innerName;
            set {
                this.RaisePropertyChanged(ref this.innerName, value);
                this.InnerClasses.ForEach(x => x.InnerName = value);
            }
        }

        private ClassAccessModifiers classAccess;
        public ClassAccessModifiers ClassAccess {
            get => this.classAccess;
            set {
                this.RaisePropertyChanged(ref this.classAccess, value);
                this.InnerClasses.ForEach(x => x.ClassAccess = value);
            }
        }

        public InnerClassPropertyEditorViewModel() : base(typeof(InnerClassViewModel)) {

        }

        protected override void OnHandlersLoaded() {
            base.OnHandlersLoaded();
            if (!this.IsEmpty) {
                const string fallback = "<different values>";
                // Update local fields because using the property setter updates the actual handlers... which would be a big no no here
                this.innerClassName = GetEqualValue(this.Handlers, x => ((InnerClassViewModel) x).InnerClassName, out string a) ? a : fallback;
                this.outerClassName = GetEqualValue(this.Handlers, x => ((InnerClassViewModel) x).OuterClassName, out string b) ? b : fallback;
                this.innerName = GetEqualValue(this.Handlers, x => ((InnerClassViewModel) x).InnerName, out string c) ? c : fallback;
                this.classAccess = GetEqualValue(this.Handlers, x => ((InnerClassViewModel) x).ClassAccess, out ClassAccessModifiers d) ? d : default;
                this.RaisePropertyChanged(nameof(this.InnerClassName));
                this.RaisePropertyChanged(nameof(this.OuterClassName));
                this.RaisePropertyChanged(nameof(this.InnerName));
                this.RaisePropertyChanged(nameof(this.ClassAccess));
            }
        }
    }
}