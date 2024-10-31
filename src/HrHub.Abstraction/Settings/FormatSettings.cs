using HrHub.Abstraction.Attributes;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("FormatSettings")]
    public class FormatSettings : ISettingsBase
    {
        public List<PhoneFormats> PhoneFormats { get; set; }
    }

    public class PhoneFormats
    {
        public string Name { get; set; }
        public string Format { get; set; }
    }
}
