using ClientWPF.Data;
using ClientWPF.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPF.ViewModel
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private byte[] chosenImage;


        private string infoImage;
        public string InfoImage
        {
            get { return infoImage; }
            set
            {
                infoImage = value;
                OnPropertyChanged("InfoImage");
            }
        }


        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }


        private string infoColor;
        public string InfoColor
        {
            get { return infoColor; }
            set
            {
                infoColor = value;
                OnPropertyChanged("InfoColor");
            }
        }


        private string messageTextColor;
        public string MessageTextColor
        {
            get { return messageTextColor; }
            set
            {
                messageTextColor = value;
                OnPropertyChanged("MessageTextColor");
            }
        }


        private string newName;
        public string NewName
        {
            get { return newName; }
            set
            {
                newName = value;
                OnPropertyChanged("NewName");
            }
        }



        private Card selectedCard;
        public Card SelectedCard
        {
            get { return selectedCard; }
            set
            {
                selectedCard = value;
                OnPropertyChanged("SelectedCard");
            }
        }



        #region Comamds


        /// <summary>
        /// Add image (byte)
        /// </summary>
        private RelayCommand commandAddImage;
        public RelayCommand CommandAddImage
        {
            get
            {
                return commandAddImage ?? (commandAddImage = new RelayCommand(obj =>
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "Image files (*.jpg)|*.jpg";
                    open.ShowDialog();
                    string str = open.FileName;
                    
                    
                    if(str == "")
                    {
                        InfoImage = "Не выбрано фото";
                        InfoColor = "#FFDC0808";
                        chosenImage = null;
                    }
                    else
                    {
                        InfoImage = "Ok";
                        InfoColor = "#FF0ED33A";
                        chosenImage = GetPhoto(str);
                    }

                }));
            }
        }


        /// <summary>
        /// Add new card
        /// </summary>
        private RelayCommand commandAddNewCard;
        public RelayCommand CommandAddNewCard
        {
            get
            {
                return commandAddNewCard ?? (commandAddNewCard = new RelayCommand(obj =>
                {
                    if (NewName != null && NewName != "" && chosenImage != null)
                    {
                        Card card = new Card() { Name = NewName, Image = chosenImage };
                        
                        CardDataApi context = new CardDataApi();
                        context.AddCard(card);

                        GetCardsFromApi();
                    }
                }));
            }
        }


        /// <summary>
        /// Delete selected card
        /// </summary>
        private RelayCommand commandDeleteCard;
        public RelayCommand CommandDeleteCard
        {
            get
            {
                return commandDeleteCard ?? (commandDeleteCard = new RelayCommand(obj =>
                {
                    Card card  = obj as Card;
                    if (card == null) return;
                    int id = CollectionCards.IndexOf(card);


                    CardDataApi context = new CardDataApi();
                    context.DeleteCard(id);
                    CollectionCards.RemoveAt(id);
                }));
            }
        }


        /// <summary>
        /// Put selected Image
        /// </summary>
        private RelayCommand сommandPutImage;
        public RelayCommand CommandPutImage
        {
            get
            {
                return сommandPutImage ?? (сommandPutImage = new RelayCommand(obj =>
                {
                    Card card = obj as Card;
                    if (card == null) return;

                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "Image files (*.jpg)|*.jpg";
                    open.ShowDialog();
                    string str = open.FileName;

                    if (str != "")
                    {
                        card.Image = GetPhoto(str);
                    }

                }));
            }
        }


        /// <summary>
        /// Put selected card
        /// </summary>
        private RelayCommand сommandPutCard;
        public RelayCommand CommandPutCard
        {
            get
            {
                return сommandPutCard ?? (сommandPutCard = new RelayCommand(obj =>
                {
                    Card card = obj as Card;
                    int id = CollectionCards.IndexOf(card);

                    CardDataApi context = new CardDataApi();
                    context.PutCard(id, card);

                    GetCardsFromApi();
                }));
            }
        }




        #endregion


        private ObservableCollection<Card> collectionCards;
        public ObservableCollection<Card> CollectionCards
        {
            get { return collectionCards; }
            set
            {
                collectionCards = value;
                OnPropertyChanged("CollectionCards");
            }
        }


        public ApplicationViewModel()
        {
            CardDataApi.Events += EventMessage;
            try
            {
                GetCardsFromApi();
            }
            catch
            {
                CollectionCards = new ObservableCollection<Card>();
                CollectionCards.Add(new Card() { Name = "Ошибка подключения!" });
            }
        }


        private void GetCardsFromApi()
        {
            CardDataApi context = new CardDataApi();
            CollectionCards = new ObservableCollection<Card>();
            var temp = context.GetCards();

            foreach (var item in temp)
            {
                CollectionCards.Add(item);
            }
        }

        private void EventMessage(string str, string color)
        {
            MessageText = str;
            MessageTextColor = color;
        }

        private static byte[] GetPhoto(string str)
        {
            byte[] image;

            // чтение файла
            using (FileStream fstream = File.OpenRead(str))
            {
                byte[] array = new byte[fstream.Length];

                fstream.Read(array, 0, array.Length);

                image = array;
            }

            return image;
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}
