namespace ExamNest.DTO.Question
{
    public class QuestionWithChoicesDTO
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public List<ChoiceDTO> Choices { get; set; }
        public int Points { get; set; }
    }


}
