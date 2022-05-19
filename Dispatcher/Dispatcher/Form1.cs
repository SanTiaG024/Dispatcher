using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//#pragma warning disable: 4996 6011 6262 26495

namespace Dispatcher
{
    public partial class Form1 : Form
    {
        Despachador despachadorDeProcesos = new Despachador();
        int numeroMicros;
        int tamañoQuantum;
        int tcc;
        int tb;
        
        public Form1()
        {	
			InitializeComponent();
        }
        private void txtBoxnNumeroMicros_TextChanged(object sender, EventArgs e)
        {
            numeroMicros = Convert.ToInt32(txtBoxnNumeroMicros.Text);
        }

        private void txtBoxTamañodeQuantum_TextChanged(object sender, EventArgs e)
        {
            tamañoQuantum = Convert.ToInt32(txtBoxTamañodeQuantum.Text);
        }

        private void txtBoxTCC_TextChanged(object sender, EventArgs e)
        {
            tcc = Convert.ToInt32(txtBoxTCC.Text);
        }

        private void txtBoxTB_TextChanged(object sender, EventArgs e)
        {
            tb = Convert.ToInt32(txtBoxTB.Text);
        }
        private void LoopThroughControls(Control parent)
        {
            
        }
        private void btnIniciar_Click(object sender, EventArgs e)
        {

            //micros[] microsref = new micros[numeroMicros];
            //for (int i = 0; i < numeroMicros; i++)
            //{
            //    microsref[i].procesos_asignados = new procesosData[17];
            //}
            Dispatcher.Despachador.microprocesadores[] microsref2 = new Dispatcher.Despachador.microprocesadores[numeroMicros];
            for (int i = 0; i < numeroMicros; i++)
            {
                microsref2[i].procesos_asignados = new Dispatcher.Despachador.datos_del_proceso[18];
            }
            despachadorDeProcesos.principal(numeroMicros, tamañoQuantum, tb, tcc, ref microsref2);

            foreach (Control c in this.tabControl1.Controls)
            {
                if (c is TabPage)
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d is DataGridView)
                        {
                            DataGridView dgv = (DataGridView)d;
                            //while(dgv.Rows.Count < microsref2[0].contador_procesos)
                            //{
                            for (int i = 0; i < microsref2[0].contador_procesos; i++)
                            {
                                int n = dgv.Rows.Add();
                                dgv.Rows[n].Cells[0].Value = microsref2[0].procesos_asignados[i].procesos;
                                dgv.Rows[n].Cells[1].Value = microsref2[0].procesos_asignados[i].t_cambioContexto;
                                dgv.Rows[n].Cells[2].Value = microsref2[0].procesos_asignados[i].tiempo_De_Ejecucion;
                                dgv.Rows[n].Cells[3].Value = microsref2[0].procesos_asignados[i].t_vencimientoQ;
                                dgv.Rows[n].Cells[4].Value = microsref2[0].procesos_asignados[i].t_bloqueo;
                                dgv.Rows[n].Cells[5].Value = microsref2[0].procesos_asignados[i].t_final;
                                dgv.Rows[n].Cells[6].Value = microsref2[0].procesos_asignados[i].t_inicial;
                                dgv.Rows[n].Cells[7].Value = microsref2[0].procesos_asignados[i].t_final + microsref2[0].procesos_asignados[i].t_inicial;
                            }
                            for (int i = 0; i < numeroMicros; i++)
                                 microsref2[i] = microsref2[i + 1];
                            numeroMicros--;
                           
                        }
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
