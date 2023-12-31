using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using BCEdit180.Core.Utils;
using Microsoft.CSharp;

namespace BCEdit180.Utils {
    /// <summary>
    /// A converter that uses a C# code generator to evaluate the converter output
    /// </summary>
    public class DynamicCodeConverter : MarkupExtension, IValueConverter {
        private static readonly HashSet<string> UsedClassNames = new HashSet<string>();

        private string lastScript;
        private MethodInfo lastCompiledMethod;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(parameter is string script)) {
                throw new Exception("Parameter must be a C# script");
            }

            if (this.lastScript == script && this.lastCompiledMethod != null) {
                return this.lastCompiledMethod.Invoke(null, new[] {value});
            }
            else {
                this.lastCompiledMethod = null;
                this.lastScript = null;
            }

            string klass = "Script_" + RandomUtils.RandomStringWhere(16, x => !UsedClassNames.Contains(x));
            UsedClassNames.Add(klass);
            string[] code = {
                "using System;" +
                "namespace BCEdit180.DynamicScript" +
                "{" +
                "   public class " + klass +
                "   {" +
                "       static public object Execute(object x)" +
                "       {" +
                "           return " + script + ";" +
                "       }" +
                "   }" +
                "}"
            };

            CompilerParameters CompilerParams = new CompilerParameters {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            CompilerParams.ReferencedAssemblies.AddRange(new[] {"System.dll"});

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);

            if (compile.Errors.HasErrors) {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors)
                    text += "\n" + ce;

                throw new Exception(text);
            }

            Module module = compile.CompiledAssembly.GetModules()[0];
            if (module == null)
                throw new Exception("Error during compilation; could not find 0th module");

            Type mt = module.GetType("BCEdit180.DynamicScript." + klass);
            if (mt == null)
                throw new Exception("Error during compilation; could not find compiled class");

            MethodInfo methInfo = mt.GetMethod("Execute");
            if (methInfo == null)
                throw new Exception("Error during compilation; could not find execute function in compiled class");

            this.lastCompiledMethod = methInfo;
            this.lastScript = script;
            return methInfo.Invoke(null, new[] {value});
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}