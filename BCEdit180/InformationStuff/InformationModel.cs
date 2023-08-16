using System;

namespace BCEdit180.InformationStuff {
    public class InformationModel {
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public InformationModel(string type, DateTime time, string message) {
            this.Type = type;
            this.Time = time;
            this.Message = message;
        }

        public InformationModel(InfoTypes type, DateTime time, string message) {
            this.Type = type.ToString();
            this.Time = time;
            this.Message = message;
        }
    }
}