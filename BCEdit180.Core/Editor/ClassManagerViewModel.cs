using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using BCEdit180.Core.Editor.Classes;
using BCEdit180.Core.Editor.FileSystem.Physical;
using BCEdit180.Core.Editor.FileSystem.Zip;
using JavaAsm;
using JavaAsm.Instructions;
using JavaAsm.Instructions.Types;
using JavaAsm.IO;

namespace BCEdit180.Core.Editor {
    /// <summary>
    /// A view model which handles all active classes
    /// </summary>
    public class ClassManagerViewModel : BaseViewModel {
        private readonly ObservableCollection<ClassViewModel> classes;
        private ClassViewModel activeClass;

        /// <summary>
        /// The primary class that is currently being viewed (aka, the selected class in the main window's tab control)
        /// </summary>
        public ClassViewModel ActiveClass {
            get => this.activeClass;
            set => this.RaisePropertyChanged(ref this.activeClass, value);
        }

        /// <summary>
        /// All of the classes currently open
        /// </summary>
        public ReadOnlyObservableCollection<ClassViewModel> Classes { get; }

        public MainViewModel MainView { get; }

        public ClassManagerViewModel(MainViewModel mainView) {
            this.MainView = mainView;
            this.classes = new ObservableCollection<ClassViewModel>();
            this.Classes = new ReadOnlyObservableCollection<ClassViewModel>(this.classes);
            this.AddDemoClass();
        }

        public void AddDemoClass() {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CarrotTools.class");
            ClassNode node;
            if (File.Exists(path)) {
                using (BufferedStream stream = new BufferedStream(File.OpenRead(path))) {
                    node = ClassFile.ParseClass(stream);
                }
            }
            else {
                node = CreateBlankClass();
                path = null;
            }

            ClassViewModel klass = new ClassViewModel() {
                ClassManager = this
            };

            klass.Load(node);

            this.classes.Add(klass);
            this.ActiveClass = klass;

            if (path != null) {
                this.MainView.Explorer.Root.AddFile(new IOFileItemViewModel(path));
            }
        }

        private static ClassNode CreateBlankClass() {
            ClassNode node = new ClassNode() {
                Name = new ClassName("BlankClass"),
                Access = ClassAccessModifiers.Public | ClassAccessModifiers.Super,
                MajorVersion = ClassVersion.Java8,
                SuperName = new ClassName("java/lang/Object")
            };

            node.Interfaces.Add(new ClassName("my/interface/called/MyInterface"));
            node.Interfaces.Add(new ClassName("java/lang/Cloneable"));

            Label label = new Label() { Index = 1 };

            node.Methods.Add(new MethodNode() {
                Owner = node,
                Access = MethodAccessModifiers.Public,
                Name = "<init>",
                Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Void, 0), new List<TypeDescriptor>()),
                MaxLocals = 1,
                // pretty sure this code won't pass the java verifier...
                Instructions = new InstructionList() {
                    new VariableInstruction(Opcode.ALOAD) { VariableIndex = 0 },
                    new MethodInstruction(Opcode.INVOKESPECIAL) {
                        Owner = new ClassName("java/lang/Object"),
                        Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Void, 0), new List<TypeDescriptor>()),
                        Name = "<init>"
                    },

                    new LdcInstruction(Opcode.LDC) {
                        Value = (int) 1
                    },
                    new JumpInstruction(Opcode.IF_ICMPEQ) {
                        Target = label
                    },

                    label,

                    new VariableInstruction(Opcode.ALOAD) { VariableIndex = 0 },
                    new MethodInstruction(Opcode.INVOKEVIRTUAL) {
                        Owner = node.Name,
                        Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Void, 0), new List<TypeDescriptor>()),
                        Name = "blankMethod"
                    },

                    new SimpleInstruction(Opcode.RETURN)
                }
            });

            node.Methods.Add(new MethodNode() {
                Owner = node,
                Access = MethodAccessModifiers.Public,
                Name = "blankMethod",
                Descriptor = new MethodDescriptor(new TypeDescriptor(PrimitiveType.Void, 0), new List<TypeDescriptor>()),
                MaxLocals = 1,
                Instructions = new InstructionList() {
                    new SimpleInstruction(Opcode.RETURN)
                }
            });

            node.Fields.Add(new FieldNode() {
                Owner = node,
                Name = "blankField",
                Access = FieldAccessModifiers.Public,
                Descriptor = new TypeDescriptor(PrimitiveType.Integer, 0),
                ConstantValue = 420
            });

            return node;
        }
    }
}