using System;
using System.Collections.Generic;
using System.IO;

namespace Robot.Logic
{
    /// <summary>
    /// Класс работы с файлами
    /// </summary>
    public class FileClass
    {
        public void DelAlgorithm(string algorithmName)
        {
            File.Delete(@"Алгоритмы\" + algorithmName + ".txt");
        }

        /// <summary>
        /// Функция записи алгоритма в файл
        /// </summary>
        /// <param name="algorithm">алгоритм</param>
        public void WriteAlgorithm(AlgorithmSettings algorithm)
        {
            StreamWriter writer = new StreamWriter(@"Алгоритмы\" + algorithm.algName + ".txt", false, System.Text.Encoding.Default);

            writer.WriteLine(algorithm.algName);

            writer.Write(algorithm.field.countGridX + "|");
            writer.Write(algorithm.field.countGridY + "|");
            writer.Write(algorithm.robot.row + "|");
            writer.Write(algorithm.robot.column + "|");
            writer.WriteLine(algorithm.robot.rotate);

            for (int i = 0; i < algorithm.commands.Count; i++)
            {
                writer.Write(algorithm.commands[i].nom + "|");
                writer.Write(algorithm.commands[i].name + "|");
                writer.Write(algorithm.commands[i].firstArg + "|");
                writer.WriteLine(algorithm.commands[i].secondArg);
            }

            writer.Close();
        }

        /// <summary>
        /// Функция чтение алгоритма из файла
        /// </summary>
        /// <returns>алгоритм</returns>
        public List<AlgorithmSettings> Readalgorithms()
        {
            List<AlgorithmSettings> algorithms = new List<AlgorithmSettings>();

            string[] dirs = Directory.GetFiles("Алгоритмы", "*.txt");

            for (int i = 0; i < dirs.Length; i++)
            {
                AlgorithmSettings algorithm = new AlgorithmSettings();

                StreamReader read = new StreamReader(dirs[i], System.Text.Encoding.Default);

                algorithm.algName = read.ReadLine();

                string[] sp = read.ReadLine().Split('|'); 

                algorithm.field.countGridX = Convert.ToInt32(sp[0]);
                algorithm.field.countGridY = Convert.ToInt32(sp[1]);
                algorithm.robot.row = Convert.ToInt32(sp[2]);
                algorithm.robot.column = Convert.ToInt32(sp[3]);
                algorithm.robot.rotate = sp[4];

                while (!read.EndOfStream)
                {
                    string[] spli = read.ReadLine().Split('|');

                    algorithm.commands.Add(new Commands
                    {
                        nom = Convert.ToInt32(spli[0]),
                        name = spli[1],
                        firstArg = spli[2],
                        secondArg = spli[3]
                    });
                }
                algorithms.Add(algorithm);

                read.Close();
            }
            return algorithms;
        }      
    }
}
