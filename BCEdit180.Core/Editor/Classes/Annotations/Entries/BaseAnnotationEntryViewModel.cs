﻿using JavaAsm.CustomAttributes.Annotation;
using static JavaAsm.CustomAttributes.Annotation.AnnotationNode;

namespace BCEdit180.Core.Editor.Classes.Annotations.Entries {
    public class BaseAnnotationEntryViewModel : BaseViewModel {
        protected readonly ElementValuePair entry;
        protected readonly ElementValue value;

        public AnnotationViewModel Annotation { get; }

        protected string entryName;
        public string EntryName {
            get => this.entryName;
            set {
                this.RaisePropertyChanged(ref this.entryName, value);
                this.entry.ElementName = value;
            }
        }

        private ElementValue.ElementValueTag valueTag;
        public ElementValue.ElementValueTag ValueTag {
            get => this.valueTag;
            set {
                this.RaisePropertyChanged(ref this.valueTag, value);
                this.entry.Value.Tag = value;
            }
        }

        protected BaseAnnotationEntryViewModel(AnnotationViewModel annotation, ElementValuePair entry) {
            this.Annotation = annotation;
            this.entry = entry;
            this.value = entry.Value;
            this.ValueTag = entry.Value.Tag;
            this.Load(entry);
        }

        public static BaseAnnotationEntryViewModel Of(AnnotationViewModel annotation, ElementValuePair entry) {
            switch (entry.Value.Tag) {
                case ElementValue.ElementValueTag.Byte:
                case ElementValue.ElementValueTag.Short:
                case ElementValue.ElementValueTag.Integer:
                case ElementValue.ElementValueTag.Long:
                case ElementValue.ElementValueTag.Float:
                case ElementValue.ElementValueTag.Double:
                case ElementValue.ElementValueTag.Character:
                case ElementValue.ElementValueTag.String:
                    return new StringValueAnnotationEntryViewModel(annotation, entry);
                case ElementValue.ElementValueTag.Boolean:
                    return new BooleanValueAnnotationEntryViewModel(annotation, entry);
                // TODO: maybe?
                case ElementValue.ElementValueTag.Enum:
                case ElementValue.ElementValueTag.Class:
                case ElementValue.ElementValueTag.Annotation:
                case ElementValue.ElementValueTag.Array:
                default:
                    return null;
            }
        }

        public virtual void Load(ElementValuePair entry) {
            this.EntryName = entry.ElementName;
            this.ValueTag = entry.Value.Tag;
        }

        public virtual void Save(ElementValuePair entry) {
            entry.ElementName = this.EntryName;
            entry.Value.Tag = this.ValueTag;
        }

        public ElementValuePair SaveAndGet() {
            this.Save(this.entry);
            return this.entry;
        }
    }
}
