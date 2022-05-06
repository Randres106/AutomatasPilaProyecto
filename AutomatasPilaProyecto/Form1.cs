using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatasPilaProyecto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string TextoCompleto = null;
            string[] TextoSeparado;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = openFileDialog1.FileName;
                TbArchivo.Text = File.ReadAllText(openFileDialog1.FileName);

                TextoCompleto = TbArchivo.Text;
                TextoSeparado = TextoCompleto.Split("\r\n");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stack<string> Pila = new Stack<string>();
            List<string> Estados = new List<string>();
            List<Nodos> Reglas = new List<Nodos>();

            int IndiceEstado = 0;
            int IndiceCinta = 0;
            string TextoTxT = TbArchivo.Text;
            string cadena = textBox1.Text;
            string[] TextoSeparado = TextoTxT.Split("\r\n");
            IndiceEstado = Convert.ToInt32(TextoSeparado[1]);

            for (int i = 3; i < TextoSeparado.Length; i++)
            {
                Nodos Nuevo = new Nodos();
                string[] Temporal = TextoSeparado[i].Split(",");
                Estados.Add(Temporal[0]);
                Estados.Add(Temporal[4]);

                Nuevo.EstadoInicia = Temporal[0];
                Nuevo.Lectura = Temporal[1];
                Nuevo.Desapila = Temporal[2];
                Nuevo.Apila = Temporal[3];
                Nuevo.EstadoTermina = Temporal[4];
                Reglas.Add(Nuevo);
            }

            string[] IndicadorEstados = Estados.Distinct().ToArray();
            char[] CintaEntrada = cadena.ToArray();
            string[] EstadosFinales = TextoSeparado[2].Split(",");
            int p = Pila.Count;
            while (IndiceCinta<CintaEntrada.Length)
            {
                string Aux = CintaEntrada[IndiceCinta].ToString();
                string Estado = IndicadorEstados[IndiceEstado-1];
                List<Nodos> ReglasAceptadas = Reglas.FindAll(x => x.EstadoInicia.Contains(Estado));
                if (ReglasAceptadas.Count != 0)
                {
                    Nodos Resultante = ReglasAceptadas.Find(x => x.Lectura.Contains(Aux));
                    if (Resultante != null)
                    {
                        if (Resultante.Apila != "")
                        {
                            Pila.Push(Resultante.Apila);
                        }
                        if (Resultante.Desapila != "") 
                        {
                            string letra = Pila.Pop();
                            if (Resultante.Desapila != letra) 
                            {
                                Pila.Push(letra);
                            }
                        }
                        IndiceCinta++;
                        IndiceEstado = Convert.ToInt32(Resultante.EstadoTermina);
                    }
                    else 
                    {
                        Nodos ResultanteVacio = ReglasAceptadas.Find(x => x.Lectura.Contains(""));
                        if (ResultanteVacio != null)
                        {
                            if (Resultante.Apila != "")
                            {
                                Pila.Push(Resultante.Apila);
                            }
                            if (Resultante.Desapila != "")
                            {
                                string letra = Pila.Pop();
                                if (Resultante.Desapila != letra)
                                {
                                    Pila.Push(letra);
                                }
                            }
                            IndiceCinta++;
                            IndiceEstado = Convert.ToInt32(Resultante.EstadoTermina);
                        }
                        else 
                        {
                            //Automata mal ingresado
                        }
                    }
                }
                else 
                {
                    //Automata mal ingresado
                }

            }
            //verifica si ya puede salir
            if (Pila.Count==0)
            {
                bool verificar = false;
                for (int i = 0; i < EstadosFinales.Length; i++)
                {
                    if (EstadosFinales[i]==IndiceEstado.ToString())
                    {
                        verificar = true;
                        i = EstadosFinales.Length;
                    }
                }
                if (verificar)
                {
                    //si lo acepta
                }
                else
                {
                    //nachos
                }
            }
        }
    }

    public class Nodos 
    {
        public string Lectura { get; set;}
        public string Desapila { get; set; }
        public string Apila { get; set; }
        public string EstadoInicia{ get; set; }
        public string EstadoTermina { get; set; }

    }
}
