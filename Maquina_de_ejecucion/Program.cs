using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina_de_ejecucion
{
    class Program
    {
        //DEFINICIÓN DE CONSTANTES
        /*Tamaño máximo en memoria para instrucciones*/
        public const int INSTRUCCIONES_MAXIMO = 10;
        /*Tamaño máximo en memoria para datos*/
        public const int DATOS_MAXIMO = 10;
        /*Cantidad de registros enteros en la máquina*/
        public const int NUMERO_REGISTROS = 8;
        /*Registro de la instrucción que se está ejecutando. Este valor se almacena en el último de los registros*/
        public static int PC_REGISTRO = 7;

        //DEFINICIÓN DE ARREGLOS
        /*Arreglo para almacenar datos*/
        public static int[] datos_memoria = new int[DATOS_MAXIMO];
        /*Arreglo para almacenar los registros*/
        public static int[] reg = new int[NUMERO_REGISTROS];
        /*Arreglo para almacenar las instrucciones a ejecutar*/
        public static string[] instrucciones_memoria = new string[INSTRUCCIONES_MAXIMO];

        #region MÉTODOS DE IMPRESIÓN
        /*Imprime de color rojo la cadena que se envíe como parámetro*/
        public static void Imprimir_Error(String _informacionError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error > ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_informacionError);
        }//Imprimir_Error

        /*Imprime de color amarilla la cadena que se envíe como parámetro*/
        public static void Imprimir_Comando(String _instruccion)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("cmd > ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(_instruccion);
        }//Imprimir_Error

        /*Imprime de color azul la cadena que se envíe como parámetro*/
        public static void Imprimir_Info(String _informacion)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("info > ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(_informacion);
        }//Imprimir_Error
        #endregion

        #region COMANDOS
        #region COMANDOS DE OPERACIÓN
        /*Detiene la ejecución de la máquina. Los parámetros son ignorados*/
        public static void HALT(int r, int s, int t)
        {
            Imprimir_Comando("HALT, finalizando ejecución\n");
            return;
        }//HALT

        /*Se lee un valor de entrada y se almacena en reg en la posición r*/
        public static void IN(int r, int s, int t)
        {
            bool valid = false;
            int input = 0;
            while (!valid)
            {
                Imprimir_Comando("IN: ");
                try
                {
                    input = int.Parse(Console.ReadLine());
                    valid = true;
                }
                catch(Exception e)
                {
                    Imprimir_Error("Valor de entrada no válido.");
                }
            }
            try
            {
                reg[r] = input;
            }
            catch
            {
                Imprimir_Error("El valor (" + r + ") no es válido para el comando IN");
            }
        }//IN

        /*Se escribe en la salida el valor de reg en la posición r*/
        public static void OUT(int r, int s, int t)
        {
            try
            {
                Imprimir_Comando("OUT: [" + r + "] --> " + reg[r] + "\n");
            }
            catch
            {
                Imprimir_Error("El valor (" + r + ") no es válido para el comando OUT");
            }
        }//OUT

        /*reg[r]=reg[s]+reg[t]*/
        public static void ADD(int r, int s, int t)
        {
            try
            {
                reg[r] = reg[s] + reg[t];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación ADD: Revisa los parámetros s(" + s + "), r(" + r + ")");
            }
        }//ADD

        /*reg[r]=reg[s]-reg[t]*/
        public static void SUB(int r, int s, int t)
        {
            try
            {
                reg[r] = reg[s] - reg[t];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación SUB: Revisa los parámetros s(" + s + "), r(" + r + ")");
            }
        }//SUB

        /*reg[r]=reg[s]*reg[t]*/
        public static void MUL(int r, int s, int t)
        {
            try
            {
                reg[r] = reg[s] * reg[t];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación MUL: Revisa los parámetros s(" + s + "), r(" + r + ")");
            }
        }//MUL

        /*reg[r]=reg[s]/reg[t] (debe generar error si se divide por cero)*/
        public static void DIV(int r, int s, int t)
        {
            try
            {
                reg[r] = reg[s] / reg[t];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación DIV: Revisa los parámetros s(" + s + "), r(" + r + ")");
            }
        }//DIV
        #endregion
        #region COMANDOS DE MEMORIA
        /*reg[r]=datos_Memoria[a]*/
        public static void LD(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                reg[r] = datos_memoria[a];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación LD: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//LD

        /*reg[r]=a*/
        public static void LDA(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                reg[r] = a;
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación LDA: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//LDA

        /*reg[r]=d (s es ignorado)*/
        public static void LDC(int r, int d, int s)
        {
            try
            {
                reg[r] = d;
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación LDC: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//LDC

        /*datos_Memoria [a]=reg[r]*/
        public static void ST(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                datos_memoria[a] = reg[r];
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación ST: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//ST

        /*If reg[r]<0 reg[PC_REGISTRO]=a*/
        public static void JLT(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] < 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JLT: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JLT

        /*If reg[r]<=0 reg[PC_REGISTRO]=a*/
        public static void JLE(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] <= 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JLE: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JLE

        /*If reg[r]>=0 reg[PC_REGISTRO]=a*/
        public static void JGE(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] >= 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JGE: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JGE

        /*If reg[r]>0 reg[PC_REGISTRO]=a*/
        public static void JGT(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] > 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JGT: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JGT

        /*If reg[r]=0 reg[PC_REGISTRO]=a*/
        public static void JEQ(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] == 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JEQ: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JEQ

        /*If reg[r]!=0 reg[PC_REGISTRO]=a*/
        public static void JNE(int r, int d, int s)
        {
            try
            {
                int a = d + reg[s];
                if (reg[r] != 0)
                    reg[PC_REGISTRO] = a - 1; //-1 porque se le va a aumentar 1 automáticamente 
            }
            catch
            {
                Imprimir_Error("No se pudo completar la operación JNE: Revisa los parámetros r(" + r + "), d(" + d + "), s(" + s + ")");
            }
        }//JNE
        #endregion 
        #endregion

        /* Método que lee el archivo con las instrucciones y las almacena en instrucciones_memoria.  El archivo debe estar en la misma carpeta que el ejecutable*/
        public static bool Leer_Instrucciones(String _nombreArchivo)
        {
            String rutaCompleta = Directory.GetCurrentDirectory() + "\\" + _nombreArchivo;
            try
            {
                StreamReader lector = new StreamReader(rutaCompleta);
                int contador_instrucciones = 0;
                while (!lector.EndOfStream)
                {
                    string linea_actual = lector.ReadLine();
                    if (!linea_actual[0].Equals('*')) // Si el primer caracter de la línea es *, significa que es un comentario
                    {
                        if (contador_instrucciones < INSTRUCCIONES_MAXIMO) //Verifica que no se haya sobrepasado el número de instrucciones
                        {
                            /*Separa la línea > 1: CMD p,p,p --> [1][CMD p,p,p]*/
                            string[] numeroInstruccion_instruccion = linea_actual.Split(':');
                            int numero_instruccion = 0;
                            try
                            {
                                numero_instruccion = int.Parse(numeroInstruccion_instruccion[0].Replace(" ", string.Empty));
                            }
                            catch (Exception e)
                            {
                                Imprimir_Error("El número de la instrucción (" + numeroInstruccion_instruccion[0] + ") no es válido");
                                return false;
                            }
                            /*Separa la línea > CMD p,p,p --> [CMD][p,p,p]*/
                            string[] instruccion_parametros = numeroInstruccion_instruccion[1].Split(' ');
                            int instruction_position = 0; //Determina en que punto de la cadena se encuentra la instrucción (en caso que haya muchos espacios en blanco)
                            for (int i = 0; i < instruccion_parametros.Length; i++)
                            {
                                if(instruccion_parametros[i] == "")
                                    instruction_position++;
                            }
                            string instruccion = instruccion_parametros[instruction_position].Replace(" ", string.Empty);
                            string[] parametros = instruccion_parametros[instruction_position + 1].Split(',');
                            if (parametros.Length == 3) //Quiere decir que es una instrucción de tipo OPERACIÓN (r,s,t)
                            {
                                instrucciones_memoria[numero_instruccion - 1] = instruccion     + "," + 
                                                                            parametros[0]   + "," + //Representa la r
                                                                            "null"          + "," + //representa la d (nulo)
                                                                            parametros[1]   + "," + //Representa la s
                                                                            parametros[2];          //Representa la t
                            }
                            else if (parametros.Length == 2) //Quiere decir que es una instrucción de tipo MEMORIA (r,d(s))
                            {
                                string[] parametros_memoria = parametros[1].Split('('); //Obtiene el valor de d y s
                                string d = parametros_memoria[0];
                                string s = parametros_memoria[1].Replace(")", string.Empty);
                                instrucciones_memoria[numero_instruccion - 1] = instruccion     + "," +
                                                                            parametros[0]   + "," + //Representa la r
                                                                            d               + "," + //Representa la d
                                                                            s               + "," + //Representa la s
                                                                            "null";             //Representa la t (nulo)
                            }
                            else
                            {
                                Imprimir_Error("La instrucción número (" + numero_instruccion + ") tiene uná cantidad inválida de parámetros (" + parametros.Length + ")");
                                return false;
                            }
                            contador_instrucciones++;
                        }
                        else
                        {
                            Imprimir_Error("Se excedió el límite de instrucciones permitidas (" + INSTRUCCIONES_MAXIMO + ")");
                            return false;
                        }
                    }
                }
                //Rellena todas las instrucciones vacías por HALT para finalizar el programa en caso que no haya sido especificado
                for (int i = INSTRUCCIONES_MAXIMO - 1; i >= 0; i--)
                {
                    if(instrucciones_memoria[i] == null)
                    {
                        instrucciones_memoria[i] = "HALT,0,0,0,0";
                    }
                    else
                    {
                        break;
                    }
                }
                lector.Close();
            }
            catch (InvalidCastException e)
            {
                Imprimir_Error("No se ha podido leer el archivo. Verifica que se exista en: " + rutaCompleta);
                return false;
            }
            return true;
        }//Read_Instructions

        /* Método que ejecuta todos los comandos almacenados en el arreglo de instrucciones*/
        public static bool Ejecutar_Instrucciones()
        {
            while (reg[PC_REGISTRO] < INSTRUCCIONES_MAXIMO)
            {
                /*Separa cada una de las instrucciones de CMD,r,d,s,t --> [CMD][r][d][s][t]*/
                string[] instrucciones_y_parametros = instrucciones_memoria[PC_REGISTRO].Split(',');
                switch(instrucciones_y_parametros[0])
                {
                    case "HALT":
                        HALT(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        return true;
                    case "IN":
                        IN(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "OUT":
                        OUT(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "ADD":
                        ADD(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "SUB":
                        SUB(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "MUL":
                        MUL(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "DIV":
                        DIV(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[3]), int.Parse(instrucciones_y_parametros[4]));
                        break;
                    case "LD":
                        LD(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "LDA":
                        LDA(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "LDC":
                        LDC(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "ST":
                        ST(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JLT":
                        JLT(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JLE":
                        JLE(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JGE":
                        JGE(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JGT":
                        JGT(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JEQ":
                        JEQ(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    case "JNE":
                        JNE(int.Parse(instrucciones_y_parametros[1]), int.Parse(instrucciones_y_parametros[2]), int.Parse(instrucciones_y_parametros[3]));
                        break;
                    default:
                        Imprimir_Error("Comando no conocido (" + instrucciones_y_parametros[0] + ") en la línea (" + PC_REGISTRO + ")");
                        return false;
                }
                reg[PC_REGISTRO]++;
            }
            return true;
        }

        /*Método que ejecuta las instrucciones dependiendo del código en el archivo.*/
        public static bool Ejecutar(String _nombreArchivo)
        {
            if (!Leer_Instrucciones(_nombreArchivo))
                return false;
            if (!Ejecutar_Instrucciones())
                return false;
            return true;
        }//Execute

        static void Main(string[] args)
        {
            Console.WriteLine("Máquina de ejecución");
            Imprimir_Comando("Indique el nombre del archivo: ");
            String nombre = Console.ReadLine();
            if(!Ejecutar(nombre))
                Imprimir_Error("Error en la ejecución. Finalizando máquina de ejecución.");
            Console.ReadKey();
        }//Main
    }
}
