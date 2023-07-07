using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Analytics.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNuke.Services.Analytics
{
    public class GoogleAnalyticsEngineWithRoles : GoogleAnalyticsEngine
    {
        /// <inheritdoc/>
        public override string RenderScript(string scriptTemplate)
        {
            AnalyticsConfiguration config = this.GetConfig();

            if (config == null)
            {
                return string.Empty;
            }

            var trackingId = string.Empty;
            var urlParameter = string.Empty;
            var trackForAdmin = true;
            var otherUntrackedRoles = string.Empty;

            foreach (AnalyticsSetting setting in config.Settings)
            {
                switch (setting.SettingName.ToLowerInvariant())
                {
                    case "trackingid":
                        trackingId = setting.SettingValue;
                        break;
                    case "urlparameter":
                        urlParameter = setting.SettingValue;
                        break;
                    case "trackforadmin":
                        if (!bool.TryParse(setting.SettingValue, out trackForAdmin))
                        {
                            trackForAdmin = true;
                        }
                        break;
                    case "otheruntrackedroles":
                        otherUntrackedRoles = setting.SettingValue;
                        break;
                }
            }

            if (string.IsNullOrEmpty(trackingId))
            {
                return string.Empty;
            }

            // check whether setting to not track traffic if current user is host user or website administrator.
            if ((!trackForAdmin &&
                (UserController.Instance.GetCurrentUserInfo().IsSuperUser
                 ||
                 (PortalSettings.Current != null &&
                  UserController.Instance.GetCurrentUserInfo().IsInRole(PortalSettings.Current.AdministratorRoleName))))
                ||
                 (!string.IsNullOrEmpty(otherUntrackedRoles) && PortalSettings.Current != null &&
                  otherUntrackedRoles.Split(',').Any(role => UserController.Instance.GetCurrentUserInfo().IsInRole(role)))
               )
            {
                return string.Empty;
            }

            scriptTemplate = scriptTemplate.Replace("[TRACKING_ID]", trackingId);
            if (!string.IsNullOrEmpty(urlParameter))
            {
                scriptTemplate = scriptTemplate.Replace("[PAGE_URL]", urlParameter);
            }
            else
            {
                scriptTemplate = scriptTemplate.Replace("[PAGE_URL]", string.Empty);
            }

            scriptTemplate = scriptTemplate.Replace("[CUSTOM_SCRIPT]", this.RenderCustomScript(config));

            return scriptTemplate;
        }


    }
}
