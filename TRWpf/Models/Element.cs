using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace TRWpf.Models
{
    public class Element {
        public int Id { get; set; }
        public String Name { get; set; }

        public readonly static List<Element> CardTypeViewModels = new List<Element>
        {
                    new Element { Id = 0, Name = "Постонянная" },
                    new Element { Id = 1, Name = "Временная" },
                    new Element { Id = 2, Name = "Разовая" }
        };

        public readonly static List<Element> UserGroupViewModels = new List<Element>
        {
                    new Element { Id = 0, Name = "Не определен" },
                    new Element { Id = 1, Name = "Сотрудники" },
                    new Element { Id = 2, Name = "Посетители" }
        };

        public readonly static List<Element> CardStatusViewModels = new List<Element>
        {
                    new Element { Id = 0, Name = "В резерве" },
                    new Element { Id = 1, Name = "На руках" },
                    new Element { Id = 2, Name = "Потеряна" },
                    new Element { Id = 3, Name = "Украдена" },
                    new Element { Id = 4, Name = "Недействительна" }
        };

        static string GetDescription(Enum enumElement)
        {
            Type type = enumElement.GetType();

            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumElement.ToString();
        }
    }

    public enum CardType : int
    {
        [Description("Постоянная")]
        Constant = 0,
        [Description("Временная")]
        Tempory = 1,
        [Description("Разовая")]
        Singl = 2
    }
}