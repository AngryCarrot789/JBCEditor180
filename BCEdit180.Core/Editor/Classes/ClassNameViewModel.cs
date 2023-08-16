using System;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes {
    public class ClassNameViewModel : BaseViewModel {
        private string fullName;
        private string lastSimpleName;
        private bool isSimpleNameValid;

        /// <summary>
        /// The full class name, where packages and the class name are separated with '/' characters
        /// </summary>
        public string FullName {
            get => this.fullName;
            set {
                this.isSimpleNameValid = false;
                this.RaisePropertyChanged(ref this.fullName, value);
                this.RaisePropertyChanged(nameof(this.SimpleName));
            }
        }

        /// <summary>
        /// The simple class name, where the packages are removed and just the class name is left
        /// </summary>
        public string SimpleName {
            get {
                if (this.isSimpleNameValid) {
                    return this.lastSimpleName;
                }

                this.isSimpleNameValid = true;
                if (this.fullName == null) {
                    return this.lastSimpleName = null;
                }

                int index = this.fullName.LastIndexOf('/');
                return this.lastSimpleName = index == -1 ? this.fullName : this.fullName.Substring(index + 1);
            }
        }

        public ClassName AsClassName => new ClassName(this.fullName);

        public ClassNameViewModel() {

        }

        public ClassNameViewModel(string fullName) {
            if (fullName != null && fullName.IndexOf('.') != -1) {
                throw new ArgumentException($"FullName cannot contain dots as those are not valid for a class name: {fullName}", nameof(fullName));
            }

            this.fullName = fullName;
        }

        public ClassNameViewModel(ClassName name) : this(name.Name) {

        }
    }
}