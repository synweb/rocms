namespace RoCMS.Base.ForWeb.Models
{
    public class EditorVM
    {
        public EditorVM()
        {

        }
        public EditorVM(string content, string elementId, string elementClass, string aceMode, bool autoInit = true) : this()
        {
            ElementId = elementId;
            ElementClass = elementClass;
            Content = content;
            ACEMode = aceMode;
            AutoInit = autoInit;
        }

        public string ElementId { get; set; }
        public string ElementClass { get; set; }
        public string Content { get; set; }
        public string ACEMode { get; set; }
        public bool ShowCommonButtons { get; set; }

        public bool AutoInit { get; set; }
        public bool DefaultIsWYSIWYG { get; set; }
        
        // TODO: можно сделать настройку переключаемости редакторов и конфигурируемые тексты кнопок прям здесь
    }
}