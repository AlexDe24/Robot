using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Linq;
using System;
using Robot.Logic;
using AvalonDock;

namespace Robot.Form
{
    /// <summary>
    /// Форма управления работой с алгоритмами
    /// </summary>
    public partial class AlgorinthmsControl
    {
        ControlWindows _controlWin; //родительская форма
        FieldControl _fieldControl; //форма управления полем
        PermissionDel permission; //форма на разрешение удаления

        List<AlgorithmSettings> _algorithms; //список алгоритмов

        FileClass _fileWork; //класс работы с файлами
        CreateAlg _createAlg; //форма создания алгоритма

        int _colv;

        public AlgorinthmsControl(ControlWindows controlWin)
        {
            InitializeComponent();

            _controlWin = controlWin; 
            _fileWork = new FileClass();

            _colv = 0;

            UpdateAlgoBox();
        }

        void UpdateAlgoBox()
        {
            int index = AlgoBox.SelectedIndex;

            _algorithms = new List<AlgorithmSettings>();
            _algorithms = _fileWork.ReadAlgorithms();

            AlgoBox.Items.Clear();

            for (int i = 0; i < _algorithms.Count; i++)
            {
                AlgoBox.Items.Add(_algorithms[i].algName);
            }

            AlgoBox.SelectedIndex = index;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (AlgoBox.SelectedIndex != -1)
            {
                _colv++;

                _fieldControl = new FieldControl((AlgoBox.Items[AlgoBox.SelectedIndex] as string), _fileWork, _controlWin, _colv);
            }
        }

        public void AlgoCreate_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlgoBox();

            _createAlg = new CreateAlg(_fileWork, null);
            _createAlg.ShowDialog();

            UpdateAlgoBox();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AlgoRedact_Click(object sender, RoutedEventArgs e)
        {
            if (AlgoBox.SelectedIndex != -1)
            {
                _createAlg = new CreateAlg(_fileWork, _algorithms.Where(x => x.algName == AlgoBox.Items[AlgoBox.SelectedIndex]).First());
                _createAlg.ShowDialog();

                UpdateAlgoBox();
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (AlgoBox.SelectedIndex != -1)
            {
                permission = new PermissionDel();
                if (permission.ShowDialog() == true)
                {
                    _fileWork.DelAlgorithm(AlgoBox.Items[AlgoBox.SelectedIndex] as string);
                    UpdateAlgoBox();
                }
            }
        }
    }
}
