/*
 * @fileoverview    {Application}
 *
 * @version         2.0
 *
 * @author          Dyson Arley Parra Tilano <dysontilano@gmail.com>
 *
 * @copyright       Dyson Parra
 * @see             github.com/DysonParra
 *
 * History
 * @version 1.0     Implementation done.
 * @version 2.0     Documentation added.
 */
using System;

namespace Program {

    /**
    * TODO: Description of {@code Application}.
    *
    * @author Dyson Parra
    */
    public class Program {

        /**
        * Entrada principal del sistema.
        *
        * @param args argumentos de la linea de comandos.
        */
        public static void Main(String[] args) {
            Console.WriteLine("\n...START...");

            String[][] requiredFlags = {
                new String[] {"-use", "--notUse", "--find"},
                new String[] {"-name", "--noName"},
                new String[] {"-encoder", "--noEncoder"}
            };

            String[][] optionalFlags = {
                new String[] {"-flag", "--noFlag"},
                new String[] {"-test", "--noTest"}
            };

            String[] defaultArgs = {
                "--useDefault",
                "-uses",
                "MKV",
                "-noTest",
                "aac",
                "-use",
                "ffmpeg",
                "-name",
                "chromedriver.exe",
                "-test",
                "_urls.xml",
                "--aac",
                "--noEncoder"
            };

            // foreach (String arg in args)
            //     Console.WriteLine(arg);
            Flag[]? flags;
            flags = FlagProcessor.ConvertArgsToFlags(args, defaultArgs, requiredFlags, optionalFlags, true);
            if (flags == null) {
                Console.WriteLine("...ERROR IN FLAGS...");
                return;
            }

            FlagProcessor.PrintFlagsArray(flags, true);
            Console.WriteLine("...END...");
        }

    }
}