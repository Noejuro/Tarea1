using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1
{
    class Program
    {
        public int P1X, P1Y, P2X, P2Y; 
        static void Main(string[] args)
        {
            //Declaracion de variables
            //Variables booleanas que nos serviran como banderas para encontrar el primer 1 en nuestra imagen y saber si ya no hay mas 1 en la imagen
            Boolean chequeo = false, termino = false;
            int n = 2;  //Variable para contar los componentes
            Program objeto = new Program();
            //Sacamos la matriz de enteros del archivbo image.png
            int[,] matrix = objeto.enteros("image.png");   
            //Sacamos matriz binaria basandonos en la matriz de enteros
            int[,] binarizado = objeto.binarizar(matrix);
            //Invoamos el metodo para crear los dos puntos aleatorios de la imagen y ademas sacamos los resultados de las tres distancias pedidas
            objeto.metricas(matrix);
            //Metodo para la creacion del camino entre los dos puntos aleatorios
            objeto.camino(matrix);
            //Creamos una nueva matriz para contabilizar el numero de componentes que estará basado en la matriz binaria
            int[,] componentes = new int[binarizado.GetLength(0), binarizado.GetLength(1)];
            componentes = (int[,])binarizado.Clone();
            //Empezamos el metodo para sacar los componentes en n8
            do
            {
                //Banderas para encontrar 1 y saber si ya no hay 1 en la imagen
                chequeo = false;
                termino = false;
                //Metodo para buscar el primer 1 en toda la imagen
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            if (componentes[i, j] == 1)
                            {
                                //Invocacion del metodo recursivo de checar en N8 para encontrar los componentes y ademas asignar el resultado a la matriz componentes. Mandamos las coordenadas del 1 que encotnramos, ademas del identificadoer del componente
                                componentes = objeto.checar8(i, j, n, componentes);
                                //Cambiamos banderas para volver a iterar
                                chequeo = true;
                                j = matrix.GetLength(0);
                                i = matrix.GetLength(1);
                                break;
                            }
                            //Bandera para saber si ya no existen 1 en la imagen
                            chequeo = false;
                        }
                //Aumentamos la cantidad de componentes
                n++;
            } while (chequeo == true);
            //Invocamos el metodo para crear la imagen de los componentes creados en N8
            objeto.crearImagenComp(componentes, n, 8);
            String texto = "Componentes en N8: " + (n - 3);

            componentes = (int[,])binarizado.Clone();

            n = 2;
            //Empezamos el metodo para sacar los componentes en n8
            do
            {
                //Banderas para encontrar 1 y saber si ya no hay 1 en la imagen
                chequeo = false;
                termino = false;
                //Metodo para buscar el primer 1 en toda la imagen
                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (componentes[i, j] == 1)
                        {
                            //Invocacion del metodo recursivo de checar en N4 para encontrar los componentes y ademas asignar el resultado a la matriz componentes. Mandamos las coordenadas del 1 que encotnramos, ademas del identificadoer del componente
                            componentes = objeto.checar4(i, j, n, componentes);
                            //Cambiamos banderas para volver a iterar
                            chequeo = true;
                            j = matrix.GetLength(0);
                            i = matrix.GetLength(1);
                            break;
                        }
                        //Bandera para saber si ya no existen 1 en la imagen
                        chequeo = false;
                    }
                //Aumentamos la cantidad de componentes
                n++;
            } while (chequeo == true);
            //Metodo para guardar en el archivo txt la cantidad de componentes en n8 y n4
            String texto2 = "Componentes en N4: " + (n - 3);
            using (StreamWriter writer = new StreamWriter(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Componentes.txt")))
            {
                writer.WriteLine(texto);
                writer.WriteLine(texto2);
            }
            //Invocamos el metodo para crear la imagen de los componentes creados en N4
            objeto.crearImagenComp(componentes, n, 4);
            //Invocamos el metodo para sacar la matriz de promedios
            objeto.promedio(matrix);

            Console.WriteLine("\nTodos los archivos han sido creados\nPresiona ENTER para salir");
            Console.ReadLine();
        }


        public int[,] checar8(int x, int y, int n, int[,] matriz)
        {
            //Asignamos el numero del componente a la posicion donde se encontraba el 1
            matriz[x, y] = n;
            //Empezamos recursividad pero con validaciones para que no cheque en posiciones que no existen eg. matriz[-1,-1]
            //Validacion para que no pase el limite inferior de X
            if (x != 0)
            {
                //Checamos si en la posicion de arriba del pixel en el que estamos hay un uno
                if (matriz[x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                    checar8((x - 1), y, n, matriz);
                }
                //Validacion para que no pase el limite superior de Y
                if (y != matriz.GetLength(1)-1)
                    //Checamos si en la posicion de arriba a la derecha del pixel en el que estamos hay un uno
                    if (matriz[x - 1, y + 1] == 1)
                    {
                        //Console.Write("Arriba derecha\n");
                        //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                        checar8((x - 1), (y + 1), n, matriz);
                    }
            }
            //Validacion para que no pase el limite superior de Y
            if (y != matriz.GetLength(1) - 1)
                //Checamos si en la posicion de derecha del pixel en el que estamos hay un uno
                if (matriz[x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                    checar8(x, (y + 1), n, matriz);
                }
            //Validacion para que no pase el limite superior de X
            if (x != matriz.GetLength(0) - 1)
            {
                //Validacion para que no pase el limite superior de Y
                if (y != matriz.GetLength(1) - 1)
                    //Checamos si en la posicion de abajo derecha del pixel en el que estamos hay un uno
                    if (matriz[x + 1, y + 1] == 1)
                    {
                        //Console.Write("Abajo derecha\n");
                        //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                        checar8((x + 1), (y + 1), n, matriz);
                    }
                //Checamos si en la posicion de abajo del pixel en el que estamos hay un uno
                if (matriz[x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                    checar8((x + 1), y, n, matriz);
                }
            }
            //Validacion para que no pase el limite inferior de Y
            if (y != 0)
            {
                //Validacion para que no pase el limite superior de X
                if (x != matriz.GetLength(0) - 1)
                    //Checamos si en la posicion de abajo izquierda del pixel en el que estamos hay un uno
                    if (matriz[x + 1, y - 1] == 1)
                    {
                        //Console.Write("Abajo izquierda\n");
                        //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                        checar8((x + 1), (y - 1), n, matriz);
                    }
                //Checamos si en la posicion de izquierda del pixel en el que estamos hay un uno
                if (matriz[x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                    checar8(x, (y - 1), n, matriz);
                }
                //Validacion para que no pase el limite inferior de X
                if (x != 0)
                    //Checamos si en la posicion de arriba izquierda del pixel en el que estamos hay un uno
                    if (matriz[x - 1, y - 1] == 1)
                    {
                        //Console.Write("Arriba Izquierda\n");
                        //Hacemos recursividad y ahora mandamos una nueva posicion como pixel central
                        checar8((x - 1), (y - 1), n, matriz);
                    }
            }
            //Regresamos la matriz con los componentes marcados
            return matriz;
        }

        public int[,] checar4(int x, int y, int n, int[,] matriz)
        {
            //Se repite el mismo procedimiento que con el etiquetado de N8, la unica diferencia es la posibilidad de pixeles que puede viajar, ya que solo puede moverse arriba, abajo, derecha e izquierda
            //Console.WriteLine(x + " " + y);
            matriz[x, y] = n;

            if (x != 0)
            {
                if (matriz[x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    checar4((x - 1), y, n, matriz);
                }
            }
            if (y != matriz.GetLength(1) - 1)
                if (matriz[x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    checar4(x, (y + 1), n, matriz);
                }
            if (x != matriz.GetLength(0) - 1)
            {
                if (matriz[x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    checar4((x + 1), y, n, matriz);
                }
            }
            if (y != 0)
            {
                if (matriz[x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    checar4(x, (y - 1), n, matriz);
                }
            }
            return matriz;
        }

        public void crearImagenComp(int[,] mat, int n, int comp)
        {
            //Creamos una nueva imagen basada en la imagen Black and white
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Black and White.png"));
            //Pintamos pixel por pixel y por componentes la imagen nueva
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    //Coloreamos dependiendo la etiqueta del componente
                    if (mat[i, j] == 1)
                        b1.SetPixel(j, i, Color.Red);
                    if (mat[i, j] == 2)
                        b1.SetPixel(j, i, Color.Blue);
                    if (mat[i, j] == 3)
                        b1.SetPixel(j, i, Color.Gold);
                    if (mat[i, j] == 4)
                        b1.SetPixel(j, i, Color.Orange);
                    if (mat[i, j] == 5)
                        b1.SetPixel(j, i, Color.Green);
                    if (mat[i, j] == 6)
                        b1.SetPixel(j, i, Color.Brown);
                    if (mat[i, j] == 7)
                        b1.SetPixel(j, i, Color.Purple);
                    if (mat[i, j] == 8)
                        b1.SetPixel(j, i, Color.Aqua);
                    if (mat[i, j] == 9)
                        b1.SetPixel(j, i, Color.DarkCyan);
                }
            }
            //Guardamos la imagen con el nombre de componentes en 4 u 8
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Componentes" + comp +".png"));
        }

        public int[,] enteros(string archivo)
        {

            //Creamos tres imagenes basadas en la imagen original
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            Bitmap b2 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            Bitmap b3 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            //Obtenemos el ancho y el alto de la iumagen original
            int height = b1.Height, width = b1.Width, promedio;
            //Creamos la nueva matriz de enteros
            int[,] mat = new int[height, width];
            double funcion;
            //Anidamos dos ciclos para checar cada pizel de la imagen original
            for (int i = 0; i < b1.Width; i++)
                for (int j = 0; j < b1.Height; j++)
                {
                    //Obtenemos por medio de una funcion ya prestablecida el valor de cada pixel basandonos en su Rojo, Verde y Azul
                    funcion = ((.2125 * b1.GetPixel(i, j).R) + (.7154 * b1.GetPixel(i, j).G) + (.0721 * b1.GetPixel(i, j).B));
                    //Lo casteamos a enteros
                    promedio = Convert.ToInt32(funcion);
                    //Asignamos el pixel para la imagen en escala de grises
                    b2.SetPixel(i, j, Color.FromArgb(b1.GetPixel(i,j).A, promedio, promedio, promedio));
                    //Asignamos el pixel para la imagen en Negativo al cual restamos 255 menos el valor del pixel
                    b3.SetPixel(i, j, Color.FromArgb(b1.GetPixel(i, j).A, 255 - promedio, 255 - promedio, 255 - promedio));
                    //Guardamos el valor entero en la matriz
                    mat[j, i] = promedio;
                }
            //Guardamos las dos imagenes
            b2.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            b3.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Negative.png"));
            //Regresamos el valor de la matriz con enteros
            return mat;
        }

        public int[,] binarizar(int[,] mat)
        {
            //Creamos una nueva imagen basada en la imagen original
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "image.png"));
            //Creamos una matriz para guardar los valores binarios
            int[,] bin = new int[mat.GetLength(0), mat.GetLength(1)];
            bin = (int[,])mat.Clone();
            //Empezamos los ciclos aninados para checar cada pixel
            for (int i = 0; i < bin.GetLength(0); i++)
                for (int j = 0; j < bin.GetLength(1); j++)
                {
                    //Utilizamos el valor 30 como umbral
                    //Checamos si el pixel es menor a 30 entonces en la imagen lo colorea en blanco y en la matriz binaria lo guarda como 0
                    if (bin[i, j] < 30) { 
                        bin[i, j] = 0;
                        b1.SetPixel(j, i, Color.White);
                    }
                    else
                    {
                        //Checamos si el pixel es mayor a 30 entonces en la imagen lo colorea en negro y en la matriz binaria lo guarda como 1
                        bin[i, j] = 1;
                        b1.SetPixel(j , i, Color.Black);
                    }
                }
            //Guarda la nueva imagen con el nombre de Black and white y regresa la matriz binaria
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Black and White.png"));
            return bin;
        }

        public void imprimir(int[,] impr)
        {
            //Metodo para imprimir cualquier matriz
            for (int i = 0; i < impr.GetLength(0); i++)
            {
                for (int j = 0; j < impr.GetLength(1); j++)
                    Console.Write(impr[i, j] + " ");
                Console.Write("\n");
            }
            Console.ReadLine();
        }
    
        public void metricas(int[,] matriz)
        {
            //Declaracion de variables
            double distanciaEuclidiana, distanciaManhattan, distanciaVa;
            Random r = new Random();
            //Generación de pixeles aleatorios para realizar operaciones de métricas entre dos pixeles que va de 0 hasta el maximo de la imagen
            P1X = r.Next(0, matriz.GetLength(1));
            P1Y = r.Next(0, matriz.GetLength(0));

            P2X = r.Next(0, matriz.GetLength(1));
            P2Y = r.Next(0, matriz.GetLength(0));

            //Calculo para distancia Euclideana
            distanciaEuclidiana = Math.Sqrt((Math.Pow((P2X - P1X), 2)) + (Math.Pow((P2Y - P1Y), 2)));
            //Calculo para distancia Manhattan d(p,q)=|q1-p1|+|q2-p2|
            distanciaManhattan = (Math.Abs((P2X - P1X)) + Math.Abs((P2Y - P1Y)));
            //Calculo para distancia  de Máximo valor absoluto d(p,q)=MAX(|qi-pi|)
            if (Math.Abs((P2X - P1X)) > Math.Abs((P2Y - P1Y)))
            {
                distanciaVa = Math.Abs((P2X - P1X));
            }
            else
            {
                distanciaVa = Math.Abs((P2Y - P1Y));
            }
            //Impresión de resultados
            Console.WriteLine("\nPrimer punto\n" + "x: " + P1X + " y:" + P1Y + "\nIntensidad: " + matriz[P1Y,P1X] );
            Console.WriteLine("\nSegundo punto\n" + "x: " + P2X + " y:" + P2Y + "\nIntensidad: " + matriz[P2Y,P2X] + "\n");
            Console.WriteLine("\nDistancia Euclidiana entre\n" + "x1: " + P1X + "\ty1:" + P1Y + "\nx2: " + P2X + "\ty2:" + P2Y + " \nEs de " + distanciaEuclidiana);
            Console.WriteLine("\nDistancia Manhattan entre\n" + "x1: " + P1X + "\ty1:" + P1Y + "\nx2: " + P2X + "\ty2:" + P2Y + " \nEs de " + distanciaManhattan);
            Console.WriteLine("\nDistancia de Máximo valor absoluto\n" + "x1: " + P1X + "\ty1:" + P1Y + "\nx2: " + P2X + "\ty2:" + P2Y + " \nEs de " + distanciaVa);

        }

        public void camino(int[,] matriz)
        {
            //Creamos una nueva imagen basada en la escala de grises, ademas creamos una nueva matriz camino
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            int[,] camino = new int[matriz.GetLength(0), matriz.GetLength(1)];
            camino = (int[,])matriz.Clone();
            //Obtenemos la diferencia entre las X y Y de los dos puntos aleatorios
            int difX = Math.Abs(P1X - P2X);
            int difY = Math.Abs(P1Y - P2Y);
            //Si la X del primer punto es menor que el del segundo punto entonces el camino se generará a la derecha
            if (P1X < P2X)
            {
                //El ciclo es para generar el camino desde la X del primer punto hasta la X del segundo Punto
                for(int i = P1X; i <= P2X; i++)
                {
                    //Guardamos el camino como 2 en la misma Y del punto 1
                    camino[P1Y, i] = 2;
                    //Pintamos el camino en la imagen
                    b1.SetPixel(i, P1Y, Color.Gold);
                }
            } 
            else
            {
                //Si la X del primer punto es mayor que el del segundo punto entonces el camino se generará a la izquierda
                //El ciclo es para generar el camino desde la X del primer punto hasta la X del segundo Punto
                for (int i = P1X; i >= P2X; i--)
                {
                    //Guardamos el camino como 2 en la misma Y del punto 1
                    camino[P1Y, i] = 2;
                    //Pintamos el camino en la imagen
                    b1.SetPixel(i, P1Y, Color.Gold);
                }
            }
            //Si la Y del primer punto es menor que el del segundo punto entonces el camino se generará hacia arriba
            if (P1Y < P2Y)
            {
                //El ciclo es para generar el camino desde la Y del primer punto hasta la Y del segundo Punto
                for (int i = P1Y; i <= P2Y; i++)
                {
                    //Guardamos el camino como 2 en la misma X del punto 1
                    camino[i, P2X] = 2;
                    //Pintamos el camino en la imagen
                    b1.SetPixel(P2X, i, Color.Gold);
                }
            }
            else
            {
                //Si la Y del primer punto es menor que el del segundo punto entonces el camino se generará hacia arriba
                //El ciclo es para generar el camino desde la Y del primer punto hasta la Y del segundo Punto
                for (int i = P1Y; i >= P2Y; i--)
                {
                    //Guardamos el camino como 2 en la misma X del punto 1
                    camino[i, P2X] = 2;
                    //Pintamos el camino en la imagen
                    b1.SetPixel(P2X, i, Color.Gold);
                }
            }
            //Pintamos el punto inicial y el punto final en la imagen
            b1.SetPixel(P1X, P1Y, Color.Green);
            b1.SetPixel(P2X, P2Y, Color.Red);
            //Guardamos la imagen con el nombre de Camino
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Camino.png"));
        }
    
        public void promedio(int[,] mat)
        {
            //Creamos una nueva imagen basada en la escala de grises
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            //Creamos una nueva matriz para usarla como base de los valores ESTA NO SE MODIFICA
            //                              X Maxima         Y Maxima
            int[,] promedio = new int[mat.GetLength(0), mat.GetLength(1)];
            promedio = (int[,])mat.Clone();
            //Creamos una nueva matriz para el resultado de los valores promedio ESTA SÍ SE MODIFICA
            int[,] resultado = new int[mat.GetLength(0), mat.GetLength(1)];
            resultado = (int[,])mat.Clone();
            //Variable para guardar la sumatoria de los vecinos en N8
            double suma = 0;
            //Variable para guardar el resultado de la suma entre 8
            int res = 0;
            //Ciclos anidados para checar cada pixel
            for (int x = 0; x < mat.GetLength(0); x++)
            {
                for(int y = 0; y < mat.GetLength(1); y++)
                {
                    //Utilizamos las mismas validaciones que usamos en la funcion recursiva de N8
                    if (x != 0)
                    {
                        //Sumamos el pixel vecino
                        suma += promedio[x - 1, y];
                        if (y != promedio.GetLength(1) - 1)
                            //Sumamos el pixel vecino
                            suma += promedio[x - 1, y + 1];
                    }
                    if (y != promedio.GetLength(1) - 1)
                        //Sumamos el pixel vecino
                        suma += promedio[x, y + 1];

                    if (x != promedio.GetLength(0) - 1)
                    {
                        if (y != promedio.GetLength(1) - 1)
                            //Sumamos el pixel vecino
                            suma += promedio[x + 1, y + 1];
                        //Sumamos el pixel vecino
                        suma += promedio[x + 1, y];
                    }
                    if (y != 0)
                    {
                        if (x != promedio.GetLength(0) - 1)
                            //Sumamos el pixel vecino
                            suma += promedio[x + 1, y - 1];
                        //Sumamos el pixel vecino
                        suma += promedio[x, y - 1];
                        if (x != 0)
                            //Sumamos el pixel vecino
                            suma += promedio[x - 1, y - 1];
                    }
                    //Obtenemos el promedio de la sumatoria
                    res = (Int32)Math.Round(suma/8);
                    //Guardamos el promedio en la amtriz
                    resultado[x, y] = res;
                    //Pintamos la imagen con el promedio obtenido
                    b1.SetPixel(y, x, Color.FromArgb(b1.GetPixel(y, x).A, res, res, res));
                    //Reiniciamos variables
                    suma = 0; res = 0;
                }
            }
            //Guardamos la imagen pintada como Average.png
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Average.png"));
        }
    }
}
