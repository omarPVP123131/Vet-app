using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace VeterinaryManagementSystem.Services
{
    public class DialogService
    {
        private readonly AnimationService _animationService;

        public DialogService(AnimationService animationService)
        {
            _animationService = animationService;
        }

        public void ShowConfirmationDialog(Grid dialogContainer, string title, string message,
            Action onConfirm, Action onCancel = null)
        {
            dialogContainer.Visibility = Visibility.Visible;
            _animationService.StartAnimation(dialogContainer, "ConfirmationDialogAnimation");
        }

        public void HideConfirmationDialog(Grid dialogContainer, Action onCompleted = null)
        {
            _animationService.StartAnimation(dialogContainer, "ConfirmationDialogFadeOut", () =>
            {
                dialogContainer.Visibility = Visibility.Collapsed;
                onCompleted?.Invoke();
            });
        }

        public void ShowNotification(Popup popup, string message, NotificationType type = NotificationType.Success)
        {
            popup.IsOpen = true;
            _animationService.StartAnimation((FrameworkElement)popup.Child, "NotificationAnimation");

            // Auto-hide after 3 seconds
            System.Threading.Tasks.Task.Delay(3000).ContinueWith(t =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    HideNotification(popup);
                });
            });
        }

        public void HideNotification(Popup popup, Action onCompleted = null)
        {
            _animationService.StartAnimation((FrameworkElement)popup.Child, "NotificationFadeOut", () =>
            {
                popup.IsOpen = false;
                onCompleted?.Invoke();
            });
        }
    }

    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }
}
