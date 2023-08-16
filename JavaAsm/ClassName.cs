using System;

namespace JavaAsm {
    /// <summary>
    /// The name of a class, where the separator for packages and the class name is the / char (e.g java/lang/String)
    /// </summary>
    public class ClassName {
        public string Name { get; }

        public ClassName(string name) {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public static bool TryParse(string value, out ClassName name) {
            if (string.IsNullOrEmpty(value)) {
                name = null;
                return false;
            }

            // convert type descriptor to class name
            if (value[0] == 'L' && value[value.Length - 1] == ';') {
                value = value.Substring(1, value.Length - 2);
            }

            name = new ClassName(value.Replace('.', '/'));
            return true;
        }

        public override string ToString() {
            return this.Name.Replace('/', '.');
        }

        private bool Equals(ClassName other) {
            return this.Name == other.Name;
        }

        public override bool Equals(object obj) {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((ClassName) obj);
        }

        public override int GetHashCode() {
            return this.Name.GetHashCode();
        }

        public ClassName Copy() {
            return new ClassName(this.Name);
        }

        public static string GetSimpleName(string className, char pathSeparator = '/') {
            if (className == null)
                return null;
            int index = className.LastIndexOf(pathSeparator);
            return index == -1 ? className : className.Substring(index + 1);
        }
    }
}