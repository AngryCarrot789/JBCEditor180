using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BCEdit180.Core.Editor.Classes.Annotations;
using BCEdit180.Core.Editor.Classes.Bytecode;
using BCEdit180.Core.Utils;
using JavaAsm;
using JavaAsm.CustomAttributes.Annotation;

namespace BCEdit180.Core.Editor.Classes.Methods {
    public class MethodViewModel : BaseViewModel, IMethodDescriptable {
        private MethodAccessModifiers access;
        public MethodAccessModifiers Access {
            get => this.access;
            set => this.RaisePropertyChanged(ref this.access, value);
        }

        private string methodName;
        public string MethodName {
            get => this.methodName;
            set => this.RaisePropertyChanged(ref this.methodName, value);
        }

        private MethodDescriptor methodDescriptor;
        public MethodDescriptor MethodDescriptor {
            get => this.methodDescriptor;
            set => this.RaisePropertyChanged(ref this.methodDescriptor, value);
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

        private ushort maxStack;
        public ushort MaxStack {
            get => this.maxStack;
            set => this.RaisePropertyChanged(ref this.maxStack, value);
        }

        private ushort maxLocals;
        public ushort MaxLocals {
            get => this.maxLocals;
            set => this.RaisePropertyChanged(ref this.maxLocals, value);
        }

        private List<AttributeNode> codeAttributes;
        public List<AttributeNode> CodeAttributes {
            get => this.codeAttributes;
            set => this.RaisePropertyChanged(ref this.codeAttributes, value);
        }

        private bool isDeprecated;
        public bool IsDeprecated {
            get => this.isDeprecated;
            set => this.RaisePropertyChanged(ref this.isDeprecated, value);
        }

        public ObservableCollection<ClassNameViewModel> Throws { get; }

        private ElementValue annotationDefaultValue;
        public ElementValue AnnotationDefaultValue {
            get => this.annotationDefaultValue;
            set => this.RaisePropertyChanged(ref this.annotationDefaultValue, value);
        }

        private bool isCreatedRuntime;
        public bool IsCreatedRuntime {
            get => this.isCreatedRuntime;
            set => this.RaisePropertyChanged(ref this.isCreatedRuntime, value);
        }

        public AnnotationEditorViewModel VisibleAnnotationEditor { get; }

        public AnnotationEditorViewModel InvisibleAnnotationEditor { get; }

        public CodeEditorViewModel CodeEditor { get;}

        public MethodListViewModel MethodList { get; set; }

        public MethodNode Node { get; set; }

        public MethodViewModel() {
            this.Throws = new ObservableCollection<ClassNameViewModel>();
            this.VisibleAnnotationEditor = new AnnotationEditorViewModel();
            this.InvisibleAnnotationEditor = new AnnotationEditorViewModel();
            this.CodeEditor = new CodeEditorViewModel(this);
        }

        public MethodViewModel(MethodNode node) : this() {
            this.Load(node);
        }

        public void Load(MethodNode node) {
            this.Node = node;
            this.MethodName = node.Name;
            this.Access = node.Access;
            this.MethodName = node.Name;
            this.MethodDescriptor = node.Descriptor;
            this.Attributes = node.Attributes;
            this.Signature = node.Signature;
            this.MaxStack = node.MaxStack;
            this.MaxLocals = node.MaxLocals;
            this.CodeAttributes = node.CodeAttributes;
            this.VisibleAnnotationEditor.Annotations.Clear();
            this.VisibleAnnotationEditor.Annotations.AddAll(node.VisibleAnnotations.Select(a => new AnnotationViewModel(a)));
            this.InvisibleAnnotationEditor.Annotations.Clear();
            this.InvisibleAnnotationEditor.Annotations.AddAll(node.InvisibleAnnotations.Select(a => new AnnotationViewModel(a)));
            this.IsDeprecated = node.IsDeprecated;
            this.Throws.Clear();
            this.Throws.AddAll(node.Throws.Select(a => new ClassNameViewModel(a.Name)));
            this.AnnotationDefaultValue = node.AnnotationDefaultValue;
            this.CodeEditor.Load(node);
        }

        public void Save(MethodNode node) {
            this.Node = node;
            node.Name = this.MethodName;
            node.Access = this.Access;
            node.Name = this.MethodName;
            node.Descriptor = this.MethodDescriptor;
            node.Attributes = this.Attributes;
            node.Signature = this.Signature;
            node.MaxStack = this.MaxStack;
            node.MaxLocals = this.MaxLocals;
            node.CodeAttributes = this.CodeAttributes;
            node.VisibleAnnotations = new List<AnnotationNode>(this.VisibleAnnotationEditor.Annotations.Select(a => a.Node));
            node.InvisibleAnnotations = new List<AnnotationNode>(this.InvisibleAnnotationEditor.Annotations.Select(a => a.Node));
            node.IsDeprecated = this.IsDeprecated;
            node.Throws = this.Throws.Select(a => new ClassName(a.FullName)).ToList();
            node.AnnotationDefaultValue = this.AnnotationDefaultValue;
            this.CodeEditor.Save(node);
        }

        public void Save(ClassNode node) {
            if (this.Node == null) {
                throw new Exception("No method node present");
            }

            if (this.IsCreatedRuntime) {
                this.IsCreatedRuntime = false;
                node.Methods.Add(this.Node);
            }

            this.Node.Attributes.Clear();
            this.Node.CodeAttributes.Clear();
            this.Save(this.Node);
        }
    }
}