using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


// Namespace donde se encuentra todo el proyecto
namespace DistribucionFuenteVariable
{
    // Definicion de la clase general
    public partial class Form1 : Form
    {
        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/
        /*                                                          CONSTANTES                                                                      */
        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/

        // Tamanio del header inicial
        private const byte TOTAL_DE_BYTES__HEADER_INICIAL = 3;

        // Tamanio del header final
        private const byte TOTAL_DE_BYTES__HEADER_FINAL = 3;

        // Total de bytes que estan presentes en todas las tramas de datos ( 3 Headers + 1 Comando + 1 Informacion + 3 Headers )
        private const byte TOTAL_DE_DATOS_BASICOS = 8;

        // Ubicacion del byte que indica el total de datos adicionales que se envian en las tramas de datos
        private const byte UBICACION_DEL_BYTE_DE_INFORMACION = 4;


        // ---------------------------------------------------------------------------- //
        // ----------- CANTIDAD DE BYTES DE INFORMACION PARA LOS COMANDOS ------------- //
        // ---------------------------------------------------------------------------- //

        // VERIFICAR SIEMPRE QUE SEA EL ULTIMO COMANDO DEL ENUM "_comandos_recibidos" PARA QUE LAS DIMENIONES SEAN CORRECTAS
        private const byte TOTAL_COMANDOS_PARA__RECIBIR = 6;




        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //
        // ------------------------------ ENUMERACIONES ------------------------------- //
        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //


        // ---------------------------------------------------------------------------- //
        // ------------------ BLOQUES DE ANALISIS EN LAS TRAMAS ----------------------- //
        // ---------------------------------------------------------------------------- //

        enum bloques_de_recepcion
        {
            RECEPCION_BLOQUE_DE_HEADER_INICIAL = 0,
            RECEPCION_BLOQUE_DE_TOTAL_DE_BYTES,
            RECEPCION_BLOQUE_DE_IDENTIDICADOR_DEL_COMANDO,
            RECEPCION_BLOQUE_DE_PARAMETROS_ADICIONALES,
            RECEPCION_BLOQUE_DE_HEADER_FINAL
        };

        enum posiciones_dentro_del_vector_recibido : byte
        {
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__ID_DE_LA_PLACA = 0,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__ID_DEL_COMANDO,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__TOTAL_DE_BYTES,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_2,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_3,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_4,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_5,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_6,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_7,
            POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_8
        };

        enum comandos_recibidos : byte
        {
            COMANDO_RECIBIDO__DATOS_DE_LAS_MEDICIONES = 0x00
        };

        enum comandos_enviados : byte
        {
            COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION = 0x80
        };




        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/
        /*                                                          VARIABLES                                                                       */
        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/

        /* VARIABLES PARA MANEJAR LA COMUNICACION SERIE */

        // Objeto para manejar el puerto serie
        private const int tasaPuertoSerie = 9600;
        private const int bitsDeDatos = 8;
        private SerialPort puertoSerie = new SerialPort("COM9", tasaPuertoSerie, Parity.None, bitsDeDatos, StopBits.One);

        // Variable para indicar el error que surja
        private string Mensaje;

        // Variables para los headers de la comunicacion
        private byte[] HeaderInicial = { 195, 62, 180 };
        private byte[] HeaderFinal = { 204, 174, 185 };

        // Se genera un buffer auxiliar para almacenar los datos del comando recibido
        private byte[] ComandoRecibido = new byte[40];

        // Trama para almacenar los datos a enviar
        private byte[] tramaParaEnviar = new byte[40];

        // Trama para almacenar los datos recibidos
        private byte[] tramaRecibida = new byte[40];

        // Contador de la cantidad de bytes a enviar
        private byte totalDeBytesParaEnviar;

        // Contador de la cantidad de bytes recibidos
        private int totalDeBytesRecibidos;

        // Contador de la cantidad total de bytes por recibir en la trama
        private int totalDeBytesPorRecibir;

        // Contador de la cantidad de bytes recibidos en cada trama en particular
        private int totalDeBytesRecibidosEnEstaTrama;

        // Variable para ejecutar el Thread de recepcion de comandos y enviar los pulsos generados
        Thread threadLecturaPuertoSerie_Universal;

        // Flag para controlar la ejecucion del Thread de recepcion de comandos
        bool threadLecturaPuertoSerieIniciado_Universal = false;

        // Variable para controlar un cierre temprano del Thread
        private bool terminarThreadDeRecepcionSerie_Universal;

        // Variable para almacenar el parametro "I_HImFU"
        private double I_HImFU;

        // Variable para almacenar el parametro "I_Peltier_Crio"
        private double I_Peltier_Crio;

        // Variable para almacenar el parametro "I_Peltier_Vacio"
        private double I_Peltier_Vacio;

        // Variable para almacenar el parametro "I_V_RF"
        private double I_V_RF;

        // Variable para almacenar el parametro "Vout_Var"
        private double Vout_Var;





        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/
        /*                                                  CONSTRUCTOR DEL FORMULARIO                                                              */
        /********************************************************************************************************************************************/
        /********************************************************************************************************************************************/
        public Form1()
        {
            // Se inicializa el formulario y todos los elementos graficos
            InitializeComponent();

            // Se centra el formulario en la pantalla
            this.StartPosition = FormStartPosition.CenterScreen;

            // Para poder controlar los objetos graficos del formulario desde otros Threads sin usar delegados
            CheckForIllegalCrossThreadCalls = false;

            // Se habilita el boton para abrir el puerto serie
            btnAbrirPuerto.Enabled = true;

            // Se deshabilita el boton para cerrar el puerto serie
            btnCerrarPuerto.Enabled = false;

            // Se deshabilita el boton para iniciar el envio de datos desde la placa
            btnRecibirDatos.Enabled = false;

            // Se deshabilita el boton para detener el envio de datos desde la placa
            btnDetenerDatos.Enabled = false;

            // Se marca el flag para que el Thread se ejecute correctamente
            terminarThreadDeRecepcionSerie_Universal = false;
        }


        // Funcion encargada de revisar que se devuelvan todos los recursos cuando se cierra la aplicación
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Se verifica si el puerto estaba abierto
            if (puertoSerie.IsOpen)
            {
                // Si el puerto estaba abierto, se lo debe cerrar
                try
                {
                    puertoSerie.Close();                // Se cierra el puerto
                }
                catch (IOException)
                {
                    Mensaje = "Ocurrio un error al cerrar el puerto serie - TerminarComunicacionSerie";
                    MessageBox.Show(Mensaje);
                }
            }

            // Se verifica si se inicio el Thread para cancelarlo
            if (threadLecturaPuertoSerieIniciado_Universal == true)
            {
                // Se cancela el thread de lectura del puerto serie
                try { threadLecturaPuertoSerie_Universal.Abort(); }
                catch (ThreadAbortException) { };
            }
        }





        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //
        // --------------------------- ELEMENTOS GRAFICOS ----------------------------- //
        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //



        // ---------------------------------------------------------------------------- //
        // ------------------------------- BOTONES ------------------------------------ //
        // ---------------------------------------------------------------------------- //


        // Handler de atencion para el evento "Click" sobre el elemento "Boton para abrir el puerto serie"
        private void btnAbrirPuerto_Click(object sender, EventArgs e)
        {
            // Se verifica el estado del puerto para saber la accion a realizar
            if (puertoSerie.IsOpen)
            {
                // Si el puerto estaba abierto, se lo debe cerrar
                try
                {
                    puertoSerie.Close();                                // Se cierra el puerto
                    lblEstadoDelPuerto.Text = "Cerrado";           // Se indica que se cerro correctamente el puerto
                    btnAbrirPuerto.Enabled = true;                      // Se habilita el boton para abrir el puerto
                    btnCerrarPuerto.Enabled = false;                    // Se deshabilita el boton para abrir el puerto
                    btnRecibirDatos.Enabled = false;                    // Se deshabilita el boton para comenzar a recibir los datos

                }
                catch (IOException)
                {
                    Mensaje = "Ocurrio un error al cerrar el puerto serie - TerminarComunicacionSerie";
                    MessageBox.Show(Mensaje);
                }
            }
            else
            {
                // Se crea el formulario para seleccionar el puerto serie donde este conectada la placa
                PuertoSerie formularioConfiguracionPuertoSerie = new PuertoSerie();

                // Se lanza a ejecucion el formulario, y se capta la devolucion, para saber si se elegio correctamente un valor
                if (formularioConfiguracionPuertoSerie.ShowDialog() == DialogResult.OK)
                {
                    // Se intenta abrir el puerto
                    try
                    {
                        puertoSerie.PortName = formularioConfiguracionPuertoSerie.nombreDelPuerto;     // Se configura el nombre del puerto
                        puertoSerie.ReadTimeout = 1000;                                                // Se establece un timeout de 1 segundo para intentar abrir el puerto
                        puertoSerie.Open();                                                            // Se abre el puerto
                        Mensaje = "Puerto abierto correctamente";                                      // Se carga un mensaje de exito en la apertura
                        btnAbrirPuerto.Enabled = false;                                                // Se deshabilita el boton para abrir el puerto
                        btnCerrarPuerto.Enabled = true;                                                // Se habilita el boton para abrir el puerto
                        btnRecibirDatos.Enabled = true;                                                // Se habilita el boton para comenzar a recibir los datos
                        lblEstadoDelPuerto.Text = "Abierto";                                           // Se indica que se abrio correctamente el puerto
                    }

                    catch (UnauthorizedAccessException)     // Open
                    {
                        Mensaje = "No se tienen permisos para abrir el puerto serie - EstablecerComunicacion";
                    }

                    catch (System.IO.IOException)
                    {
                        Mensaje = "Error al intentar abrir el puerto serie - EstablecerComunicacion";
                    }

                    catch (ArgumentException)               // PortName
                    {
                        Mensaje = "El nombre suministrado para el puerto no es correcto - EstablecerComunicacion";
                    }

                    catch (InvalidOperationException)       // PortName
                    {
                        if (puertoSerie.IsOpen)
                        {
                            Mensaje = "El puerto ya esta en uso - EstablecerComunicacion";
                        }
                        else
                        {
                            Mensaje = "No se puede asignar el nombre al puerto - EstablecerComunicacion";
                        }
                    }

                    MessageBox.Show(Mensaje);
                }
            }
        }


        // Handler de atencion para el evento "Click" sobre el elemento "Boton para cerrar el puerto serie"
        private void btnCerrarPuerto_Click(object sender, EventArgs e)
        {
            // Se verifica el estado del puerto para saber la accion a realizar
            if (puertoSerie.IsOpen)
            {
                // Si el puerto estaba abierto, se lo debe cerrar
                try
                {
                    puertoSerie.Close();                                // Se cierra el puerto
                    lblEstadoDelPuerto.Text = "Cerrado";           // Se indica que se cerro correctamente el puerto
                    btnAbrirPuerto.Enabled = true;                      // Se habilita el boton para abrir el puerto
                    btnCerrarPuerto.Enabled = false;                    // Se deshabilita el boton para abrir el puerto
                    btnRecibirDatos.Enabled = false;                    // Se deshabilita el boton para comenzar a recibir los datos

                }
                catch (IOException)
                {
                    Mensaje = "Ocurrio un error al cerrar el puerto serie - TerminarComunicacionSerie";
                    MessageBox.Show(Mensaje);
                }
            }
        }


        // Handler de atencion para el evento "Click" sobre el elemento "Boton para iniciar el envio de datos desde la placa"
        private void btnRecibirDatos_Click(object sender, EventArgs e)
        {
            // Se debe verificar si no se esta ejecutando el Thread de recepcion
            if(threadLecturaPuertoSerieIniciado_Universal == false)
            {
                // Se deshabilita el boton para volver a lanzar el Thread de recepcion
                btnRecibirDatos.Enabled = false;

                // Se habilita el boton para detener el envio de datos
                btnDetenerDatos.Enabled = true;

                // Se marca el flag para que el Thread se ejecute correctamente
                terminarThreadDeRecepcionSerie_Universal = false;

                // Se asigna la funcion del Thread para la recepcion de datos y se lo lanza a ejecucion
                threadLecturaPuertoSerie_Universal = new Thread(new ThreadStart(funcionThreadLecturaPuertoSerie_Universal));

                // Se lanza el Thread a ejecucion
                threadLecturaPuertoSerie_Universal.Start();

                // Se marca el flag para indicar que el Thread esta activo
                threadLecturaPuertoSerieIniciado_Universal = true;
            }

        }


        // Handler de atencion para el evento "Click" sobre el elemento "Boton para detener el envio de datos desde la placa"
        private void btnDetenerDatos_Click(object sender, EventArgs e)
        {
            // Se debe verificar si se esta ejecutando el Thread de recepcion
            if (threadLecturaPuertoSerieIniciado_Universal == true)
            {
                // Se habilita el boton para volver a lanzar el Thread de recepcion
                btnRecibirDatos.Enabled = true;

                // Se deshabilita el boton para detener el envio de datos
                btnDetenerDatos.Enabled = false;

                // Se marca el flag para que el Thread se termine
                terminarThreadDeRecepcionSerie_Universal = true;
            }
        }



        // ---------------------------------------------------------------------------- //
        // ------------------------------ CHECKBOXES ---------------------------------- //
        // ---------------------------------------------------------------------------- //


        // Handler para atener el evento de cambio en el elemento "chekBox" para la Peltier de RF
        private void cbxEnablePeltierRFFacial_CheckedChanged(object sender, EventArgs e)
        {
            // Se envia el comando para actualizar la configuracion
            EnviarComando((byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION);
        }


        // Handler para atener el evento de cambio en el elemento "chekBox" para la Peltier de Vacio
        private void cbxEnablePeltierVacio_CheckedChanged(object sender, EventArgs e)
        {
            // Se envia el comando para actualizar la configuracion
            EnviarComando((byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION);
        }


        // Handler para atener el evento de cambio en el elemento "chekBox" para la Fuente variable
        private void cbxEnableFuenteVariable_CheckedChanged(object sender, EventArgs e)
        {
            // Se envia el comando para actualizar la configuracion
            EnviarComando((byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION);
        }


        // Handler para atener el evento de cambio en el elemento "chekBox" para la Tension de Control
        private void cbxV_Control_CheckedChanged(object sender, EventArgs e)
        {
            // Se envia el comando para actualizar la configuracion
            EnviarComando((byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION);
        }


        // Handler para atener el evento de cambio en el elemento "chekBox" para el LED D21
        private void cbxLed_CheckedChanged(object sender, EventArgs e)
        {
            // Se envia el comando para actualizar la configuracion
            EnviarComando((byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION);
        }




        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //
        // --------------------- FUNCIONES PARA LO COMUNICACION ----------------------- //
        // ---------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------- //



        // ---------------------------------------------------------------------------- //
        // ------------------ THREAD PARA LA RECEPCION DE DATOS ----------------------- //
        // ---------------------------------------------------------------------------- //

        // Este es el thread que se encarga de la recepción de datos por el puerto serie
        private void funcionThreadLecturaPuertoSerie_Universal()
        {
            // ------------------------------- DEFINICION DE VARIABLES ------------------------------------ //

            // Se definen variables auxiliares, para hacer mas legible el codigo
            int TimeOutRequerido;
            int TotalDeBytesPorRecibir;
            int IntentosDeLectura;

            // Copia auxiliar del byte recibido
            byte byteRecibido;


            // Bufer para los comandos que se pueden recibir
            byte[] ComandosAceptadosParaRecibir = new byte[TOTAL_COMANDOS_PARA__RECIBIR];


            // --------------------- AGREGADO DE LOS COMANDOS VALIDOS PARA RECIBIR -------------------------- //

            // Se rellena el vector con los comandos que se pueden recibir
            ComandosAceptadosParaRecibir[0] = (byte)comandos_recibidos.COMANDO_RECIBIDO__DATOS_DE_LAS_MEDICIONES;


            // ------------------------------- INICIALIZACION DE LAS VARIABLES ------------------------------------ //

            // Indicador del bloque en recepcion
            bloques_de_recepcion bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

            // Contador del total de bytes recibidos correctamente del header inicial
            byte bytesDelHeaderInicialRecibidos = 0;

            // Contador del total de bytes recibidos correctamente del header final
            byte bytesDelHeaderFinalRecibidos = 0;

            // Indice auxiliar para recorrer el vector de comandos aceptados para recibir
            int indiceDeComandosAceptadosParaRecibir;

            // Contador de parametros adicionales por recibir en cada comando
            byte parametrosAdicionalesPorRecibir;

            // Contador de parametros adicionales recibidos en cada comando
            byte parametrosAdicionalesRecibidos;

            // Flag para indicar que hay un nuevo comando para ejecutar
            bool nuevoComandoParaEjecutar;

            // Se inicializa la variable, solo para que el compilador no marque error
            parametrosAdicionalesPorRecibir = 0;

            // Se inicializa la variable, solo para que el compilador no marque error
            parametrosAdicionalesRecibidos = 0;

            // Se inicializa la variable, solo para que el compilador no marque error
            nuevoComandoParaEjecutar = false;

            // Se define un timeout de 1 segundo
            TimeOutRequerido = 1000;

            // Se define la cantidad de bytes a recibir
            TotalDeBytesPorRecibir = 40;

            // Se definen la cantidad de intentos de lectura
            IntentosDeLectura = 2;

            // Se borra el contador de bytes leidos, por las dudas
            totalDeBytesRecibidos = 0;


            // ------------------------------- LOOP PERPETUO ------------------------------------ //

            while (terminarThreadDeRecepcionSerie_Universal == false)
            {

                // Se borra el buffer de recepcion para no tener datos anteriores
                Array.Clear(tramaRecibida, 0, tramaRecibida.Length);

                // Se lee la trama de datos enviada por el micro
                LeerTramaRecibida(TimeOutRequerido, TotalDeBytesPorRecibir, IntentosDeLectura);

                // Si no se leyo ningun dato, se vuelve a esperar
                if (totalDeBytesRecibidos == 0)
                {
                    // Se vuelve a ejecutar el loop desde el inicio
                    continue;
                }


                // Se borran los posibles datos que haya en el buffer despues de realizar la lectura
                puertoSerie.DiscardInBuffer();


                // Se tiene que hacer un loop para recorrer todos los bytes recibidos
                for (int indiceVectorRecibido = 0; indiceVectorRecibido < totalDeBytesRecibidos; indiceVectorRecibido++)
                {
                    // Se genera una copia auxiliar del byte
                    byteRecibido = tramaRecibida[indiceVectorRecibido];

                    // Se procesa el byte recibido en funcion de la parte de la trama que ubique
                    switch (bloqueEnRecepcion)
                    {
                        // --------------------- 1) Los primeros bytes de la trama son el encabezado inicial ----------------------- //
                        case bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL:

                            // Se verifica la correcta ubicacion de cada byte recibido del header inicial
                            if (HeaderInicial[bytesDelHeaderInicialRecibidos] == byteRecibido)
                            {
                                // Se incrementa el contador de bytes del header inicial recibidos
                                bytesDelHeaderInicialRecibidos++;

                                // Se verifica si se recibieron correctamente todos los bytes del header inicial recibidos
                                if (bytesDelHeaderInicialRecibidos >= TOTAL_DE_BYTES__HEADER_INICIAL)
                                {
                                    // Se pasa al bloque de recepcion del byte de encriptacion
                                    bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_IDENTIDICADOR_DEL_COMANDO;

                                    // Se resetea el contador de bytes del header inicial recibidos
                                    bytesDelHeaderInicialRecibidos = 0;
                                }
                            }
                            // Caso contrario, si algun byte no coincide, se reinicia el proceso de recepcion de la trama
                            else
                            {
                                // Se resetea el selector del bloque en recepcion
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

                                // Se resetea el contador de bytes del header inicial recibidos
                                bytesDelHeaderInicialRecibidos = 0;

                                // Se resetea el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos = 0;

                                // Se resetea el contador de parametros adicionales recibidos
                                parametrosAdicionalesRecibidos = 0;
                            }

                            break;  // Fin de "case RECEPCION_BLOQUE_DE_HEADER_INICIAL"



                        // ------------------- 2) Luego se encuentra el dato del comando a ejecutar ------------------- //
                        case bloques_de_recepcion.RECEPCION_BLOQUE_DE_IDENTIDICADOR_DEL_COMANDO:

                            // Se recorre el vector de todos los comandos aceptados para recibir
                            for (indiceDeComandosAceptadosParaRecibir = 0; indiceDeComandosAceptadosParaRecibir < TOTAL_COMANDOS_PARA__RECIBIR; indiceDeComandosAceptadosParaRecibir++)
                            {
                                // Se revisa que sea un comando valido
                                if (ComandosAceptadosParaRecibir[indiceDeComandosAceptadosParaRecibir] == byteRecibido)
                                {
                                    // Se almacena el comando recibido
                                    ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__ID_DEL_COMANDO] = byteRecibido;

                                    // Se pasa al bloque de recepcion de la cantidad de bytes de informacion
                                    bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_TOTAL_DE_BYTES;

                                    // Se corta el loop al verificar que es un comando valido
                                    break;
                                }
                            }

                            // Si el loop finalizo por desborde, no se encontro el dato dentro de los comandos validos
                            if (indiceDeComandosAceptadosParaRecibir == TOTAL_COMANDOS_PARA__RECIBIR)
                            {
                                // Se resetea el selector del bloque en recepcion
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

                                // Se resetea el contador de bytes del header inicial recibidos
                                bytesDelHeaderInicialRecibidos = 0;

                                // Se resetea el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos = 0;

                                // Se resetea el contador de parametros adicionales recibidos
                                parametrosAdicionalesRecibidos = 0;
                            }

                            break;  // Fin de "case RECEPCION_BLOQUE_DE_IDENTIDICADOR_DEL_COMANDO"



                        // ------------------- 3) Luego se encuentra el dato de la cantidad de bytes a recibir ------------------- //
                        case bloques_de_recepcion.RECEPCION_BLOQUE_DE_TOTAL_DE_BYTES:

                            // Se almacena el dato recibido dentro del vector de recepcion de comandos
                            ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__TOTAL_DE_BYTES] = byteRecibido;

                            // Se ramifica la recepcion, segun si el comando necesita recibir datos adicionales o no
                            if (byteRecibido != 0)
                            {
                                // Se pasa al estado de recepcion del siguiente bloque
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_PARAMETROS_ADICIONALES;

                                // Se actualiza el contador de parametros adicionales que este comando espera recibir
                                parametrosAdicionalesPorRecibir = byteRecibido;

                                // Se resetea el contador de parametros adicionales recibidos
                                parametrosAdicionalesRecibidos = 0;
                            }

                            // Si el comando no necesita recibir datos adicionales, se pasa a la recepcion del header final
                            else
                            {
                                // Se pasa al bloque de recepcion del bloque del header final, ya que no tiene datos adicionales
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_FINAL;

                                // Se resetea el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos = 0;
                            }

                            break;  // Fin de "case RECEPCION_BLOQUE_DE_TOTAL_DE_BYTES"



                        // -------------------- 4) Luego se encuentran los datos necesarios para los comandos complejos -------------------- //
                        case bloques_de_recepcion.RECEPCION_BLOQUE_DE_PARAMETROS_ADICIONALES:

                            // Se almacena el dato recibido dentro del vector de recepcion de comandos
                            ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesRecibidos] = byteRecibido;

                            // Se incrementa el contador de parametros adicionales recibidos
                            parametrosAdicionalesRecibidos++;

                            // Se verifica si se recibieron correctamente todos los parametros adicionales que necesita este comando
                            if (parametrosAdicionalesRecibidos >= parametrosAdicionalesPorRecibir)
                            {

                                // Se pasa al bloque de recepcion del header final
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_FINAL;

                                // Se resetea el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos = 0;

                            }

                            break;  // Fin de "case RECEPCION_BLOQUE_DE_PARAMETROS_ADICIONALES"



                        // ----- 5) Finalmente, para cerrar correctamente la trama, se debe colocar el header final ------ //
                        case bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_FINAL:

                            // Se verifica la correcta ubicacion de cada byte recibido del header final
                            if (HeaderFinal[bytesDelHeaderFinalRecibidos] == byteRecibido)
                            {
                                // Se incrementa el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos++;

                                // Se verifica si se recibieron correctamente todos los bytes del header final recibidos
                                if (bytesDelHeaderFinalRecibidos >= TOTAL_DE_BYTES__HEADER_FINAL)
                                {
                                    // Se pasa al bloque de recepcion del header inicial para la siguiente trama
                                    bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

                                    // Se recibio una trama correctamente. Se marca el flag para indicar que se dehe ejecutar la accion
                                    nuevoComandoParaEjecutar = true;

                                    // Se resetea el selector del bloque en recepcion
                                    bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

                                    // Se resetea el contador de bytes del header inicial recibidos
                                    bytesDelHeaderInicialRecibidos = 0;

                                    // Se resetea el contador de bytes del header final recibidos
                                    bytesDelHeaderFinalRecibidos = 0;
                                }
                            }
                            // Caso contrario, si algun byte no coincide, se reinicia el proceso de recepcion de la trama
                            else
                            {
                                // Se resetea el selector del bloque en recepcion
                                bloqueEnRecepcion = bloques_de_recepcion.RECEPCION_BLOQUE_DE_HEADER_INICIAL;

                                // Se resetea el contador de bytes del header inicial recibidos
                                bytesDelHeaderInicialRecibidos = 0;

                                // Se resetea el contador de bytes del header final recibidos
                                bytesDelHeaderFinalRecibidos = 0;

                                // Se resetea el contador de parametros adicionales recibidos
                                parametrosAdicionalesRecibidos = 0;
                            }

                            break;  // fin de "case RECEPCION_BLOQUE_DE_HEADER_FINAL"
                    }
                }



                // Se borra el contador de bytes recibidos en la ultima trama, por las dudas que no se borre en la funcion de recepcion
                totalDeBytesRecibidos = 0;

                // Se revisa si hay un nuevo comando para ejecutar
                if (nuevoComandoParaEjecutar == true)
                {
                    // Se borra el flag, para indicar que ya se ejecuto el comando recibido
                    nuevoComandoParaEjecutar = false;

                    // Se ejecuta la funcion para llevar a cabo las acciones de cada comando recibido
                    EjecutarComandoRecibido();

                    // Se borra el buffer del comando recibido, para mayor claridad en el debugueo
                    Array.Clear(ComandoRecibido, 0, ComandoRecibido.Length);

                }

            }

            // Se indica que el Thread no esta mas en ejecucion
            threadLecturaPuertoSerieIniciado_Universal = false;

            // Se cancela el thread de lectura del puerto serie
            try { threadLecturaPuertoSerie_Universal.Abort(); }
            catch (ThreadAbortException) { };

        }



        // ---------------------------------------------------------------------------- //
        // -------------------- FUNCION PARA EL ENVIO DE DATOS ------------------------ //
        // ---------------------------------------------------------------------------- //

        // Funcion para enviar los comandos
        private void EnviarComando(byte comandoParaEnviar)
        {
            // Se reinicia el contador de bytes a enviar
            totalDeBytesParaEnviar = 0;

            // Se agrega el Header inicial
            AgregarHeaderInicial();

            // Se agrega el comando a la trama
            AgregarComandoParaEnviar(comandoParaEnviar);

            // Se agrega el Header final
            AgregarHeaderFinal();

            // Se actualiza el total de bytes de informacion que se envian en el comando
            AgregarTotalDeBytesDeInformacion();

            // Se envia la trama
            EnviarTrama();
        }



        private void AgregarHeaderInicial()
        {
            // Se coloca el header inicial
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderInicial[0];
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderInicial[1];
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderInicial[2];
        }



        private void AgregarHeaderFinal()
        {
            // Se coloca el header inicial
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderFinal[0];
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderFinal[1];
            tramaParaEnviar[totalDeBytesParaEnviar++] = HeaderFinal[2];
        }



        private void AgregarTotalDeBytesDeInformacion()
        {
            // Se indica la cantidad de bytes de informacion que lleva el comando
            tramaParaEnviar[UBICACION_DEL_BYTE_DE_INFORMACION] = (byte)(totalDeBytesParaEnviar - TOTAL_DE_DATOS_BASICOS);
        }



        private void EnviarTrama()
        {
            // Se envia la trama por el puerto serie
            puertoSerie.Write(tramaParaEnviar, 0, totalDeBytesParaEnviar);
        }


        // ---------------------------------------------------------------------------- //
        // ------------------- FUNCION PARA RECIBIR UNA TRAMA ------------------------- //
        // ---------------------------------------------------------------------------- //

        // Funcion para leer una trama por el puerto serie, segun los parametros suministrados
        //////// CODIGOS DE ERROR DEVUELTOS
        //////// 0 => Trama recibida correcta
        //////// 1 => Error de timeout para la lectura de una trama
        //////// 2 => Error por agotar todas las instancias para leer la trama sin completarla

        private int LeerTramaRecibida(int TimeOutRequerido, int TotalDeBytesPorRecibir, int IntentosDeLectura)
        {
            // Se define el timeout requerido para recibir la respuesta
            puertoSerie.ReadTimeout = TimeOutRequerido;

            // Se actualiza la cantidad total de bytes que se esperan recibir en esta trama
            totalDeBytesPorRecibir = TotalDeBytesPorRecibir;

            // Se resetea el contador de bytes recibidos en esta trama
            totalDeBytesRecibidos = 0;

            // Se realizan los intentos de lectura requeridos para completar la trama
            for (int indiceDeLectura = 0; indiceDeLectura < IntentosDeLectura;)
            {
                // Se intenta la lectura de una trama
                try
                {
                    // Se lee una trama, teniendo en cuenta los bytes ya recibidos en los sucesivos intentos
                    totalDeBytesRecibidosEnEstaTrama = puertoSerie.Read(tramaRecibida, totalDeBytesRecibidos, totalDeBytesPorRecibir - totalDeBytesRecibidos);

                    // Se verifica que se haya leido algo
                    if (totalDeBytesRecibidosEnEstaTrama != 0)
                    {
                        // Se actualiza el contador de bytes recibidos
                        totalDeBytesRecibidos += totalDeBytesRecibidosEnEstaTrama;

                        // Se verifica si ya se leyeron todos los datos que se esperaban
                        if (totalDeBytesRecibidos == totalDeBytesPorRecibir)
                        {
                            // Se completo correctamente la lectura de la trama
                            return (0);
                        }

                        // Se incrementa el contador de intentos de lectura
                        indiceDeLectura++;
                    }
                }
                catch (TimeoutException)
                {
                    // Se produjo una excepcion por agotar el tiempo limite de espera en la recepcion de los datos
                    return (1);
                }
            }

            // Se agotaron los intentos de lectura sin poder completar la trama
            return (2);

        }




















        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------- FUNCIONES PARA EL USUARIO FINAL --------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //
        // ---------------------------------------------------------------------------------------------- //



        // ---------- FUNCION PARA LISTAR LAS ORDENES A EJECUTAR PARA CADA COMANDO RECIBIDO
        private void EjecutarComandoRecibido ()
        {
            // Contador de los parametros adicionales ya analizados
            byte parametrosAdicionalesAnalizados;

            // Se resetea el contador de parametros adicionales ya analizados
            parametrosAdicionalesAnalizados = 0;

            // Se acciona segun el comando recibido
            switch (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__ID_DEL_COMANDO])
            {

                // Comando para devolver la cantidad de pulsos consumidos desde la ultima consulta
                case (byte)comandos_recibidos.COMANDO_RECIBIDO__DATOS_DE_LAS_MEDICIONES:

                    // El primer dato es el valor de la corriente para "I_HImFU"
                    I_HImFU = ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++];
                    I_HImFU += (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++] << 8);
                    I_HImFU *= 1;
                    I_HImFU *= 5.0;
                    I_HImFU /= 4095;
                    lblI_HImFU.Text = I_HImFU.ToString("N3");

                    // El primer dato es el valor de la corriente para "lblI_Peltier_Crio"
                    I_Peltier_Crio = ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++];
                    I_Peltier_Crio += (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++] << 8);
                    I_Peltier_Crio *= 1;
                    I_Peltier_Crio *= 5.0;
                    I_Peltier_Crio /= 4095;
                    lblI_Peltier_Crio.Text = I_Peltier_Crio.ToString("N3");

                    // El primer dato es el valor de la corriente para "lblI_Peltier_Vacio"
                    I_Peltier_Vacio = ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++];
                    I_Peltier_Vacio += (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++] << 8);
                    I_Peltier_Vacio *= 2;
                    I_Peltier_Vacio *= 5.0;
                    I_Peltier_Vacio /= 4095;
                    lblI_Peltier_Vacio.Text = I_Peltier_Vacio.ToString("N3");

                    // El primer dato es el valor de la corriente para "lblI_V_RF"
                    I_V_RF = ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++];
                    I_V_RF += (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++] << 8);
                    I_V_RF *= 2;
                    I_V_RF *= 5.0;
                    I_V_RF /= 4095;
                    lblI_V_RF.Text = I_V_RF.ToString("N3");

                    // El primer dato es el valor de la corriente para "lblI_Peltier_Crio"
                    Vout_Var = ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++];
                    Vout_Var += (ComandoRecibido[(byte)posiciones_dentro_del_vector_recibido.POSICION_DENTRO_DEL_VECTOR_RECIBIDO__PARAMETRO_1 + parametrosAdicionalesAnalizados++] << 8);
                    Vout_Var *= 3;
                    Vout_Var *= 5.0;
                    Vout_Var /= 4095;
                    lblVout_Var.Text = Vout_Var.ToString("N3");

                    break;
            }
        }





        // ---------- FUNCION PARA PREPARAR EL BUFFER DE DATOS SEGUN EL COMANDO QUE SE QUIERA ENVIAR
        private void AgregarComandoParaEnviar(byte comandoParaEnviar)
        {
            // Se eligen las opciones segun el comando que se deba enviar
            switch (comandoParaEnviar)
            {
                // Enviar el estado de la configuracion
                case (byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION:


                    // Se coloca el identificador del comando
                    tramaParaEnviar[totalDeBytesParaEnviar] = (byte)comandos_enviados.COMANDO_ENVIADO__ACTUALIZAR_CONFIGURACION;

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se deja libre el byte de bytes de informacion, que se completa luego
                    tramaParaEnviar[totalDeBytesParaEnviar] = 0;

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se envia el dato de la activacion del parametro "Enable Peltier RF Facial"
                    if (cbxEnablePeltierRFFacial.Checked == true)
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 1;
                    }
                    else
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 0;
                    }

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se envia el dato de la activacion del parametro "Enable Peltier Vacio"
                    if (cbxEnablePeltierVacio.Checked == true)
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 1;
                    }
                    else
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 0;
                    }

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se envia el dato de la activacion del parametro "Enable Fuente Variable"
                    if (cbxEnableFuenteVariable.Checked == true)
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 1;
                    }
                    else
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 0;
                    }

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se envia el dato de la activacion del parametro "V Control"
                    if (cbxV_Control.Checked == true)
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 1;
                    }
                    else
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 0;
                    }

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Se envia el dato de la activacion del parametro "LED D21"
                    if (cbxLed.Checked == true)
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 1;
                    }
                    else
                    {
                        tramaParaEnviar[totalDeBytesParaEnviar] = 0;
                    }

                    // Se incrementa el contador de parametros utilizados
                    totalDeBytesParaEnviar++;


                    // Fin de la secuencia para este comando
                    break;
            }
        }

    }
}
