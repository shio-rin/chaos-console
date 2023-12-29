using System.Reflection;
using System.Text;
namespace ChaosConsole {
    internal class Program {
        public static string scriptFileSourceFromMainDirectory = "AntiCrash";
        public static string unrecognizedInput = "Input not recognized.";
        public static string directoryCommand = "../net8.0";
        public static string publicUserInput = "AntiCrash";
        public static string userStatus = "guest";
        public static string prefix = "";
        public static bool runningScript = false;
        public static string[] nullArray = [
            "nullArrayType"
        ];
        public static string[] primaryCommandNames = [
            "lowerCase", "CMD", "FLE", "DIR", "ADM", "RTN", "HLP"
        ];
            public static string[] cmdCommandNames = [
                "cmd", "RUN", "CLR", "RST", "ESC"
            ];
            public static string[] fleCommandNames = [
                "fle", "CRT", "OPN", "HEX", "WRT"
            ];
            public static string[] dirCommandNames = [
                "dir", "CRT", "SET", "RTN", "RST"
            ];
            public static string[] admCommandNames = [
                "adm", "CRT", "LOG", "LGT", "DEL"
            ];
            public static string[] rtnCommandNames = [
                "rtn", "DIR"
            ];
            public static string[] hlpCommandNames = [
                "hlp", "DOC"
            ];
        static void Main(string[] args) {
            ArgumentNullException.ThrowIfNull(args);
            string? temporaryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            temporaryDirectory ??= "nullDirectory";
            directoryCommand = temporaryDirectory;
            string[] temporaryVar;
            string temporaryCancelString;
            int interpretInputValue;
            int primaryCommandIndex;
            int subCommandIndex;
            string date = DateTime.Today.ToString()[..^12];
            WriteToConsole(true, true, "chaos-eaters " + date + " Konstantin Edunov");
            WriteToConsole(false, true, "type \"hlp doc\" for a list of all commands");
            WriteToConsole(false, true, "type \"cmd esc\" to exit");
            for (;;) {
                publicUserInput = ReadUserInput(true, false, runningScript, scriptFileSourceFromMainDirectory) + " ";
                if (!((publicUserInput.Length == 4 && publicUserInput.Equals("CLL ", StringComparison.CurrentCultureIgnoreCase)) || (publicUserInput.Length >= 5 && publicUserInput.ToUpper().Substring(publicUserInput.Length - 5, 5) == " CLL "))) {
                    if (publicUserInput.Length >= 5 && publicUserInput.ToUpper().Substring(publicUserInput.Length - 5, 5) == "?CLL ") {
                        temporaryCancelString = publicUserInput.Substring(publicUserInput.Length - 4, 4);
                        publicUserInput = publicUserInput[..^5];
                        publicUserInput += temporaryCancelString;
                    }
                    temporaryVar = RecievePrimaryInputInteger(ConvertInput(publicUserInput, primaryCommandNames) - 1);
                    subCommandIndex = ConvertInput(publicUserInput, temporaryVar);
                    if (publicUserInput.Length >= 1 && publicUserInput.Substring(publicUserInput.Length - 1, 1) == " ") {
                        publicUserInput = publicUserInput[..^1];
                    }
                    if (!(temporaryVar[0] == "nullArrayType")) {
                        primaryCommandIndex = Array.FindIndex(primaryCommandNames, row => row == (temporaryVar[0]).ToUpper()[..3]);
                        interpretInputValue = InterpretAndProcessInput(primaryCommandIndex - 1, subCommandIndex - 1);
                        if (interpretInputValue == 0) {
                            WriteToConsole(true, true, unrecognizedInput);
                        } else {
                            if (interpretInputValue == 1) {
                                WriteToConsole(true, true, "Action failed due to an error.");
                            } else {
                                if (!(interpretInputValue == 2)) {
                                    WriteToConsole(true, true, "Lmao how did you even do that.");
                                }
                            }
                        }
                    } else {
                        WriteToConsole(true, true, unrecognizedInput);
                    }
                } else {
                    WriteToConsole(true, false, "Action cancelled. Any input ending with \" cll\" will be cancelled. ");
                    WriteToConsole(false, false, "To ignore the cancellation, add a \"?\" before \"cll\". ");
                    WriteToConsole(false, true, "The question mark will be removed, and no space will be added. If you want to type \"?cll\", type \"??cll\" instead.");
                }
            }
        }
        static string ReadUserInput(bool useSpecialPrint, bool enteringHiddenData, bool blScript, string strScript) {
            if (useSpecialPrint) {
                string temporaryDirectory = FindDisplayedDirectory();
                if (blScript) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(strScript + "/");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(temporaryDirectory);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("/> ");
                } else {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(userStatus + "/");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(temporaryDirectory);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("/> ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (enteringHiddenData && !blScript) {
                StringBuilder hiddenUserInput = new();
                bool readingInput = true;
                while (readingInput) {
                    ConsoleKeyInfo consoleKeyInfoLower = Console.ReadKey(true);
                    char hiddenCharacter = consoleKeyInfoLower.KeyChar;
                    if (hiddenCharacter == '\r' || hiddenCharacter == '\n') {
                        readingInput = false;
                    } else {
                        hiddenUserInput.Append(hiddenCharacter);
                    }
                }
                Console.WriteLine();
                string? privateHiddenUserInput = hiddenUserInput.ToString();
                if (!(privateHiddenUserInput == null)) {
                    return privateHiddenUserInput;
                }
                return "";
            }
            if (blScript) {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            string? privateUserInput = Console.ReadLine();
            if (!(privateUserInput == null)) {
                return privateUserInput;
            }
            return "";
        }
        static void WriteToConsole(bool useSpecialPrint, bool splitLines, string printedText) {
            if (useSpecialPrint) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("console/");
                string temporaryDirectory = FindDisplayedDirectory();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(temporaryDirectory);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("/< ");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (!splitLines) {
                Console.Write(printedText);
            } else {
                Console.WriteLine(printedText);
            }
        }
        static string FindDisplayedDirectory() {
            string temporaryDirectory = directoryCommand;
            string? privateDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            privateDirectory ??= "nullDirectory";
            if (directoryCommand == privateDirectory) {
                temporaryDirectory = "$";
            }
            return temporaryDirectory;
        }
        static int ConvertInput(string privateUserInput = "", string[]? privateArrayName = null) {
            privateArrayName ??= [];
            if (privateUserInput.Length >= 3 && privateArrayName.Contains(privateUserInput.ToUpper()[..3])) {
                if (privateUserInput.Length >= 4 && privateUserInput.Substring(3,1) == " ") {
                    publicUserInput = privateUserInput[4..];
                } else {
                    if (privateUserInput.Length >= 4) {
                        publicUserInput = privateUserInput[3..];
                    } else {
                        publicUserInput = "";
                    }
                }
                return Array.FindIndex(privateArrayName, row => row == privateUserInput.ToUpper()[..3]);
            } else {
                publicUserInput = "";
            }
            return -1;
        }
        static int InterpretAndProcessInput(int primaryArrayIndex, int subArrayIndex) {
            if (primaryArrayIndex == 0) {
                if (subArrayIndex == 0) {
                    return 2;
                }
                if (subArrayIndex == 1) {
                    Console.Clear();
                    return 2;
                }
                if (subArrayIndex == 2) {
                    Console.Clear();
                    Main(nullArray);
                    return 1;
                }
                if (subArrayIndex == 3) {
                    System.Environment.Exit(0);
                    return 1;
                }
                return 0;
            }
            if (primaryArrayIndex == 1) {
                if (subArrayIndex == 0) {
                    if (!File.Exists(directoryCommand + "/" + publicUserInput) && ValidateFileName(publicUserInput)) {
                        File.Create(directoryCommand + "/" + publicUserInput).Dispose();
                        return 2;
                    }
                    return 1;
                }
                if (subArrayIndex == 1) {
                    if (ValidateFileName(publicUserInput) && File.Exists(directoryCommand + "/" + publicUserInput)) {
                        StreamReader sr = new(directoryCommand + "/" + publicUserInput);
                        WriteToConsole(true, true, sr.ReadToEnd());
                        sr.Close();
                        return 2;
                    }
                    return 1;
                }
                return 0;
            }
            if (primaryArrayIndex == 2) {
                if (subArrayIndex == 0) {
                    if (!Directory.Exists(directoryCommand + "/" + publicUserInput)) {
                        Directory.CreateDirectory(publicUserInput);
                        return 2;
                    }
                    return 1;
                }
                if (subArrayIndex == 1) {
                    if (Directory.Exists(publicUserInput)) {
                        if (publicUserInput.Length >= 1 && (publicUserInput.Substring(publicUserInput.Length - 1, 1) == "\\" || publicUserInput.Substring(publicUserInput.Length - 1, 1) == "/")) {
                            publicUserInput = publicUserInput[..^1];
                        }
                        directoryCommand = publicUserInput;
                        return 2;
                    }
                    return 1;
                }
                if (subArrayIndex == 2) {
                    string? temporaryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    temporaryDirectory ??= "nullDirectory";
                    WriteToConsole(true, true, temporaryDirectory);
                    return 2;
                }
                if (subArrayIndex == 3) {
                    string? temporaryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    temporaryDirectory ??= "nullDirectory";
                    directoryCommand = temporaryDirectory;
                    return 2;
                }
                return 0;
            }
            if (primaryArrayIndex == 3) {

                return 0;
            }
            if (primaryArrayIndex == 4) {
                if (subArrayIndex == 0) {
                    string? temporaryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    temporaryDirectory ??= "nullDirectory";
                    WriteToConsole(true, true, temporaryDirectory);
                    return 2;
                }
                return 0;
            }
            if (primaryArrayIndex == 5) {
                if (subArrayIndex == 0) {
                    WriteToConsole(true, true, "");
                    WriteToConsole(false, true, "CMD (command)");
                    WriteToConsole(false, true, "   RUN (run a script file)");
                    WriteToConsole(false, true, "   CLR (clear the console)");
                    WriteToConsole(false, true, "   RST (restart the console)");
                    WriteToConsole(false, true, "   ESC (close the console)");
                    WriteToConsole(false, true, "FLE (file)");
                    WriteToConsole(false, true, "   CRT (create a new empty file)");
                    WriteToConsole(false, true, "   OPN (open an existing text file)");
                    WriteToConsole(false, true, "   HEX (print hex of an existing file)");
                    WriteToConsole(false, true, "   WRT (overwrite text in an existing file)");
                    WriteToConsole(false, true, "DIR (directory)");
                    WriteToConsole(false, true, "   CRT (create a directory)");
                    WriteToConsole(false, true, "   SET (set the directory in witch files are created and edited by default, starting with the directory the console is in)");
                    WriteToConsole(false, true, "   RTN (return directory in which the console file is in)");
                    WriteToConsole(false, true, "   RST (reset to the default directory)");
                    WriteToConsole(false, true, "ADM (adm)");
                    WriteToConsole(false, true, "   CRT (create an account)");
                    WriteToConsole(false, true, "   LOG (log into yout account)");
                    WriteToConsole(false, true, "   LGT (log out of your account)");
                    WriteToConsole(false, true, "   DEL (delete your account)");
                    WriteToConsole(false, true, "RTN (return)");
                    WriteToConsole(false, true, "   DIR (return directory in which the console file is in)");
                    WriteToConsole(false, true, "HLP (help)");
                    WriteToConsole(false, true, "   DOC (documentation)");
                    return 2;
                }
                return 0;
            }
            return -3;
        }
        static string[] RecievePrimaryInputInteger(int index) {
            if (!(index == -1) && !(index == -2)) {
                if (index == 0) {
                    return cmdCommandNames;
                }
                if (index == 1) {
                    return fleCommandNames;
                }
                if (index == 2) {
                    return dirCommandNames;
                }
                if (index == 3) {
                    return admCommandNames;
                }
                if (index == 4) {
                    return rtnCommandNames;
                }
                if (index == 5) {
                    return hlpCommandNames;
                }
            }
            return nullArray;
        }
        static bool ValidateFileName(string fileName) {
            if (fileName.Contains('/') || fileName.Contains('\\') || fileName.Contains('<') || fileName.Contains('>') || fileName.Contains('?') || fileName.Contains('"') || fileName.Contains(':') || fileName.Contains('*') || fileName.Contains('|')) {
                return false;
            }
            return true;
        }
    }
}