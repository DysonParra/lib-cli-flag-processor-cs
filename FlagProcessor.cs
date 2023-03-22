/*
 * @fileoverview    {FlagProcessor}
 *
 * @version         2.0
 *
 * @author          Dyson Arley Parra Tilano <dysontilano@gmail.com>
 *
 * @copyright       Dyson Parra
 * @see             github.com/DysonParra
 *
 * History
 * @version 1.0     Implementación realizada.
 * @version 2.0     Documentación agregada.
 */
using System;
using System.Text.RegularExpressions;

namespace Program {

    /**
    * TODO: Definición de {@code FlagProcessor}.
    *
    * @author Dyson Parra
    * @since 1.8
    */
    public class FlagProcessor {

        /**
        * Muestra en consola el array indicado por {@code flags}
        *
        * @param flags     array de {@code Flag} que se va a imprimir en la consola.
        * @param printNull indica si se van a imprimir las flags con valor de {@code null}
        */
        public static void PrintFlagsArray(Flag[] flags, Boolean printNull) {
            Console.WriteLine("\nSTART Flags:");
            foreach (Flag flag in flags)
                if (printNull || flag != null)
                    Console.WriteLine(flag);
            Console.WriteLine("END Flags\n");
        }

        /**
        * Muestra en consola la matriz indicada por {@code flags}
        *
        * @param flags   array de {@code String} que se va a imprimir en la consola.
        * @param message mensaje que se mostrará antes de imprimir las flags.
        */
        public static void PrintFlagsMatrix(String[][] flags, String message) {
            Console.WriteLine(message);
            String line;
            foreach (String[] valueOr in flags) {
                line = valueOr[0];
                for (int i = 1; i < valueOr.Length; i++)
                    line += " or " + valueOr[i];
                Console.WriteLine(line);
            }
            if (flags.Length == 0)
                Console.WriteLine("Not specified.");
            Console.WriteLine("");
        }

        /**
        * Compara si hay {@code String} que están tanto en la matriz {@code requiredFlags} como en la
        * matriz {@code optionalFlags}.
        *
        * @param requiredFlags una matriz con las flags requeridas; en cada fila se indican las flags y
        *                      en cada columna indica cuales flags son excluyentes (si se incluye la
        *                      flag de una columna no se pueden incluir las flags en las otras columnas
        *                      de esa fila) al ser requeridas se debe incluir una y solo una flag de
        *                      cada fila.
        * @param optionalFlags una matriz con las flags opcionales; en cada fila se indican las flags y
        *                      en cada columna indica cuales flags son excluyentes (si se incluye la
        *                      flag de una columna no se pueden incluir las flags en las otras columnas
        *                      de esa fila) al ser opcionales se pueden o no incluir una y solo una
        *                      flag de cada fila.
        * @return {@code null} si no hay flags repetidas, caso contrario {@code String} con un mensaje
        *         que dice cuales flags se repiten.
        */
        private static String? CompareRequiredAndOptionalFlags(String[][] requiredFlags, String[][] optionalFlags) {
            foreach (String[] reqValuesOr in requiredFlags)
                foreach (String reqFlag in reqValuesOr)
                    foreach (String[] optValuesOr in optionalFlags)
                        foreach (String optFlag in optValuesOr)
                            if (reqFlag.Equals(optFlag))
                                return "Error: flag " + "'" + optFlag + "'" + " is defined as required flag and as optional flag.";
            return null;
        }

        /**
        * Analiza si el array {@code args} representa una secuencia válida de flags y en caso
        * afirmativo almacena en {@code inputFlags} el equivalente en {@code Flag} del array.
        *
        * @param args       un array de {@code String} que se va a procesar para verificar si es una
        *                   secuencia de flags válida.
        * @param inputFlags el array de {@code Flag} donde se va a almacenar el equivalente en
        *                   {@code Flag} del array {@code args}; se da por hecho que el array no es
        *                   {@code null} ya que si lo es se generará un {@code NullPointerException}
        * @return {@code null} si {@code inputFlags} representa una secuencia de flags válida, caso
        *         contrario {@code String} con un mensaje que indica porque no es una secuencia válida
        *         de flags.
        */
        private static String? ConvertArgsToFlagsArray(String[] args, Flag[] inputFlags) {

            String msg_Invalid_Flag = "Error: invalid flag ";
            String msg_Expect_Value_1 = "Error: expected a value for the flag ";
            String msg_Expect_Value_2 = " but found the flag ";
            String msg_Expect_Flag_1 = "Error: expected a flag or a flag withouth value after the value ";
            String msg_Expect_Flag_2 = " but found the value ";
            String msg_First_Arg_Not_Flag = "Error: expected a flag or a flag withouth value as first parameter but found the value ";
            String msg_Value_For_Flag_No_Value_1 = "Error: The flags started in '--' no need value, but found the value ";
            String msg_Value_For_Flag_No_Value_2 = " for the flag ";
            String msg_Last_Arg_Flag = "Error: Not found a value for the flag ";

            int flagNumber = 0;
            String flag = "";
            String value;
            String oldState;
            String state = "value";
            for (int i = 0; i < args.Length; i++) {
                oldState = state;
                String arg = args[i];
                if (Regex.IsMatch(arg, "^-*$") || Regex.IsMatch(arg, "^---.*$"))
                    return msg_Invalid_Flag + "'" + arg + "'";
                else if (Regex.IsMatch(arg, "^--.*$"))
                    state = "flagNoValue";
                else if (Regex.IsMatch(arg, "^-.*$"))
                    state = "flag";
                else
                    state = "value";

                switch (state) {
                    case "flag":
                        switch (oldState) {
                            case "flag":
                                return msg_Expect_Value_1 + "'" + args[i - 1] + "'"
                                        + msg_Expect_Value_2 + "'" + arg + "'";
                            case "value":
                            case "flagNoValue":
                                flag = arg;
                                break;
                        }
                        break;

                    case "value":
                        switch (oldState) {
                            case "flag":
                                value = arg;
                                //inputFlags[flagNumber] = new Flag(flag, value, false);
                                inputFlags[flagNumber] = new Flag() {
                                    Name = flag,
                                    Value = value,
                                    Required = false
                                };
                                flagNumber++;
                                break;
                            case "value":
                                if (i != 0)
                                    return msg_Expect_Flag_1 + "'" + args[i - 1] + "'"
                                            + msg_Expect_Flag_2 + "'" + arg + "'";
                                else
                                    return msg_First_Arg_Not_Flag + "'" + arg + "'";
                            case "flagNoValue":
                                return msg_Value_For_Flag_No_Value_1 + "'" + arg + "'"
                                        + msg_Value_For_Flag_No_Value_2 + "'" + args[i - 1] + "'";
                        }
                        break;

                    case "flagNoValue":
                        switch (oldState) {
                            case "flag":
                                return msg_Expect_Value_1 + "'" + args[i - 1] + "'"
                                        + msg_Expect_Value_2 + "'" + arg + "'";
                            case "value":
                            case "flagNoValue":
                                flag = arg;
                                //inputFlags[flagNumber] = new Flag(flag, null, false);
                                inputFlags[flagNumber] = new Flag() {
                                    Name = flag,
                                    Value = null,
                                    Required = false
                                };
                                flagNumber++;
                                break;
                        }
                        break;
                }
            }

            if (state.Equals("flag"))
                return msg_Last_Arg_Flag + "'" + flag + "'";
            return null;
        }

        /**
        * Verifica si el array {@code inputFlags} contiene las flags indicadas por
        * {@code acceptedFlags} de tal modo que si {@code required} es {@code true} significa que las
        * flags son requeridas entonces {@code args} deben incluir todas las flags indicadas por
        * {@code acceptedFlags}, caso contrario si {@code required} es {@code false} {@code args} puede
        * incluir cero o más flags indicadas por {@code acceptedFlags}.
        *
        * @param inputFlags    es el array de {@code Flag} que se va a procesar.
        * @param outputFlags   es un array donde se almacenarán las flags indicada por
        *                      {@code acceptedFlags} que estén en {@code inputFlags}
        * @param acceptedFlags son las flags que se revisará que estén o no en {@code inputFlags}
        * @param required      indica si las flags indicadas por {@code acceptedFlags} son requeridas u
        *                      opcionales.
        * @return {@code null} si no ocurre ningún inconveniente al procesar {@code inputFlags}, caso
        *         contrario {@code String} con un mensaje que indica porque no es posible procesar
        *         {@code inputFlags}
        */
        private static String? CompareInputFlags(Flag[] inputFlags, Flag[] outputFlags, String[][] acceptedFlags, Boolean required) {
            Boolean found;
            int flagNumber = 0;
            foreach (Flag flag in outputFlags)
                if (flag == null)
                    break;
                else
                    flagNumber++;

            String errorMessage;
            foreach (String[] reqValuesOr in acceptedFlags) {
                found = false;
                foreach (String reqFlag in reqValuesOr) {
                    //Console.Write(reqFlag);
                    for (int i = 0; i < inputFlags.Length; i++) {
                        if (inputFlags[i] != null) {
                            //Console.Write(" -> " + inputFlags[i].getName());
                            if (reqFlag.Equals(inputFlags[i].Name)) {
                                if (!found) {
                                    //Console.Write("\nAdded " + inputFlags[i].getName() + " in " + flagNumber);
                                    outputFlags[flagNumber] = inputFlags[i];
                                    if (required)
                                        inputFlags[i].Required = true;
                                    flagNumber++;
                                    inputFlags[i] = null!;
                                    found = true;
                                    //Console.Write("\n");
                                    //Console.Write("Again?");
                                    for (int j = i; j < inputFlags.Length; j++) {
                                        if (inputFlags[j] != null) {
                                            //Console.Write(" -> " + inputFlags[j].getName());
                                            if (reqFlag.Equals(inputFlags[j].Name)) {
                                                //Console.WriteLine("\nFound again!");
                                                errorMessage = "found the flag '" + reqFlag + "' more that one times";
                                                return errorMessage;
                                            }
                                        }
                                    }
                                    break;
                                } else {
                                    //Console.WriteLine("\nFound invalid combination");
                                    errorMessage = "Error: specified more that one of the flags";
                                    foreach (String aux in reqValuesOr)
                                        errorMessage += " " + aux;
                                    return errorMessage;
                                }
                            }
                        }
                    }
                    //Console.WriteLine("");
                }
                if (!found) {
                    //Console.WriteLine("Not found");
                    if (required) {
                        errorMessage = "Not found required flag ";
                        errorMessage += reqValuesOr[0];
                        for (int i = 1; i < reqValuesOr.Length; i++)
                            errorMessage += " or " + reqValuesOr[i];
                        //Console.WriteLine(errorMessage);
                        return errorMessage;
                    }
                }
                //Console.WriteLine("");
            }

            return null;
        }

        /**
        * Verifica el array {@code inputFlags} cuales de las flags indicadas por {@code requiredFlags}
        * y por {@code optionalFlags} contiene y si {@code allowUnknownFlags} es {@code true} acepta
        * que hayan flags que no estén ni en {@code requiredFlags} ni en {@code optionalFlags}.
        *
        * @param inputFlags        es el array de {@code Flag} que se va a procesar.
        * @param outputFlags       es un array donde se almacenarán las flags indicada por
        *                          {@code requiredFlags} y por {@code optionalFlags} que estén en
        *                          {@code inputFlags}
        * @param requiredFlags     una matriz con las flags requeridas; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la flag de una columna no se pueden incluir las flags en las
        *                          otras columnas de esa fila) al ser requeridas se debe incluir una y
        *                          solo una flag de cada fila.
        * @param optionalFlags     una matriz con las flags opcionales; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la flag de una columna no se pueden incluir las flags en las
        *                          otras columnas de esa fila) al ser opcionales se pueden o no incluir
        *                          una y solo una flag de cada fila.
        * @param allowUnknownFlags si {@code true} se aceptan flags que no estén en el array
        *                          {@code requiredFlags} ni en el array {@code optionalFlags}, caso
        *                          contrario si se encuentra una flag que no esté en los arrays
        *                          devuelve {@code String} con mensaje de error.
        * @return {@code null} si no ocurre ningún inconveniente al procesar {@code inputFlags}, caso
        *         contrario {@code String} con un mensaje que indica porque no es posible procesar
        *         {@code inputFlags}
        */
        private static String? ValidateSpecifedFlags(Flag[] inputFlags, Flag[] outputFlags, String[][] requiredFlags, String[][] optionalFlags, Boolean allowUnknownFlags) {
            String? result;
            result = CompareInputFlags(inputFlags, outputFlags, requiredFlags, true);
            if (result == null) {
                result = CompareInputFlags(inputFlags, outputFlags, optionalFlags, false);
                if (result == null) {
                    int flagQuantity = 0;
                    foreach (Flag outputFlag in outputFlags)
                        if (outputFlag != null)
                            flagQuantity++;
                        else
                            break;

                    foreach (Flag inputFlag in inputFlags)
                        if (inputFlag != null) {
                            if (!allowUnknownFlags) {
                                if (result == null)
                                    result = "Error: unknown flags found";
                                result += "  " + inputFlag.Name;
                                //Console.WriteLine(result);
                            } else {
                                outputFlags[flagQuantity] = inputFlag;
                                flagQuantity++;
                            }
                        }
                }
            }
            return result;
        }

        /**
        * Analiza si el array {@code args} representa una secuencia válida de flags, verifica que las
        * flags requeridas {@code requiredFlags} estén todas especificadas allí, revisa que se incluyan
        * cero o más flags opcionales {@code optionalFlags} y si {@code allowUnknownFlags} es
        * {@code true} se aceptan flags que no sean opcionales ni requeridas caso afirmativo.
        *
        * @param args              un array de {@code String} que se va a procesar para verificar si es
        *                          una secuencia de flags válida.
        * @param requiredFlags     una matriz con las flags requeridas; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la flag de una columna no se pueden incluir las flags en las
        *                          otras columnas de esa fila) al ser requeridas se debe incluir una y
        *                          solo una flag de cada fila.
        * @param optionalFlags     una matriz con las flags opcionales; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la flag de una columna no se pueden incluir las flags en las
        *                          otras columnas de esa fila) al ser opcionales se pueden o no incluir
        *                          una y solo una flag de cada fila.
        * @param allowUnknownFlags si {@code true} se aceptan flags que no estén en el array
        *                          {@code requiredFlags} ni en el array {@code optionalFlags}, caso
        *                          contrario si se encuentra una flag que no esté en los arrays se
        *                          devuelve {@code null} y se mostrará mensaje de error.
        * @return array de {@code Flag} si se puede procesar {@code args} utilizando
        *         {@code requiredFlags} y {@code optionalFlags} sin ningún inconveniente, caso
        *         contrario {@code null} y se mostrará en consola porqué no fue posible procesar
        *         {@code args}.
        */
        private static Flag[]? ValidateFlags(String[] args, String[][] requiredFlags, String[][] optionalFlags, Boolean allowUnknownFlags) {
            String? result;
            int argsQuantity = args.Length;
            Flag[] inputFlags;
            Flag[] outputFlags;

            result = CompareRequiredAndOptionalFlags(requiredFlags, optionalFlags);
            if (result != null) {
                Console.WriteLine(result);
                return null;
            }

            inputFlags = new Flag[argsQuantity];
            result = FlagProcessor.ConvertArgsToFlagsArray(args, inputFlags);
            if (result != null) {
                Console.WriteLine(result);
                return null;
            }

            int flagQuantity = 0;
            foreach (Flag flag in inputFlags)
                if (flag != null)
                    flagQuantity++;
            outputFlags = new Flag[flagQuantity];

            result = ValidateSpecifedFlags(inputFlags, outputFlags, requiredFlags, optionalFlags, allowUnknownFlags);
            if (result != null) {
                Console.WriteLine(result);
                return null;
            }
            return outputFlags;
        }

        /**
        * Analiza si el array {@code args} representa una secuencia válida de flags, verifica que las
        * flags requeridas {@code requiredFlags} estén todas especificadas allí, revisa que se incluyan
        * cero o más flags opcionales {@code optionalFlags} y si {@code allowUnknownFlags} es
        * {@code true} se aceptan flags que no sean opcionales ni requeridas caso afirmativo.
        *
        * @param args              un array de {@code String} que se va a procesar para verificar si es
        *                          una secuencia de flags válida.
        * @param defaultArgs       un array de {@code String} que se va a procesar para verificar si es
        *                          una secuencia de flags válida en caso de {@code  args} se encuentre
        *                          vacío.
        * @param requiredFlags     una matriz con las flags requeridas; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la valueOr de una columna no se pueden incluir las flags en
        *                          las otras columnas de esa fila) al ser requeridas se debe incluir
        *                          una y solo una valueOr de cada fila.
        * @param optionalFlags     una matriz con las flags opcionales; en cada fila se indican las
        *                          flags y en cada columna indica cuales flags son excluyentes (si se
        *                          incluye la valueOr de una columna no se pueden incluir las flags en
        *                          las otras columnas de esa fila) al ser opcionales se pueden o no
        *                          incluir una y solo una valueOr de cada fila.
        * @param allowUnknownFlags si {@code true} se aceptan flags que no estén en el array
        *                          {@code requiredFlags} ni en el array {@code optionalFlags}, caso
        *                          contrario si se encuentra una valueOr que no esté en los arrays se
        *                          devuelve {@code null} y se mostrará mensaje de error.
        * @return array de {@code Flag} si se puede procesar {@code args} utilizando
        *         {@code requiredFlags} y {@code optionalFlags} sin ningún inconveniente, caso
        *         contrario {@code null} y se mostrará en consola porqué no fue posible procesar
        *         {@code args}.
        */
        public static Flag[]? ConvertArgsToFlags(String[] args, String[] defaultArgs, String[][] requiredFlags, String[][] optionalFlags, Boolean allowUnknownFlags) {
            Flag[]? flags = null;
            requiredFlags = requiredFlags != null ? requiredFlags : new String[][]{};
            optionalFlags = optionalFlags != null ? optionalFlags : new String[][]{};
            if ((defaultArgs == null || defaultArgs.Length == 0)
                    && (args == null || args.Length == 0)) {
                Console.WriteLine("Flags and default flags not specified...");
                flags = null;
            } else if (args != null && args.Length != 0) {
                Console.WriteLine("Validating specified flags...");
                flags = FlagProcessor.ValidateFlags(args, requiredFlags, optionalFlags, allowUnknownFlags);
            } else if (defaultArgs != null && defaultArgs.Length != 0) {
                Console.WriteLine("No flags specified, validating default flags...");
                flags = FlagProcessor.ValidateFlags(defaultArgs, requiredFlags, optionalFlags, allowUnknownFlags);
            }

            if (flags == null) {
                Console.WriteLine("");
                PrintFlagsMatrix(requiredFlags, "Required flags:");
                PrintFlagsMatrix(optionalFlags, "Optional flags:");
            }

            return flags;
        }

    }
}