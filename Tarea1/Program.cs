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
            Program objeto = new Program();
            //Sacamos matriz de enteros
            int[,] matrix = objeto.enteros("image.png");
            //objeto.imprimir(matrix);
            //Sacamos matriz binaria
            int[,] binarizado = objeto.binarizar(matrix);
            //objeto.imprimir(binarizado);
            objeto.metricas(matrix);
            //objeto.imprimir(binarizado);

            Console.ReadLine();
        }

        public int[,] enteros(string archivo)
        {

            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            Bitmap b2 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            int height = b1.Height, width = b1.Width, promedio;
            int[,] mat = new int[height, width];
            double funcion;
            for (int i = 0; i < b1.Width; i++)
                for (int j = 0; j < b1.Height; j++)
                {
                    funcion = ((.2125 * b1.GetPixel(i, j).R) + (.7154 * b1.GetPixel(i, j).G) + (.0721 * b1.GetPixel(i, j).B));
                    promedio = Convert.ToInt32(funcion);
                    b2.SetPixel(i, j, Color.FromArgb(b1.GetPixel(i,j).A, promedio, promedio, promedio));
                    mat[j, i] = promedio;
                }
            b2.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Gray.png"));
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
            b1.Save(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Black % White.png"));
            return bin;
        }

        public void imprimir(int[,] impr)
        {
            for (int i = 0; i < impr.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < impr.GetLength(1) - 1; j++)
                    Console.Write(impr[i, j]);
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

            Console.WriteLine("Primer punto\n" + "x: " + P1X + " y:" + P1Y + "\nIntensidad: " + matriz[P1Y,P1X] );
            Console.WriteLine("\nSegundo punto\n" + "x: " + P2X + " y:" + P2Y + "\nIntensidad: " + matriz[P2Y,P2X]);

        }
    }
}
