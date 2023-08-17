using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Methods {
    public class MethodListViewModel : BaseViewModel {
        private readonly List<MethodViewModel> removedMethods;

        public ObservableCollection<MethodViewModel> Methods { get; }

        private MethodViewModel primarySelectedMethod;
        public MethodViewModel PrimarySelectedMethod {
            get => this.primarySelectedMethod;
            set => this.RaisePropertyChanged(ref this.primarySelectedMethod, value);
        }

        public ObservableCollection<MethodViewModel> SelectedMethods { get; }

        private int lastSaveIndex;
        private int primarySelectedIndex;
        public int PrimarySelectedIndex {
            get => this.primarySelectedIndex;
            set => this.RaisePropertyChanged(ref this.primarySelectedIndex, value);
        }

        public ClassViewModel Class { get; }

        public RelayCommand DeleteSelectedMethodsCommand { get; }

        public MethodListViewModel(ClassViewModel klass) {
            this.Class = klass;
            this.Methods = new ObservableCollection<MethodViewModel>();
            this.SelectedMethods = new ObservableCollection<MethodViewModel>();
            this.removedMethods = new List<MethodViewModel>();
            this.DeleteSelectedMethodsCommand = new RelayCommand(() => {
                foreach (MethodViewModel method in this.SelectedMethods.ToList()) {
                    this.Methods.Remove(method);
                    this.removedMethods.Add(method);
                }
            });
        }

        public void Load(ClassNode node) {
            this.Methods.Clear();
            foreach (MethodNode method in node.Methods) {
                this.Methods.Add(new MethodViewModel(method) {
                    MethodList = this
                });
            }

            if (this.lastSaveIndex >= 0 && this.lastSaveIndex < this.Methods.Count) {
                this.PrimarySelectedIndex = this.lastSaveIndex;
            }

            this.lastSaveIndex = 0;
        }

        public void Save(ClassNode node) {
            foreach (MethodViewModel md in this.removedMethods) {
                if (md.Node == null || !node.Methods.Contains(md.Node))
                    throw new Exception("Invalid method");
                node.Methods.Remove(md.Node);
            }

            this.lastSaveIndex = this.PrimarySelectedIndex;
            foreach (MethodViewModel method in this.Methods) {
                method.Save(node);
            }
        }
    }
}