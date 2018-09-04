using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctrlCompilador
{
    public class clsCompilador
    {
        public string pathFile { get; set; }
        public string code { get; set; }
        public string nameFolder { get; set; }
        public string nameFile { get; set; }
        public string resultado { get; set; }
        private string exeJavaC;
        private string exeJava;

        public clsCompilador()
        {
            exeJavaC = "C:\\Program Files\\Java\\jdk1.8.0_111\\bin\\javac.exe";
            exeJava = "C:\\Program Files\\Java\\jdk1.8.0_111\\bin\\java.exe";
        }

        public String Compilar(string pathServer)
        {
            pathFile = pathServer;

            if (String.IsNullOrEmpty(pathFile))
            {
                resultado = "Error, se requiere el path del servidor";
                return resultado;
            }
            else
            {
                //Generar path completo donde se guardará el archivo junto con su nombre y extensión
                GenerarPathFile();

                //Obtener path de trabajo (carpeta donde se encuentra el archivo .java)
                string workingDirectory = pathFile.Substring(0, pathFile.LastIndexOf("\\"));

                //Crear archivo
                if (CreateFile())
                {
                    //Ejecutar proceso de compilación con javac
                    ExecuteCompilador(workingDirectory, exeJavaC, pathFile);

                    if (String.IsNullOrEmpty(resultado) || resultado.Equals("\n"))
                    {
                        resultado = "Muy bien!! La compilación ha sido exitosa...\n" + 
                            "------------------------------------------------------------------------\n" +
                            "Resultado:\n";

                        //Ejecutar proceso con java
                        ExecuteCompilador(workingDirectory, exeJava, nameFile.Substring(0, nameFile.Length - 5));

                        DeleteFolder(workingDirectory);
                        return resultado;
                    }
                    else
                    {
                        DeleteFolder(workingDirectory);
                        return resultado;
                    }
                
                }
                else
                {
                    DeleteFolder(workingDirectory);
                    return "Error creando el archivo\n" + pathFile;
                }
            }
        }

        private void ExecuteCompilador(string workingDirectory, string FileNameExe, string Arguments)
        {
            //CrearFile variable tipo process para ejecutar .exe
            System.Diagnostics.Process oCompilar = new System.Diagnostics.Process();

            try
            {
                //Ocultar pantalla CMD y configurar ejecución
                oCompilar.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                oCompilar.StartInfo.UseShellExecute = false;
                oCompilar.StartInfo.CreateNoWindow = true;
                oCompilar.StartInfo.WorkingDirectory = workingDirectory;

                //Cambiar direccionamientos de entrada y salida de información
                oCompilar.StartInfo.RedirectStandardOutput = true;
                oCompilar.StartInfo.RedirectStandardInput = true;

                //Agregar ubicación del compilador .exe
                //Compilador MIDA: compilar.StartInfo.FileName = Server.MapPath("~/exeCompilador/javac.exe");
                oCompilar.StartInfo.FileName = FileNameExe;

                //Ubicación y nombre del archivo a compilar
                oCompilar.StartInfo.Arguments = Arguments;

                //Ejecutar compilador
                oCompilar.Start();

                //Obtener respuesta del compilador
                resultado = resultado + "\n" + oCompilar.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                resultado = resultado + "\n" + "Exception: " + ex.ToString();
            }

            //Liberar proceso
            oCompilar.Close();

            //Liberar espacio en memoria
            oCompilar = null;
        }

        private bool CreateFile()
        {
            //Crear archivo .java y agregarle el código a compilar
            File.WriteAllText(pathFile, code);
            if (File.Exists(pathFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteFile(string pathFile)
        {
            //Eliminar archivo .java
            File.Delete(pathFile);
            if (File.Exists(pathFile))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool DeleteFolder(string pathFolder)
        {
            //Obtiene listado de archivos dentro de la carpeta
            string[] files = Directory.GetFiles(pathFolder);

            //Invoca método para eliminar los archivos que están dentro de la carpeta
            for (int i = 0; i < files.Length; i++)
            {
                DeleteFile(files[i]);
            }

            //Eliminar carpeta vacia
            Directory.Delete(pathFolder);

            if (Directory.Exists(pathFolder))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void GenerarPathFile()
        {
            //Retirar ultimo slash del path
            pathFile = pathFile.Substring(0, pathFile.Length - 1);

            //Obtener la posición del último slash y ubicarse en la siguiente posición (+1)
            int LastBackSlash = pathFile.LastIndexOf("\\") + 1;

            //Crear carpeta 'codes' sino exite donde se almancenarán los archivos .java y .class
            if (!Directory.Exists(pathFile.Substring(0, LastBackSlash) + "\\codes"))
            {
                Directory.CreateDirectory(pathFile.Substring(0, LastBackSlash) + "\\codes");
            }

            //Obtener nombre de la carpeta (codeDDMMYYYYhhmmss) donde se almancenarán los archivos .java y .class 
            //(Working Directory)
            nameFolder = GetNameFolder();

            //Crear carpeta dentro de 'codes'
            Directory.CreateDirectory(pathFile.Substring(0, LastBackSlash) + "\\codes\\" + nameFolder);

            //Obtener nombre de la clase para crear archivo .java con ese nombre
            nameFile = GetNameFile(code) + ".java";

            //Regresar path donde debe crearse el archivo junto con el nombre
            pathFile = pathFile.Substring(0, LastBackSlash) + "codes\\" + nameFolder + "\\" + nameFile;

        }

        private string GetNameFile(string code)
        {
            string code2;
            string nameClass;
            int pos1, pos2;

            //Palabras clave para ubicar el nombre de la clase
            string keyWord1 = "public class";
            string keyWord2 = "{";

            //Obtener posición de keyWord1 en 'code'
            pos1 = code.IndexOf(keyWord1);

            //Obtener posición de keyWord2 en 'code'
            pos2 = code.IndexOf(keyWord2);

            //Variable auxiliar 'code2' con el texto que está entre keyWord1 y keyWord2
            code2 = code.Substring(pos1 + keyWord1.Length + 1, pos2 - (pos1 + keyWord1.Length + 1));

            //Obtener nombre de clase
            if (code2.Contains(" "))
            {
                nameClass = code2.Substring(0, code2.IndexOf(" "));
            }
            else
            {
                nameClass = code2;
            }

            return nameClass;
        }

        private string GetNameFolder()
        {
            //Construir nombre de la carpeta (code_DDMMYYYY_hhmmss)
            string nameFolder = "code_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00")
                + DateTime.Now.Year + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00")
                + DateTime.Now.Second.ToString("00");
            return nameFolder;
        }
    }
}
