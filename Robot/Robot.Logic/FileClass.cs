using System;
using System.Collections.Generic;
using System.IO;

namespace Robot.Logic
{
    public class FileClass
    {
        public AlgSettings Read()
        {
            AlgSettings settings = new AlgSettings();

            string[] dirs = Directory.GetFiles("Алгоритмы", "*.txt");

            for (int i = 0; i < dirs.Length; i++)
            {
                StreamReader read = new StreamReader(dirs[i]);

                settings.countGrid = read.Read();
                settings.startRow = read.Read();
                settings.startColumn = read.Read();

                while (!read.EndOfStream)
                {
                    settings.commands.Add(new Commands
                    {
                        nom = read.Read(),
                        name = Convert.ToString(read.Read()),
                        firstArg = read.Read(),
                        secondArg = read.Read()
                    });
                }

                read.Close();
            }
            

            return settings;
        }
         
    }
}
