using System;
using System.IO;
using ctrlCompilador;


namespace webCompilador
{
    public partial class Compilador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCompilar_Click(object sender, EventArgs e)
        {
            string code = Request.Form["code"].ToString();

            if (String.IsNullOrEmpty(code))
            {
                resultado.Text = "Error: antes de compilar debe insertar su código";
            }
            else
            {
                //Obtener ruta de Servidor
                string pathServer = Server.MapPath("~");

                //Crear objento de la clase clsCompilador
                clsCompilador oCompilar = new clsCompilador();

                oCompilar.code = code;

                resultado.Text = oCompilar.Compilar(pathServer);

                /*
                //Crear archivo en el servidor agregando el código a compilar
                CrearFile(pathFileCode, code);

                //Invocar compilador y obtener respuesta
                resultado.Text = Compilar(pathFileCode);

                //Borra archivo creado del servidor
                //BorrarFile(pathFileCode);
                */
            }

        }

        private String GenerarPathFile(string code)
        {
            //Obtener la URL del servidor
            string pathFile = Server.MapPath("~");

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
            string nameFolder = GetNameFolder();

            //Crear carpeta dentro de 'codes'
            Directory.CreateDirectory(pathFile.Substring(0, LastBackSlash) + "\\codes\\" + nameFolder);

            //Obtener nombre de la clase para crear archivo .java con ese nombre
            string nameFile = GetNameFile(code);

            //Regresar path donde debe crearse el archivo junto con el nombre
            pathFile = pathFile.Substring(0, LastBackSlash) + "\\codes\\" + nameFolder + "\\" + nameFile + ".java";

            return pathFile;
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

            //Crear variable auxiliar 'code2' con el texto que está entre keyWord1 y keyWord2
            code2 = code.Substring(pos1 + keyWord1.Length + 1, pos2 - (pos1 + keyWord1.Length + 1));

            //Obtener nombre de clase: pos 0 de code2 hasta el primer espacio ' '
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

        private String Compilar(string pathFile)
        {
            string workingDirectory = pathFile.Substring(0, pathFile.LastIndexOf("\\"));
            try
            {
                //CrearFile variable tipo process para ejecutar .exe
                System.Diagnostics.Process compilar = new System.Diagnostics.Process();

                //Ocultar pantalla CMD y configurar ejecución
                //compilar.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                compilar.StartInfo.UseShellExecute = false;
                compilar.StartInfo.CreateNoWindow = true;
                compilar.StartInfo.WorkingDirectory = workingDirectory;

                //
                compilar.StartInfo.RedirectStandardOutput = true;
                compilar.StartInfo.RedirectStandardInput = true;

                //Agregar ubicación del compilador .exe
                //compilar.StartInfo.FileName = Server.MapPath("~/exeCompilador/javac.exe");
                compilar.StartInfo.FileName = "C:\\Program Files\\Java\\jdk1.8.0_111\\bin\\javac.exe";

                //Ubicación y nombre del archivo a compilar
                compilar.StartInfo.Arguments = pathFile;

                //Invocar compilador
                compilar.Start();

                //Obtener respuesta del compilador
                string resultado = compilar.StandardOutput.ReadToEnd();

                if (String.IsNullOrEmpty(resultado))
                {
                    resultado = "Muy bien!! La compilación ha sido exitosa";
                    
                    //Usar java para leer los resultados de la compilación
                    compilar.StartInfo.FileName = "C:\\Program Files\\Java\\jdk1.8.0_111\\bin\\java.exe";

                    //Ubicación y nombre del archivo .class con los resultados
                    compilar.StartInfo.Arguments = pathFile.Substring(0, pathFile.Length - 4);

                    //Invocar java
                    compilar.Start();

                    //Obtener resultados de java
                    resultado = resultado + compilar.StandardOutput.ReadToEnd();
                }

                //Libera proceso
                compilar.Close();

                //Liberar espacio en memoria
                compilar = null;

                return resultado;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.ToString();
            }
        }
        private void CrearFile(string pathFile, string code)
        {
            //Crear archivo .java y agregarle el código a compilar
            File.WriteAllText(pathFile, code);
        }

        private void BorrarFile(string pathFile)
        {
            //Eliminar archivo .java
            File.Delete(pathFile);
        }
    }
}