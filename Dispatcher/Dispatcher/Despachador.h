#pragma once
#define MAX_CHAR_NOMBRE 25
#define MAX_Numero_Procesos 150
#define MAX_Numero_Micros 17

#pragma warning(disable:4996 6011 6262 26495)

struct datos_del_proceso
{
	char proceso[MAX_CHAR_NOMBRE]; // Nombre del proceso
	unsigned int tiempo_De_Ejecucion; //Tiempo de ejecucion del proceso
	unsigned int numero_De_Bloqueos; //Número de bloqueos del proceso
	unsigned int tiempo_lista_listos;
	unsigned int t_final;
	unsigned int t_inicial;
	unsigned int t_vencimientoQ;
	unsigned int t_bloqueo;
	unsigned int t_cambioContexto;
};

struct microprocesadores
{
	unsigned int identificador_del_micro;
	unsigned int contador_procesos;
	unsigned int t_acumulado;
	datos_del_proceso procesos_asignados[MAX_Numero_Procesos];
};




class Despachador
{
public:
	Despachador();
	~Despachador();
	bool lee_datos(const char* nombre_archivo);
	bool imprimeresultados();
	bool principal(unsigned int numero_de_Micros, unsigned int tiempo_Quantum, unsigned int tiempo_Bloqueo, unsigned int tiempo_cambio_De_Contexto);
private:
	void calculo_tiempoBloqueo();
	void calculo_tiempoVencimientoQuantum();
	void elimina_proceso_delaLista(unsigned int indice_a_eliminar);
	void devuelve_tiempo_CambiodeContextoyFinal(datos_del_proceso& proceso);
	bool ordena_microprocesadores_porTiempoAcumulado();
	bool ordena_microprocesadores_porIndice();
	datos_del_proceso procesos[MAX_Numero_Procesos];
	unsigned int cantidad_de_procesos;
	microprocesadores micros[MAX_Numero_Micros];
	unsigned int tiempo_Quantum;
	unsigned int numero_de_Micros;
	unsigned int tiempo_Bloqueo;
	unsigned int tiempo_cambio_De_Contexto;
};

