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
            Boolean chequeo = false, termino = false;
            int n = 2;
            Program objeto = new Program();
            //Sacamos matriz de enteros
            int[,] matrix = objeto.enteros("image.png");
            //Sacamos matriz binaria
            int[,] binarizado = objeto.binarizar(matrix);
            
            objeto.metricas(matrix);

            objeto.camino(matrix);
            
            int[,] componentes = new int[binarizado.GetLength(0), binarizado.GetLength(1)];
            componentes = (int[,])binarizado.Clone();

            do
            {
                chequeo = false;
                termino = false;

                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            if (componentes[i, j] == 1)
                            {
                                componentes = objeto.checar8(i, j, n, componentes);
                                chequeo = true;
                                j = matrix.GetLength(0);
                                i = matrix.GetLength(1);
                                break;
                            }
                            chequeo = false;
                        }
                n++;
            } while (chequeo == true);

            objeto.crearImagenComp(componentes, n, 8);
            String texto = "Componentes en N8: " + (n - 3);

            componentes = (int[,])binarizado.Clone();

            n = 2;
            do
            {
                chequeo = false;
                termino = false;

                for (int i = 0; i < matrix.GetLength(0); i++)
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (componentes[i, j] == 1)
                        {
                            componentes = objeto.checar4(i, j, n, componentes);
                            chequeo = true;
                            j = matrix.GetLength(0);
                            i = matrix.GetLength(1);
                            break;
                        }
                        chequeo = false;
                    }
                n++;
            } while (chequeo == true);

            String  texto2 = "Componentes en N4: " + (n - 3);
            using (StreamWriter writer = new StreamWriter(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Componentes.txt")))
            {
                writer.WriteLine(texto);
                writer.WriteLine(texto2);
            }
            Console.WriteLine("TXT componentes creado");
            objeto.crearImagenComp(componentes, n, 4);
            objeto.promedio(matrix);

            Console.WriteLine("Todos los archivos han sido creados");

            Console.ReadLine();
        }


        public int[,] checar8(int x, int y, int n, int[,] matriz)
        {
            matriz[x, y] = n;

            if (x != 0)
            {
                if (matriz[x - 1, y] == 1)
                {
                    //Console.Write("Arriba\n");
                    checar8((x - 1), y, n, matriz);
                }
                if (y != matriz.GetLength(1)-1)
                    if (matriz[x - 1, y + 1] == 1)
                    {
                        //Console.Write("Arriba derecha\n");
                        checar8((x - 1), (y + 1), n, matriz);
                    }
            }
            if (y != matriz.GetLength(1) - 1)
                if (matriz[x, y + 1] == 1)
                {
                    //Console.Write("Derecha\n");
                    checar8(x, (y + 1), n, matriz);
                }
            if (x != matriz.GetLength(0) - 1)
            {
                if (y != matriz.GetLength(1) - 1)
                    if (matriz[x + 1, y + 1] == 1)
                    {
                        //Console.Write("Abajo derecha\n");
                        checar8((x + 1), (y + 1), n, matriz);
                    }
                if (matriz[x + 1, y] == 1)
                {
                    //Console.Write("Abajo\n");
                    checar8((x + 1), y, n, matriz);
                }
            }
            if (y != 0)
            {
                if (x != matriz.GetLength(0) - 1)
                    if (matriz[x + 1, y - 1] == 1)
                    {
                        //Console.Write("Abajo izquierda\n");
                        checar8((x + 1), (y - 1), n, matriz);
                    }
                if (matriz[x, y - 1] == 1)
                {
                    //Console.Write("Izquierda\n");
                    checar8(x, (y - 1), n, matriz);
                }
                if (x != 0)
                    if (matriz[x - 1, y - 1] == 1)
                    {
                        //Console.Write("Arriba Izquierda\n");
                        checar8((x - 1), (y - 1), n, matriz);
                    }
            }
            return matriz;
        }

        public int[,] checar4(int x, int y, int n, int[,] matriz)
        {
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
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Black and White.png"));
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
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
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Componentes" + comp +".png"));
        }

        public int[,] enteros(string archivo)
        {

            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            Bitmap b2 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            Bitmap b3 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            int height = b1.Height, width = b1.Width, promedio;
            int[,] mat = new int[height, width];
            double funcion;
            for (int i = 0; i < b1.Width; i++)
                for (int j = 0; j < b1.Height; j++)
                {
                    funcion = ((.2125 * b1.GetPixel(i, j).R) + (.7154 * b1.GetPixel(i, j).G) + (.0721 * b1.GetPixel(i, j).B));
                    promedio = Convert.ToInt32(funcion);
                    b2.SetPixel(i, j, Color.FromArgb(b1.GetPixel(i,j).A, promedio, promedio, promedio));
                    b3.SetPixel(i, j, Color.FromArgb(b1.GetPixel(i, j).A, 255 - promedio, 255 - promedio, 255 - promedio));
                    mat[j, i] = promedio;
                }
            b2.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            b3.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Negative.png"));
            Console.WriteLine("Escala de grises creado");
            Console.WriteLine("Negativo creado");
            return mat;
        }

        public int[,] binarizar(int[,] mat)
        {
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "image.png"));
            int[,] bin = new int[mat.GetLength(0), mat.GetLength(1)];
            bin = (int[,])mat.Clone();
            for (int i = 0; i < bin.GetLength(0); i++)
                for (int j = 0; j < bin.GetLength(1); j++)
                {
                    if (bin[i, j] < 30) { 
                        bin[i, j] = 0;
                        b1.SetPixel(j, i, Color.White);
                    }
                    else
                    {
                        bin[i, j] = 1;
                        b1.SetPixel(j , i, Color.Black);
                    }
                }
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Black and White.png"));
            Console.WriteLine("Blanco y negro creado");
            return bin;
        }

        public void imprimir(int[,] impr)
        {
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
            Random r = new Random();
            P1X = r.Next(0, matriz.GetLength(1));
            P1Y = r.Next(0, matriz.GetLength(0));

            P2X = r.Next(0, matriz.GetLength(1));
            P2Y = r.Next(0, matriz.GetLength(0));

            Console.WriteLine("\nPrimer punto\n" + "x: " + P1X + " y:" + P1Y + "\nIntensidad: " + matriz[P1Y,P1X] );
            Console.WriteLine("\nSegundo punto\n" + "x: " + P2X + " y:" + P2Y + "\nIntensidad: " + matriz[P2Y,P2X] + "\n");

        }

        public void camino(int[,] matriz)
        {
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            int[,] camino = new int[matriz.GetLength(0), matriz.GetLength(1)];
            camino = (int[,])matriz.Clone();
            int difX = Math.Abs(P1X - P2X);
            int difY = Math.Abs(P1Y - P2Y);
            if (P1X < P2X)
            {
                for(int i = P1X; i <= P2X; i++)
                {
                    camino[P1Y, i] = 2;
                    b1.SetPixel(P1Y, i, Color.Gold);
                }
            } 
            else
            {
                for (int i = P1X; i >= P2X; i--)
                {
                    camino[P1Y, i] = 2;
                    b1.SetPixel(P1Y, i, Color.Gold);
                }
            }
            if(P1Y < P2Y)
            {
                for (int i = P1Y; i <= P2Y; i++)
                {
                    camino[i, P2X] = 2;
                    b1.SetPixel(i, P2X, Color.Gold);
                }
            }
            else
            {
                for (int i = P1Y; i >= P2Y; i--)
                {
                    camino[i, P2X] = 2;
                    b1.SetPixel(i, P2X, Color.Gold);
                }
            }
            b1.SetPixel(P1Y, P1X, Color.Green);
            b1.SetPixel(P2Y, P2X, Color.Red);
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Camino.png"));
            Console.WriteLine("Camino creado");
        }
    
        public void promedio(int[,] mat)
        {
            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
            //                              X Maxima         Y Maxima
            int[,] promedio = new int[mat.GetLength(0), mat.GetLength(1)];
            promedio = (int[,])mat.Clone();
            
            int[,] resultado = new int[mat.GetLength(0), mat.GetLength(1)];
            resultado = (int[,])mat.Clone();
            double suma = 0;
            int res = 0;
            
            for (int x = 0; x < mat.GetLength(0); x++)
            {
                for(int y = 0; y < mat.GetLength(1); y++)
                {

                    if (x != 0)
                    {
                        suma += promedio[x - 1, y];
                        if (y != promedio.GetLength(1) - 1)
                            suma += promedio[x - 1, y + 1];
                    }
                    if (y != promedio.GetLength(1) - 1)
                        suma += promedio[x, y + 1];

                    if (x != promedio.GetLength(0) - 1)
                    {
                        if (y != promedio.GetLength(1) - 1)
                            suma += promedio[x + 1, y + 1];
                        suma += promedio[x + 1, y];
                    }
                    if (y != 0)
                    {
                        if (x != promedio.GetLength(0) - 1)
                            suma += promedio[x + 1, y - 1];
                        suma += promedio[x, y - 1];
                        if (x != 0)
                            suma += promedio[x - 1, y - 1];
                    }
                    res = (Int32)Math.Round(suma/8);
                    resultado[x, y] = res;
                    b1.SetPixel(y, x, Color.FromArgb(b1.GetPixel(y, x).A, res, res, res));
                    suma = 0; res = 0;
                }
            }
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Average.png"));
            Console.WriteLine("Promedio creado");
        }
    }
}
