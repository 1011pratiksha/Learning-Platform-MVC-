
namespace ELearning.Models
{
    public class WatchVideoViewModel
    {
        public int Sid { get; set; }

        public int UserId { get; set; }

        public int CurrentTopicId { get; set; }

        public string Sname { get; set; } = string.Empty;

        public string Mname { get; set; } = string.Empty;

        public string Mthumbnail { get; set; } = string.Empty;

        // All topics with their unlock/completion status
        public List<TopicItemViewModel> TopicItems { get; set; }
            = new List<TopicItemViewModel>();

        // MCQs for the currently selected topic
        public List<Mcq> Mcqs { get; set; }
            = new List<Mcq>();

        // True when all topics are completed
        public bool CertificateUnlocked { get; set; }
    }
}

