namespace BCEdit180.Utils {
    public static class FileExtensionsHelper {
        public static string GetFileExtension(string fileName, string extUid) {
            string finalName = fileName;
            bool hasExtension = false;
            foreach (string extensionThing in GlobalPreferences.PRESET_EXTENSIONS) {
                if (fileName.Contains(extensionThing)) {
                    hasExtension = true;
                    break;
                }
            }

            if (hasExtension) {
                string[] dotSplits = fileName.Split('.');
                string finalExtensionMaybe = dotSplits[dotSplits.Length - 1];


                // Would have removed unnessesary dots at the end of the file name
                // but it doesnt really work well... :(

                //int numOfUnnessesaryPeriods = 0;
                //// Checks how many extra .'s there are on the end of the file name
                //foreach(string dotSplit in dotSplits)
                //{
                //    if (dotSplit == "")
                //        numOfUnnessesaryPeriods++;
                //}

                //// Removes extra .'s from the file name
                //if (finalExtensionMaybe == "")
                //{
                //    finalName = finalName.Remove(finalName.Length - 1 - numOfUnnessesaryPeriods, numOfUnnessesaryPeriods);
                //}

                if (CheckIsValidExtension("." + finalExtensionMaybe)) {
                    //add 1 to include the . (dot)
                    int charsToRemoveCount = finalExtensionMaybe.Length + 1;
                    string fileNameNoExtension = fileName.Substring(0, fileName.Length - charsToRemoveCount);
                    return fileNameNoExtension + extUid;
                }
                else
                    return finalName += extUid;
            }
            else
                return finalName += extUid;
        }

        public static bool CheckIsValidExtension(string possibleExtension) {
            foreach (string extensionThing in GlobalPreferences.PRESET_EXTENSIONS) {
                if (possibleExtension.Contains(extensionThing)) {
                    return true;
                }
            }

            return false;
        }

        public static string GetReadable(string fileNameExtension) {
            switch (fileNameExtension) {
                case ".txt": return "Plain Text";
                case ".text": return "'Text' (Sort of)";
                case ".cs": return "C#";
                case ".c": return "C";
                case ".cpp": return "C++";
                case ".h": return "C/C++ Header";
                case ".hpp": return "C++ Header";
                case ".xaml": return "XAML";
                case ".xml": return "XML";
                case ".htm": return "HTM";
                case ".html": return "HTML";
                case ".css": return "CSS";
                case ".js": return "Java Script";
                case ".exe": return "Executable (EXE)";
                default: return "Unknown";
            }
        }
    }
}