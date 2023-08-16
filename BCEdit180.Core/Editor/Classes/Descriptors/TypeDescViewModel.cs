using System;
using System.Threading.Tasks;
using System.Windows.Input;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Descriptors {
    public class TypeDescViewModel : BaseViewModel {
        private bool isInternalNameCacheValid, isSimpleNameCacheValid, isReadableNameCacheValid;
        private string cachedInternalName, cachedSimpleName, cachedReadableName;

        private TypeDescriptor typeDescriptor;
        public TypeDescriptor TypeDescriptor {
            get => this.typeDescriptor;
            set {
                this.isInternalNameCacheValid = this.isSimpleNameCacheValid = this.isReadableNameCacheValid = false;
                this.RaisePropertyChanged(ref this.typeDescriptor, value);
                this.RaisePropertyChanged(nameof(this.InternalClassName));
                this.RaisePropertyChanged(nameof(this.ClassName));
                this.RaisePropertyChanged(nameof(this.SimpleName));
                this.RaisePropertyChanged(nameof(this.ArrayDepth));
                this.RaisePropertyChanged(nameof(this.SizeOnStack));
            }
        }

        /// <summary>
        /// Returns the type descriptor's internal name
        /// </summary>
        public string InternalClassName {
            get {
                if (this.isInternalNameCacheValid) {
                    return this.cachedInternalName;
                }
                this.isInternalNameCacheValid = true;
                return this.cachedInternalName = this.typeDescriptor?.ToString();
            }
        }

        /// <summary>
        /// Returns the type descriptor's class name (packages and class name joined with a '/' character)
        /// </summary>
        public string ClassName => this.typeDescriptor?.ClassName?.Name;

        /// <summary>
        /// Returns the simple class name (no package info) or the java keyword for the primitive type,
        /// or null if no type descriptor is available
        /// </summary>
        public string SimpleName {
            get {
                if (this.isSimpleNameCacheValid) {
                    return this.cachedSimpleName;
                }

                this.isSimpleNameCacheValid = true;
                if (this.typeDescriptor != null) {
                    ClassName klass = this.typeDescriptor.ClassName;
                    if (klass != null) {
                        int index = klass.Name.LastIndexOf('/');
                        return this.cachedSimpleName = index == -1 ? klass.Name : klass.Name.Substring(index + 1);
                    }
                    else {
                        return this.cachedReadableName = this.typeDescriptor.PrimitiveType?.ToKeyword();
                    }
                }
                else {
                    return this.cachedSimpleName = null;
                }
            }
        }

        /// <summary>
        /// Gets the readable name. A primitive type descriptor returns the java keyword,
        /// and a class type returns the package and class name joined by '.' characters.
        /// Returns null if no type descriptor is available
        /// </summary>
        public string ReadableName {
            get {
                if (this.isReadableNameCacheValid) {
                    return this.cachedReadableName;
                }

                this.isReadableNameCacheValid = true;
                if (this.typeDescriptor != null) {
                    ClassName klass = this.typeDescriptor.ClassName;
                    if (klass != null) {
                        return this.cachedReadableName = klass.ToString();
                    }
                    else {
                        return this.cachedReadableName = this.typeDescriptor.PrimitiveType?.ToKeyword();
                    }
                }
                else {
                    return this.cachedReadableName = null;
                }
            }
        }

        public int ArrayDepth => this.typeDescriptor?.ArrayDepth ?? 0;

        public int SizeOnStack => this.typeDescriptor?.SizeOnStack ?? 0;

        public AsyncRelayCommand EditFieldDescriptorCommand { get; }

        public TypeDescViewModel() : this(new TypeDescriptor(PrimitiveType.Integer, 0)) {

        }

        public TypeDescViewModel(TypeDescriptor typeDescriptor) {
            this.EditFieldDescriptorCommand = new AsyncRelayCommand(this.EditTypeDescriptorAction);
            this.TypeDescriptor = typeDescriptor;
        }

        public async Task EditTypeDescriptorAction() {
            TypeDescriptor desc = await IoC.TypeDescEditors.EditTypeDesc(true, true, 0, this.typeDescriptor);
            if (desc != null) {
                this.TypeDescriptor = desc;
            }
        }
    }
}