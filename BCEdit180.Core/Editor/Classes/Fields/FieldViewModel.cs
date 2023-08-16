using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BCEdit180.Core.Editor.Classes.Annotations;
using BCEdit180.Core.Utils;
using JavaAsm;
using JavaAsm.CustomAttributes.Annotation;

namespace BCEdit180.Core.Editor.Classes.Fields {
    public class FieldViewModel : BaseViewModel {
        private FieldAccessModifiers access;
        public FieldAccessModifiers Access {
            get => this.access;
            set => this.RaisePropertyChanged(ref this.access, value);
        }

        private string fieldName;
        public string FieldName {
            get => this.fieldName;
            set => this.RaisePropertyChanged(ref this.fieldName, value);
        }

        private TypeDescriptor fieldDescriptor;
        public TypeDescriptor FieldDescriptor {
            get => this.fieldDescriptor;
            set => this.RaisePropertyChanged(ref this.fieldDescriptor, value);
        }

        private List<AttributeNode> attributes;
        public List<AttributeNode> Attributes {
            get => this.attributes;
            set => this.RaisePropertyChanged(ref this.attributes, value);
        }

        private string signature;
        public string Signature {
            get => this.signature;
            set => this.RaisePropertyChanged(ref this.signature, value);
        }

        private bool isDeprecated;
        public bool IsDeprecated {
            get => this.isDeprecated;
            set => this.RaisePropertyChanged(ref this.isDeprecated, value);
        }

        private object constantValue;
        public object ConstantValue {
            get => this.constantValue;
            set => this.RaisePropertyChanged(ref this.constantValue, value);
        }

        private bool isCreatedRuntime;
        public bool IsCreatedRuntime {
            get => this.isCreatedRuntime;
            set => this.RaisePropertyChanged(ref this.isCreatedRuntime, value);
        }

        public AnnotationEditorViewModel VisibleAnnotationEditor { get; }

        public AnnotationEditorViewModel InvisibleAnnotationEditor { get; }

        public ICommand SetToConstNullCommand { get; }

        public FieldNode Node { get; private set; }

        public FieldListViewModel FieldList { get; }

        public static RelayCommand<FieldViewModel> EditDescriptorCommand { get; } = new RelayCommand<FieldViewModel>(async (x) => {
            TypeDescriptor desc = await IoC.TypeDescEditors.EditTypeDesc(true, true, 0, x.FieldDescriptor);
            if (desc != null)
                x.FieldDescriptor = desc;
        });

        public static RelayCommand<FieldViewModel> EditAccessCommand { get; } = new RelayCommand<FieldViewModel>(async (x) => {
            FieldAccessModifiers? result = await IoC.AccessEditors.EditFieldAccessAsync(x.Access);
            if (result.HasValue)
                x.Access = result.Value;
        });

        public FieldViewModel(FieldListViewModel list, FieldNode node) {
            this.FieldList = list;
            this.Node = node;
            this.VisibleAnnotationEditor = new AnnotationEditorViewModel();
            this.InvisibleAnnotationEditor = new AnnotationEditorViewModel();
            this.SetToConstNullCommand = new RelayCommand(() => this.ConstantValue = null);
            this.Load(node);
        }

        public void Load(FieldNode node) {
            this.Node = node;
            this.FieldName = node.Name;
            this.Access = node.Access;
            this.FieldDescriptor = node.Descriptor;
            this.Attributes = node.Attributes;
            this.Signature = node.Signature;
            this.VisibleAnnotationEditor.Annotations.Clear();
            this.VisibleAnnotationEditor.Annotations.AddAll(node.VisibleAnnotations.Select(a => new AnnotationViewModel(a)));
            this.InvisibleAnnotationEditor.Annotations.Clear();
            this.InvisibleAnnotationEditor.Annotations.AddAll(node.InvisibleAnnotations.Select(a => new AnnotationViewModel(a)));
            this.IsDeprecated = node.IsDeprecated;
        }

        public void Save(FieldNode node) {
            node.Name = this.FieldName;
            node.Access = this.Access;
            node.Descriptor = this.FieldDescriptor;
            node.Attributes = this.Attributes;
            node.Signature = this.Signature;
            node.VisibleAnnotations = new List<AnnotationNode>(this.VisibleAnnotationEditor.Annotations.Select(a => a.Node));
            node.InvisibleAnnotations = new List<AnnotationNode>(this.InvisibleAnnotationEditor.Annotations.Select(a => a.Node));
            node.IsDeprecated = this.IsDeprecated;
        }

        public void Save(ClassNode node) {
            if (this.Node == null) {
                throw new Exception("No method node present");
            }

            if (this.IsCreatedRuntime) {
                this.IsCreatedRuntime = false;
                node.Fields.Add(this.Node);
            }

            this.Node.Attributes.Clear();
            this.Save(this.Node);
        }
    }
}