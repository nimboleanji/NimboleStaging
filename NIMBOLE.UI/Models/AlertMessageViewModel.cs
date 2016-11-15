using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIMBOLE.UI.Models
{
    public class AlertMessageViewModel
    {
        private string _cssClassName;
        public string CSSClassName { get { return _cssClassName; } private set { } }
        public string MessageString { get; set; }
        public string MessageHeading { get; set; }

        private MessageType _msgType;

        public MessageType MessageType
        {
            get
            {
                return _msgType;
            }
            set
            {
                _msgType = value;
                switch (_msgType)
                {
                    case MessageType.Error:
                        _cssClassName = "danger";
                        break;

                    case MessageType.Success:
                        _cssClassName = "success";
                        break;

                    case MessageType.Warning:
                        _cssClassName = "warning";
                        break;
                }
            }
        }
        public int DisappearAfter { get; set; }
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning
    }
}