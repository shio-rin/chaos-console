using System.Text;
using System.Reflection;
namespace ChaosConsole {
    internal class Program {
        public static string scriptFileSourceFromMainDirectory = "AntiCrash";
        public static string unrecognizedInput = "ERROR 1: input not recognized.";
        public static string directoryCommand = "../net8.0";
        public static string publicUserInput = "AntiCrash";
        public static string userStatus = "guest";
        public static string prefix = "";
        public static bool loggedInWithAdminPerms = false;
        public static bool runningScript = false;
        public static string[] nullArray = [
            "nullArrayType"
        ];
        public static string[] primaryCommandNames = [
            "lowerCase", "CMD", "FLE", "DIR", "ADM", "RTN", "HLP"
        ];
            public static string[] cmdCommandNames = [
                "cmd", "RUN", "CLR", "RST", "ESC"
                //      WIP   done   done   done
            ];
            public static string[] fleCommandNames = [
                "fle", "CRT", "OPN", "HEX", "WRT", "DEL"
                //     done   done    WIP    WIP    WIP
            ];
            public static string[] dirCommandNames = [
                "dir", "CRT", "SET", "RTN", "RST", "MOV"
                //     done   done   done   done    WIP
            ];
            public static string[] admCommandNames = [
                "adm", "CRT", "LOG", "LGT", "DEL"
                //      WIP    WIP    WIP    WIP
            ];
            public static string[] rtnCommandNames = [
                "rtn", "DIR"
                //     done
            ];
            public static string[] hlpCommandNames = [
                "hlp", "DOC", "ERR"
                //     done
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
                        ReturnErrorCode(interpretInputValue);
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
                    return 7;
                }
                if (subArrayIndex == 3) {
                    System.Environment.Exit(0);
                    return 7;
                }
                return 0;
            }
            if (primaryArrayIndex == 1) {
                if (subArrayIndex == 0) {
                    if (!File.Exists(directoryCommand + "/" + publicUserInput) && ValidateFileName(publicUserInput)) {
                        File.Create(directoryCommand + "/" + publicUserInput).Dispose();
                        return 2;
                    }
                    return 6;
                }
                if (subArrayIndex == 1) {
                    if (ValidateFileName(publicUserInput) && File.Exists(directoryCommand + "/" + publicUserInput)) {
                        StreamReader sr = new(directoryCommand + "/" + publicUserInput);
                        WriteToConsole(true, true, sr.ReadToEnd());
                        sr.Close();
                        return 2;
                    }
                    return 5;
                }
                return 0;
            }
            if (primaryArrayIndex == 2) {
                if (subArrayIndex == 0) {
                    if (!Directory.Exists(directoryCommand + "/" + publicUserInput)) {
                        Directory.CreateDirectory(publicUserInput);
                        return 2;
                    }
                    return 4;
                }
                if (subArrayIndex == 1) {
                    if (Directory.Exists(publicUserInput)) {
                        if (publicUserInput.Length >= 1 && (publicUserInput.Substring(publicUserInput.Length - 1, 1) == "\\" || publicUserInput.Substring(publicUserInput.Length - 1, 1) == "/")) {
                            publicUserInput = publicUserInput[..^1];
                        }
                        directoryCommand = publicUserInput;
                        return 2;
                    }
                    return 3;
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
                    WriteToConsole(false, true, "   ERR (describes error and how to fix it)");
                    return 2;
                }
                if (subArrayIndex == 1) {
                    if (publicUserInput.Length >= 1)
                    {
                        return HelpError();
                    }
                    return 1;
                }
                return 0;
            }
            return -3;
        }
        static void ReturnErrorCode(int erNum) {
            if (erNum != 2) {
                if (erNum == 0) {
                    WriteToConsole(true, true, unrecognizedInput);
                } else {
                    if (erNum == 1) {
                        WriteToConsole(true, true, "ERROR 0: action failed.");
                    } else {
                        if (erNum == -3) {
                            WriteToConsole(true, true, "ERROR 5: lmao how did you even do that.");
                        } else {
                            if (erNum == 3) {
                                WriteToConsole(true, true, "ERROR 2: directory does not exist.");
                            } else {
                                if (erNum == 4) {
                                    WriteToConsole(true, true, "ERROR 3: directory already exists.");
                                } else {
                                    if (erNum == 5) {
                                        WriteToConsole(true, true, "ERROR 4: file naming error.");
                                    } else {
                                        if (erNum == 6) {
                                            WriteToConsole(true, true, "ERROR 6: file could not be found.");
                                        } else {
                                            if (erNum == 7) {
                                                WriteToConsole(true, true, "ERROR 7:");
                                            } else {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        static int HelpError() {
            if (publicUserInput == "0") {
                WriteToConsole(true, true, "ERROR 0: an unkown error, more error codes are being added with every new version");
                return 2;
            }
            if (publicUserInput == "1") {
                WriteToConsole(true, false, "ERROR 1: your input was not recognized due to improper or wrong syntax. Use the \"hlp doc\" command for a list of commands. ");
                WriteToConsole(false, true, "Caps do not matter, however the names of the commands have to match exactly. Some commands require an additional input at the end.");
                return 2;
            }
            if (publicUserInput == "5") {
                WriteToConsole(true, true, "ERROR 5: how tf did you get that lmao it should be impossible. Actually please explain how that happened.");
                return 2;
            }
            if (publicUserInput == "2") {
                WriteToConsole(true, true, "ERROR 2: the directory you are looking for could not be found from the source directory.");
                return 2;
            }
            if (publicUserInput == "3") {
                WriteToConsole(true, true, "ERROR 3: the directory you are trying to create already exists.");
                return 2;
            }
            if (publicUserInput == "4") {
                WriteToConsole(true, true, "ERROR 4: file name was invalid due to special characters or file already exists.");
                return 2;
            }
            if (publicUserInput == "6") {
                WriteToConsole(true, true, "ERROR 6: file name was invalid due to special characters or file does not exist in the current directory.");
                return 2;
            }
            if (publicUserInput == "7") {
                WriteToConsole(true, true, "ERROR 7: app did not close or reset.");
                return 2;
            }
            return 0;
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