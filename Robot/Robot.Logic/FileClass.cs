﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Robot.Logic
{
    public class FileClass
    {
        public void WriteAlgorithm(AlgorithmSettings algorithm)
        {
            StreamWriter writer = new StreamWriter(@"Алгоритмы\" + algorithm.algName + ".txt");

            writer.WriteLine(algorithm.algName);

            writer.Write(algorithm.field.countGrid + "|");
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

                algorithm.field.countGrid = Convert.ToInt32(sp[0]);
                algorithm.robot.row = Convert.ToInt32(sp[1]);
                algorithm.robot.column = Convert.ToInt32(sp[2]);
                algorithm.robot.rotate = Convert.ToInt32(sp[3]);

                while (!read.EndOfStream)
                {
                    string[] spli = read.ReadLine().Split('|');

                    algorithm.commands.Add(new Commands
                    {
                        nom = Convert.ToInt32(spli[0]),
                        name = spli[1],
                        firstArg = Convert.ToInt32(spli[2]),
                        secondArg = Convert.ToInt32(spli[3])
                    });
                }
                algorithms.Add(algorithm);

                read.Close();
            }
            return algorithms;
        }      
    }
}
