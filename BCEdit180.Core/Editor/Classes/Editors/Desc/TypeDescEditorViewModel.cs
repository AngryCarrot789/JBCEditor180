using System;
using BCEdit180.Core.Utils;
using BCEdit180.Core.Views.Dialogs;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Editors.Desc {
    public class TypeDescEditorViewModel : BaseConfirmableDialogViewModel {
        private string inputText;
        private string previewInternalName;
        private string previewDescriptor;
        private string previewClassName;
        private PrimitiveType selectedPrimitive;
        private bool isObject;
        private bool isPrimitive;
        private ushort arrayDepth;
        private bool allowPrimitive;
        private bool allowClass;

        public string InputText {
            get => this.inputText;
            set {
                this.RaisePropertyChanged(ref this.inputText, value);
                this.UpdateClassName(true);
            }
        }

        public string PreviewInternalName {
            get => this.previewInternalName;
            set => this.RaisePropertyChanged(ref this.previewInternalName, value);
        }

        public string PreviewDescriptor {
            get => this.previewDescriptor;
            set => this.RaisePropertyChanged(ref this.previewDescriptor, value);
        }

        public string PreviewClassName {
            get => this.previewClassName;
            set => this.RaisePropertyChanged(ref this.previewClassName, value);
        }

        public PrimitiveType SelectedPrimitive {
            get => this.selectedPrimitive;
            set => this.RaisePropertyChanged(ref this.selectedPrimitive, value);
        }

        public bool IsObject {
            get => this.isObject;
            set {
                this.RaisePropertyChanged(ref this.isObject, value);
                this.UpdateClassName(true);
            }
        }

        public bool IsPrimitive {
            get => this.isPrimitive;
            set => this.RaisePropertyChanged(ref this.isPrimitive, value);
        }

        public ushort ArrayDepth {
            get => this.arrayDepth;
            set {
                this.RaisePropertyChanged(ref this.arrayDepth, value);
                this.UpdateClassName(false);
            }
        }

        public string ArrayPartString => StringUtils.Repeat('[', this.ArrayDepth);

        public bool AllowPrimitive {
            get => this.allowPrimitive;
            set => this.RaisePropertyChanged(ref this.allowPrimitive, value);
        }

        public bool AllowClass {
            get => this.allowClass;
            set => this.RaisePropertyChanged(ref this.allowClass, value);
        }

        public TypeDescEditorViewModel(IDialog dialog) : base(dialog) {
            this.ArrayDepth = 0;
            this.AllowPrimitive = true;
            this.AllowClass = true;
        }

        public void SetTypeDescriptor(TypeDescriptor desc) {
            this.ArrayDepth = (ushort) desc.ArrayDepth;
            if (this.AllowClass && desc.ClassName != null) {
                string strippedArray = StripArrayDepthPart(desc.ClassName.Name);
                this.PreviewInternalName = this.GetInternalName(strippedArray);
                this.PreviewClassName = this.GetClassName(strippedArray);
                this.PreviewDescriptor = this.ArrayPartString + this.GetDescriptor(strippedArray);
                this.inputText = desc.ClassName.ToString();
                this.RaisePropertyChanged(nameof(this.InputText));
                this.IsObject = true;
            }

            if (this.AllowPrimitive && desc.PrimitiveType != null) {
                this.SelectedPrimitive = desc.PrimitiveType.Value;
                this.IsPrimitive = true;
            }
        }

        public void UpdateClassName(bool calculateArrayDepth) {
            if (calculateArrayDepth) {
                this.arrayDepth = (ushort) (string.IsNullOrEmpty(this.InputText) ? 0 : StringUtils.CountCharsAtStart(this.InputText, '['));
                this.RaisePropertyChanged(nameof(this.ArrayDepth));
            }

            string arrayParts = this.ArrayPartString;
            if (string.IsNullOrEmpty(this.InputText)) {
                this.PreviewInternalName = arrayParts;
                this.PreviewClassName = "";
                this.PreviewDescriptor = arrayParts;
            }
            else {
                string strippedArray = StripArrayDepthPart(this.InputText);
                this.PreviewInternalName = this.GetInternalName(strippedArray);
                this.PreviewClassName = this.GetClassName(strippedArray);
                this.PreviewDescriptor = arrayParts + this.GetDescriptor(strippedArray);
            }
        }

        private static string StripDescriptorElements(string input) {
            if (input.Length > 0 && input[0] == 'L' && input[input.Length - 1] == ';') {
                input = input.Substring(1, input.Length - 2);
            }

            return input;
        }

        private static string StripArrayDepthPart(string input) {
            int i = 0, len = input.Length;
            while (i < len && input[i] == '[') {
                i++;
            }

            return i == 0 ? input : input.Substring(i);
        }

        public string GetInternalName(string input) {
            return StripDescriptorElements(StripArrayDepthPart(input.Replace('.', '/')));
        }

        public string GetClassName(string input) {
            return StripDescriptorElements(StripArrayDepthPart(input.Replace('/', '.')));
        }

        public string GetDescriptor(string input) {
            string name = StripArrayDepthPart(input).Replace('.', '/');
            if (string.IsNullOrWhiteSpace(name)) {
                return "";
            }

            if (name[0] != 'L') {
                if (name[name.Length - 1] != ';') {
                    name = ("L" + name + ";");
                }
                else {
                    name = ("L" + name);
                }
            }
            else if (name[name.Length - 1] != ';') {
                name += ";";
            }

            return name;
        }

        public string GetInternalName() {
            return this.GetInternalName(this.InputText);
        }

        public string GetClassName() {
            return this.GetClassName(this.InputText);
        }

        public string GetDescriptor() {
            return this.GetDescriptor(this.InputText);
        }
    }
}