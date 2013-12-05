using PhoneKit.Framework.Controls;
using System;
using System.Windows;

namespace SeeMensa.Controls
{
    public class LocalizedAboutControl : AboutControlBase
    {
        protected override void LocalizeContent()
        {
            ApplicationIconSource = new Uri("/Images/seeMENSA.png", UriKind.Relative);
            ApplicationTitle = SeeMensa.Language.Language.ApplicationTitle;
            ApplicationVersion = "3.2";
            ApplicationAuthor = SeeMensa.Language.Language.ApplicationAuthor;
            ApplicationDescription = SeeMensa.Language.Language.ApplicationDescription;
            SupportAndFeedbackText = SeeMensa.Language.Language.SupportAndFeedback;
            SupportAndFeedbackEmail = "apps@bsautermeister.de";
            PrivacyInfoText = SeeMensa.Language.Language.PrivacyInfo;
            PrivacyInfoLink = "http://bsautermeister.de/privacy.php";
            RateAndReviewText = SeeMensa.Language.Language.RateAndReview;
            MoreAppsText = SeeMensa.Language.Language.MoreApps;
            MoreAppsSearchTerms = "Benjamin Sautermeister";
        }
    }
}
