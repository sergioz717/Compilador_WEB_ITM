using System;
using System.IO;


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

            string pathFileMIDA = GenerarPathFile();

            CrearFile(pathFileMIDA, code);

            resultado.Text = Compilar(pathFileMIDA);

            BorrarFile(pathFileMIDA);

        }

        private String GenerarPathFile()
        {
            string pathFile = Server.MapPath("~");

            pathFile = pathFile.Substring(0, pathFile.Length - 1);

            int LastBackSlash = pathFile.LastIndexOf("\\") + 1;

            string nameFile = "code" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + 
                DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".mida";

            return pathFile.Substring(0, LastBackSlash) + nameFile;
        }

        private String Compilar(string pathFile)
        {
            try
            {
                System.Diagnostics.Process compilar = new System.Diagnostics.Process();
                compilar.StartInfo.FileName = Server.MapPath("~/exeCompilador/Compilador.exe");
                compilar.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                compilar.StartInfo.UseShellExecute = false;
                compilar.StartInfo.RedirectStandardOutput = true;
                compilar.StartInfo.CreateNoWindow = false;

                compilar.StartInfo.Arguments = pathFile;

                compilar.Start();

                string resultado = compilar.StandardOutput.ReadToEnd();

                compilar.Close();
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
            File.WriteAllText(pathFile, code);
        }

        private void BorrarFile(string pathFile)
        {
            File.Delete(pathFile);
        }
    }
}