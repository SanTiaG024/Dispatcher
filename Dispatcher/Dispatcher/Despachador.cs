using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Dispatcher
{

	public class Despachador
	{
		public struct datos_del_proceso
		{
			public string procesos; // Nombre del proceso
			public int tiempo_De_Ejecucion; //Tiempo de ejecucion del proceso
			public int numero_De_Bloqueos; //Número de bloqueos del proceso
			public int tiempo_lista_listos; 
			public int t_final;
			public int t_inicial;
			public int t_vencimientoQ;
			public int t_bloqueo;
			public int t_cambioContexto;
		};
		public struct microprocesadores
		{
			public int identificador_del_micro;
			public int contador_procesos;
			public int t_acumulado;
			public datos_del_proceso[] procesos_asignados;

		};


		const int MAX_Numero_Procesos = 150;
		const int MAX_Numero_Micros = 17;
		datos_del_proceso[] procesos_Archivo = new datos_del_proceso[MAX_Numero_Procesos];
		microprocesadores[] micros = new microprocesadores[MAX_Numero_Micros]; 
		int cantidad_de_procesos;
		int tiempo_Quantum;
		int numero_de_Micros;
		int tiempo_Bloqueo;
		int tiempo_cambio_De_Contexto;
		

		public bool lee_datos()
		{
			try
            {
				StreamReader sr = new StreamReader("newdata.txt");
				string linea;
				cantidad_de_procesos = 0;
				while ((linea = sr.ReadLine()) != null)
				{
					string[] substring = linea.Split(',');
					if (string.IsNullOrEmpty(linea) || (substring.Length != 4))
						return false;
					else
                    {
						procesos_Archivo[cantidad_de_procesos].procesos = substring[0];
						procesos_Archivo[cantidad_de_procesos].tiempo_De_Ejecucion = System.Convert.ToInt32(substring[1]);
						procesos_Archivo[cantidad_de_procesos].numero_De_Bloqueos = System.Convert.ToInt32(substring[2]);
						procesos_Archivo[cantidad_de_procesos++].tiempo_lista_listos = System.Convert.ToInt32(substring[3]);
			
					}
				}
				sr.Close();
				return true;
			}
			catch(FileNotFoundException)
            {
				return false;
            }
		}
		public bool principal(int numero_de_Micros,int tiempo_Quantum,int tiempo_Bloqueo, int tiempo_cambio_De_Contexto, ref microprocesadores[] dataPanel)
		{
			lee_datos();
            this.numero_de_Micros = numero_de_Micros;
			this.tiempo_Quantum = tiempo_Quantum;
			this.tiempo_Bloqueo = tiempo_Bloqueo;
			this.tiempo_cambio_De_Contexto = tiempo_cambio_De_Contexto;
			calculo_tiempoBloqueo();
			calculo_tiempoVencimientoQuantum();
			datos_del_proceso auxiliar = new datos_del_proceso();
			for (int i = 0; i < numero_de_Micros; i++)
			{
				
				micros[i].identificador_del_micro = i + 1;
				micros[i].procesos_asignados = new datos_del_proceso[MAX_Numero_Procesos];

			}
			while (cantidad_de_procesos != 0)
			{
				auxiliar = procesos_Archivo[0];
				ordena_microprocesadores_porTiempoAcumulado(); 
				devuelve_tiempo_CambiodeContextoyFinal(ref auxiliar);
				micros[0].procesos_asignados[micros[0].contador_procesos++] = auxiliar;
				micros[0].t_acumulado += auxiliar.t_final;
				for (int i = 0; i < numero_de_Micros; i++)
				{
					if (procesos_Archivo[1].tiempo_lista_listos > micros[i].t_acumulado)
					{
						ordena_microprocesadores_porIndice(); 
						if (cantidad_de_procesos > 2)
                        {
							micros[i].t_acumulado = procesos_Archivo[1].tiempo_lista_listos;
						}
					}
				}
				elimina_proceso_delaLista(0);
			}
			dataPanel = micros;
			return true;
		}
		public void devuelve_tiempo_CambiodeContextoyFinal(ref datos_del_proceso proceso)
		{
			if (micros[0].contador_procesos == 0)
			{
				proceso.t_cambioContexto = 0;
				if (proceso.tiempo_lista_listos == micros[0].t_acumulado)
					proceso.t_inicial = proceso.tiempo_lista_listos;
				else
					proceso.t_inicial = 0;
				proceso.t_final = proceso.tiempo_De_Ejecucion + proceso.t_bloqueo +
					proceso.t_vencimientoQ + proceso.t_cambioContexto;

			}
			else if ((proceso.tiempo_lista_listos >= micros[0].t_acumulado))
			{
				proceso.t_cambioContexto = 0;
				proceso.t_inicial = proceso.tiempo_lista_listos;
				micros[0].t_acumulado = proceso.tiempo_lista_listos;
				proceso.t_final = proceso.tiempo_De_Ejecucion + proceso.t_bloqueo +
					proceso.t_vencimientoQ + proceso.t_cambioContexto;
			}
			else
			{
				proceso.t_cambioContexto = tiempo_cambio_De_Contexto;
				proceso.t_inicial = micros[0].t_acumulado;
				proceso.t_final = proceso.tiempo_De_Ejecucion + proceso.t_bloqueo +
					proceso.t_vencimientoQ + proceso.t_cambioContexto;
			}



		}
		public bool ordena_microprocesadores_porTiempoAcumulado()
		{
			microprocesadores auxiliar = new microprocesadores();
			if (micros[0].t_acumulado == 0)
				return true;
			else
				for (int j = 0; j < numero_de_Micros - 1; j++)
				{
					if (micros[j].t_acumulado > micros[j + 1].t_acumulado)
					{
						auxiliar = micros[j];
						micros[j] = micros[j + 1];
						micros[j + 1] = auxiliar;
					}
				}
			return true;
		}
		public bool ordena_microprocesadores_porIndice()
		{
			microprocesadores auxiliar= new microprocesadores();
			if (micros[0].identificador_del_micro == 0)
				return true;
			else
			{
				for (int i = 0; i < numero_de_Micros - 1; i++)
				{
					for (int j = 0; j < numero_de_Micros - 1; j++)
					{
						if (micros[j].identificador_del_micro > micros[j + 1].identificador_del_micro)
						{
							auxiliar = micros[j];
							micros[j] = micros[j + 1];
							micros[j + 1] = auxiliar;
						}
					}
				}
			}
			return true;
		}

		public void elimina_proceso_delaLista(int indice_a_eliminar)
		{
			for (int i = indice_a_eliminar; i < cantidad_de_procesos - 1; i++)
				procesos_Archivo[i] = procesos_Archivo[i + 1];
			cantidad_de_procesos--;
		}
		public void calculo_tiempoBloqueo()
		{
			for (int i = 0; i < cantidad_de_procesos; i++)
			{
				procesos_Archivo[i].t_bloqueo = procesos_Archivo[i].numero_De_Bloqueos * tiempo_Bloqueo;
			}
		}
		public void calculo_tiempoVencimientoQuantum()
		{
			int aux = 0;
			for (int i = 0; i < cantidad_de_procesos; i++)
			{
				if (procesos_Archivo[i].tiempo_De_Ejecucion > tiempo_Quantum)
				{
					aux = (procesos_Archivo[i].tiempo_De_Ejecucion / tiempo_Quantum) - 1;
					procesos_Archivo[i].t_vencimientoQ = aux * tiempo_cambio_De_Contexto;
				}
				else
					procesos_Archivo[i].t_vencimientoQ = 0;
			}
		}
		public void imprime_en_interfaz()
        {
			
        }
	}
}
