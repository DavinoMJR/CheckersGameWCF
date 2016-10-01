using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using DamasGame.Entities;
using DamasGame.Util;
using DamasGamePlayer1.Host;
using System.Globalization;

namespace DamasGamePlayer1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DamasPlayer1GameWindow : IGameWindow
    {
        private readonly IDamasGamePlayer1Service _damasGamePlayerService1;
        private Move _currentMove;
        private string _winner;
        private string _turn;
        private bool _gonnaEat;

        public DamasPlayer1GameWindow(IDamasGamePlayer1Service damasGamePlayer1Service)
        {
            _damasGamePlayerService1 = damasGamePlayer1Service;
            InitializeComponent();
            InitializeGame();
        }

        public void SetupPlay(int row, int col, StackPanel stackPanel)
        {
            if (_currentMove == null)
                _currentMove = new Move();
            if (_currentMove.piece1 == null)
            {
                _currentMove.piece1 = new Piece(row, col);
                stackPanel.Background = Brushes.Green;
            }
            else
            {
                _currentMove.piece2 = new Piece(row, col);
                stackPanel.Background = Brushes.Green;
            }
            if ((_currentMove.piece1 != null) && (_currentMove.piece2 != null))
            {
                _gonnaEat = CheckIfEating();
                if (CheckMove(true))
                {
                    Move move = MakeMove();
                    if (move != null)
                    {
                        _damasGamePlayerService1.PlayRemote(move);
                    }
                    return;
                }
            }
        }
        public Move MakeMove()
        {
            Move moveASerEnviado = _currentMove;
            //List<Move> jumpMoves = GetCurrentBoard().checkJumps(Turn);
            //bool vaiComer = false;
            //if (jumpMoves.Count > 0)
            //{
            //    vaiComer = true;
            //}

            if ((_currentMove.piece1 != null) && (_currentMove.piece2 != null))
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    moveASerEnviado = _currentMove;
                    Console.WriteLine("Piece1 " + _currentMove.piece1.Row + ", " + _currentMove.piece1.Column);
                    Console.WriteLine("Piece2 " + _currentMove.piece2.Row + ", " + _currentMove.piece2.Column);
                    StackPanel stackPanel1 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece1.Row, _currentMove.piece1.Column);
                    StackPanel stackPanel2 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece2.Row, _currentMove.piece2.Column);
                    CheckersGrid.Children.Remove(stackPanel1);
                    CheckersGrid.Children.Remove(stackPanel2);
                    Grid.SetRow(stackPanel1, _currentMove.piece2.Row);
                    Grid.SetColumn(stackPanel1, _currentMove.piece2.Column);
                    CheckersGrid.Children.Add(stackPanel1);
                    Grid.SetRow(stackPanel2, _currentMove.piece1.Row);
                    Grid.SetColumn(stackPanel2, _currentMove.piece1.Column);
                    CheckersGrid.Children.Add(stackPanel2);
                    CheckKing(_currentMove.piece2);

                    var board = GetCurrentBoard();
                    var moves = board.CheckJumps(_turn);
                    if (!moves.Any() || !_gonnaEat || !moves.Any(t => t.piece1.Row == moveASerEnviado.piece2.Row && t.piece1.Column == moveASerEnviado.piece2.Column))
                    {
                        _currentMove = null;
                        if (_turn == "Black")
                        {
                            this.Title = Messages.MSG_VERMELHO_JOGA;
                            _turn = "Red";
                        }
                        else if (_turn == "Red")
                        {
                            this.Title = Messages.MSG_PRETO_JOGA;
                            _turn = "Black";
                        }
                    }
                    CheckWin();
                }));
            }
            return moveASerEnviado;
        }
        public void CloseGame(string message)
        {
            MessageBox.Show(message);
            this.Close();
        }


        #region Board Constructors

        private void ClearBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    CheckersGrid.Children.Remove(stackPanel);
                }
            }
        }

        private void MakeBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = new StackPanel();
                    if (r % 2 == 1)
                    {
                        if (c % 2 == 0)
                            stackPanel.Background = Brushes.White;
                        else
                            stackPanel.Background = Brushes.Gray;
                    }
                    else
                    {
                        if (c % 2 == 0)
                            stackPanel.Background = Brushes.Gray;
                        else
                            stackPanel.Background = Brushes.White;
                    }
                    Grid.SetRow(stackPanel, r);
                    Grid.SetColumn(stackPanel, c);
                    CheckersGrid.Children.Add(stackPanel);
                }
            }

            MakeButtons();
        }


        private void MakeButtons()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(button_Click);
                    button.Height = 60;
                    button.Width = 60;
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    button.VerticalAlignment = VerticalAlignment.Center;
                    var redBrush = new ImageBrush();

                    redBrush.ImageSource = new BitmapImage(new Uri(DirectoryHelper.GetRootPath() + "Resources/red60p.png", UriKind.Relative));
                    var blackBrush = new ImageBrush();
                    blackBrush.ImageSource = new BitmapImage(new Uri(DirectoryHelper.GetRootPath() + "Resources/black60p.png", UriKind.Relative));
                    switch (r)
                    {
                        case 1:
                            if (c % 2 == 1)
                            {

                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 2:
                            if (c % 2 == 0)
                            {
                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 3:
                            if (c % 2 == 1)
                            {
                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 4:
                            if (c % 2 == 0)
                            {
                                button.Background = Brushes.Gray;
                                button.Name = "button" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 5:
                            if (c % 2 == 1)
                            {
                                button.Background = Brushes.Gray;
                                button.Name = "button" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 6:
                            if (c % 2 == 0)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 7:
                            if (c % 2 == 1)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 8:
                            if (c % 2 == 0)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        UIElement GetGridElement(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;
        }

        private void AddBlackButton(Piece middleMove)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Background = Brushes.Gray;
            Button button = new Button();
            button.Click += new RoutedEventHandler(button_Click);
            button.Height = 60;
            button.Width = 60;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Background = Brushes.Gray;
            button.Name = "button" + middleMove.Row + middleMove.Column;
            stackPanel.Children.Add(button);
            Grid.SetColumn(stackPanel, middleMove.Column);
            Grid.SetRow(stackPanel, middleMove.Row);
            CheckersGrid.Children.Add(stackPanel);
        }

        #endregion

        #region Game Logic

        private bool CheckIfEating()
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.CheckJumps(_turn);
            _gonnaEat = false;
            return jumpMoves.Count > 0;
        }

        private bool CheckMoveRed(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.CheckJumps("Red");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (_currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    DisplayError(Messages.MSG_COMER_PECA);
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    Console.WriteLine("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Red"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((_currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = _currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Black"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            AddBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((_currentMove.isAdjacent("Red")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = _currentMove.checkJump("Red");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Black"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            AddBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            _currentMove = null;
            DisplayError(Messages.MSG_MOVIMENTO_INVALIDO);
            return false;
        }

        private bool CheckMoveBlack(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.CheckJumps("Black");

            if (jumpMoves.Count > 0)
            {
                bool invalid = true;
                foreach (Move move in jumpMoves)
                {
                    if (_currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    DisplayError(Messages.MSG_COMER_PECA);
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    Console.WriteLine("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Black"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((_currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = _currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Red"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            AddBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((_currentMove.isAdjacent("Black")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = _currentMove.checkJump("Black");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Red"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            AddBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            _currentMove = null;
            DisplayError(Messages.MSG_MOVIMENTO_INVALIDO);
            return false;
        }

        private CheckerBoard GetCurrentBoard()
        {
            CheckerBoard board = new CheckerBoard();
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    if (stackPanel.Children.Count > 0)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        if (button.Name.Contains("Red"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 3);
                            else
                                board.SetState(r - 1, c, 1);
                        }
                        else if (button.Name.Contains("Black"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 4);
                            else
                                board.SetState(r - 1, c, 2);
                        }
                        else
                            board.SetState(r - 1, c, 0);

                    }
                    else
                    {
                        board.SetState(r - 1, c, -1);
                    }

                }
            }
            return board;
        }

        private void CheckKing(Piece tmpPiece)
        {
            StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, tmpPiece.Row, tmpPiece.Column);
            if (stackPanel.Children.Count > 0)
            {
                Button button = (Button)stackPanel.Children[0];
                var redBrush = new ImageBrush();
                redBrush.ImageSource = new BitmapImage(new Uri(DirectoryHelper.GetRootPath() + "Resources/red60p_king.png", UriKind.Relative));
                var blackBrush = new ImageBrush();
                blackBrush.ImageSource = new BitmapImage(new Uri(DirectoryHelper.GetRootPath() + "Resources/black60p_king.png", UriKind.Relative));
                if ((button.Name.Contains("Black")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 1)
                    {
                        button.Name = "button" + "Black" + "King" + tmpPiece.Row + tmpPiece.Column;
                        button.Background = blackBrush;
                    }
                }
                else if ((button.Name.Contains("Red")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 8)
                    {
                        button.Name = "button" + "Red" + "King" + tmpPiece.Row + tmpPiece.Column;
                        button.Background = redBrush;
                    }
                }
            }
        }

        private void CheckWin()
        {
            int totalBlack = 0, totalRed = 0;
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    if (stackPanel.Children.Count > 0)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        if (button.Name.Contains("Red"))
                            totalRed++;
                        if (button.Name.Contains("Black"))
                            totalBlack++;
                    }
                }
            }
            if (totalBlack == 0)
                _winner = "Red";
            if (totalRed == 0)
                _winner = "Black";
            if (_winner != null)
            {
                for (int r = 1; r < 9; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                        if (stackPanel.Children.Count > 0)
                        {
                            Button button = (Button)stackPanel.Children[0];
                            button.Click -= new RoutedEventHandler(button_Click);
                        }
                    }
                }
                string message = string.Format(CultureInfo.CurrentCulture, Messages.MSG_FIM_JOGO, _winner);
                MessageBoxResult result = MessageBox.Show(message, Constants.VENCEDOR, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    NewGame();
            }
        }

        public Boolean CheckMove(bool isLocal)
        {
            if (isLocal)
            {
                if (_currentMove.piece1.Equals(_currentMove.piece2))
                {
                    StackPanel var1 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece1.Row, _currentMove.piece1.Column);
                    var1.Background = Brushes.Gray;
                    _currentMove = null;
                    return false;
                }

                StackPanel stackPanel1 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece1.Row, _currentMove.piece1.Column);
                StackPanel stackPanel2 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece2.Row, _currentMove.piece2.Column);
                Button button1 = (Button)stackPanel1.Children[0];
                Button button2 = (Button)stackPanel2.Children[0];
                stackPanel1.Background = Brushes.Gray;
                stackPanel2.Background = Brushes.Gray;

                if ((_turn == "Black") && (button1.Name.Contains("Red")))
                {
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    DisplayError("Preto joga.");
                    return false;
                }
                if ((_turn == "Red") && (button1.Name.Contains("Black")))
                {
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    DisplayError("Vermelho joga.");
                    return false;
                }
                if (button1.Equals(button2))
                {
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    return false;
                }
                if (button1.Name.Contains("Black"))
                {
                    return CheckMoveBlack(button1, button2);
                }
                else if (button1.Name.Contains("Red"))
                {
                    return CheckMoveRed(button1, button2);
                }
                else
                {
                    _currentMove.piece1 = null;
                    _currentMove.piece2 = null;
                    Console.WriteLine("False");
                    return false;
                }
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    StackPanel stackPanel1 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece1.Row, _currentMove.piece1.Column);
                    StackPanel stackPanel2 = (StackPanel)GetGridElement(CheckersGrid, _currentMove.piece2.Row, _currentMove.piece2.Column);
                    Button button1 = (Button)stackPanel1.Children[0];
                    Button button2 = (Button)stackPanel2.Children[0];
                    stackPanel1.Background = Brushes.Gray;
                    stackPanel2.Background = Brushes.Gray;
                    if (button1.Name.Contains("Black"))
                    {
                        CheckMoveBlack(button1, button2);
                    }
                    else
                    {
                        CheckMoveRed(button1, button2);
                    }

                }));
            }
            return true;
        }


        #endregion

        #region Events
        public void button_Click(Object sender, RoutedEventArgs e)
        {
            if (_turn == "Red")
            {
                MessageBox.Show(Messages.MSG_AGUARDANDO_JOGADOR_2);
                return;
            }

            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)button.Parent;
            int row = Grid.GetRow(stackPanel);
            int col = Grid.GetColumn(stackPanel);
            SetupPlay(row, col, stackPanel);
        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        #endregion

        #region Private Methods

        public void ActOnReceive(Move move)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this._currentMove = move;
                    _gonnaEat = _gonnaEat = CheckIfEating();
                    if (CheckMove(false))
                    {
                        MakeMove();
                    }
                }));
            }
            catch (Exception ex)
            {

            }
        }

        private void Start()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Show();
            }));
        }


        private void NewGame()
        {
            _currentMove = null;
            _winner = null;
            this.Title = "Damas! Preto joga!";
            _turn = "Black";
            ClearBoard();
            MakeBoard();
        }

        private void DisplayError(string error)
        {
            MessageBox.Show(error, "Movimento inválido.", MessageBoxButton.OK);
        }

        private void InitializeGame()
        {
            this.Title = Constants.TITLE_JOGADOR1;
            _currentMove = null;
            _winner = null;
            _turn = Constants.RED;
            MakeBoard();
        }

        #endregion

        #region IGameWindow Members

        #endregion
    }
}
