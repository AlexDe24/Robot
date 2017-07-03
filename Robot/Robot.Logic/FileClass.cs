using System;
using System.Collections.Generic;
using System.IO;

namespace Robot.Logic
{
    public class FileClass
    {
        public AlgSettings ReadAlgoritm()
        {
            AlgSettings settings = new AlgSettings();

            string[] dirs = Directory.GetFiles("Алгоритмы", "*.txt");

            for (int i = 0; i < dirs.Length; i++)
            {
                StreamReader read = new StreamReader(dirs[i], System.Text.Encoding.Default);

                string[] sp = read.ReadLine().Split('|');

                settings.countGrid = Convert.ToInt32(sp[0]);
                settings.row = Convert.ToInt32(sp[1]);
                settings.column = Convert.ToInt32(sp[2]);
                settings.rotate = Convert.ToInt32(sp[3]);

                while (!read.EndOfStream)
                {
                    string[] spli = read.ReadLine().Split('|');

                    settings.commands.Add(new Commands
                    {
                        nom = Convert.ToInt32(spli[0]),
                        name = spli[1],
                        firstArg = Convert.ToInt32(spli[2]),
                        secondArg = Convert.ToInt32(spli[3])
                    });
                }

                read.Close();
            }
            

            return settings;
        }
         
    }
}
