using HrHub.Abstraction.Attributes;

namespace HrHub.Abstraction.Settings
{
    [AppSetting("LectureSettings")]
    public class LectureSettings : ISettingsBase
    {     
        public int ThumbnailCaptureSecond { get; set; }
    }
}
