﻿using System;
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
            List<string> CadEstados = new List<string>();
            List<Nodos> Reglas = new List<Nodos>();
            List<Configuraciones> ListaConfig = new List<Configuraciones>();

            int IndiceEstado = 0;
            int IndiceCinta = 0;
            string TextoTxT = TbArchivo.Text;
            string cadena = textBox1.Text;
            string[] TextoSeparado = TextoTxT.Split("\r\n");
            IndiceEstado = Convert.ToInt32(TextoSeparado[1]);
            bool ParImpar = false;
            if ((cadena.Length % 2) == 0)
            {
                ParImpar = true;
            }
            else
            {
                ParImpar = false;
            }

            for (int i = 3; i < TextoSeparado.Length; i++)
            {
                Nodos Nuevo = new Nodos();
                string[] Temporal = TextoSeparado[i].Split(",");
                Estados.Add(Temporal[0]);
                Estados.Add(Temporal[4]);
                if (Temporal[1]=="")
                {
                    Temporal[1] = " ";
                }
                if (Temporal[2] == "")
                {
                    Temporal[2] = " ";
                }
                if (Temporal[3] == "")
                {
                    Temporal[3] = " ";
                }
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
            string CadenaEstados;

            while (IndiceCinta<CintaEntrada.Length)
            {
                Configuraciones Nuevo= new Configuraciones(); 
                string Aux = CintaEntrada[IndiceCinta].ToString();
                string Estado = IndicadorEstados[IndiceEstado-1];
                List<Nodos> ReglasAceptadas = Reglas.FindAll(x => x.EstadoInicia.Contains(Estado));
                Nuevo.Estado = Estado;

                if (ReglasAceptadas.Count != 0)
                {
                    Nodos Resultante = ReglasAceptadas.Find(x => x.Lectura.Contains(Aux));
                    if (CintaEntrada.Length/2==IndiceCinta)
                    {
                        Nodos Mitad = ReglasAceptadas.Find(x => x.Lectura.Contains(" "));
                        if (Mitad != null)
                        {
                            Resultante = Mitad;
                            IndiceCinta--;
                        }
                    }
                    if (Resultante != null)
                    {
                        if (Resultante.Desapila != " ") 
                        {
                            if (Pila.Count!=0)
                            {
                                string letra = Pila.Pop();
                                if (Resultante.Desapila != letra)
                                {
                                    Pila.Push(letra);
                                    Nodos AuxReglas = ReglasAceptadas.Find(x => x.Desapila.Contains(letra));
                                    Resultante = AuxReglas;
                                    if (Resultante.Desapila == letra)
                                    {
                                        Pila.Pop();
                                    }
                                }
                            }
                            else
                            {
                                IndiceCinta = CintaEntrada.Length;
                            }
                            
                        }
                        if (Resultante.Apila != " ")
                        {
                            Pila.Push(Resultante.Apila);
                        }
                        IndiceCinta++;
                        IndiceEstado = Convert.ToInt32(Resultante.EstadoTermina);
                    }
                    else 
                    {
                        Nodos ResultanteVacio = ReglasAceptadas.Find(x => x.Lectura.Contains(" "));
                        if (ResultanteVacio != null)
                        {
                            if (ResultanteVacio.Apila != " ")
                            {
                                Pila.Push(ResultanteVacio.Apila);
                            }
                            if (ResultanteVacio.Desapila != " ")
                            {
                                string letra = Pila.Pop();
                                if (ResultanteVacio.Desapila != letra)
                                {
                                    Pila.Push(letra);
                                }
                            }
                            IndiceEstado = Convert.ToInt32(ResultanteVacio.EstadoTermina);
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
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = ListaConfig;
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

    public class Configuraciones
    {
        public string Estado { get; set; }
        public string Cadena { get; set; }
        public string Pila { get; set; }

    }
}
