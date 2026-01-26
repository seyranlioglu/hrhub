using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Abstraction.StatusCodes
{
    public class HrStatusCodes : IStatusCode
    {
        #region 1xx_InformationalStatusCodes
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status110DatabaseError")]
        public static readonly int Status110DatabaseError = 110;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status111DataNotFound")]
        public static readonly int Status111DataNotFound = 111;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status112DataAlreadyExists")]
        public static readonly int Status112DataAlreadyExists = 112;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status113IntegrationServiceError")]
        public static readonly int Status113IntegrationServiceError = 113;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status114IncompatibleBodyModel")]
        public static readonly int Status114IncompatibleBodyModel = 114;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status115AlreadyReversed")]
        public static readonly int Status115AlreadyReversed = 115;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status116OriginalRecordNotFound")]
        public static readonly int Status116OriginalRecordNotFound = 116;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status117FileFormatError")]
        public static readonly int Status117FileFormatError = 117;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status118FTPError")]
        public static readonly int Status118FTPError = 118;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status119ValidationError")]
        public static readonly int Status119ValidationError = 119;
        #endregion

        #region 2xx_SuccessStatusCodes
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status200OK")]
        public static readonly int Status200OK = 200;
        #endregion

        #region 4xx_ClientErrorStatusCodes
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status400BadRequest")]
        public static readonly int Status400BadRequest = 400;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status401Unauthorized")]
        public static readonly int Status401Unauthorized = 401;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status403Forbidden")]
        public static readonly int Status403Forbidden = 403;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status404NotFound")]
        public static readonly int Status404NotFound = 404;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status408TimeOut")]
        public static readonly int Status408TimeOut = 408;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status409Conflict")]
        public static readonly int Status409Conflict = 409;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status422ForeignKeyConstraint")]
        public static readonly int Status422ForeignKeyConstraint = 422;
        #endregion

        #region 5xx_ServerErrorStatusCodes
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status500InternalServerError")]
        public static readonly int Status500InternalServerError = 500;
        [Display(ResourceType = typeof(HrHub.Abstraction.StatusCodes.Resources.StatusCodeResource), Name = "Status503ServiceUnavailableError")]
        public static readonly int Status503ServiceUnavailableError = 503;
        #endregion
    }
}
