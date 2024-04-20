using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MagicQuizDesktop.Models
{
    public class Answer : INotifyPropertyChanged
    {
        private int id;
        private int questionId;
        private string answerText;
        private bool isCorrect;
        private bool isActive;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty("id")]
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        [JsonProperty("question_id")]
        public int QuestionId
        {
            get { return questionId; }
            set
            {
                if (questionId != value)
                {
                    questionId = value;
                    OnPropertyChanged(nameof(QuestionId));
                }
            }
        }

        [JsonProperty("answer_text")]
        public string AnswerText
        {
            get { return answerText; }
            set
            {
                if (answerText != value)
                {
                    answerText = value;
                    OnPropertyChanged(nameof(AnswerText));
                }
            }
        }

        [JsonProperty("is_correct")]
        public bool IsCorrect
        {
            get { return isCorrect; }
            set
            {
                if (isCorrect != value)
                {
                    isCorrect = value;
                    OnPropertyChanged(nameof(IsCorrect));
                }
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }
    }
}

