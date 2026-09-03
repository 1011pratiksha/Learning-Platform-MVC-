
namespace ELearning.Models
{
    public class TopicItemViewModel
    {
        public Topic Topic { get; set; } = null!;

        public bool IsUnlocked { get; set; }

        public bool IsCompleted { get; set; }
    }
}

