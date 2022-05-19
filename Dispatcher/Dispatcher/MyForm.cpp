#include "MyForm.h"
#include <malloc.h>
#include <stdio.h>
#include "Despachador.h"
using namespace Dispatcher;
#pragma warning(disable:4996 6011 6262)

int main(int argc, char* argv[])
{
	Despachador d;
	bool resultado, asignacion, flag;
	resultado = d.lee_datos("data.txt");
	asignacion = d.principal(4, 3000, 15, 15);
	flag = d.imprimeresultados();
	Application::Run(gcnew MyForm);
	return(0);
}
