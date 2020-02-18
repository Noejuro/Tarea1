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
        static void Main(string[] args)
        {
            Program objeto = new Program();
            //Sacamos matriz de enteros
            int[,] matrix = objeto.enteros("image.png");
            //objeto.imprimir(matrix);
            //Sacamos matriz binaria
            int[,] binarizado = objeto.binarizar(matrix);
            objeto.imprimir(binarizado);
        }

        public int[,] enteros(string archivo)
        {

            Bitmap b1 = new Bitmap(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), archivo));
            int height = b1.Height, width = b1.Width, promedio;
            int[,] mat = new int[height, width];
            double funcion;
            for (int i = 0; i < b1.Width; i++)
                for (int j = 0; j < b1.Height; j++)
                {
                    funcion = ((.2125 * b1.GetPixel(i, j).R) + (.7154 * b1.GetPixel(i, j).G) + (.0721 * b1.GetPixel(i, j).B));
                    promedio = Convert.ToInt32(funcion);
                    mat[j, i] = promedio;
                }
            return mat;
        }

        public int[,] binarizar(int[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i,j] < 220)
                        mat[i, j] = 1;
                    else
                        mat[i, j] = 0;
                }
            return mat;
        }

        public void imprimir(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < matrix.GetLength(1) - 1; j++)
                    Console.Write(matrix[i, j]);
                Console.Write("\n");
            }
            Console.ReadLine();
        }
    
        public void metricas(int[,] matrix)
        {
            Random r = new Random();
            int coordX = r.Next(0, matrix.GetLength(1));
            int coordY = r.Next(0, matrix.GetLength(0));
        }
    }
}
