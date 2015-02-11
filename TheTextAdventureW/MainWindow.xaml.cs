using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheTextAdventureW
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameMode gameMode;
        private string ShowText;
        private bool systemIsRunning = true;
        private delegate void ThreadCompleted(bool isEventTrigger);
        private event ThreadCompleted OnThreadCompleted;
        private int textSpeed = 10;
        private int GameStep = -1;
        private bool _isEventTrigger = false;
        Thread threadText;

        //SQL Compact


        public MainWindow()
        {
            InitializeComponent();
            textShow.Text = string.Empty;
            this.Closing += MainWindow_Closing;
            this.OnThreadCompleted += MainWindow_OnThreadCompleted;
            MainIntro();
            InputBox.Focus();
            TestSelect();
        }


        private void TestSelect()
        {
            try
            {
                SqlCeConnection sqlceConn = clsDBConnect.GetDBConnectAsync();
                DataTable dtRet = new DataTable();
                clsTypes type = new clsTypes();

                Exception ex = clsDBConnect.SelectData("select * from MENU_T", out dtRet, sqlceConn, null);
                if (ex != null)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
            }
        }

        void MainWindow_OnThreadCompleted(bool isEventTrigger)
        {
            try
            {
                _isEventTrigger = isEventTrigger;
                switch (GameStep)
                {
                    case 0:
                        TextLeftToRight("*sing*Taaam tam tamtamtatmat\n\ntarara taaam taaam tarara ttaaam tam*sing*" +
                               "\n\nvor langer Zeit\nin einer weit weit entfernten Galaxie\n\n" +
                                "The Text Adventure\nEpisode I\nThe Return of the Text Nerds", 45, true);
                        GameStep = 1; //Next Step
                        break;
                    case 1:
                        TextLeftToRight("\n\nEs war eine dunkle Zeit der Umbrüche\ndas bitterböse Imperium der Dumpfbacken wollte den letzten\nWiderstand der Milchmäuse-Rebellen brechen\n und Sie für immer brechen, jedoch keimte eine letzte Hoffnung auf.\nDer letzte Nerd sollte erwachen...", 45);
                        GameStep = 2; //Next Step
                        break;
                    case 2:
                        SetTextAlign(TextAlignment.Left);
                        TextLeftToRight("Unbekannte Stimme: Wach auf Nerd, wach auf!\nNerd: Waasss wo bin ich?\nWas ist hier los, wo ist mein Laptop und W-Lan?", 60, true);
                        showCommands();
                        GameStep = 3; //Next Step
                        break;
                    case 4:
                        TextLeftToRight("(Der Nerd steht aus dem Bett erfolgreich auf, jedoch erbost darüber, dass sein Laptop weg ist)", 60, true);
                        TextLeftToRight("\nNerd: unverzeilich! Wie konnte das jemand wagen?\nUnbekanntte Stimme: Höre auf deine Sinne Nerd! Es war ein Dumpfbacken Spion!\nNerd: Oh nein DAS KANN NICHT SEIN!!! :-O", 60);
                        TextLeftToRight("(Der Nerd schaut sich im Zimmer um und sieht ein Zettel auf dem Boden)", 60, true);
                        showCommands();
                        GameStep = 5; //Next Step
                        break;
                    case 6:
                        TextLeftToRight("(Der Nerd las den Zettel aufmerksam auf dem stand.... FORTSETZUNG FOLGT!", 60, true);
                        TextLeftToRight("*sing*Taaam tam tamtamtatmat\n\ntarara taaam taaam tarara ttaaam tam*sing* ... \n4. zum Beenden", 60);
                        GameStep = 7; //Next Step
                        gameMode = GameMode.MAINMENU;
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void showCommands()
        {
            TextLeftToRight("\n\nBefehle: gehen|verlassen|nehmen|legen|angreifen|werfen|lesen|kombinieren|wdh(wiederholen Textabschnitt)", 35);
        }

        private void SetTextAlign(TextAlignment textAlignment)
        {
            try
            {
                Dispatcher.Invoke(new Action(delegate()
                   {
                       textShow.TextAlignment = textAlignment;
                   }));
            }
            catch (Exception)
            {

                throw;
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            systemIsRunning = false;
        }

        private enum GameMode
        {
            MAINMENU,
            STORY
        }

        private void MainIntro()
        {
            try
            {
                TextLeftToRight("eMJay Presents\nA Game By eMJay\nIdea By eMJay\n\n\n" +
                    //   "Es war vor einer langer Zeit\nin einer weit weit entfernten Galaxie\n\n" +
                    "The Text Adventure\nEpisode I\nThe Return of the Text Nerds", 45);
                TextLeftToRight("\n\n1. Start Story\n2. Laden\n3. Settings\n4. Beenden", 45);
                gameMode = GameMode.MAINMENU;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void showMainMenu()
        {
            try
            {

                //ShowText += ";

                //  TextLeftToRight("1. Start Story\n2. Laden\n3. Settings\n4. Beenden");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void TextLeftToRight(string InputText, int speed)
        {
            TextLeftToRight(InputText, speed, false);
        }

        private void TextLeftToRight(string InputText, int speed, bool clearText)
        {
            try
            {
                ShowText += InputText;

                if (clearText)
                {
                    Dispatcher.Invoke(new Action(delegate()
                     {
                         textShow.Text = string.Empty;
                     }));
                }

                //if (!clearText)
                //{
                //}
                //else
                //{
                //    ShowText = InputText;
                //}
                textSpeed = speed;
                if (threadText == null)
                {
                    threadText = new Thread(new ThreadStart(TextLeftToRightShow));
                    threadText.Start();
                }
                else if (!(threadText.IsAlive))
                {
                    threadText = new Thread(new ThreadStart(TextLeftToRightShow));
                    threadText.Start();
                }
                else if (_isEventTrigger)
                {
                    _isEventTrigger = false;
                    threadText = new Thread(new ThreadStart(TextLeftToRightShow));
                    threadText.Start();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /*
        private void TextMain(string InputText)
        {
            try
            {
                Thread thread = new Thread(new ParameterizedThreadStart(TextMainShow));
                thread.Start(InputText);


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TextMainShow(object InputText)
        {
            try
            {

                string _InputText = (string)InputText;
                for (int i = 0; i < _InputText.Split('\n').Length; i++)
                {
                    Dispatcher.BeginInvoke(new Action(delegate()
            {
                textShow.Text += _InputText.Split('\n')[i] + "\n";
            }));
                    Thread.Sleep(750);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        object mylock = new object();
        */
        private void TextLeftToRightShow()
        {

            while (ShowText.Length > 0 && systemIsRunning)
            {
                try
                {
                    Dispatcher.Invoke(new Action(delegate()
                    {
                        textShow.Text += ShowText[0];
                    }));
                    ShowText = ShowText.Remove(0, 1);

                }
                catch (Exception)
                {
                    throw;
                }
                Thread.Sleep(textSpeed);
            }
            if (systemIsRunning)
            {
                if (OnThreadCompleted != null)
                {
                    OnThreadCompleted(true);

                }
                Dispatcher.Invoke(new Action(delegate()
                {
                    InputBox.IsEnabled = true;
                }));
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                InputBox.IsEnabled = false;
                switch (gameMode)
                {
                    case GameMode.MAINMENU:
                        if (InputBox.Text.Equals("1"))
                        {
                            GameStep = 0;
                            gameMode = GameMode.STORY;
                            TextLeftToRight("\n\n...Starte neues Spiel...", 35);
                            InputBox.Clear();
                        }
                        else if (InputBox.Text.Equals("4"))
                        {
                            this.Close();
                        }
                        break;
                    case GameMode.STORY:
                        StoryMode();
                        InputBox.Clear();
                        break;
                }
            }
            else if (e.Key == Key.Escape)
            {

            }

            InputBox.Focus();
        }



        private void StoryMode()
        {
            try
            {
                //  TextLeftToRight("\n\nBefehle: nehmen|legen|angreifen|werfen|lesen", 35);
                if (InputBox.Text.ToUpper().StartsWith("WDH"))
                {
                    GameStep--;
                    MainWindow_OnThreadCompleted(false);
                }
                else if (InputBox.Text.ToUpper().Contains("ANGREIFEN"))
                {
                    switch (GameStep - 1)
                    {
                        case 2:
                            TextLeftToRight("\nwen willst du im Bett angreifen Häää?\n", 35);
                            GameStep = 3;
                            break;

                    }

                }
                else if (InputBox.Text.ToUpper().Contains("GEHEN"))
                {
                    switch (GameStep - 1)
                    {
                        case 2:
                            TextLeftToRight("\nwen willst du im Bett angreifen Häää?\n", 35);
                            GameStep = 3;
                            break;

                    }
                }
                else if (InputBox.Text.ToUpper().Contains("VERLASSE"))
                {
                    switch (GameStep - 1)
                    {
                        case 2:
                            InputBox.Text.ToUpper().Contains("BETT");
                            GameStep = 4;
                            MainWindow_OnThreadCompleted(false);
                            break;

                    }
                }
                else if (InputBox.Text.ToUpper().Contains("KOMBINIEREN"))
                {

                }
                else if (InputBox.Text.ToUpper().Contains("NEHMEN"))
                {

                }
                else if (InputBox.Text.ToUpper().Contains("LEGEN"))
                {

                }
                else if (InputBox.Text.ToUpper().Contains("WERFEN"))
                {

                }
                else if (InputBox.Text.ToUpper().Contains("LESEN"))
                {
                    switch (GameStep - 1)
                    {
                        case 4:
                            InputBox.Text.ToUpper().Contains("ZETTEL");
                            GameStep = 6;
                            MainWindow_OnThreadCompleted(false);
                            break;

                    }
                }
                else
                {
                    InputBox.IsEnabled = true;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
