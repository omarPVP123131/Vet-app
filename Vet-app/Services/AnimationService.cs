using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace VeterinaryManagementSystem.Services
{
    public class AnimationService
    {
        private readonly ResourceDictionary _animationResources;

        public AnimationService()
        {
            // Utilizar Pack URI relativo para acceder a los recursos desde la carpeta raíz
            _animationResources = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Resources/Animations.xaml", UriKind.Absolute)
            };
        }

        public void StartAnimation(FrameworkElement element, string storyboardName)
        {
            if (_animationResources[storyboardName] is Storyboard storyboard)
            {
                Storyboard clonedStoryboard = storyboard.Clone();
                Storyboard.SetTarget(clonedStoryboard, element);
                clonedStoryboard.Begin();
            }
        }

        public void StartAnimation(FrameworkElement element, string storyboardName, Action completedCallback)
        {
            if (_animationResources[storyboardName] is Storyboard storyboard)
            {
                Storyboard clonedStoryboard = storyboard.Clone();
                Storyboard.SetTarget(clonedStoryboard, element);

                EventHandler onCompleted = null;
                onCompleted = (s, e) =>
                {
                    clonedStoryboard.Completed -= onCompleted;
                    completedCallback?.Invoke();
                };

                clonedStoryboard.Completed += onCompleted;
                clonedStoryboard.Begin();
            }
        }
    }
}
