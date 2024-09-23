namespace ChapAppSignalR.ViewModels
{
    public class MessageViewModel
    {
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string MessageContent { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();

    }

    public class MessageDto  //Message entity'sinden türetilmiş bir modeldir, ancak View'de ihtiyacınız olan alanları daha kolay erişilebilir hale getirmek için kullanılır. Bu şekilde, entity'nin tüm verilerini taşımak yerine, yalnızca gerekli bilgileri taşıyan
    {
        public string SenderId { get; set; }  //Mesajı gönderen kullanıcının ID'si (AppUser'dan alınır).
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
