using PhoneKit.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeMensa.Controls
{
    /// <summary>
    /// The localized feedback dialog.
    /// </summary>
    public class LocalizedFeedbackDialogControl : FeedbackDialogControlBase
    {
        /// <summary>
        /// Localizes the user controls content and texts
        /// </summary>
        protected override void LocalizeContent()
        {
            RatingTitleText = SeeMensa.Language.Language.RatingTitleText;
            RatingMessage5Text = SeeMensa.Language.Language.RatingMessage5Text;
            RatingMessage10Text = SeeMensa.Language.Language.RatingMessage10Text;
            RatingYesText = SeeMensa.Language.Language.RatingYesText;
            RatingNoText = SeeMensa.Language.Language.RatingNoText;
            FeedbackTitleText = SeeMensa.Language.Language.FeedbackTitleText;
            FeedbackMessageText = SeeMensa.Language.Language.FeedbackMessageText;
            FeedbackEmail = SeeMensa.Language.Language.FeedbackEmail;
            FeedbackSubject = SeeMensa.Language.Language.FeedbackSubject;
            FeedbackBodyText = SeeMensa.Language.Language.FeedbackBodyText;
            FeedbackYesText = SeeMensa.Language.Language.FeedbackYesText;
            FeedbackNoText = SeeMensa.Language.Language.FeedbackNoText;
            ApplicationVersion = SeeMensa.Language.Language.ApplicationVersion;
        }
    }
}
