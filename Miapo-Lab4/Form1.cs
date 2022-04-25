using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miapo_Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> inputParams = GetInputParams();
            List<string> outputParams = GetOutputParams();

            if (Conus.Generate(inputParams, outputParams))
            {
                using (TextReader reader = File.OpenText(Conus.FILENAME))
                {
                    string code = reader.ReadToEnd();
                    StartProgramCode(code);
                }
            }
            else
            {
                MessageBox.Show("Программный код не был сгенерирован, так как недостаточно исходных дан-ных.. ", "Недостаточно данных",
                    MessageBoxButtons.OK);
            }

        }

        private List<string> GetInputParams()
        {
            List<string> inputParams = new List<string>();

            if (checkBox1L.Checked)
            {
                inputParams.Add("L");
            }

            if (checkBox2h.Checked)
            {
                inputParams.Add("h");
            }

            if (checkBox3r.Checked)
            {
                inputParams.Add("r");
            }

            if (checkBox4a.Checked)
            {
                inputParams.Add("alfa");
            }

            if (checkBox5so.Checked)
            {
                inputParams.Add("So");
            }

            if (checkBox6sb.Checked)
            {
                inputParams.Add("Sb");
            }

            if (checkBox7sp.Checked)
            {
                inputParams.Add("Sp");
            }

            if (checkBox8v.Checked)
            {
                inputParams.Add("V");
            }

            if (checkBox9d.Checked)
            {
                inputParams.Add("d");
            }

            return inputParams;
        }

        private List<string> GetOutputParams()
        {
            List<string> outputParams = new List<string>();

            if (checkBox1L1.Checked)
            {
                outputParams.Add("L");
            }

            if (checkBox2h2.Checked)
            {
                outputParams.Add("h");
            }

            if (checkBox3r3.Checked)
            {
                outputParams.Add("r");
            }

            if (checkBox4a4.Checked)
            {
                outputParams.Add("alfa");
            }

            if (checkBox5so5.Checked)
            {
                outputParams.Add("So");
            }

            if (checkBox6sb6.Checked)
            {
                outputParams.Add("Sb");
            }

            if (checkBox7sp7.Checked)
            {
                outputParams.Add("Sp");
            }

            if (checkBox8v8.Checked)
            {
                outputParams.Add("V");
            }

            if (checkBox9d9.Checked)
            {
                outputParams.Add("d");
            }

            return outputParams;
        }

        private void StartProgramCode(string code)
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string> { { "Compil-erVersion", "v3.5" } };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters { OutputAssembly = "Program.EXE", GenerateExecutable = true };
            compilerParams.ReferencedAssemblies.Add("System.Core.Dll");

            // Компиляция
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, code);

            // Открываем программу
            Process.Start("Program.EXE");
        }
        
    }

}

