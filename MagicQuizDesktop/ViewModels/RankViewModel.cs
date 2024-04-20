using MagicQuizDesktop.Commands;
using MagicQuizDesktop.Models;
using MagicQuizDesktop.Repositories;
using MagicQuizDesktop.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MagicQuizDesktop.ViewModels
{
    public class RankViewModel : ViewModelBase
    {
        private int rankId;
        private string _name;
        private int _userId;
        private int _score;
        private User _currentUser;
        private List<Rank> ranks;
        private ObservableCollection<Rank> _rankList;

        string? _errorMessage = string.Empty;
        string? _message = string.Empty;

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged(nameof(CurrentUser));
                    // Itt frissítheted a userId-t is, ha szükséges
                }
            }
        }

        public ObservableCollection<Rank> RankList
        {
            get => _rankList;
            set
            {
                _rankList = value;
                OnPropertyChanged(nameof(RankList));
            }
        }
        private readonly IRankRepository _rankRepository;

        public int RankId
        {
            get => rankId;
            set
            {
                rankId = value;
                OnPropertyChanged(nameof(RankId));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        //For errors           
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    Message = string.Empty;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }  
        public string? Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        public ICommand ResetCommand { get; }
        public ICommand UpdateCommand { get; }

        public RankViewModel()
        {
            CurrentUser = SessionManager.Instance.CurrentUser;
            _rankRepository = new RankRepository();
            ranks = new List<Rank>();
            RankList = new ObservableCollection<Rank>(ranks);
            ResetCommand = new AsyncRelayCommand(async _ => await ResetRankList());
            UpdateCommand = new AsyncRelayCommand(async _ => await SetRankOrder());
             _=SetRankOrder();
        }

        private async Task ResetRankList()
        {
            try
            {
                var response = await _rankRepository.ResetRanks(CurrentUser.AuthToken);
                if (response.Success)
                {
                    ErrorMessage = "A ranglista sikeresen törölve!";
                    await SetRankOrder();
                }
                else
                {
                    ErrorMessage = $"Hiba történt az adatok törlése közben: {response.Message}";
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";
            }
        }

        private async Task GetRanks()
         {
            try
            {
                var response = await _rankRepository.GetRanks(CurrentUser.AuthToken);
                if (response.Success)
                {
                    ranks = response.Data;
                    RankList = new ObservableCollection<Rank>(ranks);                    
                }
                else
                {
                    ErrorMessage =$"Hiba történt az adatok frissítése közben: {response.Message}";
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = $"Hiba az alkalmazásban:{ex.Message}";
            }

         }
        private async Task SetRankOrder()
        {
            await GetRanks();
                ranks = ranks.OrderByDescending(r => r.Score).ToList();
                for (int i = 0; i < ranks.Count; i++)
                {
                    ranks[i].RankNumber = i + 1;

                    switch (i)
                    {
                        case 0:
                            ranks[i].RankColor = "#FFD700";
                            break;
                        case 1:
                            ranks[i].RankColor = "#C0C0C0";
                            break;
                        case 2:
                            ranks[i].RankColor = "#CD7F32";
                            break;
                        default:
                            ranks[i].RankColor = "#07F3C0";
                            break;

                    }
                }
            RankList = new ObservableCollection<Rank>(ranks);
        }


    }
}
